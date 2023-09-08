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

		public void OnPlayerEnter(NetworkPlayer player)
		{
			PlayerState state = PlayerStateTable.Add(player.UserId);
			state.ClearDirtyReliable();
			player.BindPlayerState(state);
		}

		public void OnPlayerLeave(NetworkPlayer player)
		{
			player.ReleasePlayerState();
			PlayerStateTable.Remove(player.UserId);
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

			if (GameplayManager.PlayerCount >= GameplayManager.RoomOption.MinUser &&
				GameplayManager.PlayerCount < minUserCount)
			{
				ServerRoomSetAck_Callback(player, RoomSettingResult.CannotSetMinUserUnderConnections);
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
