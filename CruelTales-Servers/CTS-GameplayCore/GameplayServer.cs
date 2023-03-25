using System.Threading;
using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace CTS.Gameplay
{
	internal class GameplayServer
	{
		private ServerOption _serverOption;
		public int MaxConcurrentUser => _serverOption.MaxConcurrentUser;

		public GameplayServer(ServerOption serverOption)
		{
			_serverOption = serverOption;
		}

		public void Start()
		{
			EventBasedNetListener listener = new EventBasedNetListener();
			NetManager server = new NetManager(listener);
			server.Start(9050 /* port */);

			listener.ConnectionRequestEvent += request =>
			{
				if (server.GetPeersCount(ConnectionState.Connected) < 5 /* max connections */)
					request.AcceptIfKey("SomeConnectionKey");
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
				Thread.Sleep(15);
			}
			server.Stop();
		}
	}
}