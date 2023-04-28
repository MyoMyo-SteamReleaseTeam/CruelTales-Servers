using CT.Common.Synchronizations;
using CT.CorePatcher.SyncRetector.PropertyDefine;

namespace CT.CorePatcher.SyncRetector
{
	public struct MemberToken
	{
		public BaseMemberToken Member;
		public SyncType SyncType;

		public MemberToken(BaseMemberToken member, SyncType syncType)
		{
			Member = member;
			SyncType = syncType;
		}
	}
}
