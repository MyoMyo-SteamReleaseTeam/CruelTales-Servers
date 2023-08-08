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

		public partial void ClientRoomSetReq_SetPassword(NetworkPlayer player, int password)
		{
			if (checkAuthOrDisconnect(player))
			{
				GameplayManager.RoomOption.Password = password;
				Password = GameplayManager.RoomOption.Password;
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
	}
}
