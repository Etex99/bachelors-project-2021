using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace Prototype
{
	public class SurveyHost
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
        internal SurveyData data;
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
		public void RunSurvey()
		{

			//Phase 1 - making client connections and collecting emojis
			Task task1 = ReplyBroadcast();
			Task task2 = AcceptClient();
			Task.WaitAll(new Task[] { task1, task2 });

			//Phase 2 - time after the survey has concluded in which users view results and host decides whether activity vote starts
			Console.WriteLine($"Results: {data}");
			//Send results to all clients

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
		private async Task ReplyBroadcast() {

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
						await Task.Delay(1000);
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
						Console.WriteLine($"Replying... EP: {ep}");
						s.SendTo(sendbuf, ep);
					}
					else
					{
						Console.WriteLine("Received invalid Room Code");
					}
				}
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("ReplyBroadcast task was cancelled");
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

		private async Task AcceptClient() {

			try
			{
				TcpListener listener = new TcpListener(IPAddress.Any, Const.Network.ServerTCPListenerPort);
				listener.Start();

				while (true)
				{
					Console.WriteLine("Waiting to accept tcp client");
					Task<TcpClient> newClient = listener.AcceptTcpClientAsync();

					//Allow cancellation of task between adding each client
					do
					{
						if (token.IsCancellationRequested)
						{
							listener.Stop();
							token.ThrowIfCancellationRequested();
						}
						await Task.WhenAny(new Task[] { Task.Delay(1000), newClient });
					} while (newClient.Status != TaskStatus.RanToCompletion);

					//Child task to communicate with new client
					Task childtask = Task.Run( async () =>
					{
						TcpClient client = newClient.Result;

						//prepare message for sending survey
						string message = JsonConvert.SerializeObject(survey);
						Console.WriteLine($"DEBUG: message: {message}");
						byte[] bytes = Encoding.ASCII.GetBytes(message);

						try
						{
							NetworkStream ns = client.GetStream();
							//send message
							ns.Write(bytes, 0, bytes.Length);

							//try get reply
							byte[] buffer = new byte[256];
							Task<int> emojiReply = ns.ReadAsync(buffer, 0, buffer.Length);

							//allow cancellation of task here.
							do
							{
								if (token.IsCancellationRequested)
								{
									client.Close();
									token.ThrowIfCancellationRequested();
								}
								await Task.WhenAny(new Task[] { Task.Delay(1000), emojiReply });
							} while (emojiReply.Status != TaskStatus.RanToCompletion);

							//process reply
							string reply = Encoding.ASCII.GetString(buffer, 0, emojiReply.Result);
							Console.WriteLine($"Bytes read: {emojiReply.Result}");
							Console.WriteLine($"Client sent: {reply}");
							
							//add to surveydata
							data.AddEmojiResults(int.Parse(reply));

							//add this client to list of clients
							clients.Add(client);
						}
						catch (OperationCanceledException) {
							Console.WriteLine("Cancelling task, Client was dropped for being slow poke");
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
			catch (OperationCanceledException)
			{
				Console.WriteLine("AcceptClient task was cancelled");
			}
			catch (SocketException e)
			{
				Console.WriteLine("Socket exception occured in AcceptClient...");
				Console.WriteLine(e);
			}
		}
	}
}
