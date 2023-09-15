#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 2)]
	public class SceneControllerBase
	{
		[SyncVar]
		public GameSceneIdentity GameSceneIdentity;

		[SyncRpc]
		public void Server_TryLoadSceneAll(GameSceneIdentity gameScene) { }

		[SyncRpc(SyncType.ReliableTarget)]
		public void Server_TryLoadScene(GameSceneIdentity gameScene) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_OnSceneLoaded() { }
	}
}
#pragma warning restore IDE0051