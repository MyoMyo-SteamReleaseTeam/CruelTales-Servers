using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class EnumMemberToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => false;
		private string _enumSizeTypeName;
		private string _clrEnumSizeTypeName;

		public EnumMemberToken(SyncType syncType, InheritType inheritType,
							   string typeName, string memberName, bool isPublic,
							   string enumSizeTypeName, string clrEnumSizeTypeName)
			: base(syncType, inheritType, typeName, memberName, isPublic)
		{
			_enumSizeTypeName = enumSizeTypeName;
			_clrEnumSizeTypeName = clrEnumSizeTypeName;
		}

		public override string Master_InitializeProperty(GenOption option)
		{
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, $"({_typeName})0");
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
			return string.Format(MemberFormat.WriteEnum, _enumSizeTypeName, _privateMemberName);
		}

		public override string Master_CheckDirty(GenOption option) => string.Empty;
		public override string Master_ClearDirty(GenOption option) => string.Empty;

		public override string Remote_InitializeProperty(GenOption option)
		{
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, $"({_typeName})0");
		}

		public override string Remote_Declaration(GenOption option)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, option.Direction);
			string format = IsPublic ? MemberFormat.RemoteDeclarationAsPublic : MemberFormat.RemoteDeclaration;
			return string.Format(format, attribute, _typeName, _privateMemberName,
								 _publicMemberName, string.Empty, AccessModifier, _privateAccessModifier);
		}

		public override string Remote_DeserializeByReader(GenOption option)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format(MemberFormat.ReadEnum, _privateMemberName, _typeName, _clrEnumSizeTypeName));
			if (option.HasCallback)
			{
				sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			}
			return sb.ToString();
		}

		public override string Remote_IgnoreDeserialize(GenOption option, bool isStatic)
		{
			return string.Format(MemberFormat.IgnorePrimitive,
								 ReflectionHelper.GetByteSizeByTypeName(_enumSizeTypeName));
		}
	}
}