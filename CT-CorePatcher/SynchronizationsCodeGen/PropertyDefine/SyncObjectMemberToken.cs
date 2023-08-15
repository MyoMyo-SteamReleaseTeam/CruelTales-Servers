using System;
using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.Exceptions;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class SyncObjectMemberToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => false;
		private string _constructorContent;
		private bool _isPredefined = false;
		public bool IsBidirectionSync => _attributeSyncDirection == SyncDirection.Bidirection;
		private SyncDirection _attributeSyncDirection;
		private Type _ownerType;

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
									 bool isPublic, bool isPredefined,
									 SyncDirection attributeSyncDirection,
									 Type ownerType)
			: base(syncType, inheritType, typeName, memberName, isPublic)
		{
			_constructorContent = constructorContent;
			if (!string.IsNullOrEmpty(_constructorContent))
			{
				_constructorContent = ", " + _constructorContent;
			}
			_isPredefined = isPredefined;
			_syncType = syncType;
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
			_attributeSyncDirection = attributeSyncDirection;
			_ownerType = ownerType;
		}

		private void checkSyncDirectionValidation()
		{
			if (SynchronizerGenerator.TryGetGenericObjSyncDirection(_typeName, out SyncDirection correctDir))
			{
				if (_attributeSyncDirection != correctDir)
				{
					throw new WrongSyncSetting($"Wrong sync direction in {_ownerType.Name}. \"{_typeName}\" " +
						$"should be {correctDir} but current is {_attributeSyncDirection}!");
				}
			}

		}

		public string Master_Constructor()
		{
			checkSyncDirectionValidation();
			return string.Format(MemberFormat.ConstructorWithOwner, _privateMemberName, _constructorContent);
		}

		public override string Master_InitializeProperty(GenOption option)
		{
			string dirStr = NameTable.GetDirectionStringBy(option.Direction);
			return string.Format(MemberFormat.InitializeSyncObjectProperty, _privateMemberName, dirStr);
		}

		public override string Master_Declaration(GenOption option)
		{
			string attribute = MemberFormat.GetSyncObjectAttribute(_syncType, _attributeSyncDirection);
			return string.Format(MemberFormat.MasterReadonlyDeclaration, attribute, GetTypeName(option.GenDirection),
								 _privateMemberName, _privateAccessModifier);
		}

		public override string Master_GetterSetter(GenOption option, string dirtyBitname, int memberIndex)
		{
			if (!IsPublic)
				return string.Empty;

			if (option.SyncType.IsUnreliable() && _syncType == SyncType.ReliableOrUnreliable)
				return string.Empty;

#pragma warning disable CA1416
			// Collection과 같은 미리 정의된 함수의 경우 namespace 문제로 테스트시 동작하지 않음
			if (MainProcess.IsDebug && _isPredefined)
				return string.Empty;
#pragma warning restore CA1416

				return string.Format(MemberFormat.ObjectGetter, AccessModifier, _typeName, _publicMemberName, _privateMemberName);
		}

		public override string Master_SerializeByWriter(GenOption option, string dirtyBitname, int dirtyBitIndex)
		{
			if (option.SyncType == SyncType.None)
			{
				return string.Format(MemberFormat.WriteSyncObjectEntire,
									 _privateMemberName,
									 SyncGroupFormat.EntireFunctionSuffix);
			}

			if (option.Direction == SyncDirection.FromRemote || 
				NameTable.IsValueCollectionType(_typeName))
			{
				return string.Format(MemberFormat.WriteSyncObject, _privateMemberName, option.SyncType);
			}

			if (SynchronizerGenerator.HasTarget(TypeName))
			{
				return string.Format(MemberFormat.WriteSyncObjectWithPlayerAndRollback,
									 _privateMemberName, option.SyncType,
									 dirtyBitname, dirtyBitIndex);
			}
			else
			{
				return string.Format(MemberFormat.WriteSyncObjectWithPlayer, _privateMemberName, option.SyncType);
			}
		}

		public string Master_IsDirty(GenOption option)
		{
			return string.Format(MemberFormat.IsDirty, _privateMemberName, option.SyncType);
		}

		public override string Master_CheckDirty(GenOption option)
		{
			return string.Format(MemberFormat.IsDirtyBinder, _privateMemberName, option.SyncType);
		}

		public override string Master_ClearDirty(GenOption option)
		{
			return string.Format(MemberFormat.ClearDirty, _privateMemberName, option.SyncType);
		}

		public string Remote_Constructor()
		{
			checkSyncDirectionValidation();
			if (IsBidirectionSync)
				return string.Empty;

			return string.Format(MemberFormat.ConstructorWithOwner, _privateMemberName, _constructorContent);
		}

		public override string Remote_InitializeProperty(GenOption option)
		{
			string dirStr = NameTable.GetDirectionStringBy(option.Direction);
			return string.Format(MemberFormat.InitializeSyncObjectProperty, _privateMemberName, dirStr);
		}

		public override string Remote_Declaration(GenOption option)
		{
			string format;

			if (IsBidirectionSync)
			{
				if (IsPublic)
				{
					return string.Format(MemberFormat.RemoteReadonlyDeclarationAsPublic_NoDef,
										 GetTypeName(option.GenDirection), _publicMemberName, _privateAccessModifier);
				}
				else
				{
					return string.Format(MemberFormat.RemoteReadonlyDeclaration_NoDef,
										 GetTypeName(option.GenDirection), _publicMemberName, _privateAccessModifier);
				}
			}

			string attribute = MemberFormat.GetSyncObjectAttribute(_syncType, _attributeSyncDirection);
			format = IsPublic ?
				MemberFormat.RemoteReadonlyDeclarationAsPublic :
				MemberFormat.RemoteReadonlyDeclaration;
			return string.Format(format, attribute, GetTypeName(option.GenDirection), _privateMemberName,
								 _publicMemberName, string.Empty, AccessModifier, _privateAccessModifier);
		}

		public override string Remote_DeserializeByReader(GenOption option)
		{
			StringBuilder sb = new StringBuilder();
			if (option.SyncType == SyncType.None)
				sb.AppendLine(string.Format(MemberFormat.ReadSyncObjectEntire, _privateMemberName));
			else if (option.Direction == SyncDirection.FromMaster)
			{
				sb.AppendLine(string.Format(MemberFormat.ReadSyncObjectWithPlayer, _privateMemberName, option.SyncType));
			}
			else if (option.Direction == SyncDirection.FromRemote)
			{
				sb.AppendLine(string.Format(MemberFormat.ReadSyncObject, _privateMemberName, option.SyncType));
			}

			sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			return sb.ToString();
		}

		public override string Remote_IgnoreDeserialize(GenOption option, bool isStatic)
		{
#pragma warning disable CA1416
			// Collection과 같은 미리 정의된 함수의 경우 namespace 문제로 테스트시 동작하지 않음
			if (MainProcess.IsDebug && _isPredefined)
				return string.Empty;
#pragma warning restore CA1416

			if (isStatic)
				return string.Format(MemberFormat.IgnoreObjectTypeStatic, _typeName, option.SyncType);
			else
				return string.Format(MemberFormat.IgnoreObjectType, _privateMemberName, option.SyncType);
		}
	}
}