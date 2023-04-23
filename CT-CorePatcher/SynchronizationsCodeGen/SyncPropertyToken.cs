using System;
using System.Diagnostics.CodeAnalysis;
using CT.Common.Synchronizations;
using CT.CorePatcher.Helper;
using Microsoft.VisualBasic.FileIO;

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

		/// <summary>원시 타입인 경우의 CLR 타입 이름입니다.</summary>
		public string CLRTypeName { get; private set; } = string.Empty;

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
									  SyncType syncType, 
									  string initializer)
		{
			this.SerializeType = serializeType;
			this.SyncType = syncType;
			this.Initializer = initializer;
		}

		public string GeneratePraivteDeclaration()
		{
			return string.Format(SyncFormat.PrivateDeclaration,
								 SyncFormat.GetSyncVarAttribute(SyncType, SerializeType),
								 TypeName, PrivateName, Initializer);
		}

		public string GenerateRemotePropertyDeclaration()
		{
			return string.Format(SyncFormat.RemotePropertyDeclaration,
								 SyncFormat.GetSyncVarAttribute(SyncType, SerializeType),
								 TypeName, PrivateName, GetPublicPropertyName(),
								 Initializer);
		}

		public string GeneratePropertyGetSet(string dirtyBitName, int propIndex)
		{
			if (SerializeType == SerializeType.SyncObject)
				return string.Empty;

			return string.Format(SyncFormat.PropertyGetSet,
								 this.TypeName,
								 this.GetPublicPropertyName(),
								 this.PrivateName,
								 dirtyBitName,
								 propIndex);
		}

		public string GeneratetPropertySerializeIfDirty(string dirtyBitName, int curPropIndex, [AllowNull] GenerateOption option = null)
		{
			return string.Format(SyncFormat.PropertySerializeIfDirty,
								 dirtyBitName,
								 curPropIndex,
								 this.GetWriterSerialize(option));
		}

		public string GeneratetPropertyDeserializeIfDirty(string dirtyBitName, int curPropIndex, [AllowNull] GenerateOption option = null)
		{
			return string.Format(SyncFormat.PropertyDeserializeIfDirty,
								 dirtyBitName,
								 curPropIndex,
								 this.GetPublicPropertyName(),
								 this.PrivateName,
								 this.GetReadDeserialize(option));
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
			return string.Format(SyncFormat.Parameter, TypeName, PrivateName);
		}

		public string GetWriterSerialize([AllowNull]GenerateOption option = null)
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

				case SerializeType.SyncObject:
					string funcName = string.Empty;
					SyncType stype = option == null ? SyncType.None : option.SyncType;

					if (stype == SyncType.Reliable)
					{
						if (this.SyncType == SyncType.Reliable || this.SyncType == SyncType.RelibaleOrUnreliable)
						{
							funcName = nameof(IMasterSynchronizable.SerializeSyncReliable);
						}
					}
					else if (stype == SyncType.Unreliable)
					{
						if (this.SyncType == SyncType.Unreliable || this.SyncType == SyncType.RelibaleOrUnreliable)
						{
							funcName = nameof(IMasterSynchronizable.SerializeSyncUnreliable);
						}
					}
					else if (stype == SyncType.RelibaleOrUnreliable)
					{
						funcName = nameof(IMasterSynchronizable.SerializeEveryProperty);
					}

					return string.Format(SyncFormat.WriteSyncObject, PrivateName, funcName) + NewLine;

				default:
					return string.Empty;
			}
		}

		public string GetReadDeserialize([AllowNull] GenerateOption option = null)
		{
			switch (SerializeType)
			{
				case SerializeType.Primitive:
					return string.Format(SyncFormat.ReadEmbededTypeProperty, PrivateName, CLRTypeName) + NewLine;

				case SerializeType.NetString:
				case SerializeType.Class:
				case SerializeType.Struct:
					return string.Format(SyncFormat.ReadByDeserializer, PrivateName) + NewLine;

				case SerializeType.Enum:
					return string.Format(SyncFormat.ReadEnum, PrivateName, TypeName, CLRTypeName) + NewLine;

				case SerializeType.SyncObject:
					string funcName = string.Empty;
					SyncType stype = option == null ? SyncType.None : option.SyncType;

					if (stype == SyncType.Reliable)
					{
						if (this.SyncType == SyncType.Reliable || this.SyncType == SyncType.RelibaleOrUnreliable)
						{
							funcName = nameof(IRemoteSynchronizable.DeserializeSyncReliable);
						}
					}
					else if (stype == SyncType.Unreliable)
					{
						if (this.SyncType == SyncType.Unreliable || this.SyncType == SyncType.RelibaleOrUnreliable)
						{
							funcName = nameof(IRemoteSynchronizable.DeserializeSyncUnreliable);
						}
					}
					else if (stype == SyncType.RelibaleOrUnreliable)
					{
						funcName = nameof(IRemoteSynchronizable.DeserializeEveryProperty);
					}

					return string.Format(SyncFormat.ReadSyncObject, PrivateName, funcName) + NewLine;

				default:
					return string.Empty;
			}
		}

		public string GetWriterSerializeWithPrefix(string prefix, [AllowNull] GenerateOption option = null)
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

		public string GetWriterSerializeByName(string name)
		{
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

		public string GetTempReadDeserialize()
		{
			switch (SerializeType)
			{
				case SerializeType.Primitive:
					return string.Format(SyncFormat.TempReadEmbededTypeProperty, TypeName, PrivateName, CLRTypeName);

				case SerializeType.Class:
					return string.Format(SyncFormat.TempReadByDeserializerClass, PrivateName);

				case SerializeType.NetString:
				case SerializeType.Struct:
					return string.Format(SyncFormat.TempReadByDeserializerStruct, TypeName, PrivateName);

				case SerializeType.Enum:
					return string.Format(SyncFormat.TempReadEnum, PrivateName, TypeName, CLRTypeName);

				default:
					return string.Empty;
			}
		}
	}
}