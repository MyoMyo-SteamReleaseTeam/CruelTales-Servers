using CT.Common.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public struct GenOption
	{
		public CodeGenDirection GenDirection;
		public SyncType SyncType;
		public SyncDirection Direction;
		public SyncObjectType ObjectType;
		public InheritType InheritType;
	}
}
