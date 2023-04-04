using System;
using System.Runtime.Versioning;
using CT.Tools.GetOpt;
using CTS.Instance.Networks;
using CTS.Instance.Services;
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
	internal class MainProcess
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MainProcess));

		static void Main(string[] args)
		{
			Console.SetWindowSize(200, 50);

			// Create services
			_log.Info("Create services");

			ServerOption serverOption = new ServerOption();
			TickTimer tickTimer = new TickTimer();
			NetworkService networkService = new(serverOption, tickTimer);

			// Read process option
			_log.Info("Read process options");

			//string startOption = "-maxuser 500";
			//OptionParser optionParser = new OptionParser();
			//optionParser.RegisterEvent("maxuser", 1, e => serverOption.MaxConcurrentUser = int.Parse(e[0]));
			//optionParser.OnArguments(startOption);

			// Start gameplay server
			_log.Info("Start gameplay server");
			_log.Info($"Server Port\t: {serverOption.Port}");
			_log.Info($"Tick ms\t: {serverOption.FramePerMs}");
			_log.Info($"Max Game Count\t: {serverOption.GameCount}");

			GameplayServer server = new(serverOption, networkService, tickTimer);
			server.Start();

			_log.Info($"Start game server!");
		}
	}
}