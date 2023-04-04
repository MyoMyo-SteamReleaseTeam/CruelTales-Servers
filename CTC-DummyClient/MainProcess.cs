using System;
using System.Net;
using System.Threading;
using LiteNetLib;
using log4net;

namespace CTC.DummyClient
{
	public class DummyClientNetworkManager
	{
		private static ILog _log = LogManager.GetLogger(typeof(DummyClientNetworkManager));

		private EventBasedNetListener _listener;
		private NetManager _netManager;

		public DummyClientNetworkManager()
		{
			_listener = new EventBasedNetListener();
			_netManager = new NetManager(_listener);

			_listener.NetworkReceiveEvent += OnReceived;
			_listener.PeerConnectedEvent += OnConnected;
			_listener.PeerDisconnectedEvent += OnDisconnected;
		}

		public void Start()
		{
			_netManager.Start();
		}

		public void TryConnect(string address, int port)
		{
			_netManager.Connect(address, port, "TestServer");
		}

		private void OnConnected(NetPeer peer)
		{
			_log.Info($"Success to connect to the server. Server endpoint : {peer.EndPoint}");
		}

		private void OnDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			_log.Warn($"Disconnected from the server. Disconnect reason : {disconnectInfo.Reason}");
		}

		private void OnReceived(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			_log.Info($"Packet received. Size : {reader.UserDataSize}");
		}

		public void PollEvents()
		{
			_netManager.PollEvents();
		}
	}

	internal class MainProcess
	{
		public static readonly string ServerIp = "127.0.0.1";
		public static readonly int ServerPort = 60128;
		public static ILog _log = LogManager.GetLogger(typeof(MainProcess));

		static void Main(string[] args)
		{
			Thread t = new Thread(Start);
			t.IsBackground = false;
			t.Start();
		}

		public static void Start()
		{
			_log.Info("Start Dummy Client");
			DummyClientNetworkManager session = new DummyClientNetworkManager();
			session.Start();
			session.TryConnect(ServerIp, ServerPort);
			_log.Info($"Try connect {ServerIp}:{ServerPort}");

			while (true)
			{
				session.PollEvents();
				Thread.Sleep(16);
			}
		}

		public static void TestLogic()
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
				Thread.Sleep(1);
			}

			client.Stop();
		}


	}
}