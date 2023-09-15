#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 2)]
	public class MiniGameControllerBase
	{
		[SyncVar]
		public MiniGameIdentity MiniGameIdentity;

		[SyncRpc(SyncType.ReliableTarget)]
		public void Server_LoadMiniGame(MiniGameIdentity miniGameIdentity) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_OnMiniGameLoaded() { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_ReadyGame(bool isReady) { }

		[SyncRpc]
		public void Server_StartMiniGame() { }

		[SyncRpc]
		public void Server_NextGameStartCountdown(float second) { }
	}

	[SyncNetworkObjectDefinition(capacity: 2)]
	public class RedHood_MiniGameController : MiniGameControllerBase
	{

	}

	[SyncNetworkObjectDefinition(capacity: 2)]
	public class Lobby_MiniGameController : MiniGameControllerBase
	{
		[SyncRpc(sync: SyncType.ReliableTarget)]
		public void Server_TryStartGameCallback(StartGameResultType result) { }

		[SyncRpc]
		public void Server_StartGameCountdown(float second) { }

		[SyncRpc]
		public void Server_CancelStartGameCountdown() { }
	}
}
#pragma warning restore IDE0051