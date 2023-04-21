using System;
using System.Collections.Generic;
using System.Reflection;
using CT.CorePatcher.Helper;
using CT.CorePatcher.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class SyncPropertyToken
	{
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
		public string EnumSizeType { get; private set; } = "int";

		/// <summary>프로퍼티의 Public 선언 이름을 반환받습니다.</summary>
		public string GetPublicPropertyName()
		{
			string publicName = PrivateName;
			if (publicName[0] == '_')
			{
				publicName = publicName[1..];
			}

			return ($"{publicName[0]}").ToUpper() + publicName[1..];
		}

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
				else if (generator.IsEnum(typeName))
				{
					this.SerializeType = SerializeType.Enum;
					this.EnumSizeType = generator.GetEnumSizeTypeName(typeName);
				}
				else
				{
					this.SerializeType = SerializeType.Struct;
					this.Initializer = " = new()";
				}
			}
			else
			{
				this.SerializeType = SerializeType.Class;
				this.Initializer = " = new()";
			}
		}

		public string GetPraivteDeclaration()
		{
			return string.Format(SyncFormat.PrivateDeclaration,
								 SyncFormat.GetSyncVarAttribute(SyncType), 
								 TypeName, PrivateName, Initializer);
		}

		public string GetParameter()
		{
			return string.Format(SyncFormat.Parameter, TypeName, PrivateName);
		}
	}

	public class SyncFunctionToken
	{
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

		public string GetPartialDeclaraction()
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

			return string.Format(SyncFormat.FunctionPartialDeclaration,
								 SyncFormat.GetSyncRpcAttribute(SyncType),
								 FunctionName, paramContent);
		}
	}
}