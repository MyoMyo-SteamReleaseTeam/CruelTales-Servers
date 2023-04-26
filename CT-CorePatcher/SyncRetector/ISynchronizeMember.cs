using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector
{
	public interface ISynchronizeMember
	{
		public string Master_Declaration(SyncType syncType, SyncDirection direction);
		public string Master_GetterSetter(string modifier, string dirtyBitname, int propertyIndex);
		public string Master_SerializeByWriter();
		public string Master_CheckDirty(); // Object Only
		public string Master_ClearDirty(); // Object Only

		public string Remote_Declaration(SyncType syncType, SyncDirection direction);
		public string Remote_DeserializeByReader();
	}
}
