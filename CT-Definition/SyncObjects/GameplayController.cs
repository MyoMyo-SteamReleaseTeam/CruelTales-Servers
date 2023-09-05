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

		[SyncVar]
		public GameSystemState GameSystemState;

		[SyncObject(dir: SyncDirection.Bidirection)]
		public RoomSessionManager RoomSessionManager = new();

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_ReadyToSync() { }
	}
}
#pragma warning restore IDE0051