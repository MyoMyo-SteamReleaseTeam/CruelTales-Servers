using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
#if !DEBUG
using CT.Common.Tools.Data;
#endif
using CT.Networks.Runtimes;
using CTS.Instance.Networks;
using KaNet.Physics;
using log4net;

namespace CTS.Instance
{
	[SupportedOSPlatform("windows")]
	public class GameplayServer
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(GameplayServer));

		// Option
		private static readonly string ConfigurationFile = "server-config.json";
		private static ServerOption? _serverOption;

		// Server components
		private static TickTimer? _serverTimer;
		private static NetworkManager? _networkManager;

		static void Main(string[] args)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				try { Console.SetWindowSize(200, 50); } catch { }
			}

			_serverOption = new ServerOption();

#if !DEBUG
			var configRead = JsonHandler.TryReadObject<ServerOption>(ConfigurationFile);
			if (configRead.ResultType != JobResultType.Success)
			{
				_log.Warn($"There is no configuration file!");
				var createDefault = JsonHandler.TryWriteObject(ConfigurationFile, _serverOption, true);
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
				_serverOption = configRead.Value;
			}
#endif

			_log.Info("Start gameplay server");

			// Setup server option
			_serverOption.AlarmTickMs = (int)(_serverOption.FramePerMs * 0.7f);

			_log.Info($"Server Port\t: {_serverOption.Port}");
			_log.Info($"Frame per ms\t: {_serverOption.FramePerMs}");
			_log.Info($"Alarm Tick ms\t: {_serverOption.AlarmTickMs}");
			_log.Info($"Max Game Count\t: {_serverOption.GameCount}");

			// Initialize
			LayerMaskHelper.Initialize();

			// Start server
			_serverTimer = new TickTimer();
			_networkManager = new NetworkManager(_serverOption, _serverTimer);
			_networkManager.Start();

			while (true)
			{
				Thread.Sleep(10000);
			}
		}
	}
}
