using System.Collections.Generic;
using CTS.Instance.Services;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameInstanceManager
	{
		private static ILog _log = LogManager.GetLogger(typeof(GameInstanceManager));

		public int MaxGameCount { get; private set; }
		private readonly Dictionary<int, GameInstance> _gameInstanceById = new();
		private readonly List<GameInstance> _gameInstanceList = new();

		private readonly TickTimer _serverTimer;
		private readonly ServerOption _serverOption;

		public GameInstanceManager(ServerOption serverOption,
								   TickTimer tickTimer)
		{
			_serverOption = serverOption;
			_serverTimer = tickTimer;
			MaxGameCount = serverOption.GameCount;
		}

		public void Start()
		{
			for (int i = 0; i < MaxGameCount; i++)
			{
				var instance = new GameInstance(_serverOption);
				_gameInstanceList.Add(instance);
				_gameInstanceById.Add(instance.Id, instance);
			}
		}

		public void ProcessFrame(float deltaTime)
		{
			int count = _gameInstanceList.Count;
			for (int i = 0; i < count; i++)
			{
				GameInstance game = _gameInstanceList[i];
				game.DeserializePackets();
				game.Update(deltaTime);
				game.SerializePackets();
			}
		}

		public bool TryGetGameInstanceBy(int instanceID, out GameInstance? instance)
		{
			return _gameInstanceById.TryGetValue(instanceID, out instance);
		}
	}
}
