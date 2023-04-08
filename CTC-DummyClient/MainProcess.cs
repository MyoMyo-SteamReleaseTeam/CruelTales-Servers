using System;
using System.Runtime.Versioning;
using System.Threading;
using CT.Network.DataType;
using log4net;

namespace CTC.Networks
{
	public struct DummyUserInfo
	{
		public int DummyClientPort;
		public ClientId ClientId;
		public ClientToken ClientToken;
		public GameInstanceGuid GameInstanceGuid;

		public override string ToString()
		{
			return $"[{nameof(ClientId)}:{ClientId}]" +
				$"[{nameof(ClientToken)}:{ClientToken}]" +
				$"[{nameof(GameInstanceGuid)}:{GameInstanceGuid}]";
		}
	}

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

			for (int i = 2; i >= 0; i--)
			{
				_log.Info($"Start dummy client test in {i}");
				Thread.Sleep(1000);
			}

			for (int i = 1; i <= 10; i++)
			{
				DummyUserInfo info = new DummyUserInfo()
				{
					DummyClientPort = _dummyClientBindPort + i,
					ClientId = new ClientId((ulong)i * 100),
					ClientToken = new ClientToken((ulong)(i * 10000)),
					GameInstanceGuid = new GameInstanceGuid((ulong)(((i - 1) / 8) + 1))
				};

				//DummyUserInfo info = new DummyUserInfo()
				//{
				//	DummyClientPort = _dummyClientBindPort + i,
				//	ClientId = new ClientId(1),
				//	ClientToken = new ClientToken(2),
				//	GameInstanceGuid = new GameInstanceGuid(3)
				//};

				Thread t = new Thread(() => Start(info));
				t.IsBackground = false;
				t.Start();
			}
		}

		public static void Start(DummyUserInfo info)
		{
			DummyClientSession session = new(info);

			try
			{
				_log.Info($"Bind Port : {info.DummyClientPort} / Try connect {ServerIp}:{ServerPort}");

				lock (_lock)
				{
					session.Start(info.DummyClientPort);
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