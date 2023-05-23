using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using CT.Common.Tools.Data;
using CT.Networks.Runtimes;
using log4net;

namespace CTS.Instance
{
	public struct ServerOption
	{
		public int Port { get; set; } = 60128;
		public int FramePerMs = 66;
		public int AlarmTickMs = 40;
		public int GameCount = 1;

		public ServerOption() {}
	}

	[SupportedOSPlatform("windows")]
	public class MainProcess
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MainProcess));

		private static readonly string ConfigurationFile = "server-config.json";

		static void Main(string[] args)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				try { Console.SetWindowSize(200, 50); } catch { }
			}

			ServerOption serverOption = new ServerOption();

#if !DEBUG
			var configRead = JsonHandler.TryReadObject<ServerOption>(ConfigurationFile);
			if (configRead.ResultType != JobResultType.Success)
			{
				_log.Warn($"There is no configuration file!");
				var createDefault = JsonHandler.TryWriteObject(ConfigurationFile, serverOption, true);
				if (createDefault.ResultType != JobResultType.Success)
				{
					_log.Fatal($"Failed to create default configuration file!");
					_log.Fatal(createDefault.Exception);
				}
				else
				{
					_log.Info($"Create default configuration file!");
				}
			}
			else
			{
				_log.Info("Load server configuration file.");
				serverOption = configRead.Value;
			}
#endif

			// Start server
			_log.Info("Start gameplay server");

			TickTimer serverTimer = new TickTimer();

			// Setup server option
			serverOption.AlarmTickMs = (int)(serverOption.FramePerMs * 0.7f);

			_log.Info($"Server Port\t: {serverOption.Port}");
			_log.Info($"Frame per ms\t: {serverOption.FramePerMs}");
			_log.Info($"Alarm Tick ms\t: {serverOption.AlarmTickMs}");
			_log.Info($"Max Game Count\t: {serverOption.GameCount}");

			GameplayServer gameplayServer = new(serverTimer, serverOption);

			gameplayServer.Start();

			while (true)
			{
				Thread.Sleep(10000);
			}
		}
	}
}
