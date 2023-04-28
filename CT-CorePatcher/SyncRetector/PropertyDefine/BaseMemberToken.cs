﻿using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector.PropertyDefine
{
	public abstract class BaseMemberToken
	{
		public string AccessModifier { get; protected set; } = string.Empty;
		protected SyncType _syncType;
		protected string _typeName;
		protected string _privateMemberName;
		protected string _publicMemberName;

		public BaseMemberToken(SyncType syncType, string typeName, string memberName, bool isPublic)
		{
			AccessModifier = isPublic ? "public" : "private";
			_syncType = syncType;
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
		}

		public abstract string Master_Declaration(SyncDirection direction);
		public abstract string Master_GetterSetter(string dirtyBitname, int memberIndex);
		public abstract string Master_SerializeByWriter();
		public abstract string Master_CheckDirty(); // Object Only
		public abstract string Master_ClearDirty(); // Object Only

		public abstract string Remote_Declaration(SyncDirection direction);
		public abstract string Remote_DeserializeByReader();
	}
}