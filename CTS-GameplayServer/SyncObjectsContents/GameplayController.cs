using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Networks;
using CTS.Instance.Gameplay;
using CTS.Instance.Gameplay.MiniGames;
using CTS.Instance.Synchronizations;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class GameplayController : MasterNetworkObject
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(GameplayController));

		public override VisibilityType Visibility => VisibilityType.Global;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		// Reference
		public MiniGameController? MiniGameController { get; private set; }
		public HashSet<NetworkPlayer> PlayerSet { get; private set; } = new(GlobalNetwork.SYSTEM_MAX_USER);

		public const float GAME_START_COUNTDOWN = 3.0f;

		public override void OnUpdate(float deltaTime)
		{
			// Update minigame controller
			MiniGameController?.Update();
		}

		public override void OnCreated()
		{
			// Initialize server option
			ServerRuntimeOption runtimeOption = new();
			runtimeOption.PhysicsStepTime = GameplayManager.ServerOption.PhysicsStepTime;
			this.ServerRuntimeOption = runtimeOption;

			// Initialize managers
			RoomSessionManager.Initialize(this);

			MiniGameController = new MiniGameController(this, GameMapType.MiniGame_RedHood_0);
			//MiniGameController = new MiniGameController(this, GameMapType.MiniGame_Dueoksini_0);
			MiniGameController.OnGameStart();
		}

		public void OnPlayerEnter(NetworkPlayer player)
		{
			if (!PlayerSet.Add(player))
			{
				_log.Fatal($"Same user entered! : {player}");
				return;
			}

			// If it's first user give host authority
			if (PlayerSet.Count == 1)
			{
				player.IsHost = true;
				player.IsReady = true;
			}

			MiniGameController?.OnPlayerEnter(player);
			RoomSessionManager.OnPlayerEnter(player);
		}

		public void OnPlayerLeave(NetworkPlayer player)
		{
			if (!PlayerSet.Remove(player))
			{
				_log.Fatal($"There is no such user to leave! : {player}");
				return;
			}

			// Transfer host authority to another user
			if (player.IsHost)
			{
				foreach (var p in PlayerSet)
				{
					p.IsHost = true;
					p.IsReady = true;
					break;
				}
			}

			MiniGameController?.OnPlayerLeave(player);
			RoomSessionManager.OnPlayerLeave(player);
		}

		public partial void Client_ReadyToSync(NetworkPlayer player)
		{
			_log.Debug($"Client {player} ready to controll");
			Server_LoadGame(player, GameMapType.MiniGame_RedHood_0);
			//Server_LoadGame(player, GameMapType.MiniGame_Dueoksini_0);
		}

		public partial void Client_OnMapLoaded(NetworkPlayer player)
		{
			player.CanSeeViewObject = true;
		}

		public partial void Client_ReadyGame(NetworkPlayer player, bool isReady)
		{
			player.IsReady = isReady;
			RoomSessionManager.CheckAllReady();
		}

		public partial void Client_TryStartGame(NetworkPlayer player)
		{
			if (!player.IsHost)
				return;

			if (!RoomSessionManager.IsAllReady)
				return;

			Server_GameStartCountdown(GAME_START_COUNTDOWN);
		}
	}
}
