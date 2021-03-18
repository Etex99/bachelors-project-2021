using System;
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

		//Threading
		private List<Task> currentTasks;
		private CancellationTokenSource tokenSource;
		private CancellationToken token;

		public SurveyHost() {
			data = new SurveyData();
			survey = SurveyManager.GetInstance().GetSurvey();
			tokenSource = new CancellationTokenSource();
			token = tokenSource.Token;
		}
		
		//Main sequence of running the survey
		public async void RunSurvey() {
			
			//Phase 1 - making client connections and collecting emojis
			try
			{
				currentTasks = new List<Task>();
				currentTasks.Add(Task.Run(ReplyBroadcastAsync, token));
				currentTasks.Add(Task.Run(AcceptClientAsync, token));

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
		private void ReplyBroadcastAsync() {

			UdpClient listener = new UdpClient(Const.Network.ServerUDPClientPort);
			IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, Const.Network.ServerUDPClientPort);
			Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			Task childTask = Task.Run(() =>
			{
				try
				{
					//loop to serve all broadcasts
					while (true)
					{
						Console.WriteLine("Waiting for broadcast");
						byte[] bytes = listener.Receive(ref groupEP);
						string message = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
						Console.WriteLine($"Received broadcast from {groupEP} :");
						Console.WriteLine($" {message}");

						if (message == survey.RoomCode)
						{
							//prepare message and destination
							byte[] sendbuf = Encoding.ASCII.GetBytes("connection available");
							IPEndPoint ep = new IPEndPoint(groupEP.Address, Const.Network.ClientUDPClientPort);

							//reply
							s.SendTo(sendbuf, ep);
						};
					}
				}
				catch (SocketException)
				{
					//closing the socket is the only way to terminate this in a sensible way
					Console.WriteLine("Broadcast listening stopped due to closed socket or an error occured whilst replying");
				}
				finally
				{
					listener.Close();
				}
			});

			while (true)
			{
				//please microsoft make udpclient.receiveasync cancellable
				
				if (token.IsCancellationRequested) {
					token.ThrowIfCancellationRequested();
				}
			}
		}

		private void AcceptClientAsync() {

		}
	}
}
