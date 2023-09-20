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
		public bool HasCallback;

		public GenOption()
		{
			GenDirection = CodeGenDirection.None;
			SyncType = SyncType.None;
			Direction = SyncDirection.None;
			ObjectType = SyncObjectType.None;
			InheritType = InheritType.None;
			HasCallback = true;
		}
	}
}
