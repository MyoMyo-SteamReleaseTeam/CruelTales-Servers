#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Synchronizations;

namespace CT.Common.Definitions.SyncObjects
{
	[SyncObjectDefinition]
	public class NetworkPlayer
	{
		[SyncVar(SyncType.Unreliable, SyncDirection.FromRemote)]
		public UserId UserId;

		[SyncVar]
		public NetStringShort Username;

		[SyncVar]
		public int Costume;
	}
}
#pragma warning restore IDE0051