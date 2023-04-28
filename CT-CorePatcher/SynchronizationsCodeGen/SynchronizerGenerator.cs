using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CT.Common.Synchronizations;
using CT.Common.Tools.Data;
using CT.Common.Tools.GetOpt;
using CT.CorePatcher.Helper;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class GenOperation
	{
		public string TargetPath;
		public string FileName;
		public string Code;

		public GenOperation(string targetPath, string objectName, string code)
		{
			TargetPath = targetPath;
			FileName = objectName + ".cs";
			Code = code;
		}

		public bool TrySave()
		{
			string path = Path.Combine(TargetPath, FileName);
			var result = FileHandler.TryWriteText(path, Code, makeDirectory: true);
			if (result.ResultType == JobResultType.Success)
			{
				PatcherConsole.PrintSaveSuccessResult("Generate synchronization code success!", FileName, path);
				return true;
			}

			PatcherConsole.PrintError(FileName, result.Exception);
			return false;
		}
	}

	public class SynchronizerGenerator
	{
		public const string SYNC_MASTER_PATH = "syncMasterPath";
		public const string SYNC_REMOTE_PATH = "syncRemotePath";

		public void GenerateCode(string[] args)
		{
			StringArgumentArray masterTargetPathList = new();
			StringArgumentArray remoteTargetPathList = new();

			OptionParser op = new OptionParser();
			OptionParser.BindArgumentArray(op, SYNC_MASTER_PATH, 2, masterTargetPathList);
			OptionParser.BindArgumentArray(op, SYNC_REMOTE_PATH, 2, remoteTargetPathList);
			op.TryApplyArguments(args);

			var syncObjects = parseAssemblys();

			List<GenOperation> operations = new();

#pragma warning disable CA1416 // Validate platform compatibility
			if (MainProcess.IsDebug)
			{
				masterTargetPathList.ArgumentArray = new string[] { "Test/" };
				remoteTargetPathList.ArgumentArray = new string[] { "Test/" };
			}
#pragma warning restore CA1416 // Validate platform compatibility

			foreach (var syncObj in syncObjects)
			{
				string masterFileName = CommonFormat.MasterPrefix + syncObj.ObjectName;
				var masterContent = string.Format(CodeFormat.GeneratorMetadata, masterFileName, syncObj.Gen_MasterCode());
				foreach (var targetPath in masterTargetPathList.ArgumentArray)
				{
					operations.Add(new GenOperation(targetPath, masterFileName, masterContent));
				}

				string remoteFileName = CommonFormat.RemotePrefix + syncObj.ObjectName;
				var remoteContent = string.Format(CodeFormat.GeneratorMetadata, remoteFileName, syncObj.Gen_RemoteCode());
				foreach (var targetPath in remoteTargetPathList.ArgumentArray)
				{
					operations.Add(new GenOperation(targetPath, remoteFileName, remoteContent));
				}
			}

			// Remove previous files
			foreach (var targetPath in masterTargetPathList.ArgumentArray)
			{
				if (FileHandler.TryDeleteFilesIncludeDirectoies(targetPath))
				{
					PatcherConsole.PrintWarm($"Remove previous files in : {targetPath}");
				}
			}
			foreach (var targetPath in remoteTargetPathList.ArgumentArray)
			{
				if (FileHandler.TryDeleteFilesIncludeDirectoies(targetPath))
				{
					PatcherConsole.PrintWarm($"Remove previous files in : {targetPath}");
				}
			}

			foreach (var operation in operations)
			{
				if (!operation.TrySave())
				{
					break;
				}
			}
		}

		public List<SyncObjectInfo> parseAssemblys()
		{
			// Get difinition types
			List<SyncObjectInfo> syncObjects = new();

			// Sync object
			ReflectionExtension.TryGetSyncDifinitionTypes<SyncObjectDefinitionAttribute>
				(typeof(CTS.Instance.GameplayServer), out var syncObjDefinitionTypes);

			if (syncObjDefinitionTypes != null && syncObjDefinitionTypes.Count > 0)
			{
				var syncObjs = parseTypeToSyncObjectInfo(syncObjDefinitionTypes, false);
				syncObjects.AddRange(syncObjs);
			}

			// Sync network object
			ReflectionExtension.TryGetSyncDifinitionTypes<SyncNetworkObjectDefinitionAttribute>
				(typeof(CTS.Instance.GameplayServer), out var netObjDefititionTypes);

			if (netObjDefititionTypes != null && netObjDefititionTypes.Count > 0)
			{
				var syncObjs = parseTypeToSyncObjectInfo(netObjDefititionTypes, true);
				syncObjects.AddRange(syncObjs);
			}

			return syncObjects;
		}

		private List<SyncObjectInfo> parseTypeToSyncObjectInfo(IEnumerable<Type> types, bool isNetworkObject)
		{
			List<SyncObjectInfo> syncObjects = new();

			foreach (var t in types)
			{
				List<MemberToken> masterMembers = new();
				List<MemberToken> remoteMembers = new();

				if (TryParseProperty(t, out var masterPropMembers, out var remotePropMembers))
				{
					masterMembers.AddRange(masterPropMembers);
					remoteMembers.AddRange(remotePropMembers);
				}

				if (TryParseFunctions(t, out var masterFuncMambers, out var remoteFuncMembers))
				{
					masterMembers.AddRange(masterFuncMambers);
					remoteMembers.AddRange(remoteFuncMembers);
				}

				syncObjects.Add(new(t.Name, masterMembers, remoteMembers, isNetworkObject));
			}

			return syncObjects;
		}

		public static bool TryParseFunctions(Type type,
											 out List<MemberToken> masterMembers,
											 out List<MemberToken> remoteMembers)
		{
			masterMembers = new();
			remoteMembers = new();

			// Parse functions
			foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				foreach (var att in method.GetCustomAttributes())
				{
					SyncType syncType;
					SyncDirection direction;

					if (att is not SyncRpcAttribute syncAtt)
						continue;

					syncType = syncAtt.SyncType;
					direction = syncAtt.SyncDirection;

					MemberToken member = parseSyncFunction(method, syncType);

					if (direction == SyncDirection.FromMaster)
					{
						masterMembers.Add(member);
					}
					else if (direction == SyncDirection.FromRemote)
					{
						remoteMembers.Add(member);
					}

					break;
				}
			}

			return masterMembers.Count != 0 || remoteMembers.Count != 0;
		}

		public static bool TryParseProperty(Type type,
											out List<MemberToken> masterMembers,
											out List<MemberToken> remoteMembers)
		{
			masterMembers = new();
			remoteMembers = new();

			// Parse properties
			FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			foreach (var field in fieldInfos)
			{
				foreach (var att in field.GetCustomAttributes())
				{
					SyncType syncType;
					SyncDirection direction;
					MemberToken member;

					if (att is SyncVarAttribute syncVarAtt)
					{
						syncType = syncVarAtt.SyncType;
						direction = syncVarAtt.SyncDirection;
						member = parseValueField(field, syncType);
					}
					else if (att is SyncObjectAttribute syncObjAtt)
					{
						syncType = syncObjAtt.SyncType;
						direction = syncObjAtt.SyncDirection;
						member = parseSyncObjectField(field, syncType);
					}
					else
					{
						continue;
					}

					if (direction == SyncDirection.FromMaster)
					{
						masterMembers.Add(member);
					}
					else if (direction == SyncDirection.FromRemote)
					{
						remoteMembers.Add(member);
					}

					break;
				}
			}

			return masterMembers.Count != 0 || remoteMembers.Count != 0;
		}

		private static MemberToken parseValueField(FieldInfo fieldInfo, SyncType syncType)
		{
			bool isPublic = fieldInfo.IsPublic;
			string typeName = fieldInfo.FieldType.Name;
			string memberName = fieldInfo.Name;

			MemberToken member = new();
			member.SyncType = syncType;

			if (fieldInfo.FieldType.IsEnum)
			{
				var sizeTypeName = ProjectReference.Instance.GetEnumSizeTypeName(typeName);
				if (ReflectionHelper.TryGetCLRTypeByPrimitive(sizeTypeName, out string clrEnumSizeType))
				{
					var memberToken = new EnumMemberToken(syncType, typeName, memberName, isPublic, sizeTypeName, clrEnumSizeType);
					member.Member = memberToken;
				}
			}
			else if (fieldInfo.FieldType.IsPrimitive)
			{
				if (ReflectionHelper.TryGetTypeByCLRType(typeName, out string nonClrType))
				{
					member.Member = new PrimitivePropertyToken(syncType, nonClrType, memberName, typeName, isPublic);
				}
			}
			else if (fieldInfo.FieldType.IsValueType)
			{
				member.Member = new ValueTypeMemberToken(syncType, typeName, memberName, isPublic);
			}

			return member;
		}

		private static MemberToken parseSyncObjectField(FieldInfo fieldInfo, SyncType syncType)
		{
			bool isPublic = fieldInfo.IsPublic;
			string typeName = fieldInfo.FieldType.Name;
			string memberName = fieldInfo.Name;
			MemberToken member = new();
			member.SyncType = syncType;
			member.Member = new SyncObjectMemberToken(syncType, typeName, memberName, isPublic);
			return member;
		}

		private static MemberToken parseSyncFunction(MethodInfo methodInfo, SyncType syncType)
		{
			bool isPublic = methodInfo.IsPublic;
			string functionName = methodInfo.Name;
			MemberToken member = new();
			member.SyncType = syncType;

			List<BaseArgument> arguments = new();
			var parameters = methodInfo.GetParameters();

			int index = 0;
			foreach (var p in parameters)
			{
				Type pType = p.ParameterType;
				string parameterName = p.Name == null ? $"value_{index++}" : p.Name;
				string parameterTypeName = pType.Name;

				if (pType.IsEnum)
				{
					if (!ProjectReference.Instance.IsEnum(parameterTypeName))
						throw new Exception($"프로젝트에 정의되지 않은 enum type입니다.\n{pType.FullName}");

					string sizeTypeName = ProjectReference.Instance.GetEnumSizeTypeName(parameterTypeName);

					if (!ReflectionHelper.TryGetCLRTypeByPrimitive(sizeTypeName, out var clrSizeTypeName))
						throw new Exception($"알 수 없는 크기로 정의된 enum type입니다.\n{pType.FullName}");

					arguments.Add(new EnumArgument(parameterTypeName, parameterName, sizeTypeName, clrSizeTypeName));
				}
				else if (pType.IsPrimitive)
				{
					if (!ReflectionHelper.TryGetTypeByCLRType(parameterTypeName, out var nonClrType))
						throw new Exception($"잘못된 Primitive 타입입니다.\n{pType.FullName}");

					arguments.Add(new PrimitiveArgument(nonClrType, parameterName, parameterTypeName));
				}
				else if (pType.IsValueType)
				{
					arguments.Add(new ValueTypeArgument(parameterTypeName, parameterName));
				}
				else
				{
					throw new Exception($"알 수 없는 인자 타입입니다.\n{pType.FullName}");
				}
			}

			member.Member = new FunctionMemberToken(syncType, functionName, isPublic, arguments);
			return member;
		}
	}
}
