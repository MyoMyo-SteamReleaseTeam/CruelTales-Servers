using System;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public abstract class BaseMemberToken
	{
		public string AccessModifier { get; protected set; } = string.Empty;
		public abstract bool ShouldRollBackMask { get; }
		public bool IsPublic { get; }
		protected SyncType _syncType;
		protected string _typeName;
		protected string _privateMemberName;
		protected string _publicMemberName;
		public string TypeName => _typeName;
		public string PrivateMemberName => _privateMemberName;
		public string PublicMemberName => _publicMemberName;

		public BaseMemberToken(SyncType syncType, string typeName, string memberName, bool isPublic)
		{
			IsPublic = isPublic;
			AccessModifier = isPublic ? "public" : "private";
			_syncType = syncType;
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
		}

		// Master Side
		public abstract string Master_InitializeProperty(SyncDirection direction);
		public abstract string Master_Declaration(SyncDirection direction);
		public abstract string Master_GetterSetter(SyncType syncType, string dirtyBitname, int memberIndex);
		public abstract string Master_SerializeByWriter(SyncType syncType, string dirtyBitname, int index);
		public abstract string Master_CheckDirty(SyncType syncType); // Object Only
		public abstract string Master_ClearDirty(SyncType syncType); // Object Only

		// Remote Side
		public abstract string Remote_InitializeProperty(SyncDirection direction);
		public abstract string Remote_Declaration(SyncDirection direction);
		public abstract string Remote_DeserializeByReader(SyncType syncType, SyncDirection direction);
		public abstract string Remote_IgnoreDeserialize(SyncType syncType, bool isStatic);
	}
}