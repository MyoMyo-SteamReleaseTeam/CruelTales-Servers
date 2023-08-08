using CT.Common.Synchronizations;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public struct MemberToken
	{
		public BaseMemberToken Member;
		public SyncType SyncType;
		public InheritType InheritType;

		/// <summary>양방향으로 동기화되는 네트워크 객체인지 여부입니다.</summary>
		public bool IsSyncBidirection;
	}
}
