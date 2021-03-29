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
		private Survey survey = null;
		
		public SurveyClient() {
			
		}
		
		//async look for host returns a task which when completed indicates whether connection to the host was built.
		public async Task<bool> LookForHost(string RoomCode) {

			try
			{
				byte[] message = Encoding.ASCII.GetBytes(RoomCode);

				Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				UdpClient listener = new UdpClient(Const.Network.ClientUDPClientPort);
				Task<UdpReceiveResult> reply = listener.ReceiveAsync();

				for (int i = 0; i < 3; i++)
				{	
					//broadcast and wait
					s.SendTo(message, new IPEndPoint(IPAddress.Parse("192.168.1.255"), Const.Network.ServerUDPClientPort));
					await Task.Delay(1000);

					//did we get a reply?
					Console.WriteLine($"Reply Status: {reply.Status}");
					if (reply.Status == TaskStatus.RanToCompletion)
					{
						Console.WriteLine($"Received reply to broadcast from: {reply.Result.RemoteEndPoint}");
						Console.WriteLine($"Message: {Encoding.ASCII.GetString(reply.Result.Buffer, 0, reply.Result.Buffer.Length)}");

						try
						{	
							//attempt to connect
							client = new TcpClient();
							client.Connect(new IPEndPoint(reply.Result.RemoteEndPoint.Address, Const.Network.ServerTCPListenerPort));							

							//receive survey as json string
							NetworkStream ns = client.GetStream();
							byte[] buffer = new byte[2048];
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
	}
}
