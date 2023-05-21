﻿using CT.Common.Gameplay;
using CT.Common.Tools;
using CT.Common.Tools.Collections;
using CTS.Instance.Networks;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameManager : IUpdatable
	{
		// Log
		private readonly static ILog _log = LogManager.GetLogger(typeof(GameManager));

		// Reference
		private GameplayInstance _gameplayInstance;
		private WorldManager _worldManager;

		// Manage Players
		private BidirectionalMap<UserSession, NetworkPlayer> _networkPlayerByUserId;
		private ObjectPool<NetworkPlayer> _networkPlayerPool;

		// Test
		private BidirectionalMap<NetworkPlayer, PlayerCharacter> _playerCharacterByPlayer;

		public GameManager(GameplayInstance gameplayInstance,
						   InstanceInitializeOption option)
		{
			// Reference
			_gameplayInstance = gameplayInstance;
			_worldManager = gameplayInstance.WorldManager;

			// Manage Players
			_networkPlayerByUserId = new(option.SystemMaxUser);
			_networkPlayerPool = new(() => new NetworkPlayer(this, _worldManager, option),
									 option.SystemMaxUser);

			// Test
			_playerCharacterByPlayer = new(option.SystemMaxUser);
		}

		public void Update(float deltaTime)
		{
			foreach (var player in _networkPlayerByUserId.ForwardValues)
			{
				player.Update(deltaTime);
			}
		}

		public void OnUserEnterGame(UserSession userSession)
		{
			var player = _networkPlayerPool.Get();
			player.OnCreated(userSession);
			_networkPlayerByUserId.Add(userSession, player);
			_worldManager.OnPlayerEnter(player);
			
			// Test
			var playerCharacter = _worldManager.CreateObject<PlayerCharacter>();
			_playerCharacterByPlayer.Add(player, playerCharacter);
		}

		public void OnUserLeaveGame(UserSession userSession)
		{
			if (!_networkPlayerByUserId.TryGetValue(userSession, out var player))
			{
				_log.Error($"[{_gameplayInstance}] There is no {userSession}'s network player!");
				return;
			}

			_worldManager.OnPlayerLeave(player);
			_networkPlayerByUserId.TryRemove(player);
			player.OnDestroyed();
			_log.Info($"[{_gameplayInstance}] Session {userSession} leave the game");

			// Test
			if (_playerCharacterByPlayer.TryGetValue(player, out var character))
			{
				character.Destroy();
				_playerCharacterByPlayer.TryRemove(player);
			}
		}
	}
}
