using System;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector
{
	public class SyncObjectToken
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		/// <summary>직렬화 타입입니다.</summary>
		public SerializeType SerializeType => SerializeType.SyncObject;

		/// <summary>Public 여부입니다.</summary>
		public bool IsPublic { get; private set; }

		/// <summary>적용될 타입 이름입니다. SyncObject 타입인 경우 Master 여부에 따라서 Prefix가 붙습니다.</summary>
		public string TypeName { get; private set; } = string.Empty;

		/// <summary>프로퍼티 초기화 구문 입니다.</summary>
		public string Initializer => "= new()";

		/// <summary>선언된 프로퍼티의 private 이름입니다.</summary>
		public string PrivateName { get; private set; } = string.Empty;

		public SyncObjectToken(string propertyName, 
							   Type fieldType, 
							   bool isPublic)
		{
			this.TypeName = fieldType.Name;
			this.IsPublic = isPublic;
			this.PrivateName = SyncFormat.GetPrivateName(propertyName);
		}

		public string GeneratePraivteDeclaration(SyncType syncType, SyncDirection syncDirection)
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

		public string GetDirtyCheckIf(GenerateOption option, int masterIndex, int dirtyIndex)
		{
			string dirtyBitName = string.Format(option.PropertyDirtyBitName, masterIndex);
			return string.Format(option.CheckSyncObjectDirty, dirtyBitName, dirtyIndex, PrivateName) + NewLine;
		}

		public string GetPublicPropertyName()
		{
			return SyncFormat.GetPublicName(PrivateName);
		}

		public string GetParameter()
		{
			return string.Format(PropertyFormat.Parameter, TypeName, PrivateName);
		}

		public string GetWriterSerialize(SyncType syncType)
		{
			string funcName = (syncType == SyncType.None) ? "SerializeEveryProperty" : $"SerializeSync{syncType}";
			return string.Format(PropertyFormat.WriteSyncObject, PrivateName, funcName) + NewLine;
		}

		public string GetReadDeserialize(SyncType syncType)
		{
			string funcName = (syncType == SyncType.None) ? "DeserializeEveryProperty" : $"DeserializeSync{syncType}";
			return string.Format(PropertyFormat.ReadSyncObject, PrivateName, funcName) + NewLine;
		}

		public string GetWriterSerializeWithPrefix(string prefix)
		{
			string name = prefix + PrivateName;
			return string.Format(PropertyFormat.WriteSerialize, name) + NewLine;
		}

		public string GetWriterSerializeByName(string name)
		{
			return string.Format(PropertyFormat.WriteSerialize, name) + NewLine;
		}

		public string GetTempReadDeserialize()
		{
			return string.Format(PropertyFormat.TempReadByDeserializerClass, PrivateName);
		}
	}
}