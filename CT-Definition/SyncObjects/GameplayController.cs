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
		private RoomSessionManager RoomSessionManager = new();

		[SyncObject]
		public EffectController EffectController = new();

		[SyncObject]
		public SoundController SoundController = new();

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_ReadyToSync(JoinRequestToken token) { }
	}
}
#pragma warning restore IDE0051