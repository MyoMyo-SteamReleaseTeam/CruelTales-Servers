using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading;
using CT.Common.DataType;
using CT.Networks.Runtimes;
using log4net;

namespace CTC.Networks
{
	public struct DummyUserInfo
	{
		public int DummyClientPort;
		public UserId UserId;
		public UserToken UserToken;
		public GameInstanceGuid GameInstanceGuid;

		public override string ToString()
		{
			return $"[{nameof(UserId)}:{UserId}]" +
				$"[{nameof(UserToken)}:{UserToken}]" +
				$"[{nameof(GameInstanceGuid)}:{GameInstanceGuid}]";
		}
	}

	[SupportedOSPlatform("windows")]
	internal class MainProcess
	{
		// Log
		public static ILog _log = LogManager.GetLogger(typeof(MainProcess));

		// Server Endpoint
		public static readonly string ServerIp = "127.0.0.1";
		public static readonly int ServerPort = 60128;

		// Dummy client setup
		private static int _startCounter = 0;
		private static int _dummyClientBindPort = 40000;
		private static int _dummyCount = 1;
		private static int _joinRoomPerPlayer = 7;
		private static List<NetworkManager> _dummyClients = new();

		// Handle test process
		private static bool _shouldStop = false;

		static void Main(string[] args)
		{
			try { Console.SetWindowSize(160, 30); } catch { }

			//Console.WriteLine($"Start dummy test for press anykey...");
			//Console.ReadLine();

			for (int i = _startCounter; i >= 0; i--)
			{
				_log.Info($"Start dummy client test in {i}");
				Thread.Sleep(1000);
			}

			for (int i = 1; i <= _dummyCount; i++)
			{
				DummyUserInfo info = new DummyUserInfo()
				{
					DummyClientPort = _dummyClientBindPort + i,
					UserId = new UserId((ulong)i * 100),
					UserToken = new UserToken((ulong)(i * 10000)),
					GameInstanceGuid = new GameInstanceGuid((ulong)(((i - 1) / _joinRoomPerPlayer) + 1))
				};

				// Create dummy clinet and add to list
				var dummyClient = new NetworkManager(info);
				_dummyClients.Add(new NetworkManager(info));
			}

			_log.Info($"Create dummy clients success. Count : {_dummyCount}");

			for (int i = 0; i < _dummyClients.Count; i++)
			{
				var dummyClient = _dummyClients[i];

				try
				{
					int port = dummyClient.UserInfo.DummyClientPort;
					dummyClient.Start(port);
					dummyClient.TryConnect(ServerIp, ServerPort);
					_log.Info($"[Client Count {i}] Bind Port : {port} / Try connect {ServerIp}:{ServerPort}");

					// Pull events
					foreach (var client in _dummyClients)
					{
						try
						{
							client.Update(0);
						}
						catch (Exception e)
						{
							_log.Error($"Dummy client poll events error! : ", e);
						}
					}
				}
				catch (Exception e)
				{
					_log.Error($"Dummy client start error! : ", e);
				}
			}

			// Start tick thread
			TickTimer tickTimer = new TickTimer();
			TickRunner tickRunner = new TickRunner(16, tickTimer, _log);
			tickRunner.OnUpdate += ProcessTickByRunner;
			tickRunner.Run();
			//Thread tickThread = new Thread(ProcessTick);
			//tickThread.Start();
			_log.Info($"Start tick process...");

			// Stop test process
			while (Console.ReadLine() != "q");
			_log.Info($"Stop test process...");
			_shouldStop = true;
			tickRunner.Suspend();
			//tickThread.Join();

			// Disconnect all connections
			foreach (var c in _dummyClients)
			{
				c.Disconnect();
			}

			_log.Info($"Success to disconnect all connection");
		}

		public static void ProcessTickByRunner(float deltaTime)
		{
			foreach (var client in _dummyClients)
			{
				try
				{
					client?.Update(deltaTime);
				}
				catch (Exception e)
				{
					_log.Error($"Dummy client poll events error! : ", e);
				}
			}
		}

		public static void ProcessTick()
		{
			Stopwatch sw = new Stopwatch();

			while (!_shouldStop)
			{
				foreach (var client in _dummyClients)
				{
					try
					{
						client?.Update(0.016f);
					}
					catch (Exception e)
					{
						_log.Error($"Dummy client poll events error! : ", e);
					}
				}
				Thread.Sleep(1);
			}
		}
	}
}