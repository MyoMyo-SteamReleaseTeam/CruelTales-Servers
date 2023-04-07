using System;
using System.Runtime.Versioning;
using System.Threading;
using LiteNetLib;
using log4net;
using static System.Collections.Specialized.BitVector32;

namespace CTC.DummyClient
{
	[SupportedOSPlatform("windows")]
	internal class MainProcess
	{
		public static readonly string ServerIp = "127.0.0.1";
		public static readonly int ServerPort = 60128;
		public static ILog _log = LogManager.GetLogger(typeof(MainProcess));

		private static int _dummyClientBindPort = 40000;

		private static object _lock = new object();

		static void Main(string[] args)
		{
			Console.SetWindowSize(160, 25);

			for (int i = 5; i >= 0; i--)
			{
				_log.Info($"Start dummy client test in {i}");
				Thread.Sleep(1000);
			}

			for (int i = 0; i < 1000; i++)
			{
				int port = Interlocked.Increment(ref _dummyClientBindPort);
				Thread t = new Thread(() => Start(port));
				t.IsBackground = false;
				t.Start();
			}
		}

		public static void Start(int port)
		{
			DummyClientNetworkManager session = new();

			try
			{
				_log.Info($"Bind Port : {port} / Try connect {ServerIp}:{ServerPort}");

				lock (_lock)
				{
					session.Start(port);
					session.TryConnect(ServerIp, ServerPort);
				}
			}
			catch (Exception e)
			{
				_log.Error($"Dummy client start error! : ", e);
				return;
			}

			while (true)
			{
				try
				{
					session.PollEvents();
				}
				catch (Exception e)
				{
					_log.Error($"Dummy client poll events error! : ", e);
				}
				Thread.Sleep(50);
			}
		}
	}
}