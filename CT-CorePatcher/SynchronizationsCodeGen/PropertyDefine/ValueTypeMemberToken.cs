using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class ValueTypeMemberToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => false;
		public bool IsNativeStruct { get; private set; }

		public ValueTypeMemberToken(SyncType syncType, string typeName, string memberName, bool isPublic)
			: base(syncType, typeName, memberName, isPublic)
		{
			_syncType = syncType;
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
			IsNativeStruct = ReflectionHelper.IsNativeStruct(_typeName);
		}

		public override string Master_InitializeProperty(SyncDirection direction)
		{
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, @"new()");
		}

		public override string Master_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, direction);
			return string.Format(MemberFormat.MasterDeclaration, attribute, _typeName,
								 _privateMemberName, MemberFormat.NewInitializer);
		}

		public override string Master_GetterSetter(SyncType syncType, string dirtyBitname, int memberIndex)
		{
			return string.Format(MemberFormat.GetterSetter, AccessModifier, _typeName, _publicMemberName,
								 _privateMemberName, dirtyBitname, memberIndex);
		}

		public override string Master_SerializeByWriter(SyncType syncType, string dirtyBitname, int dirtyBitIndex)
		{
			string format = IsNativeStruct ? MemberFormat.WritePut : MemberFormat.WriteSerialize;
			return string.Format(format, _privateMemberName);
		}

		public override string Master_CheckDirty(SyncType syncType) => string.Empty;
		public override string Master_ClearDirty(SyncType syncType) => string.Empty;

		public override string Remote_InitializeProperty(SyncDirection direction)
		{
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, @"new()");
		}

		public override string Remote_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, direction);
			string format = IsPublic ? MemberFormat.RemoteDeclarationAsPublic : MemberFormat.RemoteDeclaration;
			return string.Format(format, attribute, _typeName, _privateMemberName,
								 _publicMemberName, MemberFormat.NewInitializer, AccessModifier);
		}

		public override string Remote_DeserializeByReader(SyncType syncType, SyncDirection direction)
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
			sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			return sb.ToString();
		}

		public override string Remote_IgnoreDeserialize(SyncType syncType, bool isStatic)
		{
			string dataTypeName = IsNativeStruct ? _typeName + "Extension" : _typeName;
			return string.Format(MemberFormat.IgnoreValueType, dataTypeName);
		}
	}
}