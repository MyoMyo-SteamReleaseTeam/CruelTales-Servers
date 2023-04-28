using CT.Common.Synchronizations;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine;

namespace CT.CorePatcher.SynchronizationsCodeGen
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
