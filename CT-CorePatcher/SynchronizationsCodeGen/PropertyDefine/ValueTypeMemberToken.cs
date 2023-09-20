using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class ValueTypeMemberToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => false;
		public bool IsNativeStruct { get; private set; }

		public ValueTypeMemberToken(SyncType syncType, InheritType inheritType,
									string typeName, string memberName, bool isPublic)
			: base(syncType, inheritType, typeName, memberName, isPublic)
		{
			_syncType = syncType;
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
			IsNativeStruct = ReflectionHelper.IsNativeStruct(_typeName);
		}

		public override string Master_InitializeProperty(GenOption option)
		{
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, @"new()");
		}

		public override string Master_Declaration(GenOption option)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, option.Direction);
			return string.Format(MemberFormat.MasterDeclaration, attribute, _typeName,
								 _privateMemberName, MemberFormat.NewInitializer, _privateAccessModifier);
		}

		public override string Master_GetterSetter(GenOption option, string dirtyBitname, int memberIndex)
		{
			return string.Format(MemberFormat.GetterSetter, AccessModifier, _typeName, _publicMemberName,
								 _privateMemberName, dirtyBitname, memberIndex, option.SyncType);
		}

		public override string Master_SerializeByWriter(GenOption option, string dirtyBitname, int dirtyBitIndex)
		{
			string format = IsNativeStruct ? MemberFormat.WritePut : MemberFormat.WriteSerialize;
			return string.Format(format, _privateMemberName);
		}

		public override string Master_CheckDirty(GenOption option) => string.Empty;
		public override string Master_ClearDirty(GenOption option) => string.Empty;

		public override string Remote_InitializeProperty(GenOption option)
		{
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, @"new()");
		}

		public override string Remote_Declaration(GenOption option)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, option.Direction);
			string format = IsPublic ? MemberFormat.RemoteDeclarationAsPublic : MemberFormat.RemoteDeclaration;
			return string.Format(format, attribute, _typeName, _privateMemberName,
								 _publicMemberName, MemberFormat.NewInitializer, AccessModifier, _privateAccessModifier);
		}

		public override string Remote_DeserializeByReader(GenOption option)
		{
			StringBuilder sb = new StringBuilder();
			if (IsNativeStruct)
			{
				sb.AppendLine(string.Format(MemberFormat.ReadEmbededTypeProperty, _privateMemberName, _typeName));
			}
			else
			{
				sb.AppendLine(string.Format(MemberFormat.ReadByDeserializer, _privateMemberName));
			}
			if (option.HasCallback)
			{
				sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			}
			return sb.ToString();
		}

		public override string Remote_IgnoreDeserialize(GenOption option, bool isStatic)
		{
			string dataTypeName = IsNativeStruct ? _typeName + "Extension" : _typeName;
			return string.Format(MemberFormat.IgnoreValueType, dataTypeName);
		}
	}
}