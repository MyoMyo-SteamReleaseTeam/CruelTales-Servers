using System;
using CT.Common.Gameplay;
using CTS.Instance.Data;
using CTS.Instance.Gameplay;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class MiniGameControllerBase : SceneControllerBase
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MiniGameControllerBase));

#if DEBUG2
		public const float TIMER_SYNC_INTERVAL = 50.0f;
		public const float MISSION_SHOW_TIME = 0.5f;
		public const float GAME_START_COUNTDOWN_TIME = 1.0f;
		public const float GAME_END_COUNTDOWN_TIME = 1.0f;
		public const float RESULT_SHOW_TIME = 1.0f;
		// TODO : Need to set cut scene duration from DB
		public const float EXECUTION_CUT_SCENE_TIME = 1.0f;
		public const float MAP_VOTE_TIME = 3.0f;
		public const float NEXT_MAP_SHOW_TIME = 1.0f;
#else
		public const float TIMER_SYNC_INTERVAL = 50.0f;
		public const float MISSION_SHOW_TIME = 1.5f;
		public const float GAME_START_COUNTDOWN_TIME = 3.0f;
		public const float GAME_END_COUNTDOWN_TIME = 3.0f;
		public const float RESULT_SHOW_TIME = 3.0f;
		// TODO : Need to set cut scene duration from DB
		public const float EXECUTION_CUT_SCENE_TIME = 3.0f;
		public const float MAP_VOTE_TIME = 3.0f;
		public const float NEXT_MAP_SHOW_TIME = 3.0f;
#endif

		public float CurrentTime
		{
			get => _currentTime;
			set => _currentTime = value;
		}

		public MiniGameData MiniGameData { get; private set; }

		// Coroutine events
		private Action _syncGameTime;
		private Action _onGameStart;
		private Action _onShowResult;
		private Action _onShowExecution;
		private Action _onStartVoteMap;
		private Action _onShowVotedNextMap;
		private Action _onChangeNextMap;

		public override void Constructor()
		{
			base.Constructor();
			MapVoteController.Initialize(this);
			_onGameStart = onGameStart;
			_syncGameTime = syncGameTime;
			_onShowResult = onShowResult;
			_onShowExecution = onShowExecution;
			_onStartVoteMap = onStartVoteMap;
			_onShowVotedNextMap = onShowVotedNextMap;
			_onChangeNextMap = onChangeNextMap;
		}

		public override void OnCreated()
		{
			base.OnCreated();

			// Set game state to ready
			GameplayController.GameSystemState = GameSystemState.Ready;
			GameplayState = GameplayState.Ready;

			foreach (NetworkPlayer player in _playerCharacterByPlayer.ForwardKeys)
			{
				player.IsMapLoaded = false;
				player.IsReady = false;
				player.CanSeeViewObject = false;
			}

			MiniGameData = MiniGameDataDB.GetMiniGameData(GameSceneIdentity);
			GameTime = MiniGameData.GameTime;
			CurrentTime = GameTime;

			Server_TryLoadSceneAll(GameSceneIdentity);
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			if (CurrentTime > 0)
			{
				CurrentTime -= deltaTime;
			}
			else if (GameplayState == GameplayState.Gameplay)
			{
				onGameEnd();

				// TODO : Lock input
			}
		}

		public override void Client_OnSceneLoaded(NetworkPlayer player)
		{
			base.Client_OnSceneLoaded(player);

			// TODO : For test
			player.IsReady = true;

			if (!isAllReady())
				return;

			// TODO : For test
			gameStart();
		}

		public virtual partial void Client_ReadyGame(NetworkPlayer player, bool isReady)
		{
			player.IsReady = isReady;

			if (!isAllReady())
				return;

			gameStart();
		}

		private bool isAllReady()
		{
			foreach (var player in RoomSessionManager.PlayerStateTable.Values)
			{
				if (!player.IsReady || !player.IsMapLoaded)
					return false;
			}

			return true;
		}

		public override void OnPlayerEnter(NetworkPlayer player)
		{
		}

		public override void OnPlayerLeave(NetworkPlayer player)
		{
			MapVoteController.OnPlayerLeave(player);

			if (_playerCharacterByPlayer.TryGetValue(player, out var pc))
			{
				pc.Destroy();
				_playerCharacterByPlayer.TryRemove(player);
			}
		}

		#region Game Flow

		private void gameStart()
		{
			GameplayController.GameSystemState = GameSystemState.InGame;
			GameplayState = GameplayState.StartCountdown;
			Server_GameStartCountdown(MISSION_SHOW_TIME, GAME_START_COUNTDOWN_TIME);
			StartCoroutine(_onGameStart, MISSION_SHOW_TIME + GAME_START_COUNTDOWN_TIME);
			StartCoroutine(_syncGameTime, TIMER_SYNC_INTERVAL);
		}

		private void syncGameTime()
		{
			Server_SyncTimer(CurrentTime);
			StartCoroutine(_syncGameTime, TIMER_SYNC_INTERVAL);
		}

		protected virtual void onGameStart()
		{
			GameplayState = GameplayState.Gameplay;
			Server_GameStart(CurrentTime);

			// TODO : Unlock input
		}

		protected virtual void onGameEnd()
		{
			GameplayState = GameplayState.GameEnd;
			Server_GameEnd(GAME_END_COUNTDOWN_TIME);
			StartCoroutine(_onShowResult, GAME_END_COUNTDOWN_TIME);
		}

		private void onShowResult()
		{
			GameplayState = GameplayState.Result;
			Server_ShowResult(RESULT_SHOW_TIME);
			StartCoroutine(_onShowExecution, RESULT_SHOW_TIME);
		}

		private void onShowExecution()
		{
			GameplayState = GameplayState.Execute;
			Server_ShowExecution(MiniGameData.ExecutionCutScene, EXECUTION_CUT_SCENE_TIME);
			StartCoroutine(_onStartVoteMap, EXECUTION_CUT_SCENE_TIME);
		}

		private void onStartVoteMap()
		{
			GameplayState = GameplayState.VoteMap;

			Span<GameSceneIdentity> maps = stackalloc GameSceneIdentity[2];
			maps[0] = GameSceneMapDataDB.RedHood;
			maps[1] = GameSceneMapDataDB.Dueoksini;
			MapVoteController.SetNextMapVotes(maps);
			Server_StartVoteMap(MAP_VOTE_TIME);
			StartCoroutine(_onShowVotedNextMap, MAP_VOTE_TIME);
		}

		private void onShowVotedNextMap()
		{
			MapVoteController.OnMapVoteEnd();
			Server_ShowVotedNextMap(MapVoteController.NextMap, NEXT_MAP_SHOW_TIME);
			StartCoroutine(_onChangeNextMap, NEXT_MAP_SHOW_TIME);
		}

		private void onChangeNextMap()
		{
			GameplayController.TryChangeSceneTo(MapVoteController.NextMap);
		}

		#endregion
	}
}