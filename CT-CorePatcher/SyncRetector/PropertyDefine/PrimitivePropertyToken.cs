using System.Text;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector.PropertyDefine
{
	public class PrimitivePropertyToken : BaseMemberToken
	{
		private string _clrTypeName;

		public PrimitivePropertyToken(SyncType syncType, string typeName, string memberName, string clrTypeName, bool isPublic)
			: base(syncType, typeName, memberName, isPublic)
		{
			_syncType = syncType;
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
			_clrTypeName = clrTypeName;
		}

		public override string Master_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, direction);
			return string.Format(MemberFormat.MasterDeclaration, attribute, _typeName, _privateMemberName, string.Empty);
		}

		public override string Master_GetterSetter(string dirtyBitname, int memberIndex)
		{
			return string.Format(MemberFormat.GetterSetter, AccessModifier, _typeName, _publicMemberName,
								 _privateMemberName, dirtyBitname, memberIndex);
		}

		public override string Master_SerializeByWriter()
		{
			return string.Format(MemberFormat.WritePut, _privateMemberName);
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
			sb.AppendLine(string.Format(MemberFormat.ReadEmbededTypeProperty, _privateMemberName , _clrTypeName));
			sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			return sb.ToString();
		}
	}
}