using System.Text;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class SyncObjectMemberToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => false;
		private bool _isPredefined = false;
		private bool _isBidirectionSync = false;

		public SyncObjectMemberToken(SyncType syncType, InheritType inheritType,
									 string typeName, string memberName, bool isPublic, bool isPredefined, bool isBidirectionSync)
			: base(syncType, inheritType, typeName, memberName, isPublic)
		{
			_isPredefined = isPredefined;
			_isBidirectionSync = isBidirectionSync;
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
								 _privateMemberName, MemberFormat.NewInitializer, _privateAccessModifier);
		}

		public override string Master_GetterSetter(SyncType syncType, string dirtyBitname, int memberIndex)
		{
			if (!IsPublic)
				return string.Empty;

			if (syncType.IsUnreliable() && _syncType == SyncType.ReliableOrUnreliable)
				return string.Empty;

			return string.Format(MemberFormat.ObjectGetter, AccessModifier, _typeName, _publicMemberName, _privateMemberName);
		}

		public override string Master_SerializeByWriter(SyncType syncType, SyncDirection direction, string dirtyBitname, int dirtyBitIndex)
		{
			if (syncType == SyncType.None)
			{
				return string.Format(MemberFormat.WriteSyncObjectEntire,
									 _privateMemberName,
									 SyncGroupFormat.EntireFunctionSuffix);
			}

			if (direction == SyncDirection.FromRemote)
			{
				return string.Format(MemberFormat.WriteSyncObject, _privateMemberName, syncType);
			}

			if (SynchronizerGenerator.HasTarget(TypeName))
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
			string format;

			if (_isBidirectionSync)
			{
				if (IsPublic)
				{
					return string.Format(MemberFormat.RemoteReadonlyDeclarationAsPublic_NoDef,
										 _typeName, _publicMemberName, _privateAccessModifier);
				}
				else
				{
					return string.Format(MemberFormat.RemoteReadonlyDeclaration_NoDef,
										 _typeName, _publicMemberName, _privateAccessModifier);
				}
			}

			string attribute = MemberFormat.GetSyncObjectAttribute(_syncType, direction);
			format = IsPublic ?
				MemberFormat.RemoteReadonlyDeclarationAsPublic :
				MemberFormat.RemoteReadonlyDeclaration;
			return string.Format(format, attribute, _typeName, _privateMemberName,
								 _publicMemberName, MemberFormat.NewInitializer, AccessModifier, _privateAccessModifier);
		}

		public override string Remote_DeserializeByReader(SyncType syncType, SyncDirection direction)
		{
			StringBuilder sb = new StringBuilder();
			if (syncType == SyncType.None)
				sb.AppendLine(string.Format(MemberFormat.ReadSyncObjectEntire, _privateMemberName));
			else if (direction == SyncDirection.FromMaster)
			{
				sb.AppendLine(string.Format(MemberFormat.ReadSyncObjectWithPlayer, _privateMemberName, syncType));
			}
			else if (direction == SyncDirection.FromRemote)
			{
				sb.AppendLine(string.Format(MemberFormat.ReadSyncObject, _privateMemberName, syncType));
			}

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