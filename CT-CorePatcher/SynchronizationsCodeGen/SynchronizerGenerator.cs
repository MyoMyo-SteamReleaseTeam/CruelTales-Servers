﻿#pragma warning disable CA1416 // Validate platform compatibility

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using CT.Common.Synchronizations;
using CT.Common.Tools.CodeGen;
using CT.Common.Tools.Data;
using CT.Common.Tools.GetOpt;
using CT.CorePatcher.Exceptions;
using CT.CorePatcher.Helper;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments;
using CT.Definitions;

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
		public const string MASTER_POOL_PATH = "masterPoolPath";

		private static BindingFlags _parseFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

		private static Dictionary<string, SyncObjectInfo> _syncObjectByName = new();
		private static Dictionary<Type, InheritType> _typeByInheritType = new();
		private static Dictionary<Type, HashSet<(string TypeName, string PropertyName)>> _propertySetByType = new();
		private static Dictionary<Type, HashSet<string>> _functionSetByType = new();

		public static bool TryGetSyncObjectByTypeName(string typeName,
													  [NotNullWhen(true)]
													  out SyncObjectInfo? syncObjectInfo)
		{
			return _syncObjectByName.TryGetValue(typeName, out syncObjectInfo);
		}

		public static bool HasTarget(string typeName)
		{
			if (TryGetSyncObjectByTypeName(typeName, out var syncObj))
			{
				return syncObj.HasTarget;
			}

			var definedType = NameTable.GetPredefinedType(typeName);
			var genericTypes = NameTable.GetGenericTypeNames(typeName);

			switch (definedType)
			{
				case PredefinedType.SyncList:
					return false;

				case PredefinedType.SyncDictionary:
					return false;

				case PredefinedType.SyncObjectList:
					return HasTarget(genericTypes[0]);

				case PredefinedType.SyncObjectDictionary:
					return HasTarget(genericTypes[1]);
			}

			throw new ArgumentException($"There is no such type to check HasTarget: {typeName}");
		}

		public static bool TryGetGenericObjSyncDirection(string typeName, out SyncDirection direction)
		{
			direction = SyncDirection.None;

			if (!NameTable.IsSyncObjCollectionType(typeName))
				return false;

			var definedType = NameTable.GetPredefinedType(typeName);
			var genericTypes = NameTable.GetGenericTypeNames(typeName);

			string genericType;

			switch (definedType)
			{
				case PredefinedType.SyncObjectList:
					genericType = genericTypes[0];
					break;

				case PredefinedType.SyncObjectDictionary:
					genericType = genericTypes[1];
					break;

				default:
					return false;
			}

			if (!TryGetSyncObjectByTypeName(genericType, out var syncObjInfo))
				return false;

			if (syncObjInfo.IsBidirection)
			{
				direction = SyncDirection.Bidirection;
			}
			else if (syncObjInfo.HasMaster && !syncObjInfo.HasRemote)
			{
				direction = SyncDirection.FromMaster;
			}
			else if (!syncObjInfo.HasMaster && syncObjInfo.HasRemote)
			{
				direction = SyncDirection.FromRemote;
			}
			else
			{
				Debug.Assert(false);
				throw new ArgumentException($"Cannot parse generic object sync direction. Type : {typeName}");
			}

			return true;
		}

		public void GenerateCode(string[] args)
		{
			UsingTable.Initialize();

			// Parse options
			StringArgumentArray masterTargetPathList = new();
			StringArgumentArray remoteTargetPathList = new();
			StringArgument masterPoolPath = new();

			OptionParser op = new OptionParser();
			OptionParser.BindArgumentArray(op, SYNC_MASTER_PATH, 2, masterTargetPathList);
			OptionParser.BindArgumentArray(op, SYNC_REMOTE_PATH, 2, remoteTargetPathList);
			OptionParser.BindArgument(op, MASTER_POOL_PATH, 2, masterPoolPath);
			if (!op.TryApplyArguments(args))
			{
				return;
			}

			List<SyncObjectInfo> syncObjects = parseAssemblys();
			foreach (var syncObj in syncObjects)
			{
				_syncObjectByName.Add(syncObj.ObjectName, syncObj);
			}

			foreach (var syncObj in syncObjects)
			{
				syncObj.CheckValidation();
				syncObj.InitializeSyncGroup();
			}

			List<GenOperation> operations = new();

			if (MainProcess.IsDebug)
			{
				masterTargetPathList.ArgumentArray = new string[] { "Test/" };
				remoteTargetPathList.ArgumentArray = new string[] { "Test/" };
				masterPoolPath.Argument = "Test/";
			}

			// Create network object synchronize code
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

			// Create network object enum types
			if (!MainProcess.IsDebug)
			{
				var networkEnums = syncObjects
					.Where(obj => obj.SyncObjectType == SyncObjectType.NetworkObject)
					.Select(obj => obj.ObjectName).ToList();
				var netTypeFileName = CommonFormat.NetworkObjectTypeTypeName + ".cs";

				var masterEnumContent = CodeGenerator_Enumerate
					.Generate(CommonFormat.NetworkObjectTypeTypeName,
							  CommonFormat.MasterNamespace,
							  hasNone: true, useTab: true,
							  usingList: new List<string>(),
							  networkEnums);
				var masterEumeCode = string.Format(CodeFormat.GeneratorMetadata, netTypeFileName, masterEnumContent);

				var remoteEnumContent = CodeGenerator_Enumerate
					.Generate(CommonFormat.NetworkObjectTypeTypeName,
							  CommonFormat.RemoteNamespace,
							  hasNone: true, useTab: true,
							  usingList: new List<string>(),
							  networkEnums);
				var remoteEumeCode = string.Format(CodeFormat.GeneratorMetadata, netTypeFileName, remoteEnumContent);

				foreach (var targetPath in masterTargetPathList.ArgumentArray)
				{
					operations.Add(new GenOperation(targetPath, netTypeFileName, masterEumeCode));
				}

				foreach (var targetPath in remoteTargetPathList.ArgumentArray)
				{
					operations.Add(new GenOperation(targetPath, netTypeFileName, remoteEumeCode));
				}

				// Create server side network object pool setting code
				var poolCode = ObjectPoolCodeGen.GenerateMasterNetworkObjectPoolCode(syncObjects);
				operations.Add(new GenOperation(masterPoolPath.Argument, "NetworkObjectPoolManager", poolCode));
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

			if (MainProcess.IsDebug)
			{
				bool hasFocusWindow = false;

				string explorerName = "explorer";
				string openDir = Path.Combine(Directory.GetCurrentDirectory(), "Test");

				var t = Type.GetTypeFromProgID("Shell.Application");
				Debug.Assert(t != null);
				dynamic o = Activator.CreateInstance(t) ?? new object();
				try
				{
					var ws = o.Windows();
					for (int i = 0; i < ws.Count; i++)
					{
						var ie = ws.Item(i);
						if (ie == null) continue;
						var path = System.IO.Path.GetFileName((string)ie.FullName);
						if (path.ToLower() == "explorer.exe")
						{
							string locationPath = ie.LocationURL;
							Uri uri = new Uri(locationPath);
							string curExplorerPath = Path.GetFullPath(uri.AbsolutePath);
							if (Path.Equals(curExplorerPath, openDir))
							{
								hasFocusWindow = true;
								break;
							}
						}
					}
				}
				finally
				{
					Marshal.FinalReleaseComObject(o);
				}

				if (!hasFocusWindow)
				{
					try
					{
						ProcessStartInfo explorer = new(explorerName, openDir);
						Process.Start(explorer);
					}
					catch (Exception e)
					{
						PatcherConsole.PrintError("Open test directory error!");
						PatcherConsole.PrintException(e);
					}
				}
			}
		}

		public List<SyncObjectInfo> parseAssemblys()
		{
			// Get difinition types
			List<SyncObjectInfo> syncObjects = new();

			// Sync object
			ReflectionExtension.TryGetSyncDifinitionTypes<SyncObjectDefinitionAttribute>
				(typeof(DefinitionAssemblyReferenceClass), out var syncObjDefinitionTypes);

			if (syncObjDefinitionTypes != null && syncObjDefinitionTypes.Count > 0)
			{
				var syncObjs = parseTypeToSyncObjectInfo(syncObjDefinitionTypes,
														 SyncObjectType.SyncObject);
				syncObjects.AddRange(syncObjs);
			}

			// Sync network object
			ReflectionExtension.TryGetSyncDifinitionTypes<SyncNetworkObjectDefinitionAttribute>
				(typeof(DefinitionAssemblyReferenceClass), out var netObjDefititionTypes);

			if (netObjDefititionTypes != null && netObjDefititionTypes.Count > 0)
			{
				var syncObjs = parseTypeToSyncObjectInfo(netObjDefititionTypes,
														 SyncObjectType.NetworkObject);
				syncObjects.AddRange(syncObjs);
			}

			return syncObjects;
		}

		private List<SyncObjectInfo> parseTypeToSyncObjectInfo(IEnumerable<Type> types,
															   SyncObjectType objectType)
		{
			// Parse inheritance tree
			HashSet<Type> baseTypes = new();

			foreach (var t in types)
			{
				// Parse properties and functions
				var propertySet = new HashSet<(string TypeName, string PropertyName)>();
				var functionSet = new HashSet<string>();

				FieldInfo[] fieldInfos = t.GetFields(_parseFlags);
				foreach (var f in fieldInfos)
				{
					propertySet.Add((f.FieldType.Name, f.Name));
				}

				MethodInfo[] methods = t.GetMethods(_parseFlags);
				foreach (var m in methods)
				{
					functionSet.Add(m.Name);
				}

				_propertySetByType.Add(t, propertySet);
				_functionSetByType.Add(t, functionSet);

				// Add if it's child
				Type? baseType = t.BaseType;
				if (baseType == null || baseType == typeof(object))
					continue;
				baseTypes.Add(baseType);
				_typeByInheritType.Add(t, InheritType.Child);
			}

			foreach (var t in types)
			{
				var inheritType = baseTypes.Contains(t) ? InheritType.Parent : InheritType.None;

				if (!_typeByInheritType.ContainsKey(t))
				{
					_typeByInheritType.Add(t, inheritType);
				}
			}

			List<SyncObjectInfo> syncObjects = new();

			// Parse synchronize objects
			foreach (var t in types)
			{
				var attNetObj = t.GetCustomAttribute<SyncNetworkObjectDefinitionAttribute>();
				var attSyncObj = t.GetCustomAttribute<SyncObjectDefinitionAttribute>();
				int capacity = 0;
				bool multiplyByMaxUser = false;
				bool isDebugObject = false;

				if (attNetObj != null)
				{
					capacity = attNetObj.Capacity;
					multiplyByMaxUser = attNetObj.MultiplyByMaxUser;
					isDebugObject = attNetObj.IsDebugOnly;
				}

				if (attSyncObj != null)
				{
					isDebugObject = attSyncObj.IsDebugOnly;
				}

				if (MainProcess.IsDebug != isDebugObject) continue;

				List<MemberToken> masterMembers = new();
				List<MemberToken> remoteMembers = new();

				/*
				 * # Synchronize property order
				 * 
				 * - Parent properties
				 * - Parent functions
				 * - Child properties
				 * - Child functions
				 */

				TryParsePropertyByOrder(t, out var masterPropMembers, out var remotePropMembers);
				TryParseFunctionsByOrder(t, out var masterFuncMambers, out var remoteFuncMembers);

				int propMinCount = Math.Min(masterPropMembers.Count, remotePropMembers.Count);
				int funcMinCount = Math.Min(masterFuncMambers.Count, remoteFuncMembers.Count);
				int minCount = Math.Min(propMinCount, funcMinCount);

				int propMaxCount = Math.Max(masterPropMembers.Count, remotePropMembers.Count);
				int funcMaxCount = Math.Max(masterFuncMambers.Count, remoteFuncMembers.Count);
				int maxCount = Math.Max(propMaxCount, funcMaxCount);

				if (minCount != maxCount)
				{
					throw new ArgumentException($"SyncObject parse error! Faild to parse type inherit tree! " +
						$"Type: {t.Name}");
				}

				for (int i = 0; i < maxCount; i++)
				{
					masterMembers.AddRange(masterPropMembers[i]);
					masterMembers.AddRange(masterFuncMambers[i]);
					remoteMembers.AddRange(remotePropMembers[i]);
					remoteMembers.AddRange(remoteFuncMembers[i]);
				}

				InheritType inheritType = _typeByInheritType[t];
				string parentName = string.Empty;
				if (inheritType == InheritType.Child)
				{
					parentName = t.BaseType?.Name ?? string.Empty;
				}
				SyncObjectInfo objectInfo = new(t.Name, inheritType,
												masterMembers, remoteMembers,
												objectType, capacity,
												multiplyByMaxUser, isDebugObject, parentName);
				objectInfo.HasReliable = masterMembers.Any((m) => m.SyncType.IsReliable());
				objectInfo.HasUnreliable = masterMembers.Any((m) => m.SyncType.IsUnreliable());
				objectInfo.HasTarget = masterMembers.Any((m) => m.SyncType.IsTarget());
				syncObjects.Add(objectInfo);
			}

			return syncObjects;
		}

		public static bool TryParseFunctionsByOrder(
			Type type,
			out List<List<MemberToken>> masterMemberByOrder,
			out List<List<MemberToken>> remoteMemberByOrder)
		{
			bool hasProperty = false;

			masterMemberByOrder = new();
			remoteMemberByOrder = new();

			Stack<Type> inheritStack = new();
			Type? targetType = type;
			while (targetType != null && targetType != typeof(object))
			{
				inheritStack.Push(targetType);
				targetType = targetType.BaseType;
			}

			if (inheritStack.Count == 0)
			{
				throw new ArgumentException($"Type inherit parse error! {type.Name}");
			}

			InheritType inheritType = _typeByInheritType[type];

			while (inheritStack.Count > 0)
			{
				// Parse functions
				Type curType = inheritStack.Pop();
				MethodInfo[] methods = curType.GetMethods(_parseFlags);
				Type? baseType = curType.BaseType;

				List<MemberToken> masterMembers = new();
				List<MemberToken> remoteMembers = new();

				foreach (var method in methods)
				{
					foreach (var att in method.GetCustomAttributes())
					{
						SyncType syncType;
						SyncDirection direction;

						if (att is not SyncRpcAttribute syncAtt)
							continue;

						// Check it's inherited
						if (baseType != null &&
							baseType != typeof(object) &&
							_functionSetByType[baseType].Contains(method.Name))
						{
							break;
						}

						InheritType memberInheritType = InheritType.None;
						if (inheritType != InheritType.None)
						{
							memberInheritType = (inheritStack.Count == 0) ?
								InheritType.Parent : InheritType.Child;

							if (method.IsPrivate && !method.IsFamily)
							{
								throw new ArgumentException($"If {method.Name} can be inherited you must set to protected! Type : {type.Name}");
							}
						}

						// Parse member function
						syncType = syncAtt.SyncType;
						direction = syncAtt.SyncDirection;

						if (direction == SyncDirection.FromRemote &&
							(syncType == SyncType.ReliableTarget || syncType == SyncType.UnreliableTarget))
						{
							throw new WrongSyncSetting(type, method.Name, $"You can not set target type from remote side!");
						}
						if (direction == SyncDirection.Bidirection)
						{
							throw new WrongSyncSetting(type, method.Name,
													   $"You can not set bidirection sync if it's method.");
						}

						MemberToken member = parseSyncFunction(method, syncType, memberInheritType);
						member.InheritType = memberInheritType;

						// Add to list
						if (direction == SyncDirection.FromMaster)
						{
							masterMembers.Add(member);
						}
						else if (direction == SyncDirection.FromRemote)
						{
							remoteMembers.Add(member);
						}

						hasProperty = true;

						break;
					}
				}

				masterMemberByOrder.Add(masterMembers);
				remoteMemberByOrder.Add(remoteMembers);
			}

			return hasProperty;
		}

		public static bool TryParsePropertyByOrder(
			Type type,
			out List<List<MemberToken>> masterMemberByOrder,
			out List<List<MemberToken>> remoteMemberByOrder)
		{
			bool hasProperty = false;

			masterMemberByOrder = new();
			remoteMemberByOrder = new();

			Stack<Type> inheritStack = new();
			Type? targetType = type;
			while (targetType != null && targetType != typeof(object))
			{
				inheritStack.Push(targetType);
				targetType = targetType.BaseType;
			}

			if (inheritStack.Count == 0)
			{
				throw new ArgumentException($"Type inherit parse error! {type.Name}");
			}

			InheritType inheritType = _typeByInheritType[type];

			while (inheritStack.Count > 0)
			{
				// Parse properties
				Type curType = inheritStack.Pop();
				FieldInfo[] fieldInfos = curType.GetFields(_parseFlags);
				Type? baseType = curType.BaseType;

				List<MemberToken> masterMembers = new();
				List<MemberToken> remoteMembers = new();

				foreach (var field in fieldInfos)
				{
					foreach (var att in field.GetCustomAttributes())
					{
						SyncType syncType;
						SyncDirection direction;
						MemberToken member;

						SyncVarAttribute? syncVarAtt = att as SyncVarAttribute;
						SyncObjectAttribute? syncObjAtt = att as SyncObjectAttribute;

						if (syncVarAtt == null && syncObjAtt == null)
							continue;

						// Check it's inherited
						var propertyToken = (field.FieldType.Name, field.Name);
						if (baseType != null &&
							baseType != typeof(object) &&
							_propertySetByType[baseType].Contains(propertyToken))
						{
							break;
						}

						InheritType memberInheritType = InheritType.None;
						if (inheritType != InheritType.None)
						{
							memberInheritType = (inheritStack.Count == 0)?
								InheritType.Parent : InheritType.Child;

							if (field.IsPrivate && !field.IsFamily)
							{
								throw new ArgumentException($"If {field.Name} can be inherited you must set to protected! Type : {type.Name}");
							}
						}

						// Parse member property
						if (syncVarAtt != null)
						{
							syncType = syncVarAtt.SyncType;
							direction = syncVarAtt.SyncDirection;
							if (direction == SyncDirection.FromRemote && syncType == SyncType.ColdData)
							{
								throw new WrongSyncSetting(type, field.Name,
														   $"You can not set cold data from remote side!");
							}
							if (direction == SyncDirection.Bidirection)
							{
								throw new WrongSyncSetting(type, field.Name,
														   $"You can not set bidirection sync if it's variable.");
							}
							member = parseValueField(field, syncType, memberInheritType);
						}
						else if (syncObjAtt != null)
						{
							syncType = syncObjAtt.SyncType;
							direction = syncObjAtt.SyncDirection;
							if (syncType == SyncType.ColdData)
							{
								throw new WrongSyncSetting(type, field.Name,
														   $"You can not set cold/hot data at sync object!");
							}


							string typeName = field.FieldType.Name;
							bool isPredefined = false;
							if (field.FieldType.IsGenericType)
							{
								var pType = NameTable.GetPredefinedType(typeName);
								if (pType != PredefinedType.None)
								{
									isPredefined = true;
									typeName = NameTable.GetPredefinedTypeName(field.FieldType, pType);
								}
							}

							member = parseSyncObjectField(type, typeName, field, syncType, direction,
														  syncObjAtt.ConstructorContent,
														  memberInheritType,
														  isPredefined);
						}
						else
						{
							continue;
						}

						member.InheritType = memberInheritType;

						// Add to list
						if (direction == SyncDirection.FromMaster)
						{
							masterMembers.Add(member);
						}
						else if (direction == SyncDirection.FromRemote)
						{
							remoteMembers.Add(member);
						}
						else if (direction == SyncDirection.Bidirection)
						{
							masterMembers.Add(member);
							remoteMembers.Add(member);
						}

						hasProperty = true;

						break;
					}
				}

				masterMemberByOrder.Add(masterMembers);
				remoteMemberByOrder.Add(remoteMembers);
			}

			return hasProperty;
		}

		private static MemberToken parseValueField(FieldInfo fieldInfo, SyncType syncType, InheritType inheritType)
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
					var memberToken = new EnumMemberToken(syncType, inheritType, typeName, memberName, isPublic,
														  sizeTypeName, clrEnumSizeType);
					member.Member = memberToken;
				}
			}
			else if (fieldInfo.FieldType.IsPrimitive)
			{
				if (ReflectionHelper.TryGetTypeByCLRType(typeName, out string nonClrType))
				{
					member.Member = new PrimitivePropertyToken(syncType, inheritType, nonClrType, memberName,
															   typeName, isPublic);
				}
			}
			else if (fieldInfo.FieldType.IsValueType)
			{
				member.Member = new ValueTypeMemberToken(syncType, inheritType, typeName, memberName, isPublic);
			}
			else
			{
				throw new NotSupportedException($"Undefined field type : {fieldInfo.FieldType}");
			}

			return member;
		}

		private static MemberToken parseSyncObjectField(Type ownerType,
														string typeName,
														FieldInfo fieldInfo,
														SyncType syncType,
														SyncDirection attributeSyncDirection,
														string constructorContent,
														InheritType inheritType,
														bool isPredefined)
		{
			bool isPublic = fieldInfo.IsPublic;
			string memberName = fieldInfo.Name;
			MemberToken member = new();
			member.SyncType = syncType;

			member.Member = new SyncObjectMemberToken(syncType, inheritType, typeName, memberName, constructorContent,
													  isPublic, isPredefined, attributeSyncDirection, ownerType);
			return member;
		}

		private static MemberToken parseSyncFunction(MethodInfo methodInfo, SyncType syncType, InheritType inheritType)
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

			if (syncType == SyncType.Reliable || syncType == SyncType.Unreliable)
			{
				member.Member = new FunctionMemberToken(syncType, inheritType, functionName, isPublic, arguments);
			}
			else if (syncType == SyncType.ReliableTarget || syncType == SyncType.UnreliableTarget)
			{
				member.Member = new TargetFunctionMemberToken(syncType, inheritType, functionName, isPublic, arguments);
			}
			return member;
		}
	}
}

#pragma warning restore CA1416 // Validate platform compatibility