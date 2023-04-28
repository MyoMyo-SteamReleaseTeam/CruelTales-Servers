using System.Text;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector.PropertyDefine
{
	public class EnumMemberToken : BaseMemberToken
	{
		private string _enumSizeTypeName;
		private string _clrEnumSizeTypeName;

		public EnumMemberToken(SyncType syncType, string typeName, string memberName, bool isPublic,
							   string enumSizeTypeName, string clrEnumSizeTypeName)
			: base(syncType, typeName, memberName, isPublic)
		{
			_enumSizeTypeName = enumSizeTypeName;
			_clrEnumSizeTypeName = clrEnumSizeTypeName;
		}

		public override string Master_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, direction);
			return string.Format(MemberFormat.MasterDeclaration, attribute, _typeName,
								 _privateMemberName, MemberFormat.NewInitializer);
		}

		public override string Master_GetterSetter(string dirtyBitname, int memberIndex)
		{
			return string.Format(MemberFormat.GetterSetter, AccessModifier, _typeName, _publicMemberName,
								 _privateMemberName, dirtyBitname, memberIndex);
		}

		public override string Master_SerializeByWriter()
		{
			return string.Format(MemberFormat.WriteEnum, _enumSizeTypeName, _privateMemberName);
		}

		public override string Master_CheckDirty() => string.Empty;
		public override string Master_ClearDirty() => string.Empty;

		public override string Remote_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, direction);
			return string.Format(MemberFormat.RemoteDeclaration, attribute, _typeName,
								 _privateMemberName, _publicMemberName, string.Empty);
		}

		public override string Remote_DeserializeByReader()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format(MemberFormat.ReadEnum, _privateMemberName, _enumSizeTypeName, _clrEnumSizeTypeName));
			sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			return sb.ToString();
		}
	}
}