using System;
using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CTS.Instance.Gameplay;

namespace CTS.Instance.SyncObjects
{
	public partial class RoomSessionManager
	{
		[AllowNull] public GameplayManager GameplayManager { get; private set; }
		[AllowNull] public GameplayController GameplayController { get; private set; }

		public int PlayerCount => PlayerStateTable.Count;

		public void OnCreated(GameplayController gameplayController)
		{
			GameplayController = gameplayController;
			GameplayManager = gameplayController.GameplayManager;
			MaxPlayerCount = GameplayManager.Option.SystemMaxUser;
			MinPlayerCount = GameplayManager.Option.SystemMinUser;
			SetRoomName("Cruel Tales official test server");
			SetRoomDiscription("Play to fun");
			SetRoomMaxUser(MaxPlayerCount);
			SetPassword(-1);
		}

		public void TrimDisconnectedUsers()
		{
			// Remove disconnected user's state
			int disconnectedUserCount = 0;
			Span<UserId> disconnectedUsers = stackalloc UserId[PlayerStateTable.Count];
			foreach (var player in PlayerStateTable.Values)
			{
				if (!ConnectedPlayers.Contains(player.UserId))
				{
					disconnectedUsers[disconnectedUserCount++] = player.UserId;
				}
			}
			for (int i = 0; i < disconnectedUserCount; i++)
			{
				PlayerStateTable.Remove(disconnectedUsers[i]);
			}
		}

		public void AddPlayerState(NetworkPlayer player)
		{
			PlayerState state = PlayerStateTable.Add(player.UserId);
			state.ClearDirtyReliable();
			player.BindPlayerState(state);
			ConnectedPlayers.Add(player.UserId);
		}

		public void ReleasePlayerState(NetworkPlayer player, bool shouldRemoveState)
		{
			if (shouldRemoveState)
			{
				PlayerStateTable.Remove(player.UserId);
			}
			ConnectedPlayers.Remove(player.UserId);
		}

		public void SetPassword(int password)
		{
			GameplayManager.RoomOption.Password = password;
			Password = password;
		}

		public void SetRoomName(NetStringShort roomName)
		{
			GameplayManager.RoomOption.Name = roomName;
			RoomName = roomName;
		}

		public void SetRoomDiscription(NetStringShort roomDiscription)
		{
			GameplayManager.RoomOption.Discription = roomDiscription;
			RoomDiscription = roomDiscription;
		}

		public void SetRoomMaxUser(int maxUserCount)
		{
			GameplayManager.RoomOption.MaxUser = maxUserCount;
			MaxPlayerCount = maxUserCount;
		}

		public void SetRoomMinUser(int minUserCount)
		{
			GameplayManager.RoomOption.MinUser = minUserCount;
			MinPlayerCount = minUserCount;
		}

		public partial void ClientRoomSetReq_SetPassword(NetworkPlayer player, int password)
		{
			if (!checkAuthOrDisconnect(player)) return;
			SetPassword(password);
			ServerRoomSetAck_Callback(player, RoomSettingResult.Success);
		}

		public partial void ClientRoomSetReq_SetRoomName(NetworkPlayer player, NetStringShort roomName)
		{
			if (!checkAuthOrDisconnect(player)) return;
			SetRoomName(roomName);
			ServerRoomSetAck_Callback(player, RoomSettingResult.Success);
		}

		public partial void ClientRoomSetReq_SetRoomDiscription(NetworkPlayer player, NetStringShort roomDiscription)
		{
			if (!checkAuthOrDisconnect(player)) return;
			SetRoomDiscription(roomDiscription);
			ServerRoomSetAck_Callback(player, RoomSettingResult.Success);
		}

		public partial void ClientRoomSetReq_SetRoomMaxUser(NetworkPlayer player, int maxUserCount)
		{
			if (!checkAuthOrDisconnect(player)) return;

			if (maxUserCount < GameplayManager.RoomOption.MinUser)
			{
				ServerRoomSetAck_Callback(player, RoomSettingResult.CrossMaximumAndMinimum);
				return;
			}

			if (GameplayManager.PlayerCount > maxUserCount)
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

			SetRoomMaxUser(maxUserCount);
			ServerRoomSetAck_Callback(player, RoomSettingResult.Success);
		}

		public partial void ClientRoomSetReq_SetRoomMinUser(NetworkPlayer player, int minUserCount)
		{
			if (!checkAuthOrDisconnect(player)) return;

			if (minUserCount > GameplayManager.RoomOption.MaxUser)
			{
				ServerRoomSetAck_Callback(player, RoomSettingResult.CrossMaximumAndMinimum);
				return;
			}

			if (minUserCount < GameplayManager.Option.SystemMinUser)
			{
				ServerRoomSetAck_Callback(player, RoomSettingResult.MinimumUsersRequired);
				return;
			}

			if (minUserCount > GameplayManager.Option.SystemMaxUser)
			{
				ServerRoomSetAck_Callback(player, RoomSettingResult.MaximumUsersReached);
				return;
			}

			SetRoomMinUser(minUserCount);
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
