using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
			}

			MiniGameController?.OnPlayerEnter(player);
			CurrentPlayerCount = PlayerSet.Count;
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
					break;
				}
			}

			MiniGameController?.OnPlayerLeave(player);
			CurrentPlayerCount = PlayerSet.Count;
		}

		public override void OnUpdate(float deltaTime)
		{

			// Update minigame controller
			MiniGameController?.Update();
		}

		public override void OnCreated()
		{
			MiniGameController = new MiniGameController(this);
			MiniGameController.OnGameStart();
		}

		public partial void Client_ReadyToSync(NetworkPlayer player)
		{
			_log.Debug($"Client {player} ready to controll");
			Server_LoadGame(player, GameMapType.MiniGame_RedHood_0);
		}

		public partial void Client_OnMapLoaded(NetworkPlayer player)
		{
			player.CanSeeViewObject = true;
		}

		#region Room Setting Request

		public partial void ClientRoomSetReq_SetPassword(NetworkPlayer player, int password)
		{
			if (checkAuthOrDisconnect(player))
			{
				GameplayManager.RoomOption.Password = password;
				Password = password;
			}
		}

		public partial void ClientRoomSetReq_SetRoomName(NetworkPlayer player, NetStringShort roomName)
		{
			if (checkAuthOrDisconnect(player))
			{
				GameplayManager.RoomOption.Name = roomName;
				RoomName = roomName;
			}
		}

		public partial void ClientRoomSetReq_SetRoomDiscription(NetworkPlayer player, NetStringShort roomDiscription)
		{
			if (checkAuthOrDisconnect(player))
			{
				GameplayManager.RoomOption.Discription = roomDiscription;
				RoomDiscription = roomDiscription;
			}
		}

		public partial void ClientRoomSetReq_SetRoomMaxUser(NetworkPlayer player, int maxUser)
		{
			if (checkAuthOrDisconnect(player))
			{
				if (GameplayManager.CurrentPlayerCount > maxUser)
				{
					ServerRoomSetAck_Callback(player, RoomSettingResult.MaximumUsersReached);
					return;
				}

				if (GameplayManager.CurrentPlayerCount < GameplayManager.Option.SystemMinUser)
				{
					ServerRoomSetAck_Callback(player, RoomSettingResult.MinimumUsersRequired);
					return;
				}

				if (maxUser > GameplayManager.Option.SystemMaxUser)
				{
					ServerRoomSetAck_Callback(player, RoomSettingResult.CannotSetMaxUserUnderConnections);
					return;
				}

				GameplayManager.RoomOption.MaxUser = maxUser;
			}
		}

		private bool checkAuthOrDisconnect(NetworkPlayer player)
		{
			if (!player.IsHost)
			{
				player.Session?.Disconnect(DisconnectReasonType.ServerError_YouAreNotHost);
			}

			return player.IsHost;
		}

		#endregion
	}
}
