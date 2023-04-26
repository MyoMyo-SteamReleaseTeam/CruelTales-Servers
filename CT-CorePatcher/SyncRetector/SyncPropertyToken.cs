using System;
using CT.Common.Synchronizations;
using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SyncRetector
{
	public class SyncPropertyToken
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		/// <summary>직렬화 타입입니다.</summary>
		public SerializeType SerializeType { get; private set; }

		/// <summary>Public 여부입니다.</summary>
		public bool IsPublic { get; private set; }

		/// <summary>적용될 타입 이름입니다. SyncObject 타입인 경우 Master 여부에 따라서 Prefix가 붙습니다.</summary>
		public string TypeName { get; private set; } = string.Empty;

		/// <summary>원시 타입인 경우의 CLR 타입 이름입니다.</summary>
		public string CLRTypeName { get; private set; } = string.Empty;

		/// <summary>프로퍼티 초기화 구문 입니다.</summary>
		public string Initializer { get; private set; } = string.Empty;

		/// <summary>선언된 프로퍼티의 private 이름입니다.</summary>
		public string PrivateName { get; private set; } = string.Empty;

		/// <summary>Enum의 원시 타입 이름입니다.</summary>
		public string EnumSizeTypeName { get; private set; } = "int";

		public SyncPropertyToken(SynchronizerGenerator generator,
								 string propertyName, 
								 Type fieldType, 
								 bool isPublic)
		{
			this.TypeName = fieldType.Name;
			this.IsPublic = isPublic;

			if (propertyName[0] != '_')
			{
				propertyName = '_' + propertyName;
			}
			if (propertyName[1].IsUpperCase())
			{
				propertyName = '_' + $"{propertyName[1]}".ToLower() + propertyName[2..];
			}
			this.PrivateName = propertyName;

			// Set serialize type
			string baseTpyeName = fieldType.BaseType != null ? fieldType.BaseType.Name : string.Empty;
			string typeName = this.TypeName;
			if (baseTpyeName == "ValueType")
			{
				if (ReflectionHelper.IsCLRPrimitiveType(typeName))
				{
					this.SerializeType = SerializeType.Primitive;
					this.CLRTypeName = typeName;
					ReflectionHelper.TryGetTypeByCLRType(this.CLRTypeName, out var primitiveType);
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
					ReflectionHelper.TryGetCLRTypeByPrimitive(this.EnumSizeTypeName, out string clrType);
					this.CLRTypeName = clrType;
					this.Initializer = string.Empty;
				}
				else
				{
					this.SerializeType = SerializeType.Class;
					this.Initializer = " = new()";
				}
			}
		}

		public void SetSyncObjectType(SerializeType serializeType,
									  string initializer)
		{
			this.SerializeType = serializeType;
			this.Initializer = initializer;
		}

		public string GenDeclaration(SyncType syncType, SyncDirection syncDirection)
		{
			return string.Format(PropertyFormat.Declaration,
								 SyncFormat.GetSyncVarAttribute(SerializeType, syncType, syncDirection),
								 TypeName, PrivateName, Initializer);
		}

		public string GenerateRemotePropertyDeclaration(SyncType syncType, SyncDirection syncDirection)
		{
			return string.Format(PropertyFormat.RemoteDeclaration,
								 SyncFormat.GetSyncVarAttribute(SerializeType, syncType, syncDirection),
								 TypeName, PrivateName, GetPublicPropertyName(),
								 Initializer);
		}

		public string GeneratePropertyGetSet(string dirtyBitName, int propIndex)
		{
			if (SerializeType == SerializeType.SyncObject)
				return string.Empty;

			return string.Format(PropertyFormat.GetterSetter,
								 IsPublic ? "public" : "private",
								 this.TypeName,
								 this.GetPublicPropertyName(),
								 this.PrivateName,
								 dirtyBitName,
								 propIndex);
		}

		public string GeneratetPropertySerializeIfDirty(string dirtyBitName, int curPropIndex, SyncType syncType)
		{
			return string.Format(PropertyFormat.SerializeIfDirty,
								 dirtyBitName,
								 curPropIndex,
								 this.GetWriterSerialize(syncType));
		}

		public string GeneratetPropertyDeserializeIfDirty(string dirtyBitName, int curPropIndex, SyncType syncType)
		{
			return string.Format(PropertyFormat.DeserializeIfDirty,
								 dirtyBitName,
								 curPropIndex,
								 this.GetPublicPropertyName(),
								 this.PrivateName,
								 this.GetReadDeserialize(syncType));
		}

		public string GetDirtyCheckIfSyncObject(GenerateOption option, int masterIndex, int dirtyIndex)
		{
			if (SerializeType != SerializeType.SyncObject)
			{
				return string.Empty;
			}
			string dirtyBitName = string.Format(option.PropertyDirtyBitName, masterIndex);
			return string.Format(option.CheckSyncObjectDirty, dirtyBitName, dirtyIndex, PrivateName) + NewLine;
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
			return string.Format(PropertyFormat.Parameter, TypeName, PrivateName);
		}

		public string GetWriterSerialize(SyncType syncType)
		{
			switch (SerializeType)
			{
				case SerializeType.Primitive:
					return string.Format(PropertyFormat.WritePut, PrivateName) + NewLine;

				case SerializeType.NetString:
				case SerializeType.Class:
				case SerializeType.Struct:
					return string.Format(PropertyFormat.WriteSerialize, PrivateName) + NewLine;

				case SerializeType.Enum:
					return string.Format(PropertyFormat.WriteEnum, EnumSizeTypeName, PrivateName) + NewLine;

				case SerializeType.SyncObject:
					string funcName = (syncType == SyncType.None) ? "SerializeEveryProperty" : $"SerializeSync{syncType}";
					return string.Format(PropertyFormat.WriteSyncObject, PrivateName, funcName) + NewLine;

				default:
					return string.Empty;
			}
		}

		public string GetReadDeserialize(SyncType syncType)
		{
			switch (SerializeType)
			{
				case SerializeType.Primitive:
					return string.Format(PropertyFormat.ReadEmbededTypeProperty, PrivateName, CLRTypeName) + NewLine;

				case SerializeType.NetString:
				case SerializeType.Class:
				case SerializeType.Struct:
					return string.Format(PropertyFormat.ReadByDeserializer, PrivateName) + NewLine;

				case SerializeType.Enum:
					return string.Format(PropertyFormat.ReadEnum, PrivateName, TypeName, CLRTypeName) + NewLine;

				case SerializeType.SyncObject:
					string funcName = (syncType == SyncType.None) ? "DeserializeEveryProperty" : $"DeserializeSync{syncType}";
					return string.Format(PropertyFormat.ReadSyncObject, PrivateName, funcName) + NewLine;

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
					return string.Format(PropertyFormat.WritePut, name) + NewLine;

				case SerializeType.NetString:
				case SerializeType.Class:
				case SerializeType.Struct:
					return string.Format(PropertyFormat.WriteSerialize, name) + NewLine;

				case SerializeType.Enum:
					return string.Format(PropertyFormat.WriteEnum, EnumSizeTypeName, name) + NewLine;

				default:
					return string.Empty;
			}
		}

		public string GetWriterSerializeByName(string name)
		{
			switch (SerializeType)
			{
				case SerializeType.Primitive:
					return string.Format(PropertyFormat.WritePut, name) + NewLine;

				case SerializeType.NetString:
				case SerializeType.Class:
				case SerializeType.Struct:
					return string.Format(PropertyFormat.WriteSerialize, name) + NewLine;

				case SerializeType.Enum:
					return string.Format(PropertyFormat.WriteEnum, EnumSizeTypeName, name) + NewLine;

				default:
					return string.Empty;
			}
		}

		public string GetTempReadDeserialize()
		{
			switch (SerializeType)
			{
				case SerializeType.Primitive:
					return string.Format(PropertyFormat.TempReadEmbededTypeProperty, TypeName, PrivateName, CLRTypeName);

				case SerializeType.Class:
					return string.Format(PropertyFormat.TempReadByDeserializerClass, PrivateName);

				case SerializeType.NetString:
				case SerializeType.Struct:
					return string.Format(PropertyFormat.TempReadByDeserializerStruct, TypeName, PrivateName);

				case SerializeType.Enum:
					return string.Format(PropertyFormat.TempReadEnum, PrivateName, TypeName, CLRTypeName);

				default:
					return string.Empty;
			}
		}
	}
}