using System.Text;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class SyncObjectMemberToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => false;
		private bool _isCollection = false;

		public SyncObjectMemberToken(SyncType syncType, string typeName, string memberName, bool isPublic, bool isCollection)
			: base(syncType, typeName, memberName, isPublic)
		{
			_isCollection = isCollection;
			_syncType = syncType;
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
		}

		public override string Master_InitializeProperty(SyncDirection direction)
		{
			string dirStr = NameTable.GetDirectionStringBy(direction);
			return string.Format(MemberFormat.InitializeSyncObjectProperty, _privateMemberName, dirStr);
		}

		public override string Master_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncObjectAttribute(_syncType, direction);
			return string.Format(MemberFormat.MasterReadonlyDeclaration, attribute, _typeName,
								 _privateMemberName, MemberFormat.NewInitializer);
		}

		public override string Master_GetterSetter(SyncType syncType, string dirtyBitname, int memberIndex)
		{
			if (!IsPublic)
				return string.Empty;

			if (syncType.IsUnreliable() && _syncType == SyncType.ReliableOrUnreliable)
				return string.Empty;

			return string.Format(MemberFormat.ObjectGetter, AccessModifier, _typeName, _publicMemberName, _privateMemberName);
		}

		public override string Master_SerializeByWriter(SyncType syncType, string dirtyBitname, int dirtyBitIndex)
		{
			if (syncType == SyncType.None)
			{
				return string.Format(MemberFormat.WriteSyncObjectEntire,
									 _privateMemberName,
									 SyncGroupFormat.EntireFunctionSuffix);
			}

			if (_isCollection)
			{
				return string.Format(MemberFormat.WriteSyncObject, _privateMemberName, syncType);
			}

			SynchronizerGenerator.TryGetSyncObjectByTypeName(TypeName, out var syncObj);
			if (syncObj == null)
			{
				throw new System.Exception($"There is no such sync object type : {TypeName}");
			}

			if (syncObj.HasTarget)
			{
				return string.Format(MemberFormat.WriteSyncObjectWithPlayerAndRollback,
									 _privateMemberName, syncType,
									 dirtyBitname, dirtyBitIndex);
			}
			else
			{
				return string.Format(MemberFormat.WriteSyncObjectWithPlayer, _privateMemberName, syncType);
			}
		}

		public string Master_IsDirty(SyncType syncType)
		{
			return string.Format(MemberFormat.IsDirty, _privateMemberName, syncType);
		}

		public override string Master_CheckDirty(SyncType syncType)
		{
			return string.Format(MemberFormat.IsDirtyBinder, _privateMemberName, syncType);
		}

		public override string Master_ClearDirty(SyncType syncType)
		{
			return string.Format(MemberFormat.ClearDirty, _privateMemberName, syncType);
		}

		public override string Remote_InitializeProperty(SyncDirection direction)
		{
			string dirStr = NameTable.GetDirectionStringBy(direction);
			return string.Format(MemberFormat.InitializeSyncObjectProperty, _privateMemberName, dirStr);
		}

		public override string Remote_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncObjectAttribute(_syncType, direction);
			string format = IsPublic ? MemberFormat.RemoteReadonlyDeclarationAsPublic : MemberFormat.RemoteReadonlyDeclaration;
			return string.Format(format, attribute, _typeName, _privateMemberName,
								 _publicMemberName, MemberFormat.NewInitializer, AccessModifier);
		}

		public override string Remote_DeserializeByReader(SyncType syncType, SyncDirection direction)
		{
			StringBuilder sb = new StringBuilder();
			if (syncType == SyncType.None)
				sb.AppendLine(string.Format(MemberFormat.ReadSyncObjectEntire, _privateMemberName));
			else
				sb.AppendLine(string.Format(MemberFormat.ReadSyncObject, _privateMemberName, syncType));
			sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			return sb.ToString();
		}

		public override string Remote_IgnoreDeserialize(SyncType syncType, bool isStatic)
		{
			if (isStatic)
				return string.Format(MemberFormat.IgnoreObjectTypeStatic, _typeName, syncType);
			else
				return string.Format(MemberFormat.IgnoreObjectType, _privateMemberName, syncType);
		}
	}
}