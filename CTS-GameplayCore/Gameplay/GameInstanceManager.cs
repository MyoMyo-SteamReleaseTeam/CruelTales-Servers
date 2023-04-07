using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CT.Network.DataType;
using CTS.Instance.Utils;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameInstanceManager
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(GameInstanceManager));

		// Server setup
		private readonly TickTimer _serverTimer;
		private readonly ServerOption _serverOption;

		// Game Instance
		public int MaxGameCount { get; private set; }

		// GUID는 바뀔 수 있음
		private readonly Dictionary<GameInstanceGuid, GameInstance> _gameInstanceById = new();
		// 생성되고 변하지 않음
		private readonly List<GameInstance> _gameInstanceList = new();

		private object _instanceManagerlock = new object();

		// TickRunner
		private TickRunner _gameplayTickRunner;

		private Random _random = new Random();

		public GameInstanceManager(ServerOption serverOption,
								   TickTimer tickTimer)
		{
			_serverOption = serverOption;
			_serverTimer = tickTimer;
			MaxGameCount = serverOption.GameCount;

			for (int i = 0; i < MaxGameCount; i++)
			{
				var instance = new GameInstance();
				instance.Initialize(new GameInstanceGuid((ulong)_random.NextInt64()),
									new GameInstanceOption() { MaxMember = 7 });
				_gameInstanceList.Add(instance);
				_gameInstanceById.Add(instance.Guid, instance);
			}

			_gameplayTickRunner = new TickRunner(_serverOption.FramePerMs, _serverTimer, _log);
			_gameplayTickRunner.OnUpdate += Update;
		}

		public void Start()
		{
			_log.Info($"Start GameInstanceManager");
			_gameplayTickRunner.Run();
		}

		public void Update(float deltaTime)
		{
			Parallel.ForEach(_gameInstanceList, (i) =>
			{
				i.Update(deltaTime);
			});
		}

		public bool TryGetGameInstanceBy(GameInstanceGuid instanceID,
										 [MaybeNullWhen(false)] out GameInstance instance)
		{
			lock (_instanceManagerlock)
			{
				return _gameInstanceById.TryGetValue(instanceID, out instance);
			}
		}

		public void ProcessFrame(float deltaTime)
		{
			int count = _gameInstanceList.Count;
			for (int i = 0; i < count; i++)
			{
				GameInstance game = _gameInstanceList[i];
				game.Update(deltaTime);
			}
		}
	}
}
