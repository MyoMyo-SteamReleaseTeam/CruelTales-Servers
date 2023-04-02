using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace CTS.Instance
{
	public class ServerOption
	{
		public int MaxConcurrentUser { get; set; }
		public int Port { get; set; } = 60128;
		public int FramePerMs = 50;
		public int GameCount = 700;
		public int UpdateStress = 4000;
		public int UserCount = 7;
		public int ReadSize = 500;
		public int WriteSize = 1500;
	}

	public class GameplayServer
	{
		private ServerService _serverService;
		private ServerOption _serverOption;
		public int MaxConcurrentUser => _serverOption.MaxConcurrentUser;

		public GameplayServer(ServerService serverService, ServerOption serverOption)
		{
			_serverService = serverService;
			_serverOption = serverOption;
		}

		public void Start()
		{
			EventBasedNetListener listener = new EventBasedNetListener();
			NetManager server = new NetManager(listener);
			server.Start(_serverOption.Port /* port */);

			listener.ConnectionRequestEvent += request =>
			{
				if (server.GetPeersCount(ConnectionState.Connected) < 5 /* max connections */)
					request.AcceptIfKey("CTMatchmakingTestServer");
				else
					request.Reject();
			};

			listener.PeerConnectedEvent += peer =>
			{
				Console.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
				NetDataWriter writer = new NetDataWriter();                 // Create writer class
				writer.Put("Hello client!");                                // Put some string
				peer.Send(writer, DeliveryMethod.ReliableOrdered);             // Send with reliability
			};


			while (!Console.KeyAvailable)
			{
				server.PollEvents();
			}
			server.Stop();
		}

		public void StartMainNetwork()
		{

		}

	}
}