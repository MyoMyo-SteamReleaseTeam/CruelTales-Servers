using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Networks;
using CTS.Instance.Data;
using CTS.Instance.Gameplay;
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
		public SceneControllerBase? SceneController { get; private set; }
		public HashSet<NetworkPlayer> PlayerSet { get; private set; } = new(GlobalNetwork.SYSTEM_MAX_USER);

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void OnCreated()
		{
			IsPersistent = true;

			// Initialize server option
			ServerRuntimeOption runtimeOption = new()
			{
				PhysicsStepTime = GameplayManager.ServerOption.PhysicsStepTime
			};
			ServerRuntimeOption = runtimeOption;

			// Initialize managers
			RoomSessionManager.OnCreated(this);

			GameSceneIdentity lobbyGameId = GameSceneMapDataDB.Square;
			SceneController = WorldManager.CreateSceneControllerBy(lobbyGameId);
			SceneController.Initialize(lobbyGameId);
		}

		public override void OnDestroyed()
		{
			if (SceneController != null)
			{
				SceneController.Destroy();
				SceneController = null;
			}
		}

		public bool CheckIfCanJoin(out DisconnectReasonType reason)
		{
			if (GameSystemState != GameSystemState.Lobby)
			{
				reason = DisconnectReasonType.Reject_GameCurrentlyPlaying;
				return false;
			}

			reason = DisconnectReasonType.None;
			return true;
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

			SceneController?.OnPlayerEnter(player);
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

			SceneController?.OnPlayerLeave(player);
			RoomSessionManager.OnPlayerLeave(player);
		}

		public partial void Client_ReadyToSync(NetworkPlayer player, JoinRequestToken token)
		{
			_log.Debug($"Client {player} ready to controll");
			if (RoomSessionManager.PlayerStateTable.TryGetValue(player.UserId, out var state))
			{
				state.SelectedSkin = token.ClientSkinSet;
				state.CurrentSkin = state.SelectedSkin;
			}
		}

		public void ChangeSceneTo(GameSceneIdentity gameId)
		{
			SceneController?.Destroy();
			WorldManager.ClearWithoutPersistentObject();
			SceneController = WorldManager.CreateSceneControllerBy(gameId);
			SceneController.Initialize(gameId);
		}
	}
}
