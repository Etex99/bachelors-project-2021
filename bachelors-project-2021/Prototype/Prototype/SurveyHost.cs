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
		public ActivityVote voteCalc { get; private set; } = null;
		private List<TcpClient> clients;

		//Threading
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

			//Phase 2 - time after the survey has concluded in which users view results
			Console.WriteLine($"Results: {data}");
			Console.WriteLine("Sending results to clients");
			SendToAllClients(data);
		}

		//Main sequence of running activity vote
		public async void RunActivityVote()
		{
			//send first vote to all candidates
			SendToAllClients(voteCalc.GetVote1Candidates());
			SendToAllClients(voteCalc.vote1Timer.ToString());

			//for first vote duration accept votes from all clients
			await AcceptVotes1(voteCalc.vote1Timer + 5);

			//prepare second vote and send it to all clients
			voteCalc.calcVote2Candidates(data.GetVote1Results());
			SendToAllClients(voteCalc.GetVote2Candidates());
			SendToAllClients(voteCalc.vote2Timer.ToString());

			//for vote 2 duration accept votes from all clients
			await AcceptVotes2(voteCalc.vote2Timer + 5);

			//prepare result and send it to all clients
			string result = voteCalc.calcFinalResult(data.GetVote2Results());
			data.voteResult = result;
			SendToAllClients(result);
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
			State = HostState.AwaitingVotes1;
			//prepare first vote
			voteCalc = new ActivityVote();
			voteCalc.calcVote1Candidates(survey.emojis, data.GetEmojiResults());
			Task.Run( async () =>
			{
				RunActivityVote();
			});
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

					//message received
					string message = Encoding.ASCII.GetString(broadcast.Result.Buffer, 0, broadcast.Result.Buffer.Length);
					Console.WriteLine($"Received broadcast from {broadcast.Result.RemoteEndPoint} :");
					Console.WriteLine($" {message}");

					if (message == survey.RoomCode)
					{
						//prepare message and destination
						byte[] sendbuf = Encoding.ASCII.GetBytes("Connect please");
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
		private async Task AcceptVotes1(int seconds) {

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
							byte[] buffer = new byte[64];
							NetworkStream ns = client.GetStream();
							ns.ReadTimeout = 1000 * seconds;
							int bytesRead = 0;
							Console.WriteLine("Waiting for client vote 1");
							bytesRead = ns.Read(buffer, 0, buffer.Length);
							Console.WriteLine($"DEBUG: AcceptVotes 1 read {bytesRead} bytes from: {client}");

							//set timeout back to normal
							ns.ReadTimeout = int.MaxValue;

							//if read times out bytes read remains 0?
							if (bytesRead <= 0)
							{
								return;
							}

							//read was successful, expecting JSON string containing Dictionary<int, string> 
							data.AddVote1Results(JsonConvert.DeserializeObject<Dictionary<int, string>>(Encoding.ASCII.GetString(buffer, 0, bytesRead)));
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

					})
				);
			}

			//wait for all tasks to complete before returning
			await Task.WhenAll(clientVotes);
			return;
		}
		private async Task AcceptVotes2(int seconds) {
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
							byte[] buffer = new byte[64];
							NetworkStream ns = client.GetStream();
							ns.ReadTimeout = 1000 * seconds;
							int bytesRead = 0;
							Console.WriteLine("Waiting for client vote 2");
							bytesRead = ns.Read(buffer, 0, buffer.Length);
							Console.WriteLine($"DEBUG: AcceptVotes 2 read {bytesRead} bytes from: {client}");

							//set timeout back to normal
							ns.ReadTimeout = int.MaxValue;

							//if read times out bytes read remains 0
							if (bytesRead == 0)
							{
								return;
							}

							//read was successful, expecting string containing final activity vote
							data.AddVote2Results(Encoding.ASCII.GetString(buffer, 0, bytesRead));
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

					})
				);
			}

			//wait for all tasks to complete before returning
			await Task.WhenAll(clientVotes);
			return;
		}

		private void SendToAllClients(object obj) {

			//prepare data for transmission
			byte[] message;
			message = Encoding.ASCII.GetBytes(
				JsonConvert.SerializeObject(obj)
			);

			//iterate each recorded client
			foreach (var client in clients)
			{
				//catch errors per client
				try
				{
					NetworkStream ns = client.GetStream();
					ns.Write(message, 0, message.Length);
				}
				catch (ObjectDisposedException e)
				{
					Console.WriteLine($"Connection lost with client: {client.Client.RemoteEndPoint}. Dropping client");
					Console.WriteLine(e);
					clients.Remove(client);
				}
			}
		}
		private void SendToAllClients(string text)
		{

			//prepare data for transmission
			byte[] message;
			message = Encoding.ASCII.GetBytes( text );

			//iterate each recorded client
			foreach (var client in clients)
			{
				//catch errors per client
				try
				{
					NetworkStream ns = client.GetStream();
					ns.Write(message, 0, message.Length);
				}
				catch (ObjectDisposedException e)
				{
					Console.WriteLine($"Connection lost with client: {client.Client.RemoteEndPoint}. Dropping client");
					Console.WriteLine(e);
					clients.Remove(client);
				}
			}
		}
		public void DestroyHost() {
			
			//close all connections
			foreach (var item in clients)
			{
				item.Close();
			}
		}
	}
}
