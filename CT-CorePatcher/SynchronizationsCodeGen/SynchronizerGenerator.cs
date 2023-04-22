using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	}

	public class SynchronizerGenerator
	{
		public List<Assembly> ReferenceAssemblys { get; set; } = new();
		private Dictionary<string, string> _enumSizeByTypeName = new();

		public void Initialize()
		{
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
		}

		public string ParseCode()
		{
			var syncObjects = parseAssemblys();

			string code = string.Empty;

			foreach (var syncObj in syncObjects)
			{
				code += syncObj.GenerateServerDeclaration();
			}

			return code;
		}

		public List<SyncNetworkObjectInfo> parseAssemblys()
		{
			if (!TryGetSyncDifinitionTypes(GetType(), out var syncTypes))
				return new();

			Debug.Assert(syncTypes != null);

			List<SyncNetworkObjectInfo> syncObjects = new();

			foreach (var st in syncTypes)
			{
				SyncNetworkObjectInfo syncObject = new(st.Name);

				// Parse properties
				foreach (var f in st.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
				{
					foreach (var att in f.GetCustomAttributes())
					{
						if (att is not SyncVarAttribute syncAtt)
							continue;

						SyncPropertyToken token = new(this, syncAtt.SyncType, f.Name, f.FieldType);
						syncObject.Properties.Add(token);
					}
				}

				// Parse functions
				foreach (var f in st.GetMethods(BindingFlags.Instance | BindingFlags.Public))
				{
					foreach (var att in f.GetCustomAttributes())
					{
						if (att is not SyncRpcAttribute syncAtt)
							continue;

						SyncFunctionToken token = new(this, syncAtt.SyncType, f);
						syncObject.Functions.Add(token);
					}
				}

				syncObjects.Add(syncObject);
			}

			return syncObjects;
		}

		public bool IsEnum(string typeName)
		{
			return _enumSizeByTypeName.TryGetValue(typeName, out _);
		}

		public string GetEnumSizeTypeName(string typeName)
		{
			return _enumSizeByTypeName[typeName];
		}

		public static bool TryGetSyncDifinitionTypes(Type typeInAssembly, out IEnumerable<Type>? types)
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
				.Where((t) => t.GetCustomAttributes<SyncDefinitionAttribute>().Count() > 0);

			if (findTypes == null)
				return false;


			types = findTypes;
			return true;
		}
	}
}
