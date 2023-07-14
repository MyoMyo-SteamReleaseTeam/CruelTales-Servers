using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class PrimitivePropertyToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => false;
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

		public override string Master_InitializeProperty(SyncDirection direction)
		{
			string initializeValue = _clrTypeName == "Boolean" ? "false" : "0";
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, initializeValue);
		}

		public override string Master_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, direction);
			return string.Format(MemberFormat.MasterDeclaration, attribute, _typeName, _privateMemberName, string.Empty);
		}

		public override string Master_GetterSetter(SyncType syncType, string dirtyBitname, int memberIndex)
		{
			return string.Format(MemberFormat.GetterSetter, AccessModifier, _typeName, _publicMemberName,
								 _privateMemberName, dirtyBitname, memberIndex);
		}

		public override string Master_SerializeByWriter(SyncType syncType, string dirtyBitname, int dirtyBitIndex)
		{
			return string.Format(MemberFormat.WritePut, _privateMemberName);
		}

		public override string Master_CheckDirty(SyncType syncType) => string.Empty;

		public override string Master_ClearDirty(SyncType syncType) => string.Empty;

		public override string Remote_InitializeProperty(SyncDirection direction)
		{
			string initializeValue = _clrTypeName == "Boolean" ? "false" : "0";
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, initializeValue);
		}

		public override string Remote_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, direction);
			string format = IsPublic ? MemberFormat.RemoteDeclarationAsPublic : MemberFormat.RemoteDeclaration;
			return string.Format(format, attribute, _typeName, _privateMemberName, 
								 _publicMemberName, string.Empty, AccessModifier);
		}

		public override string Remote_DeserializeByReader(SyncType syncType, SyncDirection direction)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format(MemberFormat.ReadEmbededTypeProperty, _privateMemberName, _clrTypeName));
			sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			return sb.ToString();
		}

		public override string Remote_IgnoreDeserialize(SyncType syncType, bool isStatic)
		{
			return string.Format(MemberFormat.IgnorePrimitive,
								 ReflectionHelper.GetByteSizeByTypeName(_typeName));
		}
	}
}