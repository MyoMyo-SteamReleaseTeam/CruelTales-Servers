using CTS.Instance.Networks;
using CTS.Instance.Utils;
using log4net;

namespace CTS.Instance
{
	public class GameplayServer
	{
		private static ILog _log = LogManager.GetLogger(typeof(GameplayServer));

		// Global Setup
		private readonly TickTimer _serverTimer;
		private readonly ServerOption _serverOption;

		// Network
		private NetworkManager _networkManager;

		public GameplayServer(TickTimer serverTimer,
							  ServerOption serverOption)
		{
			_serverTimer = serverTimer;
			_serverOption = serverOption;

			_networkManager = new NetworkManager(_serverOption, _serverTimer);
		}

		public void Start()
		{
			_networkManager.Start();
		}
	}
}
