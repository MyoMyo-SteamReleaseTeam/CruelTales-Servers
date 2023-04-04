using System;
using System.Net;
using System.Threading;
using LiteNetLib;

namespace CTC.DummyClient
{
	public class DummySession
	{
		private EventBasedNetListener _listener;
		private NetManager _netManager;

		public DummySession()
		{
			_listener = new EventBasedNetListener();
			_netManager = new NetManager(_listener);

			_listener.NetworkReceiveEvent += _listener_NetworkReceiveEvent;
		}

		private void _listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			throw new NotImplementedException();
		}

		public void Start(IPEndPoint endpoint, int port)
		{
		}
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			EventBasedNetListener listener = new EventBasedNetListener();
			NetManager client = new NetManager(listener);
			client.Start();
			client.Connect("localhost" /* host ip or name */, 60128 /* port */, "CTMatchmakingTestServer" /* text key or NetDataWriter */);
			listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
			{
				Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
				dataReader.Recycle();
			};

			while (!Console.KeyAvailable)
			{
				client.PollEvents();
				Thread.Sleep(15);
			}

			client.Stop();
		}


	}
}