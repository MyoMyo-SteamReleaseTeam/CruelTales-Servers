using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using CT.Common.Serialization.Type;
using CT.CorePatcher.Helper;
using CT.CorePatcher.Synchronizations;

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
		public List<Assembly> ReferenceAssemblys { get; set; } = new();
		private Dictionary<string, string> _enumSizeByTypeName = new();

		public string ParseCode()
		{
			var syncObjects = parseAssemblys();

			string code = string.Empty;

			foreach (var syncObj in syncObjects)
			{
				code += syncObj.GenerateMasterDeclaration();
				//code += syncObj.GenerateRemoteDeclaration();
			}

			return code;
		}

		public List<SyncObjectInfo> parseAssemblys()
		{
			// Setup Enums
			var netcore = Assembly.GetAssembly(typeof(NetString));
			if (netcore != null)
			{
				ReferenceAssemblys.Add(netcore);
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
			TryGetSyncDifinitionTypes<SyncObjectDefinitionAttribute>(GetType(), out var syncObjDefinitionTypes);
			TryGetSyncDifinitionTypes<SyncNetworkObjectDefinitionAttribute>(GetType(), out var netObjDefititionTypes);

			List<Type> syncObjectTypes = new(syncObjDefinitionTypes ?? new Type[0]);
			List<Type> syncNetObjectTypes = new(netObjDefititionTypes ?? new Type[0]);

			List<SyncObjectInfo> syncObjects = new();

			foreach (var st in syncObjectTypes)
			{
				syncObjects.Add(getSyncObjectInfo(st, false));
			}

			foreach (var st in syncNetObjectTypes)
			{
				syncObjects.Add(getSyncObjectInfo(st, true));
			}

			return syncObjects;
		}

		private SyncObjectInfo getSyncObjectInfo(Type type, bool isNetworkObject)
		{
			SyncObjectInfo syncObject = new(type.Name, isNetworkObject);

			// Parse properties
			foreach (var f in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				foreach (var att in f.GetCustomAttributes())
				{
					if (att is SyncVarAttribute syncVarAtt)
					{
						SyncPropertyToken token = new(this, syncVarAtt.SyncType, f.Name, f.FieldType);
						syncObject.AddPropertyToken(token);
					}
					else if (att is SyncObjectAttribute syncObjAtt)
					{
						SyncPropertyToken token = new(this, syncObjAtt.SyncType, f.Name, f.FieldType);
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
