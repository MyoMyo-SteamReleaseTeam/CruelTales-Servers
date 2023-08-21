using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CTS.Instance.Gameplay;

namespace CTS.Instance.SyncObjects
{
	public partial class RoomSessionManager
	{
		[AllowNull] public GameplayManager GameplayManager { get; private set; }
		[AllowNull] public GameplayController GameplayController { get; private set; }

		public void Initialize(GameplayController gameplayController)
		{
			GameplayController = gameplayController;
			GameplayManager = gameplayController.GameplayManager;
		}

		public void OnPlayerEnter(NetworkPlayer player)
		{
			PlayerState state = PlayerStateTable.Add(player.UserId);
			state.ClearDirtyReliable();
			player.BindPlayerState(state);
			CheckAllReady();
		}

		public void OnPlayerLeave(NetworkPlayer player)
		{
			player.ReleasePlayerState();
			PlayerStateTable.Remove(player.UserId);
			CheckAllReady();
		}

		public void CheckAllReady()
		{
			foreach (var player in PlayerStateTable.Values)
			{
				if (!player.IsReady)
				{
					IsAllReady = false;
					return;
				}
			}

			IsAllReady = true;
		}

		public partial void ClientRoomSetReq_SetPassword(NetworkPlayer player, int password)
		{
			if (!checkAuthOrDisconnect(player)) return;

			GameplayManager.RoomOption.Password = password;
			Password = password;
			ServerRoomSetAck_Callback(player, RoomSettingResult.Success);
		}

		public partial void ClientRoomSetReq_SetRoomName(NetworkPlayer player, NetStringShort roomName)
		{
			if (!checkAuthOrDisconnect(player)) return;

			GameplayManager.RoomOption.Name = roomName;
			RoomName = roomName;
			ServerRoomSetAck_Callback(player, RoomSettingResult.Success);
		}

		public partial void ClientRoomSetReq_SetRoomDiscription(NetworkPlayer player, NetStringShort roomDiscription)
		{
			if (!checkAuthOrDisconnect(player)) return;

			GameplayManager.RoomOption.Discription = roomDiscription;
			RoomDiscription = roomDiscription;
			ServerRoomSetAck_Callback(player, RoomSettingResult.Success);
		}

		public partial void ClientRoomSetReq_SetRoomMaxUser(NetworkPlayer player, int maxUserCount)
		{
			if (!checkAuthOrDisconnect(player)) return;

			if (GameplayManager.CurrentPlayerCount > maxUserCount)
			{
				ServerRoomSetAck_Callback(player, RoomSettingResult.CannotSetMaxUserUnderConnections);
				return;
			}

			if (maxUserCount < GameplayManager.Option.SystemMinUser)
			{
				ServerRoomSetAck_Callback(player, RoomSettingResult.MinimumUsersRequired);
				return;
			}

			if (maxUserCount > GameplayManager.Option.SystemMaxUser)
			{
				ServerRoomSetAck_Callback(player, RoomSettingResult.MaximumUsersReached);
				return;
			}

			GameplayManager.RoomOption.MaxUser = maxUserCount;
			MaxPlayerCount = maxUserCount;
			ServerRoomSetAck_Callback(player, RoomSettingResult.Success);
		}

		private bool checkAuthOrDisconnect(NetworkPlayer player)
		{
			if (!player.IsHost)
			{
				ServerRoomSetAck_Callback(player, RoomSettingResult.YouAreNotHost);
			}

			return player.IsHost;
		}
	}
}
