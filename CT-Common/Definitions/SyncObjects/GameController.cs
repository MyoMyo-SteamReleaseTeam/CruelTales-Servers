﻿#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Common.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 1)]
	public class GameController
	{
		[SyncRpc(SyncType.ReliableTarget)]
		public void Server_LoadGame(GameMapType mapType) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_ReadyToSync() { }
	}
}
#pragma warning restore IDE0051