using System;
using System.Threading;
using CTS.Instance.Gameplay;
using CTS.Instance.Networks;
using CTS.Instance.Services;
using log4net;

namespace CTS.Instance
{
	public class GameplayServer : TickRunner
	{
		private readonly ServerOption _serverOption;
		private readonly NetworkService _networkService;
		private readonly GameInstanceManager _gameInstanceManager;

		public GameplayServer(ServerOption serverOption,
							  NetworkService networkService,
							  TickTimer tickTimer)
			: base(serverOption.FramePerMs, tickTimer, 
				   LogManager.GetLogger(typeof(GameplayServer)))
		{
			_serverOption = serverOption;
			_networkService = networkService;
			_gameInstanceManager = new(serverOption, tickTimer);
		}

		public void Start()
		{
			_networkService.Start(_serverOption.Port);

			Thread t = new Thread(Run);
			t.IsBackground = false;
			t.Start();
		}

		protected override void OnUpdate(float deltaTime)
		{
			_networkService.PollEvents();
			_gameInstanceManager.ProcessFrame(deltaTime);
			_networkService.PollEvents();
		}

		public void TryToJoinGame()
		{

		}

		public void PrintCollecitonCount()
		{
			_log.Info($"[Gen 0 : {GC.CollectionCount(0).ToString()}][Gen 1 : {GC.CollectionCount(1).ToString()}][Gen 2 : {GC.CollectionCount(2)}]");
		}
	}
}