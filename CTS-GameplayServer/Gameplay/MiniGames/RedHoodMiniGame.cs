using System.Collections.Generic;
using System.Numerics;
using CT.Common.Tools.Collections;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Gameplay.MiniGames
{
	public class MiniGameData
	{
		public List<Vector3> SpawnPositions = new()
		{
			new Vector3(-9.85f, 0.00f, 6.52f),
			new Vector3(-8.35f, 0.00f, 4.82f),
			new Vector3(-6.35f, 0.00f, 4.32f),
			new Vector3(-4.35f, 0.00f, 4.82f),
			new Vector3(-2.85f, 0.00f, 6.52f),
			new Vector3(-7.55f, 0.00f, 6.82f),
			new Vector3(-5.15f, 0.00f, 6.82f),
			new Vector3(-6.35f, 0.00f, 5.62f),
		};
	}

	public class MiniGameController
	{
		// Reference
		public GameplayController GameplayController { get; private set; }
		private GameplayManager _gameplayManager;
		private WorldManager _worldManager;
		private MiniGameData _miniGameData;

		// Player Management
		private BidirectionalMap<NetworkPlayer, PlayerCharacter> _playerCharacterByPlayer;
		private int _spawnIndex = 0;

		public MiniGameController(GameplayController gameplayController)
		{
			GameplayController = gameplayController;
			_gameplayManager = gameplayController.GameplayManager;
			_worldManager = _gameplayManager.WorldManager;
			_playerCharacterByPlayer = new(_gameplayManager.Option.SystemMaxUser);
			_miniGameData = new MiniGameData();
		}

		public void Update()
		{
			CheckGameOverCondition();
		}

		public void OnGameStart()
		{
			_spawnIndex = 0;
			var spawnPositions = _miniGameData.SpawnPositions;
			int spawnPosCount = spawnPositions.Count;
			foreach (NetworkPlayer player in GameplayController.PlayerSet)
			{
				Vector3 spawnPos = spawnPositions[_spawnIndex];
				createPlayerBy(player, spawnPos);
				_spawnIndex = (_spawnIndex + 1) % spawnPosCount;
			}
		}

		public void OnGameEnd()
		{
			foreach (var pc in _playerCharacterByPlayer.ForwardValues)
			{
				pc.Destroy();
			}
		}

		public void OnPlayerEnter(NetworkPlayer player)
		{
			var spawnPositions = _miniGameData.SpawnPositions;
			int spawnPosCount = spawnPositions.Count;
			Vector3 spawnPos = spawnPositions[_spawnIndex];
			createPlayerBy(player, spawnPos);
			_spawnIndex = (_spawnIndex + 1) % spawnPosCount;
		}

		public void OnPlayerLeave(NetworkPlayer player)
		{
			if (_playerCharacterByPlayer.TryGetValue(player, out var pc))
			{
				pc.Destroy();
				_playerCharacterByPlayer.TryRemove(player);
			}

			CheckGameOverCondition();
		}

		private void createPlayerBy(NetworkPlayer player, Vector3 spawnPos)
		{
			var playerCharacter = _worldManager.CreateObject<PlayerCharacter>(spawnPos);
			playerCharacter.BindNetworkPlayer(player);
			_playerCharacterByPlayer.Add(player, playerCharacter);
		}

		public void CheckGameOverCondition()
		{

		}
	}
}