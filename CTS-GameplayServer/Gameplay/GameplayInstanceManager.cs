﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CT.Common.DataType;
using CT.Networks.Runtimes;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameplayInstanceManager
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(GameplayInstanceManager));

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

		public GameplayInstanceManager(ServerOption serverOption,
								   TickTimer tickTimer)
		{
			_serverOption = serverOption;
			_serverTimer = tickTimer;
			MaxGameCount = serverOption.GameCount;

			var option = new InstanceInitializeOption() { SystemMaxUser = 7 };
			var gameplayOption = new GameplayOption();
			gameplayOption.MaxUser = 7;

			for (int i = 1; i <= MaxGameCount; i++)
			{
				var instance = new GameplayInstance(_serverTimer, option);
				instance.Initialize(gameplayOption, new GameInstanceGuid((ulong)i));
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

		private long _current = 0;
		private static float _globalDeltaTime;
		public void Update(float deltaTime)
		{
			// Check tick
			_current = _serverTimer.CurrentMs;

			// Process
			_globalDeltaTime = deltaTime;
			Parallel.ForEach(_gameInstanceList, processUpdate);

			// Alarm high CPU load
			long tickElapsed = _serverTimer.CurrentMs - _current;
			if (tickElapsed > _serverOption.AlarmTickMs)
			{
				_log.Fatal($"Current tick elapsed : {tickElapsed}");
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
