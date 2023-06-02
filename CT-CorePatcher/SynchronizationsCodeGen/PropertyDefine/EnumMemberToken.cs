﻿using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
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

		public override string Master_SerializeByWriter(SyncType syncType)
		{
			return string.Format(MemberFormat.WriteEnum, _enumSizeTypeName, _privateMemberName);
		}

		public override string Master_InitializeProperty()
		{
			return string.Format(MemberFormat.InitializeProperty, _privateMemberName, $"({_typeName})0");
		}

		public override string Master_CheckDirty(SyncType syncType) => string.Empty;
		public override string Master_ClearDirty(SyncType syncType) => string.Empty;

		public override string Remote_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncVarAttribute(_syncType, direction);
			return string.Format(MemberFormat.RemoteDeclaration, attribute, _typeName,
								 _privateMemberName, _publicMemberName, string.Empty);
		}

		public override string Remote_DeserializeByReader(SyncType syncType)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format(MemberFormat.ReadEnum, _privateMemberName, _typeName, _clrEnumSizeTypeName));
			sb.AppendLine(string.Format(MemberFormat.CallbackEvent, _publicMemberName, _privateMemberName));
			return sb.ToString();
		}

		public override string Remote_IgnoreDeserialize(SyncType syncType)
		{
			return string.Format(MemberFormat.IgnorePrimitive,
								 ReflectionHelper.GetByteSizeByTypeName(_enumSizeTypeName));
		}
	}
}