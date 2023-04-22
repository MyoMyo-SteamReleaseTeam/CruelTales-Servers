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

			GenerateOption reliableOption = new(SyncType.Reliable,
												nameof(IMasterSynchronizable.SerializeSyncReliable),
												ReliableProperties,
												ReliableFunctions);

			GenerateOption unreliableOption = new(SyncType.Unreliable,
												  nameof(IMasterSynchronizable.SerializeSyncUnreliable),
												  UnreliableProperties,
												  UnreliableFunctions);

			declarationContent += Master_GenDeclaration(reliableOption) + NewLine;
			synchronizeContent += Master_GenPropertySynchronizeContent(reliableOption) + NewLine;
			serializeFuncContent += Master_GenSerializefunction(reliableOption) + NewLine;
			dirtyBitClearContent += Master_GenDirtyBitsClearFunction(reliableOption) + NewLine;

			declarationContent += Master_GenDeclaration(unreliableOption);
			synchronizeContent += Master_GenPropertySynchronizeContent(unreliableOption);
			serializeFuncContent += Master_GenSerializefunction(unreliableOption);
			dirtyBitClearContent += Master_GenDirtyBitsClearFunction(unreliableOption);

			GenerateOption allOption = new(SyncType.None, nameof(IMasterSynchronizable.SerializeEveryProperty),
										   AllProperties, AllFunctions);
			serializeAllContent += Master_GenMasterSerializeEveryProperty(allOption) + NewLine;

			string synchronization = synchronizeContent + NewLine +
									 serializeFuncContent + NewLine +
									 dirtyBitClearContent + NewLine +
									 serializeAllContent;

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

		#region Master side

		private string Master_GenDeclaration(GenerateOption option)
		{
			string declarationContent = string.Empty;
			foreach (var prop in option.Properties)
			{
				declarationContent += prop.GeneratePraivteDeclaration() + NewLine + NewLine;
			}
			foreach (var func in option.Functions)
			{
				declarationContent += func.GeneratePartialDeclaraction() + NewLine + NewLine;
			}
			CodeFormat.AddIndent(ref declarationContent);
			return declarationContent;
		}

		private string Master_GenPropertySynchronizeContent(GenerateOption option)
		{
			string syncContent = string.Empty;

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

			// Function Synchronization
			int funcIndex = 0;
			int funcDirtyBitsCount = 0;
			foreach (var func in option.Functions)
			{
				string dirtyBitName = string.Format(option.FunctionDirtyBitName, funcDirtyBitsCount);
				syncContent += func.GenerateFunctionCallWithStack(dirtyBitName, funcIndex);

				funcIndex++;
				if (funcIndex >= 8)
				{
					funcIndex = 0;
					funcDirtyBitsCount++;
				}
			}

			// Declaration dirty bits and IsDirty Getter
			string dirtyBitsContent = string.Empty;
			string isDirtyContent = string.Empty;
			for (int i = 0; i <= propDirtyBitsCount; i++)
			{
				string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
				dirtyBitsContent += string.Format(SyncFormat.DeclarationDirtyBits,
												  nameof(BitmaskByte), dirtyBitName) + NewLine;
				isDirtyContent += string.Format(SyncFormat.IsDirtyBinder, dirtyBitName) + NewLine;
			}
			for (int i = 0; i <= funcDirtyBitsCount; i++)
			{
				string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
				dirtyBitsContent += string.Format(SyncFormat.DeclarationDirtyBits,
												  nameof(BitmaskByte), dirtyBitName) + NewLine;
				isDirtyContent += string.Format(SyncFormat.IsDirtyBinder, dirtyBitName) + NewLine;
			}

			CodeFormat.AddIndent(ref isDirtyContent, 2);
			isDirtyContent = string.Format(option.IsDirtyGetter, isDirtyContent);

			syncContent = dirtyBitsContent + NewLine + isDirtyContent + NewLine + syncContent;
			CodeFormat.AddIndent(ref syncContent);

			return syncContent;
		}

		private string Master_GenSerializefunction(GenerateOption option)
		{
			string anyDirtyBitsContent = string.Empty;
			for (int i = 0; i <= option.Properties.Count / 8; i++)
			{
				string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
				anyDirtyBitsContent += string.Format(SyncFormat.AnyDirtyBits, i, dirtyBitName) + NewLine;
			}
			for (int i = 0; i <= option.Functions.Count / 8; i++)
			{
				string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
				anyDirtyBitsContent += string.Format(SyncFormat.AnyDirtyBits, i + 4, dirtyBitName) + NewLine;
			}
			CodeFormat.AddIndent(ref anyDirtyBitsContent);

			// Serialize reliable
			generateSerializeCode(option, SyncType.Reliable,
								  out var propSerialize,
								  out var funcSerialize);

			string serializeContent = string.Format(SyncFormat.SerializeSync,
													option.SerializeFunctionName,
													option.BitmaskTypeName,
													anyDirtyBitsContent,
													propSerialize,
													funcSerialize);

			CodeFormat.AddIndent(ref serializeContent);
			return serializeContent;

			void generateSerializeCode(GenerateOption option,
													  SyncType syncType,
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

					if (prop.SyncType == syncType)
					{
						string dirtyBitName = string.Format(option.PropertyDirtyBitName, propDirtyBitsCounter);
						propSerializeGroup[propDirtyBitsCounter] += prop.GeneratetPropertySerializeIfDirty(dirtyBitName, curPropIndex);
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

					if (func.SyncType == syncType)
					{
						string dirtyBitName = string.Format(option.FunctionDirtyBitName, funcDirtyBitsCounter);
						funcSerializeGroup[funcDirtyBitsCounter] += func.GenerateSerializeIfDirty(dirtyBitName, curFuncIndex);
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

		private string Master_GenDirtyBitsClearFunction(GenerateOption option)
		{
			string dirtyBitClearContent = string.Empty;
			for (int i = 0; i <= option.Properties.Count / 8; i++)
			{
				string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
				dirtyBitClearContent += string.Format(SyncFormat.DirtyBitsClear, dirtyBitName) + NewLine;
			}
			for (int i = 0; i <= option.Functions.Count / 8; i++)
			{
				string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
				dirtyBitClearContent += string.Format(SyncFormat.DirtyBitsClear, dirtyBitName) + NewLine;
			}
			CodeFormat.AddIndent(ref dirtyBitClearContent);

			var dirtyBitClearFunction = string.Format(SyncFormat.ClearDirtyBitFunction,
													  dirtyBitClearContent);
			CodeFormat.AddIndent(ref dirtyBitClearFunction);

			return dirtyBitClearContent;
		}

		private string Master_GenMasterSerializeEveryProperty(GenerateOption option)
		{

			string everyContent = string.Empty;
			foreach (var p in option.Properties)
			{
				everyContent += p.GetWriterSerialize();
			}
			CodeFormat.AddIndent(ref everyContent);
			everyContent = string.Format(SyncFormat.SerializeEveryProperty,
										 option.SerializeFunctionName,
										 everyContent) + NewLine;
			CodeFormat.AddIndent(ref everyContent);
			return everyContent;
		}

		#endregion

		public static string GenerateRemoteDeclaration(GenerateOption option)
		{
			// Declaration
			string declarationContent = string.Empty;

			foreach (var prop in option.Properties)
			{
				declarationContent += prop.GenerateRemotePropertyDeclaration() + NewLine + NewLine;
			}
			foreach (var func in option.Functions)
			{
				declarationContent += func.GeneratePartialDeclaraction() + NewLine + NewLine;
			}

			// Property Deserialize group content
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
				propDeserializeGroup[propDirtyBitsCounter] += prop.GeneratetPropertyDeserializeIfDirty(dirtyBitName, curPropIndex);
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

			// Funtion serilalize group content
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
				funcDeserializeGroup[funcDirtyBitsCounter] += func.GenerateDeserializeIfDirty(dirtyBitName, curFuncIndex);
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
			var serializeContent = string.Format(SyncFormat.DeserializeSync,
												 nameof(RemoteNetworkObject.DeserializeSyncReliable),
												 nameof(BitmaskByte),
												 propDeserialize,
												 funcDeserialize);

			CodeFormat.AddIndent(ref serializeContent);
			CodeFormat.AddIndent(ref declarationContent);

			// Deserialize every property
			string everyContent = string.Empty;
			foreach (var p in option.Properties)
			{
				everyContent += p.GetReadDeserialize();
			}
			CodeFormat.AddIndent(ref everyContent);
			everyContent = string.Format(SyncFormat.DeserializeEveryProperty,
										 nameof(RemoteNetworkObject.DeserializeEveryProperty),
										 everyContent) + NewLine;
			CodeFormat.AddIndent(ref everyContent);

			return string.Format(SyncFormat.RemoteDeclaration,
								 option.ObjectName,
								 nameof(RemoteNetworkObject),
								 declarationContent,
								 serializeContent,
								 everyContent);
		}
	}
}
