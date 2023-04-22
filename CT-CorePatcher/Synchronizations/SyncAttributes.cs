using System;

namespace CT.CorePatcher.Synchronizations
{
	public enum SyncType
	{
		None = 0,
		Reliable,
		Unreliable,
		RelibaleOrUnreliable,
	}

	public class SyncNetworkObjectDefinitionAttribute : Attribute
	{

	}

	public class SyncObjectDefinitionAttribute : Attribute
	{
		public bool IsCustom { get; private set; }

		public SyncObjectDefinitionAttribute(bool isCustom = false)
		{
			IsCustom = isCustom;
		}
	}

	public class SyncObjectAttribute : Attribute
	{
		public SyncType SyncType { get; private set; }

		public SyncObjectAttribute(SyncType syncType = SyncType.Reliable)
		{
			SyncType = syncType;
		}
	}

	public class SyncVarAttribute : Attribute
	{
		public SyncType SyncType { get; private set; }

		public SyncVarAttribute(SyncType syncType = SyncType.Reliable)
		{
			SyncType = syncType;
		}
	}

	public class SyncRpcAttribute : Attribute
	{
		public SyncType SyncType { get; private set; }

		public SyncRpcAttribute(SyncType syncType = SyncType.Reliable)
		{
			SyncType = syncType;
		}
	}
}
