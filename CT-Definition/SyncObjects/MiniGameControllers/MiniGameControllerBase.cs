#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 1)]
	public class MiniGameControllerBase
	{
		[SyncVar]
		public MiniGameIdentity MiniGameIdentity;

		[SyncRpc(SyncType.ReliableTarget)]
		public void Server_LoadMiniGame() { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_OnMiniGameLoaded() { }
	}

	[SyncNetworkObjectDefinition(capacity: 1)]
	public class RedHood_MiniGameController : MiniGameControllerBase
	{

	}

	[SyncNetworkObjectDefinition(capacity: 1)]
	public class Lobby_MiniGameController : MiniGameControllerBase
	{
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_TryStartGame() { }

		[SyncRpc]
		public void Server_TryStartGameCallback(StartGameResultType result) { }

		[SyncRpc]
		public void Server_GameStartCountdown(float second) { }

		[SyncRpc]
		public void Server_CancelGameStartCountdown() { }
	}
}
#pragma warning restore IDE0051