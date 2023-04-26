using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector
{
	public struct MemberToken
	{
		public ISynchronizeMember Member;
		public SyncType SyncType;

		public MemberToken(ISynchronizeMember member, SyncType syncType)
		{
			Member = member;
			SyncType = syncType;
		}
	}
}
