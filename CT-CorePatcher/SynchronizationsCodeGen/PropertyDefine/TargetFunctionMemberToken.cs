using System.Collections.Generic;
using CT.Common.Synchronizations;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class TargetFunctionMemberToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => true;
		private string _functionName;
		private SyncArgumentGroup _argGroup;

		public TargetFunctionMemberToken(SyncType syncType, string functionName, bool isPublic, List<BaseArgument> args)
			: base(syncType, string.Empty, functionName, isPublic)
		{
			_syncType = syncType;
			_functionName = functionName;
			_argGroup = new SyncArgumentGroup(args);
		}

		public override string Master_InitializeProperty()
		{
			return string.Empty;
		}

		public override string Master_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncRpcAttribute(_syncType, direction);
			if (_argGroup.Count == 0)
				return string.Format(FuncMemberFormat.TargetDeclarationVoid,
									 attribute, AccessModifier, _functionName);

			return string.Format(FuncMemberFormat.TargetDeclaration,
								 attribute, AccessModifier, _functionName,
								 _argGroup.GetParameterDeclaration());
		}

		public override string Master_GetterSetter(string dirtyBitname, int memberIndex)
		{
			if (_argGroup.Count == 0)
			{
				return string.Format(FuncMemberFormat.TargetCallWithStackVoid, AccessModifier,
									 _functionName, dirtyBitname, memberIndex);
			}

			return string.Format(FuncMemberFormat.TargetCallWithStack,
								 AccessModifier,
								 _functionName,
								 _argGroup.GetParameterDeclaration(),
								 _argGroup.GetTupleEnqueueValue(),
								 _argGroup.GetTupleDeclaration(),
								 dirtyBitname, memberIndex);
		}

		public override string Master_SerializeByWriter(SyncType syncType, string dirtyBitname, int dirtyBitIndex)
		{
			if (_argGroup.Count == 0)
				return string.Format(FuncMemberFormat.TargetSerializeIfDirtyVoid, _functionName, dirtyBitname, dirtyBitIndex);

			string content = _argGroup.GetWriteParameterContent();
			CodeFormat.AddIndent(ref content, 2);
			return string.Format(FuncMemberFormat.TargetSerializeIfDirty, _functionName, content, dirtyBitname, dirtyBitIndex);
		}

		public override string Master_CheckDirty(SyncType syncType) => string.Empty;

		public override string Master_ClearDirty(SyncType syncType)
		{
			return string.Format(FuncMemberFormat.ClearCallStack, _functionName);
		}

		public override string Remote_InitializeProperty()
		{
			return string.Empty;
		}

		public override string Remote_Declaration(SyncDirection direction)
		{
			string attribute = MemberFormat.GetSyncRpcAttribute(_syncType, direction);
			string format = string.Empty;

			if (direction == SyncDirection.FromRemote)
			{
				format = _argGroup.Count == 0 ?
					FuncMemberFormat.TargetDeclarationVoid :
					FuncMemberFormat.DeclarationFromRemote;
			}
			else if (direction == SyncDirection.FromMaster)
			{
				format = FuncMemberFormat.Declaration;
			}

			return string.Format(format, attribute, AccessModifier, _functionName,
								 _argGroup.GetParameterDeclaration());
		}

		public override string Remote_DeserializeByReader(SyncType syncType, SyncDirection direction)
		{
			if (_argGroup.Count == 0)
			{
				string format = string.Empty;

				if (direction == SyncDirection.FromRemote)
					format = FuncMemberFormat.DeserializeIfDirtyVoid;
				else if (direction == SyncDirection.FromMaster)
					format = FuncMemberFormat.TargetDeserializeIfDirtyVoid;

				return string.Format(format, _functionName);
			}

			string paramContent = _argGroup.GetReadParameterContent();
			CodeFormat.AddIndent(ref paramContent);
			string callParameters = _argGroup.GetCallParameters();
			if (direction == SyncDirection.FromMaster)
			{
				callParameters = NameTable.NetworkPlayerParameterName + ", " + callParameters;
			}

			return string.Format(FuncMemberFormat.DeserializeIfDirty,
								 _functionName, paramContent, callParameters);
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