namespace CTS.Instance
{
	public class GameplayServer
	{
		private readonly ServerServices _serverService;
		private readonly ServerOption _serverOption;
		private readonly NetworkService _networkService;

		public int MaxConcurrentUser => _serverOption.MaxConcurrentUser;

		public GameplayServer(ServerServices serverService, ServerOption serverOption)
		{
			_serverService = serverService;
			_serverOption = serverOption;
			_networkService = serverService.NetworkService;
		}

		public void RunServer()
		{

		}
	}
}