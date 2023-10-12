using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Networks;
using CTS.Instance.ClientShared;
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

		// References
		public SceneControllerBase? SceneController { get; private set; }

		// Gameplay Collections
		public readonly HashSet<NetworkPlayer> PlayerSet = new(GlobalNetwork.SYSTEM_MAX_USER);
		public readonly Dictionary<NetworkPlayer, CameraController> CameraControllerByPlayer = new(GlobalNetwork.SYSTEM_MAX_USER);

		// Scene managements
		private GameSceneIdentity _currentSceneId;
		private Action _onStartNextScene;
		private bool _isCurrentlyLoading = false;

		// Returns
		private readonly List<NetworkPlayer> _returnNetworkPlayerList = new(GlobalNetwork.SYSTEM_MAX_USER);

		public override void Constructor()
		{
			_onStartNextScene =	onStartNextScene;
		}

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

			// Load lobby
			GotoLobby();
		}

		public override void OnDestroyed()
		{
			if (SceneController != null)
			{
				SceneController.Destroy();
				SceneController = null;
			}
		}

		public void GotoLobby()
		{
			TryChangeSceneTo(GameSceneMapDataDB.Square);
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

			// Create camera for player
			CameraController playerCamera = WorldManager.CreateObject<CameraController>();
			playerCamera.BindNetworkPlayer(player);
			player.BindCamera(playerCamera);
			CameraControllerByPlayer.Add(player, playerCamera);

			// Enter events
			SceneController?.OnPlayerEnter(player);
			RoomSessionManager.AddPlayerState(player);
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

			// Leave events
			SceneController?.OnPlayerLeave(player);
			bool shouldRemoveState = SceneController is not MiniGameControllerBase;
			RoomSessionManager.ReleasePlayerState(player, shouldRemoveState);

			// Destroy player's camera
			if (!CameraControllerByPlayer.TryGetValue(player, out var playerCamera))
			{
				_log.Fatal($"There is no {player}'s camera to destroy!");
			}
			else
			{
				player.ReleaseCamera(playerCamera);
				CameraControllerByPlayer.Remove(player);
				playerCamera.ReleaseNetworkPlayer(player);
				playerCamera.Destroy();
			}
		}

		public partial void Client_ReadyToSync(NetworkPlayer player, JoinRequestToken token)
		{
			_log.Debug($"Client {player} ready to controll");
			if (RoomSessionManager.PlayerStateTable.TryGetValue(player.UserId, out var state))
			{
				state.SelectedCostume.SetBy(token.ClientSkinSet);
				state.CurrentCostume.SetBy(state.SelectedCostume);
			}
		}

		public bool TryChangeSceneTo(GameSceneIdentity gameId)
		{
			if (_isCurrentlyLoading)
			{
				return false;
			}

			SceneController?.Destroy();
			SceneController?.Release();
			_currentSceneId = gameId;
			WorldManager.ClearWithoutPersistentObject();
			_isCurrentlyLoading = true;
			StartCoroutine(_onStartNextScene, 0.01f);
			return true;
		}

		private void onStartNextScene()
		{
			SceneController = WorldManager.CreateSceneControllerBy(_currentSceneId);
			SceneController.Initialize(_currentSceneId);
			_isCurrentlyLoading = false;
		}

		public List<NetworkPlayer> GetAlivePlayers()
		{
			_returnNetworkPlayerList.Clear();
			foreach (NetworkPlayer player in PlayerSet)
			{
				if (player.IsEliminated)
					continue;
				_returnNetworkPlayerList.Add(player);
			}
			return _returnNetworkPlayerList;
		}

		public List<NetworkPlayer> GetShuffledAlivePlayers()
		{
			var result = GetAlivePlayers();
			result.Shuffle();
			return result;
		}
	}
}
