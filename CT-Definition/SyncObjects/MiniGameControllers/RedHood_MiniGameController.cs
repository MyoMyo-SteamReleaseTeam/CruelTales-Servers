#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 2)]
	public class RedHood_MiniGameController : MiniGameControllerBase
	{
		[SyncObject(cc: Constant.MAX_CAPACITY_BY_PLAYER)]
		protected SyncObjectDictionary<UserId, PlayerRoleState> PlayerRoleStateTable = new();
	}

	[SyncObjectDefinition]
	public class PlayerRoleState
	{
		[SyncVar]
		public bool IsWolf;
	}
}

#pragma warning restore IDE0051