namespace CTS.Gameplay
{
	internal class GameplayServer
	{
		private ServerOption _serverOption;
		public int MaxConcurrentUser => _serverOption.MaxConcurrentUser;

		public GameplayServer(ServerOption serverOption)
		{
			_serverOption = serverOption;
		}

		public void Start()
		{

		}
	}
}