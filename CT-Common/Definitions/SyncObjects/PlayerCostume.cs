#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.Synchronizations;

namespace CT.Common.Definitions.SyncObjects
{
	[SyncObjectDefinition]
	public class PlayerCostume
	{
		[SyncVar]
		public int Head;

		[SyncVar]
		public int Hair;

		[SyncVar]
		public int Body;
	}
}
#pragma warning restore IDE0051