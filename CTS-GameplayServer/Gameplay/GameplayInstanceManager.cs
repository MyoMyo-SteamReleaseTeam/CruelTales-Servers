//#define MULTITHREADING

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#if MULTITHREADING
using System.Threading.Tasks;
#endif
using CT.Common.DataType;
using CT.Networks.Runtimes;
using CTS.Instance.Networks;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameplayInstanceManager
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(GameplayInstanceManager));

		// Reference
		private NetworkManager _networkManager;

		// Server setup
		private readonly TickTimer _serverTimer;
		private readonly ServerOption _serverOption;

		// Game Instance
		public int MaxGameCount { get; private set; }

		private readonly Dictionary<GameInstanceGuid, GameplayInstance> _gameInstanceById = new();
		private readonly List<GameplayInstance> _gameInstanceList = new();

		private object _instanceManagerlock = new object();

		// TickRunner
		private TickRunner _gameplayTickRunner;

		private Random _random = new Random();

		public GameplayInstanceManager(NetworkManager networkManager,
									   ServerOption serverOption,
									   TickTimer tickTimer)
		{
			_networkManager = networkManager;
			_serverOption = serverOption;
			_serverTimer = tickTimer;
			MaxGameCount = serverOption.GameCount;

			var option = new InstanceInitializeOption() { SystemMaxUser = 7 };

			for (int i = 1; i <= MaxGameCount; i++)
			{
				var instance = new GameplayInstance(_serverOption, _serverTimer, option);
				instance.Initialize(new GameInstanceGuid((ulong)i));
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

		private long _currentTick = 0;
		private long _current = 0;
		private long _aveElapsed = 0;
		private static float _globalDeltaTime;

		private int[] _gcCollectionCount = new int[3];
		public void Update(float deltaTime)
		{
			// Check tick
			_current = _serverTimer.CurrentMs;

			// Process
			_globalDeltaTime = deltaTime;

#if MULTITHREADING
			try
			{
				Parallel.ForEach(_gameInstanceList, processUpdate);
			}
			catch (Exception e)
			{
				_log.Fatal(e);
			}
#else
			foreach (var instance in _gameInstanceList)
			{
				instance.Update(deltaTime);
			}
#endif
			// Alarm high CPU load
			_currentTick++;
			long tickElapsed = _serverTimer.CurrentMs - _current;
			_aveElapsed = (_aveElapsed * 7 + tickElapsed) / 8;

			if (tickElapsed > _serverOption.AlarmTickMs)
			{
				_log.Fatal($"Current tick elapsed : {tickElapsed}");
			}
			else
			{
				//if (_currentTick % 100  == 0)
				//{
				//	_log.Info($"Packet pool size : {_networkManager.NetManager.PoolCount}");
				//	_log.Info($"Average tick elapsed : {_aveElapsed}");
				//}
			}

			// Check GC Collection
			bool hasGcCall = false;
			for (int i = 0; i <= 2; i++)
			{
				int collection = GC.CollectionCount(i);
				if (_gcCollectionCount[i] != collection)
				{
					hasGcCall = true;
					_gcCollectionCount[i] = collection;
				}
			}

			if (hasGcCall)
			{
				_log.Warn($"GC Collect [{_gcCollectionCount[0]}\t][{_gcCollectionCount[1]}\t][{_gcCollectionCount[2]}\t]");
			}
		}

		private static void processUpdate(GameplayInstance gameplayInstance)
		{
			gameplayInstance.Update(_globalDeltaTime);
		}

		public bool TryGetGameInstanceBy(GameInstanceGuid instanceID,
										 [MaybeNullWhen(false)] out GameplayInstance instance)
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
				GameplayInstance game = _gameInstanceList[i];
				game.Update(deltaTime);
			}
		}
	}
}
