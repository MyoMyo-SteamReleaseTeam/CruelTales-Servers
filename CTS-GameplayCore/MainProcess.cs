using System;
using System.Reflection;
using CT.Tools.GetOpt;
using log4net;

namespace CTS.Instance
{

	internal class MainProcess
	{
		///private static readonly ILog log = LogManager.GetLogger(typeof(MainProcess));
		private static readonly ILog log = LogManager.GetLogger(type: MethodBase.GetCurrentMethod()?.DeclaringType);

		static void Main(string[] args)
		{
			//Console.SetWindowPosition(0, 0);
			Console.SetWindowSize(200, 50);

			log.Info("Hello info");
			log.Error("Test error");
			log.Warn("Warn test est");
			log.Debug("Debug debug");
			log.Fatal("Fatal !!!!!");

			Console.Read();

			return;

			string startOption = "-maxuser 500";

			ServerOption serverOption = new ServerOption();

			OptionParser optionParser = new OptionParser();
			optionParser.RegisterEvent("maxuser", 1, e => serverOption.MaxConcurrentUser = int.Parse(e[0]));
			optionParser.OnArguments(startOption);

			ServerService serverService = new ServerService();

			GameplayServer server = new(serverService, serverOption);
			server.Start();
		}
	}
}