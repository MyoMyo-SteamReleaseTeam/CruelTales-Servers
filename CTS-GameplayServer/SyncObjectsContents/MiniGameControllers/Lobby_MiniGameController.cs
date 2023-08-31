using CT.Common.DataType;
using CT.Common.Gameplay;
using CTS.Instance.Coroutines;
using CTS.Instance.Data;
using CTS.Instance.Gameplay;

namespace CTS.Instance.SyncObjects
{
	public partial class Lobby_MiniGameController : MiniGameControllerBase
	{
		public const float GAME_START_COUNTDOWN = 3.95f;

		private CoroutineRunnerVoid _onGameStartRunner;

		public override void Initialize(GameplayController gameplayController, MiniGameIdentity identity)
		{
			base.Initialize(gameplayController, identity);
			GameplayController.GameSystemState = GameSystemState.Lobby;
		}

		public override void Constructor()
		{
			_onGameStartRunner = new CoroutineRunnerVoid(this, onGameStart);
		}

		public override void OnPlayerEnter(NetworkPlayer player)
		{
			base.OnPlayerEnter(player);

			if (GameplayController.GameSystemState == GameSystemState.Countdown)
			{
				cancelCountdown();
			}
		}

		public override void OnPlayerLeave(NetworkPlayer player)
		{
			base.OnPlayerLeave(player);

			if (GameplayController.GameSystemState == GameSystemState.Countdown)
			{
				cancelCountdown();
			}
		}

		public partial void Client_TryStartGame(NetworkPlayer player)
		{
			StartGameResultType result = StartGameResultType.Success;

			if (!player.IsHost)
				result = StartGameResultType.YouAreNotHost;

			if (GameplayController.RoomSessionManager.PlayerCount < GameplayManager.Option.SystemMinUser)
				result = StartGameResultType.NoEnoughPlayer;

			if (GameplayController.RoomSessionManager.PlayerCount > GameplayManager.Option.SystemMaxUser)
				result = StartGameResultType.TooManyPlayer;

			if (!GameplayController.RoomSessionManager.IsAllReady)
				result = StartGameResultType.SomePlayerNotReady;

			if (GameplayController.GameSystemState != GameSystemState.Lobby)
			{
				result = GameplayController.GameSystemState == GameSystemState.Countdown ?
					StartGameResultType.AlreadyStarting : StartGameResultType.NotInLobby;
			}

			if (result != StartGameResultType.Success)
			{
				Server_TryStartGameCallback(result);
				return;
			}

			Server_TryStartGameCallback(StartGameResultType.Success);
			Server_GameStartCountdown(GAME_START_COUNTDOWN);
			GameplayController.GameSystemState = GameSystemState.Countdown;
			_onGameStartRunner.StartCoroutine(GAME_START_COUNTDOWN);
		}

		private void cancelCountdown()
		{
			GameplayController.GameSystemState = GameSystemState.Lobby;
			_onGameStartRunner.StopCoroutine();
			Server_CancelGameStartCountdown();
		}

		private void onGameStart()
		{
			GameplayController.GameSystemState = GameSystemState.Ready;
			GameplayController.ChangeMiniGameTo(MiniGameMapDataDB.RedHood);
		}
	}
}