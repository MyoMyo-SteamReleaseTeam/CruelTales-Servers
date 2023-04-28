using CT.Common.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class SyncObjectMemberToken : BaseMemberToken
	{
		public SyncObjectMemberToken(SyncType syncType, string typeName, string memberName, bool isPublic)
			: base(syncType, typeName, memberName, isPublic)
		{
			_syncType = syncType;
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
		}

		public override string Master_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncObjectAttribute(_syncType, direction);
			return string.Format(MemberFormat.MasterDeclaration, attribute, _typeName,
								 _privateMemberName, MemberFormat.NewInitializer);
		}

		public override string Master_GetterSetter(string dirtyBitname, int memberIndex) => string.Empty;

		public override string Master_SerializeByWriter()
		{
			return string.Format(MemberFormat.WriteSyncObject, _privateMemberName, _syncType);
		}

		public override string Master_CheckDirty()
		{
			return string.Format(MemberFormat.IsDirtyBinder, _privateMemberName, _syncType);
		}

		public override string Master_ClearDirty()
		{
			return string.Format(MemberFormat.ClearDirty, _privateMemberName, _syncType);
		}

		public override string Remote_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncObjectAttribute(_syncType, direction);
			return string.Format(MemberFormat.RemoteDeclaration, attribute, _typeName,
								 _privateMemberName, _publicMemberName, string.Empty);
		}

		public override string Remote_DeserializeByReader()
		{
			return string.Format(MemberFormat.ReadSyncObject, _privateMemberName, _syncType);
		}
	}
}