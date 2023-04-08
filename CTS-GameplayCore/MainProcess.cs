using System;
using System.Runtime.Versioning;
using CT.Network.Runtimes;
using log4net;

namespace CTS.Instance
{
	public class ServerOption
	{
		public int Port { get; set; } = 60128;
		public int FramePerMs = 66;
		public int GameCount = 700;
	}

	[SupportedOSPlatform("windows")]
	public class MainProcess
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MainProcess));

		static void Main(string[] args)
		{
			Console.SetWindowSize(200, 50);

			// Start server
			_log.Info("Start gameplay server");

			TickTimer serverTimer = new TickTimer();

			// Setup server option
			ServerOption serverOption = new ServerOption();
			_log.Info($"Server Port\t: {serverOption.Port}");
			_log.Info($"Tick ms\t: {serverOption.FramePerMs}");
			_log.Info($"Max Game Count\t: {serverOption.GameCount}");

			GameplayServer gameplayServer = new(serverTimer, serverOption);
			gameplayServer.Start();
		}
	}
}
