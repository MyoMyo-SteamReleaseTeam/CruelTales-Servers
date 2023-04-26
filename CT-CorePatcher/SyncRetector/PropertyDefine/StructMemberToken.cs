using System;
using System.Text;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector.PropertyDefine
{
	public class PrimitivePropertyToken : ISynchronizeMember
	{
		private string _typeName;
		private string _privateMemberName;
		private string _publicMemberName;
		private string _clrTypeName;

		public PrimitivePropertyToken(string typeName, string memberName, string clrTypeName)
		{
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
			_clrTypeName = clrTypeName;
		}

		public string Master_Declaration(SyncType syncType, SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(syncType, direction);
			return string.Format(MemberFormat.MasterDeclaration, attribute, _typeName, _privateMemberName, string.Empty);
		}

		public string Master_GetterSetter(string modifier, string dirtyBitname, int propertyIndex)
		{
			return string.Format(MemberFormat.GetterSetter,
								 modifier,
								 _publicMemberName,
								 _privateMemberName,
								 dirtyBitname,
								 propertyIndex);
		}

		public string Master_SerializeByWriter()
		{
			return string.Format(MemberFormat.WritePut, _privateMemberName);
		}

		public string Master_CheckDirty() => string.Empty;

		public string Master_ClearDirty() => string.Empty;

		public string Remote_Declaration(SyncType syncType, SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(syncType, direction);
			return string.Format(MemberFormat.RemoteDeclaration, attribute, _typeName,
								 _privateMemberName, _publicMemberName, string.Empty);
		}

		public string Remote_DeserializeByReader()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format(MemberFormat.ReadEmbededTypeProperty, _privateMemberName , _clrTypeName));
			sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			return sb.ToString();
		}
	}

	public class StructMemberToken : ISynchronizeMember
	{
		private string _typeName;
		private string _privateMemberName;
		private string _publicMemberName;

		public StructMemberToken(string typeName, string memberName)
		{
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
		}

		public string Master_Declaration(SyncType syncType, SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(syncType, direction);
			return string.Format(MemberFormat.MasterDeclaration, attribute, _typeName,
								 _privateMemberName, MemberFormat.NewInitializer);
		}

		public string Master_GetterSetter(string modifier, string dirtyBitname, int propertyIndex)
		{
			return string.Format(MemberFormat.GetterSetter, modifier, _publicMemberName,
								 _privateMemberName, dirtyBitname, propertyIndex);
		}

		public string Master_SerializeByWriter()
		{
			return string.Format(MemberFormat.WriteSerialize, _privateMemberName);
		}

		public string Master_CheckDirty() => string.Empty;
		public string Master_ClearDirty() => string.Empty;

		public string Remote_Declaration(SyncType syncType, SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(syncType, direction);
			return string.Format(MemberFormat.RemoteDeclaration, attribute, _typeName,
								 _privateMemberName, _publicMemberName, MemberFormat.NewInitializer);
		}

		public string Remote_DeserializeByReader()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format(MemberFormat.ReadByDeserializer, _privateMemberName));
			sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			return sb.ToString();
		}
	}

	public class EnumMemberToken : ISynchronizeMember
	{
		private string _typeName;
		private string _privateMemberName;
		private string _publicMemberName;
		private string _enumSizeTypeName;
		private string _clrEnumSizeTypeName;

		public EnumMemberToken(string typeName, string memberName, string enumSizeTypeName, string clrEnumSizeTypeName)
		{
			_typeName = typeName;
			_privateMemberName = MemberFormat.GetPrivateName(memberName);
			_publicMemberName = MemberFormat.GetPublicName(memberName);
			_enumSizeTypeName = enumSizeTypeName;
			_clrEnumSizeTypeName = clrEnumSizeTypeName;
		}

		public string Master_Declaration(SyncType syncType, SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(syncType, direction);
			return string.Format(MemberFormat.MasterDeclaration, attribute, _typeName,
								 _privateMemberName, MemberFormat.NewInitializer);
		}

		public string Master_GetterSetter(string modifier, string dirtyBitname, int propertyIndex)
		{
			return string.Format(MemberFormat.GetterSetter, modifier, _publicMemberName,
								 _privateMemberName, dirtyBitname, propertyIndex);
		}

		public string Master_SerializeByWriter()
		{
			return string.Format(MemberFormat.WriteEnum, _enumSizeTypeName, _privateMemberName);
		}

		public string Master_CheckDirty() => string.Empty;
		public string Master_ClearDirty() => string.Empty;

		public string Remote_Declaration(SyncType syncType, SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(syncType, direction);
			return string.Format(MemberFormat.RemoteDeclaration, attribute, _typeName,
								 _privateMemberName, _publicMemberName, string.Empty);
		}

		public string Remote_DeserializeByReader()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format(MemberFormat.ReadEnum, _privateMemberName, _enumSizeTypeName, _clrEnumSizeTypeName));
			sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			return sb.ToString();
		}
	}

	//public class SyncObjectMemberToken : ISynchronizeMember // TODO

	//public class FunctionMemberToken : ISynchronizeMember // TODO 
}