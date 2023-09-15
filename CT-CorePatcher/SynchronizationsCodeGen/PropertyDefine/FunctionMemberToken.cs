using System;
using System.Collections.Generic;
using CT.Common.Synchronizations;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine
{
	public class FunctionMemberToken : BaseMemberToken
	{
		public override bool ShouldRollBackMask => false;
		private string _functionName;
		private string _callstackName = string.Empty;
		private SyncArgumentGroup _argGroup;

		public FunctionMemberToken(SyncType syncType, InheritType inheritType,
								   string functionName, bool isPublic, List<BaseArgument> args)
			: base(syncType, inheritType, string.Empty, functionName, isPublic)
		{
			_syncType = syncType;
			_functionName = functionName;
			_callstackName += functionName;
			foreach (var arg in args)
			{
				_callstackName += arg.TypeName[0];
			}

			_argGroup = new SyncArgumentGroup(args);
		}

		public override string Master_InitializeProperty(GenOption option)
		{
			return string.Empty;
		}

		public override string Master_Declaration(GenOption option)
		{
			if (option.Direction == SyncDirection.FromMaster)
			{
			}
			string attribute = MemberFormat.GetSyncRpcAttribute(_syncType, option.Direction);
			return string.Format(FuncMemberFormat.Declaration,
								 attribute, AccessModifier, _functionName,
								 _argGroup.GetParameterDeclaration(), string.Empty);
		}

		public override string Master_GetterSetter(GenOption option, string dirtyBitname, int memberIndex)
		{
			if (_argGroup.Count == 0)
			{
				return string.Format(FuncMemberFormat.CallWithStackVoid,
									 AccessModifier,
									 _functionName,
									 dirtyBitname,
									 memberIndex,
									 _privateAccessModifier,
									 _callstackName,
									 option.SyncType);
			}
			else
			{
				return string.Format(FuncMemberFormat.CallWithStack,
									 AccessModifier,
									 _functionName,
									 _argGroup.GetParameterDeclaration(),
									 _argGroup.GetTupleEnqueueValue(),
									 _argGroup.GetTupleDeclaration(),
									 dirtyBitname, memberIndex,
									 _privateAccessModifier,
									 _callstackName,
									 option.SyncType);
			}
		}

		public override string Master_SerializeByWriter(GenOption option, string dirtyBitname, int dirtyBitIndex)
		{
			if (_argGroup.Count == 0)
				return string.Format(FuncMemberFormat.SerializeIfDirtyVoid, _callstackName);

			string content = _argGroup.GetWriteParameterContent();
			CodeFormat.AddIndent(ref content);
			return string.Format(FuncMemberFormat.SerializeIfDirty, _callstackName, content);
		}

		public override string Master_CheckDirty(GenOption option) => string.Empty;

		public override string Master_ClearDirty(GenOption option)
		{
			if (_argGroup.Count == 0)
				return string.Format(FuncMemberFormat.ClearCallCount, _callstackName);

			return string.Format(FuncMemberFormat.ClearCallStack, _callstackName);
		}

		public override string Remote_InitializeProperty(GenOption option)
		{
			return string.Empty;
		}

		public override string Remote_Declaration(GenOption option)
		{
			string attribute = MemberFormat.GetSyncRpcAttribute(_syncType, option.Direction);
			string format = string.Empty;

			if (option.Direction == SyncDirection.FromRemote)
			{
				if (_argGroup.Count == 0)
				{
					return string.Format(FuncMemberFormat.TargetDeclarationVoid,
										 attribute, AccessModifier, _functionName, _inheritKeyword);
				}
				else
				{
					return string.Format(FuncMemberFormat.DeclarationFromRemote,
										 attribute, AccessModifier, _functionName,
										 _argGroup.GetParameterDeclaration(), _inheritKeyword);
				}
			}
			else if (option.Direction == SyncDirection.FromMaster)
			{
				return string.Format(FuncMemberFormat.Declaration,
									 attribute, AccessModifier, _functionName,
									 _argGroup.GetParameterDeclaration(), _inheritKeyword);
			}

			throw new ArgumentOutOfRangeException($"There is no match declaration format in function : {PublicMemberName}");
		}

		public override string Remote_DeserializeByReader(GenOption option)
		{
			if (_argGroup.Count == 0)
			{
				string format = string.Empty;

				if (option.Direction == SyncDirection.FromRemote)
					format = FuncMemberFormat.DeserializeIfDirtyVoid;
				else if (option.Direction == SyncDirection.FromMaster)
					format = FuncMemberFormat.TargetDeserializeIfDirtyVoid;

				return string.Format(format, _functionName);
			}

			string paramContent = _argGroup.GetReadParameterContent();
			CodeFormat.AddIndent(ref paramContent);
			string callParameters = _argGroup.GetCallParameters();
			if (option.Direction == SyncDirection.FromMaster)
			{
				callParameters = NameTable.NetworkPlayerParameterName + ", " + callParameters;
			}

			return string.Format(FuncMemberFormat.DeserializeIfDirty,
								 _functionName, paramContent, callParameters);
		}

		public override string Remote_IgnoreDeserialize(GenOption option, bool isStatic)
		{
			if (_argGroup.Count == 0)
				return FuncMemberFormat.IgnoreVoid;

			string paramContent = _argGroup.GetIgnoreParameterContent();
			CodeFormat.AddIndent(ref paramContent);

			return string.Format(FuncMemberFormat.IgnoreFunction, paramContent);
		}
	}
}