using System.Collections.Generic;
using System.Numerics;
using CT.Common.Tools.Collections;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Gameplay.MiniGames
{
	public class MiniGameData
	{
		public List<Vector2> SpawnPositions = new()
		{
			new Vector2(-9.85f, 6.52f),
			new Vector2(-8.35f, 4.82f),
			new Vector2(-6.35f, 4.32f),
			new Vector2(-4.35f, 4.82f),
			new Vector2(-2.85f, 6.52f),
			new Vector2(-7.55f, 6.82f),
			new Vector2(-5.15f, 6.82f),
			new Vector2(-6.35f, 5.62f),
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

		private List<TestCube> _testCubeList = new();

		public void Update()
		{
			CheckGameOverCondition();

			if (_testCubeList.Count < 1)
			{
				Vector2 lb = new Vector2(-30, -30);
				Vector2 rt = new Vector2(30, 30);
				//Vector2 lb = new Vector2(0, 0);
				//Vector2 rt = new Vector2(0, 0);
				//var createPos = RandomHelper.NextVectorBetween(lb, rt);
				var createPos = Vector2.Zero;
				var testCube = _worldManager.CreateObject<TestCube>(createPos);
				testCube.BindMiniGame(this);
				_testCubeList.Add(testCube);
			}
		}

		public void OnTestCubeDestroyed(TestCube testCube)
		{
			_testCubeList.Remove(testCube);
		}

		public void OnGameStart()
		{
			_spawnIndex = 0;
			var spawnPositions = _miniGameData.SpawnPositions;
			int spawnPosCount = spawnPositions.Count;
			foreach (NetworkPlayer player in GameplayController.PlayerSet)
			{
				Vector2 spawnPos = spawnPositions[_spawnIndex];
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
			Vector2 spawnPos = spawnPositions[_spawnIndex];
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

		private void createPlayerBy(NetworkPlayer player, Vector2 spawnPos)
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