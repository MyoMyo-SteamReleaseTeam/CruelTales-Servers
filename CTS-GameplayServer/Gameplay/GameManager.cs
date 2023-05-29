using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Tools;
using CT.Common.Tools.Collections;
using CTS.Instance.Networks;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameManager
	{
		// Log
		private readonly static ILog _log = LogManager.GetLogger(typeof(GameManager));

		// Reference
		private GameplayInstance _gameplayInstance;
		private WorldManager _worldManager;

		// Manage Players
		private BidirectionalMap<UserId, NetworkPlayer> _networkPlayerByUserId;
		private ObjectPool<NetworkPlayer> _networkPlayerPool;

		// Test
		private BidirectionalMap<NetworkPlayer, PlayerCharacter> _playerCharacterByPlayer;
		private InstanceInitializeOption _option;
		private List<TestCube> _testCubes = new(30);

		public GameManager(GameplayInstance gameplayInstance,
						   InstanceInitializeOption option)
		{
			// Reference
			_gameplayInstance = gameplayInstance;
			_worldManager = gameplayInstance.WorldManager;
			_option = option;

			// Manage Players
			_networkPlayerByUserId = new(option.SystemMaxUser);
			_networkPlayerPool = new(() => new NetworkPlayer(this, _worldManager, option),
									 option.SystemMaxUser);

			// Test
			_playerCharacterByPlayer = new(option.SystemMaxUser);
		}

		float timer = 0;

		public void Update(float deltaTime)
		{
			foreach (var player in _networkPlayerByUserId.ForwardValues)
			{
				player.Update(deltaTime);
			}

			timer += deltaTime;
			if (timer > 0.2f)
			{
				timer = 0;
				if (_testCubes.Count < 30)
				{
					float x = (float)(_random.NextDouble() - 0.5) * 20;
					float y = (float)(_random.NextDouble() - 0.5) * 20;
					var testCube = _worldManager.CreateObject<TestCube>(new Vector3(x, 0, y));
					testCube.BindPool(_testCubes);
					_testCubes.Add(testCube);
				}
			}
		}

		private static Random _random = new Random();

		public void StartGame()
		{
		}

		public void OnUserEnterGame(UserSession userSession)
		{
			var player = _networkPlayerPool.Get();
			player.OnCreated(userSession);
			_networkPlayerByUserId.Add(userSession.UserId, player);
			_worldManager.OnPlayerEnter(player);
			
			// Test
			var playerCharacter = _worldManager.CreateObject<PlayerCharacter>();
			_playerCharacterByPlayer.Add(player, playerCharacter);
		}

		public void OnUserLeaveGame(UserSession userSession)
		{
			if (!_networkPlayerByUserId.TryGetValue(userSession.UserId, out var player))
			{
				_log.Error($"[{_gameplayInstance}] There is no {userSession}'s network player!");
				return;
			}

			_worldManager.OnPlayerLeave(player);
			_networkPlayerByUserId.TryRemove(player);
			player.OnDestroyed();
			_log.Debug($"[{_gameplayInstance}] Session {userSession} leave the game");

			// Test
			if (_playerCharacterByPlayer.TryGetValue(player, out var character))
			{
				character.Destroy();
				_playerCharacterByPlayer.TryRemove(player);
			}
		}

		public bool TryGetNetworkPlayer(UserId user, [MaybeNullWhen(false)] out NetworkPlayer player)
		{
			return _networkPlayerByUserId.TryGetForward(user, out player);
		}

		public bool IsConnectedPlayer(UserId user)
		{
			return _networkPlayerByUserId.Contains(user);
		}
	}
}
