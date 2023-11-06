using System;
using System.Collections.Generic;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CTS.Instance.Coroutines;
using CTS.Instance.Data;
using CTS.Instance.Gameplay;

namespace CTS.Instance.SyncObjects
{
	public partial class CustomLobby_SceneController : SceneControllerBase
	{
		//public const float GAME_START_COUNTDOWN_TIME = 4.0f;
		public const float GAME_START_COUNTDOWN_TIME = 1.0f;

		private CoroutineRunnerVoid _onGameStartRunner;

		private List<TestCube> _testCubeList = new();
		private Action<TestCube> _onDestroyed;
		
		public override void Initialize(GameSceneIdentity identity)
		{
			base.Initialize(identity);
			GameplayController.GameSystemState = GameSystemState.Lobby;
		}

		public override void OnCreated()
		{
			base.OnCreated();
			GameplayController.RoomSessionManager.TrimDisconnectedUsers();
		}

		public override void OnDestroyed()
		{
			base.OnDestroyed();
			GameplayController.RoomSessionManager.TrimDisconnectedUsers();
		}

		public override void Constructor()
		{
			base.Constructor();

			_onGameStartRunner = new CoroutineRunnerVoid(this, onGameStart);
			_onDestroyed = OnTestCubeDestroyed;
		}

		public override void OnUpdate(float deltaTime)
		{
			if (_testCubeList.Count < 0)
			{
				//Vector2 lb = new Vector2(-30, -30);
				//Vector2 rt = new Vector2(30, 30);
				Vector2 lb = new Vector2(0, 0);
				Vector2 rt = new Vector2(0, 0);
				var createPos = RandomHelper.NextVector2(lb, rt);
				//var createPos = Vector2.Zero;
				var testCube = WorldManager.CreateObject<TestCube>(createPos);
				testCube.BindMiniGame(_onDestroyed);
				_testCubeList.Add(testCube);
			}
		}

		public void OnTestCubeDestroyed(TestCube testCube)
		{
			_testCubeList.Remove(testCube);
		}

		public override void OnPlayerEnter(NetworkPlayer player)
		{
			base.OnPlayerEnter(player);

			Server_TryLoadScene(player, GameSceneIdentity);
			SpawnPlayerBy<PlayerCharacter>(player, onCreated: null);

			if (GameplayController.GameSystemState == GameSystemState.GameStartCountdown)
			{
				cancelCountdown();
			}
		}

		public override void OnPlayerLeave(NetworkPlayer player)
		{
			base.OnPlayerLeave(player);

			if (GameplayController.GameSystemState == GameSystemState.GameStartCountdown)
			{
				cancelCountdown();
			}
		}

		public virtual partial void Client_ReadyGame(NetworkPlayer player, bool isReady)
		{
			player.IsReady = isReady;

			if (!player.IsHost || !player.IsReady)
				return;

			if (!tryStartGameBy(player))
			{
				player.IsReady = false;
				return;
			}

			// TODO : OnAllReady();

			Server_StartGameCountdown(GAME_START_COUNTDOWN_TIME);
			GameplayController.GameSystemState = GameSystemState.GameStartCountdown;
			_onGameStartRunner.StartCoroutine(GAME_START_COUNTDOWN_TIME);

			bool tryStartGameBy(NetworkPlayer player)
			{
				int playerCount = GameplayController.RoomSessionManager.PlayerCount;

				if (!player.IsHost)
				{
					Server_TryStartGameCallback(player, StartGameResultType.YouAreNotHost);
					return false;
				}

				foreach (var p in RoomSessionManager.PlayerStateTable.Values)
				{
					if (!p.IsReady)
					{
						Server_TryStartGameCallback(player, StartGameResultType.SomePlayerNotReady);
						return false;
					}
				}

				if (playerCount < RoomSessionManager.MinPlayerCount)
				{
					Server_TryStartGameCallback(player, StartGameResultType.NoEnoughPlayer);
					return false;
				}

				if (playerCount > RoomSessionManager.MaxPlayerCount)
				{
					Server_TryStartGameCallback(player, StartGameResultType.TooManyPlayer);
					return false;
				}

				if (GameplayController.GameSystemState != GameSystemState.Lobby)
				{
					StartGameResultType callback =
						GameplayController.GameSystemState == GameSystemState.GameStartCountdown ?
						StartGameResultType.AlreadyStarting : StartGameResultType.NotInLobby;
					Server_TryStartGameCallback(player, callback);
					return false;
				}

				return true;
			}
		}

		private void cancelCountdown()
		{
			GameplayController.GameSystemState = GameSystemState.Lobby;
			_onGameStartRunner.StopCoroutine();
			Server_CancelStartGameCountdown();
		}

		private void onGameStart()
		{
			GameplayController.TryChangeSceneTo(GameSceneMapDataDB.RedHood);
			//GameplayController.TryChangeSceneTo(GameSceneMapDataDB.Dueoksini);
		}
	}
}