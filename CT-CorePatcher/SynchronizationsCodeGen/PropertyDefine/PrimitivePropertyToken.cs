using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class PrimitivePropertyToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => false;
		private string _clrTypeName;

		public PrimitivePropertyToken(SyncType syncType, InheritType inheritType,
									  string typeName, string memberName, string clrTypeName, bool isPublic)
			: base(syncType, inheritType, typeName, memberName, isPublic)
		{
			_syncType = syncType;
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
			_clrTypeName = clrTypeName;
		}

		public override string Master_InitializeProperty(GenOption option)
		{
			string initializeValue = _clrTypeName == "Boolean" ? "false" : "0";
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, initializeValue);
		}

		public override string Master_Declaration(GenOption option)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, option.Direction);
			return string.Format(MemberFormat.MasterDeclaration, attribute, _typeName, _privateMemberName,
								 string.Empty, _privateAccessModifier);
		}

		public override string Master_GetterSetter(GenOption option, string dirtyBitname, int memberIndex)
		{
			return string.Format(MemberFormat.GetterSetter, AccessModifier, _typeName, _publicMemberName,
								 _privateMemberName, dirtyBitname, memberIndex, option.SyncType);
		}

		public override string Master_SerializeByWriter(GenOption option, string dirtyBitname, int dirtyBitIndex)
		{
			return string.Format(MemberFormat.WritePut, _privateMemberName);
		}

		public override string Master_CheckDirty(GenOption option) => string.Empty;

		public override string Master_ClearDirty(GenOption option) => string.Empty;

		public override string Remote_InitializeProperty(GenOption option)
		{
			string initializeValue = _clrTypeName == "Boolean" ? "false" : "0";
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, initializeValue);
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
			sb.AppendLine(string.Format(MemberFormat.ReadEmbededTypeProperty, _privateMemberName, _clrTypeName));
			if (option.HasCallback)
			{
				sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			}
			return sb.ToString();
		}

		public override string Remote_IgnoreDeserialize(GenOption option, bool isStatic)
		{
			return string.Format(MemberFormat.IgnorePrimitive,
								 ReflectionHelper.GetByteSizeByTypeName(_typeName));
		}
	}
}