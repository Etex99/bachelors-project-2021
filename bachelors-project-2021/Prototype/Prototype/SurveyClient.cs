using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Prototype
{
	public class SurveyClient
	{
		private TcpClient client = null;
		public Survey survey { get; private set; } = null;
		public SurveyData summary { get; private set; } = null;

		public Dictionary<int, IList<string>> voteCandidates1 { get; private set; } = null;
		public int vote1Time = 0;
		public List<string> voteCandidates2 { get; private set; } = null;
		public int vote2Time = 0;
		public string voteResult = null;
		public SurveyClient() {
			
		}
		
		//async look for host returns a task which when completed indicates whether connection to the host was built.
		public async Task<bool> LookForHost(string RoomCode) {

			try
			{
				byte[] message = Encoding.ASCII.GetBytes(RoomCode);

				Socket sendOut = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				sendOut.EnableBroadcast = true;

				UdpClient listener = new UdpClient() { EnableBroadcast = true };
				listener.Client.Bind(new IPEndPoint(IPAddress.Any, Const.Network.ClientUDPClientPort));
				
				Task<UdpReceiveResult> reply = listener.ReceiveAsync();

				for (int i = 0; i < 3; i++)
				{	
					//broadcast and wait
					int bytesSent = sendOut.SendTo(message, new IPEndPoint(IPAddress.Broadcast, Const.Network.ServerUDPClientPort));
					Console.WriteLine($"Bytes sent: {bytesSent}");
					await Task.Delay(1000);

					//did we get a reply?
					Console.WriteLine($"Reply Status: {reply.Status}");
					if (reply.Status == TaskStatus.RanToCompletion)
					{
						Console.WriteLine($"Received reply to broadcast from: {reply.Result.RemoteEndPoint}");
						string replyMessage = Encoding.ASCII.GetString(reply.Result.Buffer, 0, reply.Result.Buffer.Length);
						Console.WriteLine($"Message: {replyMessage}");

						try
						{	
							//attempt to connect
							client = new TcpClient();
							client.Connect(new IPEndPoint(reply.Result.RemoteEndPoint.Address, Const.Network.ServerTCPListenerPort));							

							//receive survey as json string
							NetworkStream ns = client.GetStream();
							byte[] buffer = new byte[4096];
							Task<int> bytesRead = ns.ReadAsync(buffer, 0, buffer.Length);
							await bytesRead;
							Console.WriteLine($"Bytes read: {bytesRead.Result}");
							survey = JsonConvert.DeserializeObject<Survey>(Encoding.ASCII.GetString(buffer, 0 ,bytesRead.Result));
							Console.WriteLine("Received survey successfully!");
							Console.WriteLine(survey.ToString());

							//if no error occurs return success
							return true;

						}
						catch (JsonException e) {
							Console.WriteLine("Received bad Json");
							Console.WriteLine(e);
						}
						catch (ObjectDisposedException e)
						{
							Console.WriteLine("Host abruptly closed connection, most likely");
							Console.WriteLine(e);
						} 
						finally 
						{
							//received garbage, lets try that again.
							reply = listener.ReceiveAsync();
						}
					}
				}

				listener.Close();

			}
			catch (SocketException e)
			{
				Console.WriteLine("Socket exception occured in LookForHost");
				Console.WriteLine(e);
			}

			return false;
		}
	
		//Async send result returning success of operation
		public async Task<bool> SendResult(string emojiID) {

			try
			{
				//prepare message
				byte[] bytes = Encoding.ASCII.GetBytes(emojiID);

				//send
				NetworkStream ns = client.GetStream();
				await ns.WriteAsync(bytes, 0, bytes.Length);

				//no error, returning success
				return true;
			}
			catch (ObjectDisposedException e)
			{
				Console.WriteLine("Host abruptly closed connection, most likely");
				Console.WriteLine(e);
			}

			return false;
		}

		public async Task<bool> SendVote1Result(Dictionary<int, string> answer)
		{

			try
			{
				//prepare message
				byte[] bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(answer));

				//send
				NetworkStream ns = client.GetStream();
				await ns.WriteAsync(bytes, 0, bytes.Length);

				//no error, returning success
				return true;
			}
			catch (ObjectDisposedException e)
			{
				Console.WriteLine("Host abruptly closed connection, most likely");
				Console.WriteLine(e);
			}

			return false;
		}

		public async Task<bool> SendVote2Result(string answer)
		{

			try
			{
				//prepare message
				byte[] bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(answer));

				//send
				NetworkStream ns = client.GetStream();
				await ns.WriteAsync(bytes, 0, bytes.Length);

				//no error, returning success
				return true;
			}
			catch (ObjectDisposedException e)
			{
				Console.WriteLine("Host abruptly closed connection, most likely");
				Console.WriteLine(e);
			}

			return false;
		}

		public async Task<bool> ReceiveSurveyDataAsync() {

			try
			{
				NetworkStream ns = client.GetStream();
				byte[] readBuffer = new byte[4096];
				int bytesRead = await ns.ReadAsync(readBuffer, 0, readBuffer.Length);
				summary = JsonConvert.DeserializeObject<SurveyData>(Encoding.ASCII.GetString(readBuffer, 0, bytesRead));

				Console.WriteLine($"Received summary: {summary}");
				return true;
			}
			catch (JsonException e)
			{
				Console.WriteLine("Received bad Json");
				Console.WriteLine(e);
			}
			catch (ObjectDisposedException e)
			{
				Console.WriteLine($"Connection closed or lost to server at: {client.Client.RemoteEndPoint}");
				Console.WriteLine(e);
			}

			return false;
		}
		public async Task<bool> ReceiveVote1Candidates()  {

			try
			{
				NetworkStream ns = client.GetStream();
				byte[] readBuffer = new byte[128];
				Console.WriteLine("Waiting for activity vote");
				int bytesRead = await ns.ReadAsync(readBuffer, 0, readBuffer.Length);
				Console.WriteLine($"Bytes read: {bytesRead}");

				//expecting JSON string containing Dictionary<int, IList<string>>
				voteCandidates1 = JsonConvert.DeserializeObject<Dictionary<int, IList<string>>>(Encoding.ASCII.GetString(readBuffer, 0, bytesRead));
				Console.WriteLine("Received vote 1 candidates");

				//next, receive vote time
				readBuffer = new byte[128];
				Console.WriteLine("Waiting for vote 1 timer");
				bytesRead = await ns.ReadAsync(readBuffer, 0, readBuffer.Length);
				Console.WriteLine($"Bytes read: {bytesRead}");

				//expecting string containing int
				vote1Time = int.Parse(Encoding.ASCII.GetString(readBuffer, 0, bytesRead));
				return true;
			}
			catch (JsonException e) 
			{
				Console.WriteLine("Received bad JSON");
				Console.WriteLine(e);
			}
			catch (ObjectDisposedException e)
			{
				Console.WriteLine($"Connection closed or lost to server at: {client.Client.RemoteEndPoint}");
				Console.WriteLine(e);
			}
			catch (FormatException e)
			{
				Console.WriteLine("Received bad int");
				Console.WriteLine(e);
			}

			return false;

		}
		public async Task<bool> ReceiveVote2Candidates()
		{

			try
			{
				NetworkStream ns = client.GetStream();
				byte[] readBuffer = new byte[128];
				Console.WriteLine("Waiting for activity vote 2");
				int bytesRead = await ns.ReadAsync(readBuffer, 0, readBuffer.Length);
				Console.WriteLine($"Bytes read: {bytesRead}");

				//expecting JSON string containing List<string>
				voteCandidates2 = JsonConvert.DeserializeObject<List<string>>(Encoding.ASCII.GetString(readBuffer, 0, bytesRead));
				Console.WriteLine("Received vote 2 candidates");

				//next, receive vote time
				readBuffer = new byte[128];
				Console.WriteLine("Waiting for vote 2 timer");
				bytesRead = await ns.ReadAsync(readBuffer, 0, readBuffer.Length);
				Console.WriteLine($"Bytes read: {bytesRead}");

				//expecting string containing int
				vote2Time = int.Parse(Encoding.ASCII.GetString(readBuffer, 0, bytesRead));
				return true;
			}
			catch (JsonException e)
			{
				Console.WriteLine("Received bad JSON");
				Console.WriteLine(e);
			}
			catch (ObjectDisposedException e)
			{
				Console.WriteLine($"Connection closed or lost to server at: {client.Client.RemoteEndPoint}");
				Console.WriteLine(e);
			}
			catch (FormatException e)
			{
				Console.WriteLine("Received bad int");
				Console.WriteLine(e);
			}

			return false;

		}

		public async Task<bool> ReceiveVoteResult()
		{

			try
			{
				NetworkStream ns = client.GetStream();
				byte[] readBuffer = new byte[128];
				Console.WriteLine("Waiting for vote result");
				int bytesRead = await ns.ReadAsync(readBuffer, 0, readBuffer.Length);
				Console.WriteLine($"Bytes read: {bytesRead}");

				//expecting string containing voteResult
				voteResult = Encoding.ASCII.GetString(readBuffer, 0, bytesRead);
				Console.WriteLine("Received vote result");

				return true;
			}
			catch (ObjectDisposedException e)
			{
				Console.WriteLine($"Connection closed or lost to server at: {client.Client.RemoteEndPoint}");
				Console.WriteLine(e);
			}

			return false;

		}

		//this is as sophisticated as it gets
		public void DestroyClient() {
			client = null;
		}
	}
}
