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
        public SurveyData data { get; private set; }

		public int clientCount { get; private set; }
		private List<TcpClient> clients;
		private List<IPAddress> clientHistory;

		//Threading
		private List<Task> cancellableTasks;
		private CancellationTokenSource tokenSource;
		private CancellationToken token;

		//vote
		public ActivityVote voteCalc { get; private set; } = null;
		public Task voteTask { get; private set; } = null;

		public SurveyHost() {
			data = new SurveyData();
			survey = SurveyManager.GetInstance().GetSurvey();
			clientCount = 0;
			clients = new List<TcpClient>();
			clientHistory = new List<IPAddress>();
			cancellableTasks = new List<Task>();
			tokenSource = new CancellationTokenSource();
			token = tokenSource.Token;
		}

		//Main sequence of running the survey
		public async Task<bool> RunSurvey()
		{

			//Phase 1 - making client connections and collecting emojis
			Task<bool> task1 = ReplyBroadcast();
			Task<bool> task2 = AcceptClient();
			cancellableTasks.Add(task1);
			cancellableTasks.Add(task2);

			await Task.WhenAll(cancellableTasks.ToArray());
			if (task1.Result == false || task2.Result == false)
			{
				//Fatal unexpected error do something...
				return false;
			}

			//Phase 2 - time after the survey has concluded in which users view results
			Console.WriteLine($"Results: {data}");
			Console.WriteLine("Sending results to clients");
			SendToAllClients(data);

			return true;
		}

		//Main sequence of running activity vote
		public async Task RunActivityVote()
		{
			//send first vote to all candidates
			SendToAllClients(voteCalc.GetVote1Candidates());
			SendToAllClients(voteCalc.vote1Timer.ToString());

			//for first vote duration wait for clients to reply
			await Task.Delay(1000 * (voteCalc.vote1Timer + voteCalc.coolDown));
			await AcceptVotes1();

			//prepare second vote and send it to all clients
			voteCalc.calcVote2Candidates(data.GetVote1Results());
			SendToAllClients(voteCalc.GetVote2Candidates());
			SendToAllClients(voteCalc.vote2Timer.ToString());

			//for vote 2 duration wait for clients to reply
			await Task.Delay(1000 * (voteCalc.vote2Timer + voteCalc.coolDown));
			await AcceptVotes2();

			//prepare result and send it to all clients
			string result = voteCalc.calcFinalResult(data.GetVote2Results());
			data.voteResult = result;
			SendToAllClients(result);
		}

		//Transition from awaiting emojis to summary
		public async Task CloseSurvey() {
			State = HostState.Results;
			tokenSource.Cancel();
			await Task.WhenAll(cancellableTasks.ToArray());
			return;
		}

		//Continue survey to activity voting
		public void StartActivityVote() {
			State = HostState.AwaitingVotes1;
			//prepare first vote
			voteCalc = new ActivityVote();
			voteCalc.calcVote1Candidates(survey.emojis, data.GetEmojiResults());
			Task.Run(() =>
			{
				voteTask = RunActivityVote();
			});
		}

		//replies to broadcasts in the network which contain the correct roomCode
		private async Task<bool> ReplyBroadcast() {

			try
			{
				UdpClient listener = new UdpClient(Const.Network.ServerUDPClientPort);
				Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

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
						await Task.WhenAny(new Task[] { Task.Delay(1000), broadcast });
					} while (broadcast.Status != TaskStatus.RanToCompletion);

					//message received
					string message = Encoding.Unicode.GetString(broadcast.Result.Buffer, 0, broadcast.Result.Buffer.Length);
					Console.WriteLine($"Received broadcast from {broadcast.Result.RemoteEndPoint} :");
					Console.WriteLine($" {message}");

					if (message == survey.RoomCode)
					{
						//has this client answered the survey already?
						if (!clientHistory.Contains(broadcast.Result.RemoteEndPoint.Address))
						{
							//prepare message and destination
							byte[] sendbuf = Encoding.Unicode.GetBytes("Connect please");
							IPEndPoint ep = new IPEndPoint(broadcast.Result.RemoteEndPoint.Address, Const.Network.ClientUDPClientPort);

							//reply
							Console.WriteLine($"Replying... EP: {ep}");
							s.SendTo(sendbuf, ep);

						} else
						{
							Console.WriteLine("Old client tried to connect again");
						}
					}
					else
					{
						Console.WriteLine("Received invalid Room Code");
					}
				}
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("ReplyBroadcast task was cancelled gracefully");
				return true;
			}
			catch (ObjectDisposedException e)
			{
				Console.WriteLine(e);
			}
			catch (SocketException e)
			{
				Console.WriteLine(e);
			}

			//handle unexpected errors
			Console.WriteLine("Fatal error occured, aborting survey.");
			tokenSource.Cancel();
			return false;
		}

		private async Task<bool> AcceptClient() {

			try
			{
				TcpListener listener = new TcpListener(IPAddress.Any, Const.Network.ServerTCPListenerPort);
				listener.Start();

				while (true)
				{
					Console.WriteLine("Waiting to accept tcp client");
					Task<TcpClient> newClient = null;
					newClient = listener.AcceptTcpClientAsync();

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
					Task childtask = Task.Run(() => ServeNewClient(newClient.Result, token));
				}				
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("AcceptClient task was cancelled gracefully");
				return true;
			}
			catch (InvalidOperationException e)
			{
				Console.WriteLine(e);
			}
			catch (SocketException e)
			{
				Console.WriteLine(e);
			}

			//handle unexpected errors
			Console.WriteLine("Fatal error occured, aborting survey.");
			tokenSource.Cancel();
			return false;
		}
		private async void ServeNewClient(TcpClient client, CancellationToken token)
		{
			try
			{
				//wait for emoji from client, expecting 1 int
				NetworkStream ns = client.GetStream();
				byte[] buffer = new byte[4];
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

				if (emojiReply.Result == 0)
				{
					//we read nothing out of disconnected network, nice
					//we don't want this client
					return;
				}

				//process reply
				string reply = Encoding.Unicode.GetString(buffer, 0, emojiReply.Result);
				Console.WriteLine($"Bytes read: {emojiReply.Result}");
				Console.WriteLine($"Client sent: {reply}");

				//add to surveydata
				data.AddEmojiResults(int.Parse(reply));

				//add this client to list of clients
				clients.Add(client);
				clientHistory.Add(((IPEndPoint)client.Client.RemoteEndPoint).Address);
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Cancelling task, Client was dropped for being slow poke");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Something went wrong in first communication with client: {client.Client.RemoteEndPoint}");
				Console.WriteLine(e);
				client.Close();
			}
		}

		private async Task AcceptVotes1() {

			//listen to each client for their answer
			List<Task> clientVotes = new List<Task>();
			foreach (var client in clients)
			{
				clientVotes.Add(

					//task for one client
					Task.Run(() =>
					{

						try
						{
							//waiting for vote for limited time by setting network stream read timeout
							byte[] buffer = new byte[2048];
							NetworkStream ns = client.GetStream();
							int bytesRead = 0;
							Console.WriteLine("Waiting for client vote 1");

							//if client does not reply anything, data is not available.
							if (!ns.DataAvailable)
							{
								return;
							}

							bytesRead = ns.Read(buffer, 0, buffer.Length);

							Console.WriteLine($"DEBUG: AcceptVotes 1 read {bytesRead} bytes from: {client}");

							//if client has exited read results in 0 bytes read
							if (bytesRead <= 0)
							{
								return;
							}

							//read was successful, expecting JSON string containing Dictionary<int, string> 
							data.AddVote1Results(JsonConvert.DeserializeObject<Dictionary<int, string>>(Encoding.Unicode.GetString(buffer, 0, bytesRead)));
							Console.WriteLine("Added voting 1 answer to surveydata");
							return;

						}
						catch (JsonException e)
						{
							Console.WriteLine("Received bad JSON");
							Console.WriteLine(e);
							//so... you have chosen death
							clients.Remove(client);
						}
						catch (ObjectDisposedException e)
						{
							Console.WriteLine("Connection lost to client");
							Console.WriteLine(e);
							//long live the king
							clients.Remove(client);
						}
						catch (System.IO.IOException e)
						{
							Console.WriteLine("Error reading socket or network");
							Console.WriteLine(e);
						}

					})
				);
			}

			//wait for all tasks to complete before returning
			await Task.WhenAll(clientVotes);
			Console.WriteLine("Stopped accepting votes in phase 1");
			return;
		}
		private async Task AcceptVotes2() {
			//listen to each client for their answer
			List<Task> clientVotes = new List<Task>();
			foreach (var client in clients)
			{
				clientVotes.Add(

					//task for one client
					Task.Run(() =>
					{

						try
						{
							//waiting for vote for limited time by setting network stream read timeout
							byte[] buffer = new byte[128];
							NetworkStream ns = client.GetStream();
							int bytesRead = 0;
							Console.WriteLine("Waiting for client vote 2");

							//if client does not reply anything, data is not available.
							if (!ns.DataAvailable)
							{
								return;
							}

							bytesRead = ns.Read(buffer, 0, buffer.Length);
							Console.WriteLine($"DEBUG: AcceptVotes 2 read {bytesRead} bytes from: {client}");

							//if client has exited read results in 0 bytes read
							if (bytesRead == 0)
							{
								return;
							}

							//read was successful, expecting string containing final activity vote
							data.AddVote2Results(Encoding.Unicode.GetString(buffer, 0, bytesRead));
							Console.WriteLine("Added voting 2 answer to surveydata");
							return;

						}
						catch (JsonException e)
						{
							Console.WriteLine("Received bad JSON");
							Console.WriteLine(e);
							//we don't do that here
							clients.Remove(client);
						}
						catch (ObjectDisposedException e)
						{
							Console.WriteLine("Connection lost to client");
							Console.WriteLine(e);
							//this is sparta
							clients.Remove(client);
						}
						catch (System.IO.IOException e)
						{
							Console.WriteLine("Error reading socket or network");
							Console.WriteLine(e);
						}

					})
				);
			}

			//wait for all tasks to complete before returning
			await Task.WhenAll(clientVotes);
			Console.WriteLine("Stopped accepting votes in phase 2");
			return;
		}

		private void SendToAllClients(object obj) {

			//prepare data for transmission
			byte[] message;
			message = Encoding.Unicode.GetBytes(
				JsonConvert.SerializeObject(obj)
			);

			//iterate each recorded client
			foreach (var client in clients)
			{
				//catch errors per client
				try
				{
					NetworkStream ns = client.GetStream();

					if (!ns.CanWrite)
					{
						return;
					}

					ns.Write(message, 0, message.Length);
				}
				catch (ObjectDisposedException e)
				{
					Console.WriteLine($"Connection lost with client: {client.Client.RemoteEndPoint}. Dropping client");
					Console.WriteLine(e);
					clients.Remove(client);
				}
				catch (System.IO.IOException e)
				{
					Console.WriteLine("Error reading socket or network");
					Console.WriteLine(e);
				}
			}
		}
		private void SendToAllClients(string text)
		{

			//prepare data for transmission
			byte[] message;
			message = Encoding.Unicode.GetBytes( text );

			//iterate each recorded client
			foreach (var client in clients)
			{
				//catch errors per client
				try
				{
					NetworkStream ns = client.GetStream();

					if (!ns.CanWrite)
					{
						return;
					}

					ns.Write(message, 0, message.Length);
				}
				catch (ObjectDisposedException e)
				{
					Console.WriteLine($"Connection lost with client: {client.Client.RemoteEndPoint}. Dropping client");
					Console.WriteLine(e);
					clients.Remove(client);
				}
				catch (System.IO.IOException e)
				{
					Console.WriteLine("Error reading socket or network");
					Console.WriteLine(e);
				}
			}
		}
		public void DestroyHost() {
			//cancel tasks
			tokenSource.Cancel();
			//close all connections
			foreach (var item in clients)
			{
				item.Close();
			}
		}
	}
}
