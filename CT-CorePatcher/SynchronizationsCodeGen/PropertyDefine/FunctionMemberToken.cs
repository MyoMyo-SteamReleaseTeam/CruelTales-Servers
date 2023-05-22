using System.Collections.Generic;
using CT.Common.Synchronizations;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class FunctionMemberToken : BaseMemberToken
	{
		private string _functionName;
		private SyncArgumentGroup _argGroup;

		public FunctionMemberToken(SyncType syncType, string functionName, bool isPublic, List<BaseArgument> args)
			: base(syncType, string.Empty, functionName, isPublic)
		{
			_syncType = syncType;
			_functionName = functionName;
			_argGroup = new SyncArgumentGroup(args);
		}

		public override string Master_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncRpcAttribute(_syncType, direction);
			return string.Format(FuncMemberFormat.Declaration,
								 attribute, AccessModifier, _functionName,
								 _argGroup.GetParameterDeclaration());
		}

		public override string Master_GetterSetter(string dirtyBitname, int memberIndex)
		{
			if (_argGroup.Count == 0)
			{
				return string.Format(FuncMemberFormat.CallWithStackVoid, AccessModifier,
									 _functionName, dirtyBitname, memberIndex);
			}

			return string.Format(FuncMemberFormat.CallWithStack,
								 AccessModifier,
								 _functionName,
								 _argGroup.GetParameterDeclaration(),
								 _argGroup.GetTupleEnqueueValue(),
								 _argGroup.GetTupleDeclaration(),
								 dirtyBitname, memberIndex);
		}

		public override string Master_SerializeByWriter(SyncType syncType)
		{
			if (_argGroup.Count == 0)
				return string.Format(FuncMemberFormat.SerializeIfDirtyVoid, _functionName);

			string content = _argGroup.GetWriteParameterContent();
			CodeFormat.AddIndent(ref content);
			return string.Format(FuncMemberFormat.SerializeIfDirty, _functionName, content);
		}

		public override string Master_CheckDirty(SyncType syncType) => string.Empty;

		public override string Master_ClearDirty(SyncType syncType)
		{
			return string.Format(FuncMemberFormat.ClearCallStack, _functionName);
		}

		public override string Remote_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncRpcAttribute(_syncType, direction);
			return string.Format(FuncMemberFormat.Declaration,
								 attribute, AccessModifier, _functionName,
								 _argGroup.GetParameterDeclaration());
		}

		public override string Remote_DeserializeByReader(SyncType syncType)
		{
			if (_argGroup.Count == 0)
				return string.Format(FuncMemberFormat.DeserializeIfDirtyVoid, _functionName);

			string paramContent = _argGroup.GetReadParameterContent();
			CodeFormat.AddIndent(ref paramContent);

			return string.Format(FuncMemberFormat.DeserializeIfDirty,
								 _functionName,
								 paramContent,
								 _argGroup.GetCallParameters());
		}

		public override string Remote_IgnoreDeserialize(SyncType syncType)
		{
			if (_argGroup.Count == 0)
				return FuncMemberFormat.IgnoreVoid;

			string paramContent = _argGroup.GetIgnoreParameterContent();
			CodeFormat.AddIndent(ref paramContent);

			return string.Format(FuncMemberFormat.IgnoreFunction, paramContent);
		}
	}
}