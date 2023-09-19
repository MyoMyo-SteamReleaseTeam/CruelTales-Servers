#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 2)]
	public class MiniGameControllerBase : SceneControllerBase
	{
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_ReadyGame(bool isReady) { }

		[SyncRpc]
		public void Server_StartMiniGame() { }

		[SyncRpc]
		public void Server_NextGameStartCountdown(float second) { }
	}
}
#pragma warning restore IDE0051