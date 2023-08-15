#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncObjectDefinition]
	public class PlayerState
	{
		[SyncVar]
		public UserId UserId;

		[SyncVar]
		public NetStringShort Username;

		[SyncObject]
		public PlayerCostume Costume = new();
	}
}
#pragma warning restore IDE0051