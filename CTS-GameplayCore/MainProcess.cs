using CT.Tools.GetOpt;

namespace CTS.Instance
{

	internal class MainProcess
	{
		static void Main(string[] args)
		{
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