﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace Prototype
{
	class SurveyHost
	{
		public enum HostState
		{
			AwaitingAnswers = 0,
			Results = 1,
			AwaitingVotes1 = 2,
			AwaitingVotes2 = 3,
			VoteResult = 4
		}
		public HostState State { get; private set; } = HostState.AwaitingAnswers;

		private Survey survey;
		private SurveyData data;
		private List<TcpClient> clients;

		//Threading
		private List<Task> currentTasks;
		private CancellationTokenSource tokenSource;
		private CancellationToken token;

		public SurveyHost() {
			data = new SurveyData();
			survey = SurveyManager.GetInstance().GetSurvey();
			clients = new List<TcpClient>();
			tokenSource = new CancellationTokenSource();
			token = tokenSource.Token;
		}
		
		//Main sequence of running the survey
		public async void RunSurvey() {
			
			//Phase 1 - making client connections and collecting emojis
			try
			{
				currentTasks = new List<Task>();
				currentTasks.Add(Task.Run(ReplyBroadcast, token));
				currentTasks.Add(Task.Run(AcceptClient, token));

				await Task.WhenAll(currentTasks.ToArray());

			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("ReplyBroadcast task was cancelled");
				Console.WriteLine("AcceptClient task was cancelled");
			}
			//Phase 2 - time after the survey has concluded in which users view results and host decides whether activity vote starts
			

		}

		//Transition from awaiting emojis to summary
		public void CloseSurvey() {
			State = HostState.Results;
			tokenSource.Cancel();
		}

		//End survey for good
		public void EndSurvey() {
			
		}

		//Continue survey to activity voting
		public void StartActivityVote() {

		}

		//replies to broadcasts in the network which contain the correct roomCode
		private void ReplyBroadcast() {

			UdpClient listener = new UdpClient(Const.Network.ServerUDPClientPort);
			Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			try
			{
				//loop to serve all broadcasts
				while (true)
				{
					Console.WriteLine("Waiting for broadcast");
					Task<UdpReceiveResult> broadcast = listener.ReceiveAsync();

					//allow cancellation of this task between each udp message
					do
					{
						if (token.IsCancellationRequested)
						{
							listener.Close();
							s.Close();
							token.ThrowIfCancellationRequested();
						}
					} while (broadcast.Status != TaskStatus.RanToCompletion);

					string message = Encoding.ASCII.GetString(broadcast.Result.Buffer, 0, broadcast.Result.Buffer.Length);
					Console.WriteLine($"Received broadcast from {broadcast.Result.RemoteEndPoint} :");
					Console.WriteLine($" {message}");

					if (message == survey.RoomCode)
					{
						//prepare message and destination
						byte[] sendbuf = Encoding.ASCII.GetBytes("connection available");
						IPEndPoint ep = new IPEndPoint(broadcast.Result.RemoteEndPoint.Address, Const.Network.ClientUDPClientPort);

						//reply
						s.SendTo(sendbuf, ep);
					};
				}
			}
			catch (SocketException e)
			{
				Console.WriteLine("Socket exception occured in ReplyBroadcast...");
				Console.WriteLine(e);
				throw;
			}
			finally
			{
				listener.Close();
				s.Close();
			}
		}

		private void AcceptClient() {

			TcpListener listener = new TcpListener(IPAddress.Any, Const.Network.ServerTCPListenerPort);

			try
			{
				listener.Start();
				while (true)
				{
					Task<TcpClient> newClient = listener.AcceptTcpClientAsync();

					//Allow cancellation of task between adding each client
					do
					{
						if (token.IsCancellationRequested)
						{
							listener.Stop();
							token.ThrowIfCancellationRequested();
						}
					} while (newClient.Status != TaskStatus.RanToCompletion);

					//Child task to get emoji response from new client
					Task childtask = Task.Run(() =>
					{
						TcpClient client = newClient.Result;

						//prepare message for survey emojis
						string message = "";
						foreach (var item in survey.emojis)
						{
							message += item.ID;
							message += ",";
						}
						//remove trailing comma
						message = message.Substring(0, message.Length - 1);

						Console.WriteLine($"DEBUG: message: {message}");
						byte[] bytes = Encoding.ASCII.GetBytes(message);

						try
						{
							NetworkStream ns = client.GetStream();
							//send message
							ns.Write(bytes, 0, bytes.Length);

							//try get reply
							byte[] buffer = new byte[4]; //this is enough for expected 1 int 
							Task<int> emojiReply = ns.ReadAsync(buffer, 0, buffer.Length);

							//allow cancellation of task here.
							do
							{
								if (token.IsCancellationRequested)
								{
									client.Close();
									token.ThrowIfCancellationRequested();
								}
							} while (emojiReply.Status != TaskStatus.RanToCompletion);

							//process reply
							string reply = Encoding.ASCII.GetString(bytes, 0, emojiReply.Result);
							Console.WriteLine($"Client sent: {reply}");

							//add to surveydata
							data.AddEmojiResults(int.Parse(reply));

							//add this client to list of clients
							clients.Add(client);
						}
						catch (Exception e)
						{
							Console.WriteLine($"Something went wrong in first communication with client: {client.Client.RemoteEndPoint}");
							Console.WriteLine(e);
							client.Close();
							return;
						}
						
					}, token);
				}				
			}
			catch (SocketException e)
			{
				Console.WriteLine("Socket exception occured in AcceptClient...");
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
