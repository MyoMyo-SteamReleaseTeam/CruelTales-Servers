#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 2)]
	public class Dueoksini_MiniGameController : MiniGameControllerBase
	{

	}

	//[SyncObjectDefinition]
	//public class PlayerRoleState
	//{
	//	[SyncVar]
	//	public bool IsWolf;
	//}
}

#pragma warning restore IDE0051