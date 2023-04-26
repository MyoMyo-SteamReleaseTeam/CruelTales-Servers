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
using CT.CorePatcher.SyncRetector.Previous;

namespace CT.CorePatcher.SyncRetector
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

//			var syncObjects = parseAssemblys();

//			SyncGenerateOperation master = new();
//			master.SyncObjects = syncObjects;
//			master.Namespace = $"CTS.Instance.SyncObjects";
//			master.UsingStatements = new List<string>()
//			{
//				"using System;",
//				"using System.Collections.Generic;",
//				"using CT.Common.DataType;",
//				"using CT.Common.Serialization;",
//				"using CT.Common.Serialization.Type;",
//				"using CT.Common.Synchronizations;",
//				"using CT.Common.Tools.Collections;",
//				"using CTS.Instance.Synchronizations;",
//			};

//			SyncGenerateOperation remote = new();
//			remote.SyncObjects = syncObjects;
//			remote.Namespace = $"CTC.Networks.SyncObjects.TestSyncObjects";
//			remote.UsingStatements = new List<string>()
//			{
//				"using System;",
//				"using System.Collections.Generic;",
//				"using CT.Common.DataType;",
//				"using CT.Common.Serialization;",
//				"using CT.Common.Serialization.Type;",
//				"using CT.Common.Synchronizations;",
//				"using CT.Common.Tools.Collections;",
//				"using CTC.Networks.Synchronizations;",
//			};

//#pragma warning disable CA1416 // Validate platform compatibility
//			if (MainProcess.IsDebug)
//			{
//				master.TargetPath = "Master";
//				master.Run(SyncDirection.FromMaster);
//				remote.TargetPath = "Remote";
//				remote.Run(SyncDirection.FromRemote);
//				return;
//			}
//#pragma warning restore CA1416 // Validate platform compatibility

//			foreach (var path in masterTargetPathList.ArgumentArray)
//			{
//				master.TargetPath = path;
//				master.Run(SyncDirection.FromMaster);
//			}

//			foreach (var path in remoteTargetPathList.ArgumentArray)
//			{
//				remote.TargetPath = path;
//				remote.Run(SyncDirection.FromRemote);
//			}
		}

		public List<Previous.SyncObjectInfo> parseAssemblys()
		{
			List<Assembly> referenceAssemblys = new();

			// Add references
			foreach (var type in _referenceTypes)
			{
				addReferenceAssembly(type.Assembly);
			}

			// Find enums from assemblys
			foreach (var a in referenceAssemblys)
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
			List<Previous.SyncObjectInfo> syncObjects = new();

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

				referenceAssemblys.Add(assembly);
			}
		}

		private SyncObjectInfo getSyncObjectInfo(Type type, bool isNetworkObject)
		{
			List<MemberToken> masterMember = new();
			List<MemberToken> remoteMember = new();

			// Parse properties
			foreach (var f in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
			{
				foreach (var att in f.GetCustomAttributes())
				{
					SyncType syncType = SyncType.None;
					SyncDirection direction = SyncDirection.None;

					if (att is DEF_SyncVarAttribute syncVarAtt)
					{
						syncType = syncVarAtt.SyncType;
						direction = syncVarAtt.SyncDirection;

					}
					else if (att is DEF_SyncObjectAttribute syncObjAtt)
					{
						syncType = syncObjAtt.SyncType;
						direction = syncObjAtt.SyncDirection;
					}
					else
					{
						continue;
					}

					SyncPropertyToken token = new(this, f.Name, f.FieldType, f.IsPublic);
					syncObject.AddPropertyToken(token, syncType, direction);
				}
			}

			// Parse functions
			foreach (var f in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
			{
				foreach (var att in f.GetCustomAttributes())
				{
					if (att is DEF_SyncRpcAttribute syncAtt)
					{
						SyncFunctionToken token = new(this, f);
						syncObject.AddFunctionToken(token, syncAtt.SyncType, syncAtt.SyncDirection);
					}
				}
			}

			return new SyncObjectInfo(type.Name, masterMember, remoteMember, isNetworkObject);
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
