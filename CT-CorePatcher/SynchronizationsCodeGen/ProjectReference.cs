using System;
using System.Collections.Generic;
using System.Reflection;
using CT.Common.Serialization.Type;
using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class ProjectReference
	{
		private static ProjectReference? _instance;
		public static ProjectReference Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ProjectReference();
				}
				return _instance;
			}
		}

		private ProjectReference()
		{
			_referenceTypes = new() { typeof(NetString), typeof(CTS.Instance.GameplayServer) };
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

			void addReferenceAssembly(Assembly? assembly)
			{
				if (assembly == null)
					return;
				referenceAssemblys.Add(assembly);
			}
		}

		private List<Type> _referenceTypes;
		private Dictionary<string, string> _enumSizeByTypeName = new();

		public bool IsEnum(string typeName)
		{
			return _enumSizeByTypeName.TryGetValue(typeName, out _);
		}

		public string GetEnumSizeTypeName(string typeName)
		{
			return _enumSizeByTypeName[typeName];
		}
	}
}
