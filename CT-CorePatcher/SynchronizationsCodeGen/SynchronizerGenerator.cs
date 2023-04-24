using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CT.Common.Serialization.Type;
using CT.Common.Synchronizations;
using CT.Common.Tools.CodeGen;
using CT.Common.Tools.Data;
using CT.Common.Tools.GetOpt;
using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public enum SerializeType
	{
		None = 0,
		Primitive,
		NetString,
		Class,
		Struct,
		Enum,
		SyncObject,
	}

	public class SyncGenerateOperation
	{
		class CodeGenInfo
		{
			public string ObjectName { get; private set; }
			public string GenCode = string.Empty;
			public string FileName => ObjectName + ".cs";

			public CodeGenInfo(string objectName, bool isMaster)
			{
				ObjectName = objectName;

				//if (isMaster)
				//	ObjectName = SyncFormat.MasterPrefix + ObjectName;
				//else
				//	ObjectName = SyncFormat.RemotePrefix + ObjectName;
			}
		}

		public List<string> UsingStatements = new();
		public List<SyncObjectInfo> SyncObjects = new();
		public string Namespace = string.Empty;
		public string TargetPath = string.Empty;

		public void Run(bool isMaster)
		{
			PatcherConsole.PrintJobInfo("Generate sync object definitions");

			foreach (var syncObj in SyncObjects)
			{
				syncObj.SetSyncDirection(isMaster);
			}

			string usingStatements = string.Empty;

			foreach (var u in UsingStatements)
			{
				usingStatements += u + CodeFormat.NewLine;
			}

			// Generate Code
			List<string> enumTypes = new();
			List<CodeGenInfo> genCodes = new();
			foreach (var syncObj in SyncObjects)
			{
				if (syncObj.IsNetworkObject)
				{
					enumTypes.Add(syncObj.OriginObjectName);
				}
				CodeGenInfo info = new(syncObj.ObjectName, isMaster);

				string code;
				if (isMaster)
					code = syncObj.GenerateMasterDeclaration();
				else
					code = syncObj.GenerateRemoteDeclaration();

				CodeFormat.AddIndent(ref code);
				info.GenCode = string.Format(SyncFormat.FileFormat, usingStatements, Namespace, code);
				info.GenCode = string.Format(CodeFormat.GeneratorMetadata, info.FileName, info.GenCode);
				CodeFormat.RemoveNewLine(ref info.GenCode, startFromNamespace: true);
				
				genCodes.Add(info);
			}

			PatcherConsole.PrintJobInfo("Create sync object code files");

			// Remove existing files
			foreach (var path in Directory.GetFiles(TargetPath))
			{
				File.Delete(path);
			}

			// Create code files
			foreach (var info in genCodes)
			{
				string targetPath = Path.Combine(TargetPath, info.FileName);
				var result = FileHandler.TryWriteText(targetPath, info.GenCode, true);
				if (result.ResultType == JobResultType.Success)
				{
					PatcherConsole.PrintSaveSuccessResult("Sync code gen completed : ", info.FileName, targetPath);
				}
				else
				{
					PatcherConsole.PrintError(result.Exception.Message);
				}
			}

			{
				var enumCodeFileName = SyncFormat.NetworkObjectTypeTypeName + ".cs";
				var enumCode = CodeGenerator_Enumerate.Generate(SyncFormat.NetworkObjectTypeTypeName,
																Namespace, true, true, new string[0], enumTypes,
																addUsingAndSemicolon: false);
				enumCode = string.Format(CodeFormat.GeneratorMetadata, enumCodeFileName, enumCode);

				string targetPath = Path.Combine(TargetPath, enumCodeFileName);
				var result = FileHandler.TryWriteText(targetPath, enumCode, true);
				if (result.ResultType == JobResultType.Success)
				{
					PatcherConsole.PrintSaveSuccessResult("Sync enum code gen completed : ", enumCodeFileName, targetPath);
				}
				else
				{
					PatcherConsole.PrintError(result.Exception.Message);
				}
			}
		}
	}

	public class SynchronizerGenerator
	{
		public List<Type> _referenceTypes = new()
		{
			typeof(NetString), typeof(CTS.Instance.GameplayServer)
		};

		public List<Assembly> ReferenceAssemblys { get; private set; } = new();
		private Dictionary<string, string> _enumSizeByTypeName = new();

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

			SyncGenerateOperation master = new();
			master.SyncObjects = syncObjects;
			master.Namespace = $"CTS.Instance.SyncObjects";
			master.UsingStatements = new List<string>()
			{
				"using System;",
				"using System.Collections.Generic;",
				"using CT.Common.DataType;",
				"using CT.Common.Serialization;",
				"using CT.Common.Serialization.Type;",
				"using CT.Common.Synchronizations;",
				"using CT.Common.Tools.Collections;",
				"using CTS.Instance.Synchronizations;",
			};

			SyncGenerateOperation remote = new();
			remote.SyncObjects = syncObjects;
			remote.Namespace = $"CTC.Networks.SyncObjects.TestSyncObjects";
			remote.UsingStatements = new List<string>()
			{
				"using System;",
				"using System.Collections.Generic;",
				"using CT.Common.DataType;",
				"using CT.Common.Serialization;",
				"using CT.Common.Serialization.Type;",
				"using CT.Common.Synchronizations;",
				"using CT.Common.Tools.Collections;",
				"using CTC.Networks.Synchornizations;",
			};

#pragma warning disable CA1416 // Validate platform compatibility
			if (MainProcess.IsDebug)
			{
				master.TargetPath = "Test";
				master.Run(isMaster: true);
				remote.TargetPath = "Test";
				remote.Run(isMaster: false);
				return;
			}
#pragma warning restore CA1416 // Validate platform compatibility

			foreach (var path in masterTargetPathList.ArgumentArray)
			{
				master.TargetPath = path;
				master.Run(isMaster: true);
			}

			foreach (var path in remoteTargetPathList.ArgumentArray)
			{
				remote.TargetPath = path;
				remote.Run(isMaster: false);
			}
		}

		public List<SyncObjectInfo> parseAssemblys()
		{
			// Add references
			foreach (var type in _referenceTypes)
			{
				addReferenceAssembly(type.Assembly);
			}

			// Find enums from assemblys
			foreach (var a in ReferenceAssemblys)
			{
				var enumTypes = ReflectionExtension.GetTypeNamesBy(a, typeof(Enum));

				foreach (var et in enumTypes)
				{
					var enumSizeTypeName = Enum.GetUnderlyingType(et).Name;
					ReflectionHelper.TryGetTypeByCLRType(enumSizeTypeName, out var primitiveType);
					_enumSizeByTypeName.Add(et.Name, primitiveType);
				}
			}

			// Get difinition types
			List<SyncObjectInfo> syncObjects = new();

			// Sync object
			TryGetSyncDifinitionTypes<SyncObjectDefinitionAttribute>
				(typeof(CTS.Instance.GameplayServer), out var syncObjDefinitionTypes);

			List<Type> syncObjectTypes = new(syncObjDefinitionTypes ?? new Type[0]);
			foreach (var st in syncObjectTypes)
			{
				syncObjects.Add(getSyncObjectInfo(st, false));
			}

			// Sync network object
			TryGetSyncDifinitionTypes<SyncNetworkObjectDefinitionAttribute>
				(typeof(CTS.Instance.GameplayServer), out var netObjDefititionTypes);

			List<Type> syncNetObjectTypes = new(netObjDefititionTypes ?? new Type[0]);
			foreach (var st in syncNetObjectTypes)
			{
				syncObjects.Add(getSyncObjectInfo(st, true));
			}

			return syncObjects;

			void addReferenceAssembly(Assembly? assembly)
			{
				if (assembly == null)
					return;

				ReferenceAssemblys.Add(assembly);
			}
		}

		private SyncObjectInfo getSyncObjectInfo(Type type, bool isNetworkObject)
		{
			SyncObjectInfo syncObject = new(type.Name, isNetworkObject);

			// Parse properties
			foreach (var f in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
			{
				foreach (var att in f.GetCustomAttributes())
				{
					if (att is SyncVarAttribute syncVarAtt)
					{
						SyncPropertyToken token = new(this, syncVarAtt.SyncType, f.Name, f.FieldType, f.IsPublic);
						syncObject.AddPropertyToken(token);
					}
					else if (att is SyncObjectAttribute syncObjAtt)
					{
						SyncPropertyToken token = new(this, syncObjAtt.SyncType, f.Name, f.FieldType, f.IsPublic);
						token.SetSyncObjectType(SerializeType.SyncObject, syncObjAtt.SyncType, " = new()");
						syncObject.AddPropertyToken(token);
					}
				}
			}

			// Parse functions
			foreach (var f in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
			{
				foreach (var att in f.GetCustomAttributes())
				{
					if (att is SyncRpcAttribute syncAtt)
					{
						SyncFunctionToken token = new(this, syncAtt.SyncType, f);
						syncObject.AddFunctionToken(token);
					}
				}
			}

			return syncObject;
		}

		public bool IsEnum(string typeName)
		{
			return _enumSizeByTypeName.TryGetValue(typeName, out _);
		}

		public string GetEnumSizeTypeName(string typeName)
		{
			return _enumSizeByTypeName[typeName];
		}

		public static bool TryGetSyncDifinitionTypes<T>(Type typeInAssembly, out IEnumerable<Type>? types)
			where T : Attribute
		{
			types = null;
			var assemTypes = Assembly.GetAssembly(typeInAssembly)?.GetTypes();
			if (assemTypes == null)
			{
				return false;
			}

			var findTypes = Assembly
				.GetAssembly(typeInAssembly)?
				.GetTypes()
				.Where((t) => t.GetCustomAttributes<T>().Count() > 0);

			if (findTypes == null)
				return false;

			types = findTypes;
			return true;
		}
	}
}
