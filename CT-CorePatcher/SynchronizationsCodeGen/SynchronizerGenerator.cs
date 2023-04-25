using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using CT.Common.Serialization.Type;
using CT.Common.Synchronizations;
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
				"using CTC.Networks.Synchronizations;",
			};

#pragma warning disable CA1416 // Validate platform compatibility
			if (MainProcess.IsDebug)
			{
				master.TargetPath = "Master";
				master.Run(SyncDirection.FromMaster);
				remote.TargetPath = "Remote";
				remote.Run(SyncDirection.FromRemote);
				return;
			}
#pragma warning restore CA1416 // Validate platform compatibility

			foreach (var path in masterTargetPathList.ArgumentArray)
			{
				master.TargetPath = path;
				master.Run(SyncDirection.FromMaster);
			}

			foreach (var path in remoteTargetPathList.ArgumentArray)
			{
				remote.TargetPath = path;
				remote.Run(SyncDirection.FromRemote);
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
			TryGetSyncDifinitionTypes<DEF_SyncObjectDefinitionAttribute>
				(typeof(CTS.Instance.GameplayServer), out var syncObjDefinitionTypes);

			List<Type> syncObjectTypes = new(syncObjDefinitionTypes ?? new Type[0]);
			foreach (var st in syncObjectTypes)
			{
				syncObjects.Add(getSyncObjectInfo(st, false));
			}

			// Sync network object
			TryGetSyncDifinitionTypes<DEF_SyncNetworkObjectDefinitionAttribute>
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
					SyncType syncType = SyncType.None;
					SyncDirection syncDirection = SyncDirection.None;

					if (att is DEF_SyncVarAttribute syncVarAtt)
					{
						syncType = syncVarAtt.SyncType;
						syncDirection = syncVarAtt.SyncDirection;

					}
					else if (att is DEF_SyncObjectAttribute syncObjAtt)
					{
						syncType = syncObjAtt.SyncType;
						syncDirection = syncObjAtt.SyncDirection;
					}
					else
					{
						continue;
					}

					SyncPropertyToken token = new(this, syncType, syncDirection, f.Name, f.FieldType, f.IsPublic);
					syncObject.AddPropertyToken(token);
				}
			}

			// Parse functions
			foreach (var f in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
			{
				foreach (var att in f.GetCustomAttributes())
				{
					if (att is DEF_SyncRpcAttribute syncAtt)
					{
						SyncFunctionToken token = new(this, syncAtt.SyncType, syncAtt.SyncDirection, f);
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
