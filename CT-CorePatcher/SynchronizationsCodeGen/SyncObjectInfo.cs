using System.Collections.Generic;
using CT.CorePatcher.Synchronizations;
using CT.Networks.Synchronizations;
using CT.Tools.Collections;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class SyncObjectInfo
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		public bool IsNetworkObject { get; private set; } = false;
		public string ObjectName { get; private set; } = string.Empty;

		public List<SyncPropertyToken> ReliableProperties { get; private set; } = new();
		public List<SyncFunctionToken> ReliableFunctions { get; private set; } = new();
		public List<SyncPropertyToken> UnreliableProperties { get; private set; } = new();
		public List<SyncFunctionToken> UnreliableFunctions { get; private set; } = new();

		public List<SyncPropertyToken> AllProperties { get; private set; } = new();
		public List<SyncFunctionToken> AllFunctions { get; private set; } = new();

		public bool HasReliable => ReliableProperties.Count > 0 || ReliableFunctions.Count > 0;
		public bool HasUnreliable => UnreliableProperties.Count > 0 || UnreliableFunctions.Count > 0;

		public SyncObjectInfo(string objectName, bool isNetworkObject)
		{
			ObjectName = objectName;
			IsNetworkObject = isNetworkObject;
		}

		public void AddPropertyToken(SyncPropertyToken token)
		{
			AllProperties.Add(token);

			if (token.SyncType == SyncType.Reliable)
			{
				ReliableProperties.Add(token);
			}
			else if (token.SyncType == SyncType.Unreliable)
			{
				UnreliableProperties.Add(token);
			}
			else if (token.SyncType == SyncType.RelibaleOrUnreliable)
			{
				ReliableProperties.Add(token);
				UnreliableProperties.Add(token);
			}
		}

		public void AddFunctionToken(SyncFunctionToken token)
		{
			AllFunctions.Add(token);

			if (token.SyncType == SyncType.Reliable)
			{
				ReliableFunctions.Add(token);
			}
			else if (token.SyncType == SyncType.Unreliable)
			{
				UnreliableFunctions.Add(token);
			}
		}

		public string GenerateMasterDeclaration()
		{
			string declarationContent = string.Empty;
			string synchronizeContent = string.Empty;
			string serializeFuncContent = string.Empty;
			string dirtyBitClearContent = string.Empty;
			string serializeAllContent = string.Empty;

			if (HasReliable)
			{
				GenerateOption reliableOption = new(SyncType.Reliable, ReliableProperties, ReliableFunctions);

				declarationContent += Master_GenDeclaration(reliableOption) + NewLine;
				synchronizeContent += Master_GenPropertySynchronizeContent(reliableOption) + NewLine;
				serializeFuncContent += Master_GenSerializeFunction(reliableOption) + NewLine;
				dirtyBitClearContent += Master_GenDirtyBitsClearFunction(reliableOption) + NewLine;
			}

			if (HasUnreliable)
			{
				GenerateOption unreliableOption = new(SyncType.Unreliable, UnreliableProperties, UnreliableFunctions);

				declarationContent += Master_GenDeclaration(unreliableOption) + NewLine;
				synchronizeContent += Master_GenPropertySynchronizeContent(unreliableOption) + NewLine;
				serializeFuncContent += Master_GenSerializeFunction(unreliableOption) + NewLine;
				dirtyBitClearContent += Master_GenDirtyBitsClearFunction(unreliableOption) + NewLine;
			}

			GenerateOption allOption = new(SyncType.RelibaleOrUnreliable, AllProperties, AllFunctions);
			serializeAllContent += Master_GenMasterSerializeEveryProperty(allOption) + NewLine;

			string synchronization = synchronizeContent + NewLine +
									 serializeFuncContent + NewLine +
									 dirtyBitClearContent + NewLine +
									 serializeAllContent;

			// Combine all contents
			string inheritType = nameof(IMasterSynchronizable);

			if (this.IsNetworkObject)
			{
				inheritType = nameof(NetworkObject);
				string netTypeEnumName = nameof(NetworkObjectType);
				string netTypeDeclaration = $"{Indent}public override {netTypeEnumName} Type => {netTypeEnumName}.{ObjectName};";
				declarationContent = netTypeDeclaration + NewLine + NewLine + declarationContent;
			}

			return string.Format(SyncFormat.MasterDeclaration,
								 ObjectName, inheritType,
								 declarationContent,
								 synchronization);
		}

		public string GenerateRemoteDeclaration()
		{
			string declarationContent = string.Empty;
			string synchronizeContent = string.Empty;
			string deserializeAllContent = string.Empty;

			if (HasReliable)
			{
				GenerateOption reliableOption = new(SyncType.Reliable, ReliableProperties, ReliableFunctions);

				declarationContent += Remote_GenDeclaration(reliableOption) + NewLine;
				synchronizeContent += Remote_GenPropertyDeserializeContent(reliableOption) + NewLine;
			}

			if (HasUnreliable)
			{
				GenerateOption unreliableOption = new(SyncType.Unreliable, UnreliableProperties, UnreliableFunctions);

				declarationContent += Remote_GenDeclaration(unreliableOption) + NewLine;
				synchronizeContent += Remote_GenPropertyDeserializeContent(unreliableOption) + NewLine;
			}

			GenerateOption allOption = new(SyncType.RelibaleOrUnreliable, AllProperties, AllFunctions);
			deserializeAllContent += Remote_GenMasterDeserializeEveryProperty(allOption) + NewLine;

			// Combine all contents
			string inheritType = nameof(IRemoteSynchronizable);

			if (this.IsNetworkObject)
			{
				inheritType = nameof(RemoteNetworkObject);
				string netTypeEnumName = nameof(NetworkObjectType);
				string netTypeDeclaration = $"{Indent}public override {netTypeEnumName} Type => {netTypeEnumName}.{ObjectName};";
				declarationContent = netTypeDeclaration + NewLine + NewLine + declarationContent;
			}

			return string.Format(SyncFormat.RemoteDeclaration,
								 ObjectName,
								 inheritType,
								 declarationContent,
								 synchronizeContent,
								 deserializeAllContent);
		}

		#region Master side

		private static string Master_GenDeclaration(GenerateOption option)
		{
			string declarationContent = string.Empty;
			foreach (var prop in option.Properties)
			{
				declarationContent += prop.GeneratePraivteDeclaration() + NewLine + NewLine;
			}
			foreach (var func in option.Functions)
			{
				declarationContent += func.GetPartialDeclaraction() + NewLine + NewLine;
			}
			CodeFormat.AddIndent(ref declarationContent);
			return declarationContent;
		}

		private static string Master_GenPropertySynchronizeContent(GenerateOption option)
		{
			string syncContent = string.Empty;

			if (option.Properties.Count > 0)
			{
				int propIndex = 0;
				int propDirtyBitsCount = 0;
				foreach (var prop in option.Properties)
				{
					string dirtyBitName = string.Format(option.PropertyDirtyBitName, propDirtyBitsCount);
					syncContent += prop.GeneratePropertyGetSet(dirtyBitName, propIndex);
					propIndex++;
					if (propIndex >= 8)
					{
						propIndex = 0;
						propDirtyBitsCount++;
					}
				}
			}

			// Function Synchronization
			if (option.Functions.Count > 0)
			{
				int funcIndex = 0;
				int funcDirtyBitsCount = 0;
				foreach (var func in option.Functions)
				{
					string dirtyBitName = string.Format(option.FunctionDirtyBitName, funcDirtyBitsCount);
					syncContent += func.GetFunctionCallWithStack(dirtyBitName, funcIndex);

					funcIndex++;
					if (funcIndex >= 8)
					{
						funcIndex = 0;
						funcDirtyBitsCount++;
					}
				}
			}

			// Declaration dirty bits and IsDirty Getter
			string dirtyBitsContent = string.Empty;
			string isDirtyContent = string.Empty;
			if (option.Properties.Count > 0)
			{
				for (int i = 0; i <= option.Properties.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
					dirtyBitsContent += string.Format(SyncFormat.DeclarationDirtyBits,
													  nameof(BitmaskByte), dirtyBitName) + NewLine;

					var prop = option.Properties[i];
					if (prop.SerializeType == SerializeType.SyncObject)
					{
						isDirtyContent += string.Format(option.IsObjectDirtyBinder, dirtyBitName) + NewLine;
					}
					else
					{
						isDirtyContent += string.Format(option.IsDirtyBinder, dirtyBitName) + NewLine;
					}
				}
			}
			if (option.Functions.Count > 0)
			{
				for (int i = 0; i <= option.Functions.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
					dirtyBitsContent += string.Format(SyncFormat.DeclarationDirtyBits,
													  nameof(BitmaskByte), dirtyBitName) + NewLine;
					isDirtyContent += string.Format(option.IsDirtyBinder, dirtyBitName) + NewLine;
				}
			}

			CodeFormat.AddIndent(ref isDirtyContent, 2);
			isDirtyContent = string.Format(option.IsDirtyGetter, isDirtyContent);

			syncContent = dirtyBitsContent + NewLine + isDirtyContent + NewLine + syncContent;
			CodeFormat.AddIndent(ref syncContent);

			return syncContent;
		}

		private static string Master_GenSerializeFunction(GenerateOption option)
		{
			string syncObjectDirtyCheck = string.Empty;
			string anyDirtyBitsContent = string.Empty;
			if (option.Properties.Count > 0)
			{
				for (int i = 0; i <= option.Properties.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
					anyDirtyBitsContent += string.Format(SyncFormat.AnyDirtyBits, i, dirtyBitName) + NewLine;
				}
				for (int i = 0; i < option.Properties.Count; i++)
				{
					var prop = option.Properties[i];
					if (prop.SerializeType == SerializeType.SyncObject)
					{
						syncObjectDirtyCheck += prop.GetDirtyCheckIfSyncObject(option, i / 8, i);
					}
				}
			}
			if (option.Functions.Count > 0)
			{
				for (int i = 0; i <= option.Functions.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
					anyDirtyBitsContent += string.Format(SyncFormat.AnyDirtyBits, i + 4, dirtyBitName) + NewLine;
				}
			}

			anyDirtyBitsContent = syncObjectDirtyCheck + NewLine + anyDirtyBitsContent;

			CodeFormat.AddIndent(ref anyDirtyBitsContent);

			// Serialize reliable
			generateSerializeCode(option, out var propSerialize, out var funcSerialize);

			string serializeContent = string.Format(SyncFormat.SerializeSync,
													option.SerializeFunctionName,
													option.BitmaskTypeName,
													anyDirtyBitsContent,
													propSerialize,
													funcSerialize);

			CodeFormat.AddIndent(ref serializeContent);
			return serializeContent;

			void generateSerializeCode(GenerateOption option,
									   out string propSerialize,
									   out string funcSerialize)
			{
				propSerialize = string.Empty;
				funcSerialize = string.Empty;

				// Property serialize group content
				List<string> propSerializeGroup = new();
				int propDirtyBitsCounter = 0;
				int curPropIndex = 0;
				for (int i = 0; i < option.Properties.Count; i++)
				{
					if (propSerializeGroup.Count <= propDirtyBitsCounter)
					{
						propSerializeGroup.Add(string.Empty);
					}
					var prop = option.Properties[i];

					if (prop.SyncType == option.SyncType)
					{
						string dirtyBitName = string.Format(option.PropertyDirtyBitName, propDirtyBitsCounter);
						string serializePropContent = prop.GeneratetPropertySerializeIfDirty(dirtyBitName, curPropIndex, option);
						propSerializeGroup[propDirtyBitsCounter] += serializePropContent;
					}

					curPropIndex++;
					if (curPropIndex >= 8)
					{
						curPropIndex = 0;
						propDirtyBitsCounter++;
					}
				}

				for (int i = 0; i < propSerializeGroup.Count; i++)
				{
					string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
					string contentGroup = propSerializeGroup[i];
					CodeFormat.AddIndent(ref contentGroup);
					propSerialize += string.Format(SyncFormat.PropertySerializeGroup, i, dirtyBitName, contentGroup);
				}
				CodeFormat.AddIndent(ref propSerialize);

				// Funtion serilalize group content
				List<string> funcSerializeGroup = new();
				int funcDirtyBitsCounter = 0;
				int curFuncIndex = 0;
				for (int i = 0; i < option.Functions.Count; i++)
				{
					if (funcSerializeGroup.Count <= funcDirtyBitsCounter)
					{
						funcSerializeGroup.Add(string.Empty);
					}
					var func = option.Functions[i];

					if (func.SyncType == option.SyncType)
					{
						string dirtyBitName = string.Format(option.FunctionDirtyBitName, funcDirtyBitsCounter);
						funcSerializeGroup[funcDirtyBitsCounter] += func.GetSerializeIfDirty(dirtyBitName, curFuncIndex);
					}

					curFuncIndex++;
					if (curFuncIndex >= 8)
					{
						curFuncIndex = 0;
						funcDirtyBitsCounter++;
					}
				}

				for (int i = 0; i < funcSerializeGroup.Count; i++)
				{
					string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
					string contentGroup = funcSerializeGroup[i];
					CodeFormat.AddIndent(ref contentGroup);
					funcSerialize += string.Format(SyncFormat.FunctionSerializeGroup, i, dirtyBitName, contentGroup);
				}
				CodeFormat.AddIndent(ref funcSerialize);
			}
		}

		private static string Master_GenDirtyBitsClearFunction(GenerateOption option)
		{
			string dirtyBitClearContent = string.Empty;
			if (option.Properties.Count > 0)
			{
				for (int i = 0; i <= option.Properties.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
					dirtyBitClearContent += string.Format(SyncFormat.DirtyBitsClear, dirtyBitName) + NewLine;
				}
			}
			if (option.Functions.Count > 0)
			{
				for (int i = 0; i <= option.Functions.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
					dirtyBitClearContent += string.Format(SyncFormat.DirtyBitsClear, dirtyBitName) + NewLine;
				}
			}

			CodeFormat.AddIndent(ref dirtyBitClearContent);
			var dirtyBitClearFunction = string.Format(SyncFormat.ClearDirtyBitFunction,
													  option.ClearFunctionName,
													  dirtyBitClearContent);
			CodeFormat.AddIndent(ref dirtyBitClearFunction);

			return dirtyBitClearFunction;
		}

		private static string Master_GenMasterSerializeEveryProperty(GenerateOption option)
		{
			string everyContent = string.Empty;
			foreach (var p in option.Properties)
			{
				everyContent += p.GetWriterSerialize(option);
			}
			CodeFormat.AddIndent(ref everyContent);
			everyContent = string.Format(SyncFormat.SerializeEveryProperty,
										 option.SerializeFunctionName,
										 everyContent) + NewLine;
			CodeFormat.AddIndent(ref everyContent);
			return everyContent;
		}

		#endregion

		#region Remote Side

		private static string Remote_GenDeclaration(GenerateOption option)
		{
			string declarationContent = string.Empty;
			foreach (var prop in option.Properties)
			{
				declarationContent += prop.GenerateRemotePropertyDeclaration() + NewLine + NewLine;
			}
			foreach (var func in option.Functions)
			{
				declarationContent += func.GetPartialDeclaraction() + NewLine + NewLine;
			}
			CodeFormat.AddIndent(ref declarationContent);
			return declarationContent;
		}

		private static string Remote_GenPropertyDeserializeContent(GenerateOption option)
		{
			List<string> propDeserializeGroup = new();
			int propDirtyBitsCounter = 0;
			int curPropIndex = 0;
			for (int i = 0; i < option.Properties.Count; i++)
			{
				if (propDeserializeGroup.Count <= propDirtyBitsCounter)
				{
					propDeserializeGroup.Add(string.Empty);
				}
				var prop = option.Properties[i];

				string dirtyBitName = string.Format(option.PropertyDirtyBitName, propDirtyBitsCounter);
				string deserializePropContent = prop.GeneratetPropertyDeserializeIfDirty(dirtyBitName, curPropIndex, option);
				propDeserializeGroup[propDirtyBitsCounter] += deserializePropContent;
				curPropIndex++;
				if (curPropIndex >= 8)
				{
					curPropIndex = 0;
					propDirtyBitsCounter++;
				}
			}

			string propDeserialize = string.Empty;
			for (int i = 0; i < propDeserializeGroup.Count; i++)
			{
				string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
				string contentGroup = propDeserializeGroup[i];
				CodeFormat.AddIndent(ref contentGroup);
				propDeserialize += string.Format(SyncFormat.PropertyDeserializeGroup, i,
												 nameof(BitmaskByte), dirtyBitName, contentGroup);
			}
			CodeFormat.AddIndent(ref propDeserialize);

			// Funtion deserialize group content
			List<string> funcDeserializeGroup = new();
			int funcDirtyBitsCounter = 0;
			int curFuncIndex = 0;
			for (int i = 0; i < option.Functions.Count; i++)
			{
				if (funcDeserializeGroup.Count <= funcDirtyBitsCounter)
				{
					funcDeserializeGroup.Add(string.Empty);
				}
				var func = option.Functions[i];

				string dirtyBitName = string.Format(option.FunctionDirtyBitName, funcDirtyBitsCounter);
				funcDeserializeGroup[funcDirtyBitsCounter] += func.GetFunctionDeserializeIfDirty(dirtyBitName, curFuncIndex);
				curFuncIndex++;
				if (curFuncIndex >= 8)
				{
					curFuncIndex = 0;
					funcDirtyBitsCounter++;
				}
			}

			string funcDeserialize = string.Empty;
			for (int i = 0; i < funcDeserializeGroup.Count; i++)
			{
				string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
				string contentGroup = funcDeserializeGroup[i];
				CodeFormat.AddIndent(ref contentGroup);
				funcDeserialize += string.Format(SyncFormat.FunctionDeserializeGroup, i, dirtyBitName, nameof(BitmaskByte), contentGroup);
			}
			CodeFormat.AddIndent(ref funcDeserialize);

			// Combind
			var deserializeContent = string.Format(SyncFormat.DeserializeSync,
												   option.DeserializeFunctionName,
												   option.BitmaskTypeName,
												   propDeserialize,
												   funcDeserialize);
			CodeFormat.AddIndent(ref deserializeContent);

			return deserializeContent;
		}

		private static string Remote_GenMasterDeserializeEveryProperty(GenerateOption option)
		{
			string everyContent = string.Empty;
			foreach (var p in option.Properties)
			{
				everyContent += p.GetReadDeserialize(option);
			}
			CodeFormat.AddIndent(ref everyContent);
			everyContent = string.Format(SyncFormat.DeserializeEveryProperty,
										 option.DeserializeFunctionName,
										 everyContent) + NewLine;
			CodeFormat.AddIndent(ref everyContent);
			return everyContent;
		}

		#endregion
	}
}