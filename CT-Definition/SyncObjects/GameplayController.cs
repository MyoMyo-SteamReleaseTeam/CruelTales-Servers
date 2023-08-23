#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 1)]
	public class GameplayController
	{
		[SyncVar]
		public ServerRuntimeOption ServerRuntimeOption;

		[SyncObject(dir: SyncDirection.Bidirection)]
		public RoomSessionManager RoomSessionManager = new();

		[SyncRpc(SyncType.ReliableTarget)]
		public void Server_LoadGame(GameMapType mapType) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_ReadyToSync() { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_OnMapLoaded() { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_ReadyGame(bool isReady) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_TryStartGame() { }

		[SyncRpc]
		public void Server_GameStartCountdown(float second) { }
	}
}
#pragma warning restore IDE0051