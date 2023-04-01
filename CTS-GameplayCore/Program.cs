using CT.Tools.GetOpt;

namespace CTS.Instance
{
	public class ServerOption
	{
		public int MaxConcurrentUser { get; set; }
		public int Port { get; set; }
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			string startOption = "-maxuser 500";

			ServerOption serverOption = new ServerOption();

			OptionParser optionParser = new OptionParser();
			optionParser.RegisterEvent("maxuser", 1, e => serverOption.MaxConcurrentUser = int.Parse(e[0]));
			optionParser.OnArguments(startOption);

			GameplayServer server = new(serverOption);
			server.Start();
		}
	}
}