using System;
using System.Runtime.Versioning;
using CT.Tools.GetOpt;
using log4net;

namespace CTS.Instance
{
	public class ServerOption
	{
		public int MaxConcurrentUser { get; set; }
		public int Port { get; set; } = 60128;
		public int FramePerMs = 50;
		public int GameCount = 700;
		//public int UpdateStress = 4000;
		//public int UserCount = 7;
		//public int ReadSize = 500;
		//public int WriteSize = 1500;
	}

	[SupportedOSPlatform("windows")]
	internal class MainProcess
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(MainProcess));

		static void Main(string[] args)
		{
			Console.SetWindowSize(200, 50);

			// Set server option
			string startOption = "-maxuser 500";

			ServerOption serverOption = new ServerOption();

			// Read process option
			OptionParser optionParser = new OptionParser();
			optionParser.RegisterEvent("maxuser", 1, e => serverOption.MaxConcurrentUser = int.Parse(e[0]));
			optionParser.OnArguments(startOption);

			// Initialize server services
			ServerServices serverService = new ServerServices(serverOption);

			// Create server
			GameplayServer server = new(serverService, serverOption);
		}
	}
}