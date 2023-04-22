using System;
using System.Collections.Generic;
using System.Reflection;
using CT.CorePatcher.Helper;
using CT.CorePatcher.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class SyncPropertyToken
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		/// <summary>동기화 타입입니다.</summary>
		public SyncType SyncType { get; private set; }

		/// <summary>직렬화 타입입니다.</summary>
		public SerializeType SerializeType { get; private set; }

		/// <summary>타입 이름입니다.</summary>
		public string TypeName { get; private set; } = string.Empty;

		/// <summary>원시 타입인 경우의 CRL 타입 이름입니다.</summary>
		public string CRLTypeName { get; private set; } = string.Empty;

		/// <summary>프로퍼티 초기화 구문 입니다.</summary>
		public string Initializer { get; private set; } = string.Empty;

		/// <summary>선언된 프로퍼티의 이름입니다.</summary>
		public string PrivateName { get; private set; } = string.Empty;

		/// <summary>Enum의 원시 타입 이름입니다.</summary>
		public string EnumSizeTypeName { get; private set; } = "int";

		public SyncPropertyToken(SynchronizerGenerator generator, SyncType syncType, string propertyName, Type fieldType)
		{
			this.SyncType = syncType;
			this.PrivateName = propertyName;
			this.TypeName = fieldType.Name;

			// Set serialize type
			string baseTpyeName = fieldType.BaseType != null ? fieldType.BaseType.Name : string.Empty;
			string typeName = this.TypeName;
			if (baseTpyeName == "ValueType")
			{
				if (ReflectionHelper.IsCLRPrimitiveType(typeName))
				{
					this.SerializeType = SerializeType.Primitive;
					this.CRLTypeName = typeName;
					ReflectionHelper.TryGetTypeByCLRType(this.CRLTypeName, out var primitiveType);
					this.TypeName = primitiveType;

				}
				else if (ReflectionHelper.IsNetString(typeName))
				{
					this.SerializeType = SerializeType.NetString;
					this.Initializer = " = string.Empty";
				}
				else
				{
					this.SerializeType = SerializeType.Struct;
					this.Initializer = " = new()";
				}
			}
			else
			{
				if (generator.IsEnum(typeName))
				{
					this.SerializeType = SerializeType.Enum;
					this.EnumSizeTypeName = generator.GetEnumSizeTypeName(typeName);
					this.Initializer = string.Empty;
				}
				else
				{
					this.SerializeType = SerializeType.Class;
					this.Initializer = " = new()";
				}
			}
		}

		public string GeneratePraivteDeclaration()
		{
			return string.Format(SyncFormat.PrivateDeclaration,
								 SyncFormat.GetSyncVarAttribute(SyncType),
								 TypeName, PrivateName, Initializer);
		}

		public string GeneratePropertyGetSet(string dirtyBitName, int propIndex)
		{
			return string.Format(SyncFormat.PropertyGetSet,
								 this.TypeName,
								 this.GetPublicPropertyName(),
								 this.PrivateName,
								 dirtyBitName,
								 propIndex);
		}

		public string GeneratetPropertySerializeIfDirty(string dirtyBitName, int curPropIndex)
		{
			return string.Format(SyncFormat.PropertySerializeIfDirty,
								 dirtyBitName,
								 curPropIndex,
								 this.GetWriterSerialize());
		}

		public string GetPublicPropertyName()
		{
			string publicName = PrivateName;
			if (publicName[0] == '_')
			{
				publicName = publicName[1..];
			}

			return ($"{publicName[0]}").ToUpper() + publicName[1..];
		}

		public string GetParameter()
		{
			return string.Format(SyncFormat.Parameter, TypeName, PrivateName);
		}

		public string GetWriterSerialize()
		{
			switch (SerializeType)
			{
				case SerializeType.Primitive:
					return string.Format(SyncFormat.WritePut, PrivateName) + NewLine;

				case SerializeType.NetString:
				case SerializeType.Class:
				case SerializeType.Struct:
					return string.Format(SyncFormat.WriteSerialize, PrivateName) + NewLine;

				case SerializeType.Enum:
					return string.Format(SyncFormat.WriteEnum, EnumSizeTypeName, PrivateName) + NewLine;

				default:
					return string.Empty;
			}
		}

		public string GetWriterSerializeWithPrefix(string prefix)
		{
			string name = prefix + PrivateName;

			switch (SerializeType)
			{
				case SerializeType.Primitive:
					return string.Format(SyncFormat.WritePut, name) + NewLine;

				case SerializeType.NetString:
				case SerializeType.Class:
				case SerializeType.Struct:
					return string.Format(SyncFormat.WriteSerialize, name) + NewLine;

				case SerializeType.Enum:
					return string.Format(SyncFormat.WriteEnum, EnumSizeTypeName, name) + NewLine;

				default:
					return string.Empty;
			}
		}
	}

	public class SyncFunctionToken
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		/// <summary>동기화 타입입니다.</summary>
		public SyncType SyncType { get; private set; }

		/// <summary>함수의 이름입니다.</summary>
		public string FunctionName { get; private set; } = string.Empty;

		/// <summary>인자 목록입니다.</summary>
		public List<SyncPropertyToken> Parameters { get; private set; } = new();

		public SyncFunctionToken(SynchronizerGenerator generator, SyncType syncType, MethodInfo methodInfo)
		{
			this.SyncType = syncType;
			this.FunctionName = methodInfo.Name;

			var paramInfo = methodInfo.GetParameters();
			foreach (var param in paramInfo)
			{
				SyncPropertyToken syncParam = new(generator, SyncType.None, param.Name ?? string.Empty, param.ParameterType);
				Parameters.Add(syncParam);
			}
		}

		public string GeneratePartialDeclaraction()
		{
			return string.Format(SyncFormat.FunctionPartialDeclaration,
								 SyncFormat.GetSyncRpcAttribute(SyncType),
								 FunctionName, GetParameterContent());
		}

		public string GenerateFunctionCallWithStack(string dirtyBitsName, int funcIndex)
		{
			if (Parameters.Count == 0)
			{
				return string.Format(SyncFormat.FunctionCallWithStackVoid,
									 this.FunctionName,
									 this.GetParameterContent(),
									 dirtyBitsName,
									 funcIndex);
			}
			else
			{
				return string.Format(SyncFormat.FunctionCallWithStack,
									 this.FunctionName,
									 this.GetParameterContent(),
									 this.GetCallStackTupleContent(),
									 dirtyBitsName,
									 funcIndex);
			}
		}

		public string GenerateSerializeIfDirty(string dirtyBitName, int curFuncIndex)
		{
			if (Parameters.Count == 0)
			{
				return string.Format(SyncFormat.FunctionSerializeIfDirtyVoid,
									 dirtyBitName, curFuncIndex,
									 this.FunctionName);
			}
			else
			{
				var funcSerializeContent = this.GetCallstackSerializeContent();
				CodeFormat.AddIndent(ref funcSerializeContent, 2);
				return string.Format(SyncFormat.FunctionSerializeIfDirty,
									 dirtyBitName, curFuncIndex,
									 this.FunctionName,
									 funcSerializeContent);
			}
		}

		public string GetParameterContent()
		{
			string paramContent = string.Empty;

			for (int i = 0; i < Parameters.Count; i++)
			{
				var param = Parameters[i];
				paramContent += param.GetParameter();

				if (i < Parameters.Count - 1)
				{
					paramContent += ", ";
				}
			}

			return paramContent;
		}

		public string GetCallStackTupleContent()
		{
			string paramContent = string.Empty;

			for (int i = 0; i < Parameters.Count; i++)
			{
				var param = Parameters[i];
				paramContent += param.PrivateName;

				if (i < Parameters.Count - 1)
				{
					paramContent += ", ";
				}
			}

			return paramContent;
		}

		public string GetCallstackSerializeContent()
		{
			string content = string.Empty;
			foreach (var param in Parameters)
			{
				content += param.GetWriterSerializeWithPrefix("args");
			}

			return content;
		}
	}
}