using System.Collections.Generic;
using CT.Common.Synchronizations;
using CT.CorePatcher.SyncRetector.PropertyDefine.FunctionArguments;

namespace CT.CorePatcher.SyncRetector.PropertyDefine
{
	public class FunctionMemberToken : BaseMemberToken
	{
		private string _functionName;
		private SychArgumentGroup _argGroup;

		public FunctionMemberToken(SyncType syncType, string functionName, bool isPublic, List<BaseArgument> args)
			: base(syncType, string.Empty, functionName, isPublic)
		{
			_syncType = syncType;
			_functionName = functionName;
			_argGroup = new SychArgumentGroup(args);
		}

		public override string Master_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncRpcAttribute(_syncType, direction);
			return string.Format(FuncMemberFormat.Declaration, attribute, _functionName, _argGroup.GetParameterDeclaration());
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

		public override string Master_SerializeByWriter()
		{
			return string.Format(FuncMemberFormat.SerializeIfDirty,
								 _functionName, _argGroup.GetWriteParameterContent());
		}

		public override string Master_CheckDirty() => string.Empty;
		public override string Master_ClearDirty() => string.Empty;

		public override string Remote_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncRpcAttribute(_syncType, direction);
			return string.Format(FuncMemberFormat.Declaration, attribute, _functionName,
								 _argGroup.GetParameterDeclaration());
		}

		public override string Remote_DeserializeByReader()
		{
			if (_argGroup.Count == 0)
			{
				return string.Format(FuncMemberFormat.DeserializeIfDirtyVoid, _functionName);
			}

			return string.Format(FuncMemberFormat.DeserializeIfDirty,
								 _functionName,
								 _argGroup.GetReadParameterContent(),
								 _argGroup.GetCallParameters());
		}
	}
}