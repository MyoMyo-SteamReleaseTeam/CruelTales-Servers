using System;
using System.Net;
using System.Threading;
using LiteNetLib;
using log4net;

namespace CTC.DummyClient
{
	internal class MainProcess
	{
		public static readonly string ServerIp = "127.0.0.1";
		public static readonly int ServerPort = 60128;
		public static ILog _log = LogManager.GetLogger(typeof(MainProcess));

		private static int _dummyClientBindPort = 40000;

		static void Main(string[] args)
		{
			for (int i = 5; i >= 0; i--)
			{
				_log.Info($"Start dummy client test in {i}");
				Thread.Sleep(1000);
			}

			for (int i = 0; i < 100; i++)
			{
				Thread t = new Thread(Start);
				t.IsBackground = false;
				t.Start();
				Thread.Sleep(1);
			}
		}

		public static void Start()
		{
			DummyClientNetworkManager session = new DummyClientNetworkManager();
			Interlocked.Increment(ref _dummyClientBindPort);
			session.Start(_dummyClientBindPort);
			session.TryConnect(ServerIp, ServerPort);
			_log.Info($"Bind Port : {_dummyClientBindPort} / Try connect {ServerIp}:{ServerPort}");

			while (true)
			{
				session.PollEvents();
				Thread.Sleep(200);
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