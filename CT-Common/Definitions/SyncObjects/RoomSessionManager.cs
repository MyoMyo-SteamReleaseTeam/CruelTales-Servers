﻿#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Synchronizations;

namespace CT.Common.Definitions.SyncObjects
{
	[SyncObjectDefinition]
	public class  RoomSessionManager
	{
		[SyncVar]
		public NetStringShort RoomName;

		[SyncVar]
		public NetStringShort RoomDiscription;

		[SyncVar]
		public int Password;

		[SyncRpc(SyncType.ReliableTarget)]
		public void ServerRoomSetAck_Callback(RoomSettingResult callback) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void ClientRoomSetReq_SetPassword(int password) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void ClientRoomSetReq_SetRoomName(NetStringShort roomName) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void ClientRoomSetReq_SetRoomDiscription(NetStringShort roomDiscription) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void ClientRoomSetReq_SetRoomMaxUser(int maxUser) { }
	}
}
#pragma warning restore IDE0051