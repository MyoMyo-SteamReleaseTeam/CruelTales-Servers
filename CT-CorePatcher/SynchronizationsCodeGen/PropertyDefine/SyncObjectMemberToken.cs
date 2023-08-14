using System.Text;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class SyncObjectMemberToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => false;
		private string _constructorContent;
		private bool _isPredefined = false;
		private bool _isBidirectionSync = false;

		public string GetTypeName(CodeGenDirection genDirection)
		{
#pragma warning disable CA1416
			if (MainProcess.IsDebug)
#pragma warning restore CA1416
			{
				if (NameTable.IsSyncObjCollectionType(_typeName) &&
					genDirection == CodeGenDirection.Master)
				{
					return $"Synchronizations.{_typeName}";
				}
			}

			return _typeName;
		}

		public SyncObjectMemberToken(SyncType syncType, InheritType inheritType,
									 string typeName, string memberName, string constructorContent,
									 bool isPublic, bool isPredefined, bool isBidirectionSync)
			: base(syncType, inheritType, typeName, memberName, isPublic)
		{
			_constructorContent = constructorContent;
			_isPredefined = isPredefined;
			_isBidirectionSync = isBidirectionSync;
			_syncType = syncType;
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
		}

		public string Master_Constructor()
		{
			return string.Format(MemberFormat.Constructor, _privateMemberName, _constructorContent);
		}

		public override string Master_InitializeProperty(SyncDirection direction)
		{
			string dirStr = NameTable.GetDirectionStringBy(direction);
			return string.Format(MemberFormat.InitializeSyncObjectProperty, _privateMemberName, dirStr);
		}

		public override string Master_Declaration(CodeGenDirection genDirection, SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncObjectAttribute(_syncType, direction);
			return string.Format(MemberFormat.MasterReadonlyDeclaration, attribute, GetTypeName(genDirection),
								 _privateMemberName, _privateAccessModifier);
		}

		public override string Master_GetterSetter(SyncType syncType, string dirtyBitname, int memberIndex)
		{
			if (!IsPublic)
				return string.Empty;

			if (syncType.IsUnreliable() && _syncType == SyncType.ReliableOrUnreliable)
				return string.Empty;

#pragma warning disable CA1416
			// Collection과 같은 미리 정의된 함수의 경우 namespace 문제로 테스트시 동작하지 않음
			if (MainProcess.IsDebug && _isPredefined)
				return string.Empty;
#pragma warning restore CA1416

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

			if (direction == SyncDirection.FromRemote || 
				NameTable.IsValueCollectionType(_typeName))
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

		public override string Remote_Declaration(CodeGenDirection genDirection, SyncDirection direction)
		{
			string format;

			if (_isBidirectionSync)
			{
				if (IsPublic)
				{
					return string.Format(MemberFormat.RemoteReadonlyDeclarationAsPublic_NoDef,
										 GetTypeName(genDirection), _publicMemberName, _privateAccessModifier);
				}
				else
				{
					return string.Format(MemberFormat.RemoteReadonlyDeclaration_NoDef,
										 GetTypeName(genDirection), _publicMemberName, _privateAccessModifier);
				}
			}

			string attribute = MemberFormat.GetSyncObjectAttribute(_syncType, direction);
			format = IsPublic ?
				MemberFormat.RemoteReadonlyDeclarationAsPublic :
				MemberFormat.RemoteReadonlyDeclaration;
			return string.Format(format, attribute, GetTypeName(genDirection), _privateMemberName,
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
#pragma warning disable CA1416
			// Collection과 같은 미리 정의된 함수의 경우 namespace 문제로 테스트시 동작하지 않음
			if (MainProcess.IsDebug && _isPredefined)
				return string.Empty;
#pragma warning restore CA1416

			if (isStatic)
				return string.Format(MemberFormat.IgnoreObjectTypeStatic, _typeName, syncType);
			else
				return string.Format(MemberFormat.IgnoreObjectType, _privateMemberName, syncType);
		}
	}
}