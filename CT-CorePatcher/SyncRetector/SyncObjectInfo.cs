using System;
using System.Collections.Generic;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;

namespace CT.CorePatcher.SyncRetector
{
	public class SyncObjectInfo
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		public bool IsNetworkObject { get; private set; } = false;
		public string OriginObjectName { get; private set; } = string.Empty;
		public string ObjectName { get; private set; } = string.Empty;

		/// <summary>Master에서 Remote로 동기화되는 요소들입니다.</summary>
		private SyncElementBucket _masterElementBucket = new();

		/// </summary>Remote에서 Master로 동기화되는 요소들입니다.</summary>
		private SyncElementBucket _remoteElementBucket = new();

		public SyncObjectInfo(string objectName, bool isNetworkObject)
		{
			OriginObjectName = objectName;
			IsNetworkObject = isNetworkObject;
		}

		public void AddPropertyToken(SyncPropertyToken token, SyncType syncType, SyncDirection direction)
		{
			if (direction == SyncDirection.FromMaster)
			{
				_masterElementBucket.AddPropertyToken(token, syncType);
			}
			else if (direction == SyncDirection.FromRemote)
			{
				_remoteElementBucket.AddPropertyToken(token, syncType);
			}
		}

		public void AddFunctionToken(SyncFunctionToken token, SyncType syncType, SyncDirection direction)
		{
			if (direction == SyncDirection.FromMaster)
			{
				_masterElementBucket.AddFunctionToken(token, syncType);
			}
			else if (direction == SyncDirection.FromRemote)
			{
				_remoteElementBucket.AddFunctionToken(token, syncType);
			}
		}

		public string GenerateCode(SyncDirection syncDirection)
		{
			return string.Empty;

			//			// Set name
			//			ObjectName = OriginObjectName;

			//			// Setting extra comments and region
			//			string declarFrom = Indent + syncDirection.GetDeclarationComment() + NewLine;
			//			string declarTo = Indent + syncDirection.Reverse().GetDeclarationComment() + NewLine;

			//			string regionFrom = @"#region SYNCHRONIZATIONS" + NewLine;
			//			string regionTo = syncDirection.Reverse().GetRegionContent() + NewLine;

			//			string endRegion = @"#endregion" + NewLine;

			//			// Set element buckets
			//			SyncElementBucket syncFrom = _masterElementBucket;
			//			SyncElementBucket syncTo = _remoteElementBucket;

			//			if (syncDirection != SyncDirection.FromMaster)
			//			{
			//				syncFrom = _remoteElementBucket;
			//				syncTo = _masterElementBucket;
			//			}

			//			// Initialize content strings
			//			string declarationContent = string.Empty;
			//			string synchronizeContent = string.Empty;
			//			string syncFuncContent = string.Empty;
			//			string dirtyBitClearContent = string.Empty;

			//			// Generate forward
			//			GenerateOption fromReliableOption = new(SyncType.Reliable,
			//													syncFrom.ReliableProperties,
			//													syncFrom.ReliableFunctions);

			//			GenerateOption fromUnreliableOption = new(SyncType.Unreliable,
			//													  syncFrom.UnreliableProperties,
			//													  syncFrom.UnreliableFunctions);

			//			GenerateOption fromAllOption = new(SyncType.RelibaleOrUnreliable,
			//											   syncFrom.AllProperties,
			//											   syncFrom.AllFunctions);

			//// Bind separator
			//			if (fromAllOption.HasProperties)
			//			{
			//				declarationContent += declarFrom;
			//			}

			//			// Create content
			//			if (IsNetworkObject) fromReliableOption.Modifier = "override ";
			//			declarationContent += Master_GenDeclaration(fromReliableOption) + NewLine;
			//			synchronizeContent += Master_GenPropertySynchronizeContent(fromReliableOption) + NewLine;
			//			syncFuncContent += Master_GenSerializeFunction(fromReliableOption) + NewLine;
			//			dirtyBitClearContent += Master_GenDirtyBitsClearFunction(fromReliableOption) + NewLine;

			//			if (IsNetworkObject) fromUnreliableOption.Modifier = "override ";
			//			declarationContent += Master_GenDeclaration(fromUnreliableOption) + NewLine;
			//			synchronizeContent += Master_GenPropertySynchronizeContent(fromUnreliableOption) + NewLine;
			//			syncFuncContent += Master_GenSerializeFunction(fromUnreliableOption) + NewLine;
			//			dirtyBitClearContent += Master_GenDirtyBitsClearFunction(fromUnreliableOption) + NewLine;

			//			if (IsNetworkObject) fromAllOption.Modifier = "override ";
			//			syncFuncContent += Master_GenMasterSerializeEveryProperty(fromAllOption) + NewLine;

			//			// Generate backward
			//			GenerateOption toReliableOption = new(SyncType.Reliable, syncTo.ReliableProperties, syncTo.ReliableFunctions);
			//			GenerateOption toUnreliableOption = new(SyncType.Unreliable, syncTo.UnreliableProperties, syncTo.UnreliableFunctions);
			//			GenerateOption toAllOption = new(SyncType.RelibaleOrUnreliable, syncTo.AllProperties, syncTo.AllFunctions);

			//// Bind separator
			//			if (toAllOption.HasProperties)
			//			{
			//				declarationContent += declarTo;
			//			}

			//			// Create content
			//			if (IsNetworkObject) toReliableOption.Modifier = "override ";
			//			declarationContent += Remote_GenDeclaration(toReliableOption) + NewLine;
			//			syncFuncContent += Remote_GenPropertyDeserializeContent(toReliableOption) + NewLine;

			//			if (IsNetworkObject) toUnreliableOption.Modifier = "override ";
			//			declarationContent += Remote_GenDeclaration(toUnreliableOption) + NewLine;
			//			syncFuncContent += Remote_GenPropertyDeserializeContent(toUnreliableOption) + NewLine;

			//			if (IsNetworkObject) toAllOption.Modifier = "override ";
			//			syncFuncContent += Remote_GenMasterDeserializeEveryProperty(toAllOption) + NewLine;

			//			// Set names
			//			string inheritType = string.Empty;

			//			if (this.IsNetworkObject)
			//			{
			//				if (syncDirection == SyncDirection.FromMaster)
			//				{
			//					inheritType = SyncFormat.MasterNetworkObjectTypeName;
			//				}
			//				else
			//				{
			//					inheritType = SyncFormat.RemoteNetworkObjectTypeName;
			//				}
			//				string netTypeName = SyncFormat.NetworkObjectTypeTypeName;
			//				string netTypeDeclaration = $"{Indent}public override {netTypeName} Type => {netTypeName}.{OriginObjectName};";
			//				declarationContent = netTypeDeclaration + NewLine + NewLine + declarationContent + NewLine;
			//			}
			//			else
			//			{
			//				inheritType = nameof(ISynchronizable);
			//			}

			//			// Combine all contents
			//			string synchronization = regionFrom + 
			//									 synchronizeContent + NewLine +
			//									 syncFuncContent + NewLine +
			//									 dirtyBitClearContent + NewLine + 
			//									 endRegion;

			//			return string.Format(ObjectFormat.SyncObjectFormat,
			//								 ObjectName, inheritType,
			//								 declarationContent,
			//								 synchronization);
		}
	}

	public static class SyncCodeGenerator
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		#region Master side

		private static string Master_GenDeclaration(GenerateOption option, SyncType syncType, SyncDirection direction)
		{
			string declarationContent = string.Empty;
			foreach (var prop in option.Properties)
			{
				declarationContent += prop.GenDeclaration(syncType, direction) + NewLine + NewLine;
			}
			foreach (var func in option.Functions)
			{
				declarationContent += func.GetPartialDeclaraction(syncType, direction) + NewLine + NewLine;
			}
			CodeFormat.AddIndent(ref declarationContent);
			return declarationContent;
		}

		private static string Master_GenPropertySynchronizeContent(GenerateOption option)
		{
			// If there are no elements to sync
			if (!option.HasSyncElement)
			{
				string isDirtyNoElementContent = string.Format(option.IsDirtyNoElement, option.Modifier);
				CodeFormat.AddIndent(ref isDirtyNoElementContent);
				return isDirtyNoElementContent + NewLine;
			}

			// Generate content
			string syncContent = string.Empty;

			// Property synchronization
			if (option.HasProperties)
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

			// Function synchronization
			if (option.HasFunctions)
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
			if (option.HasProperties)
			{
				for (int i = 0; i <= option.Properties.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
					dirtyBitsContent += string.Format(ObjectFormat.DeclarationDirtyBits,
													  nameof(BitmaskByte), dirtyBitName) + NewLine;

					var prop = option.Properties[i];
					if (prop.SerializeType == SerializeType.SyncObject)
					{
						isDirtyContent += string.Format(option.IsObjectDirtyBinder, prop.PrivateName) + NewLine;
					}
					else
					{
						isDirtyContent += string.Format(option.IsDirtyBinder, dirtyBitName) + NewLine;
					}
				}
			}

			if (option.HasFunctions)
			{
				for (int i = 0; i <= option.Functions.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
					dirtyBitsContent += string.Format(ObjectFormat.DeclarationDirtyBits,
													  nameof(BitmaskByte), dirtyBitName) + NewLine;
					isDirtyContent += string.Format(option.IsDirtyBinder, dirtyBitName) + NewLine;
				}
			}

			CodeFormat.AddIndent(ref isDirtyContent, 2);
			isDirtyContent = string.Format(option.IsDirtyGetter, option.Modifier, isDirtyContent);

			syncContent = dirtyBitsContent + NewLine + isDirtyContent + NewLine + syncContent;
			CodeFormat.AddIndent(ref syncContent);

			return syncContent;
		}

		private static string Master_GenSerializeFunction(GenerateOption option, SyncType syncType)
		{
			// If there are no elements to sync
			if (!option.HasSyncElement)
			{
				string noElement = string.Format(ObjectFormat.SerializeSyncNoElement,
												 option.Modifier, option.SerializeFunctionName);
				CodeFormat.AddIndent(ref noElement);
				return noElement + NewLine;
			}

			// Serialize content
			string syncObjectDirtyCheck = string.Empty;
			string anyDirtyBitsContent = string.Empty;
			if (option.HasProperties)
			{
				for (int i = 0; i <= option.Properties.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
					anyDirtyBitsContent += string.Format(ObjectFormat.AnyDirtyBits, i, dirtyBitName) + NewLine;
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
			if (option.HasFunctions)
			{
				for (int i = 0; i <= option.Functions.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
					anyDirtyBitsContent += string.Format(ObjectFormat.AnyDirtyBits, i + 4, dirtyBitName) + NewLine;
				}
			}

			anyDirtyBitsContent = syncObjectDirtyCheck + NewLine + anyDirtyBitsContent;

			CodeFormat.AddIndent(ref anyDirtyBitsContent);

			// Serialize reliable
			generateSerializeCode(syncType, out var propSerialize, out var funcSerialize);

			string serializeContent = string.Format(ObjectFormat.SerializeSync,
													option.Modifier,
													option.SerializeFunctionName,
													option.BitmaskTypeName,
													anyDirtyBitsContent,
													propSerialize,
													funcSerialize);

			CodeFormat.AddIndent(ref serializeContent);
			return serializeContent;

			void generateSerializeCode(SyncType syncType,
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

					string dirtyBitName = string.Format(option.PropertyDirtyBitName, propDirtyBitsCounter);
					string serializePropContent = prop.GeneratetPropertySerializeIfDirty(dirtyBitName, curPropIndex, syncType);
					propSerializeGroup[propDirtyBitsCounter] += serializePropContent;

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
					propSerialize += string.Format(ObjectFormat.PropertySerializeGroup, i, dirtyBitName, contentGroup);
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

					string dirtyBitName = string.Format(option.FunctionDirtyBitName, funcDirtyBitsCounter);
					funcSerializeGroup[funcDirtyBitsCounter] += func.GetSerializeIfDirty(dirtyBitName, curFuncIndex);

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
					funcSerialize += string.Format(ObjectFormat.FunctionSerializeGroup, i + 4, dirtyBitName, contentGroup);
				}
				CodeFormat.AddIndent(ref funcSerialize);
			}
		}

		private static string Master_GenDirtyBitsClearFunction(GenerateOption option)
		{
			// If there are no elements to sync
			if (!option.HasSyncElement)
			{
				string noElement = string.Format(ObjectFormat.ClearDirtyBitFunctionNoElement,
												 option.Modifier, option.ClearFunctionName);
				CodeFormat.AddIndent(ref noElement);
				return noElement + NewLine;
			}

			string dirtyBitClearContent = string.Empty;
			if (option.HasProperties)
			{
				for (int i = 0; i <= option.Properties.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.PropertyDirtyBitName, i);
					dirtyBitClearContent += string.Format(ObjectFormat.DirtyBitsClear, dirtyBitName) + NewLine;
				}
			}
			if (option.HasFunctions)
			{
				for (int i = 0; i <= option.Functions.Count / 8; i++)
				{
					string dirtyBitName = string.Format(option.FunctionDirtyBitName, i);
					dirtyBitClearContent += string.Format(ObjectFormat.DirtyBitsClear, dirtyBitName) + NewLine;
				}
			}

			CodeFormat.AddIndent(ref dirtyBitClearContent);
			var dirtyBitClearFunction = string.Format(ObjectFormat.ClearDirtyBitFunction,
													  option.Modifier,
													  option.ClearFunctionName,
													  dirtyBitClearContent);
			CodeFormat.AddIndent(ref dirtyBitClearFunction);

			return dirtyBitClearFunction;
		}

		private static string Master_GenMasterSerializeEveryProperty(GenerateOption option, SyncType syncType)
		{
			// If there are no elements to sync
			if (!option.HasProperties)
			{
				var noElement = string.Format(ObjectFormat.SerializeSyncNoElement,
											  option.Modifier,
											  option.SerializeFunctionName) + NewLine;
				CodeFormat.AddIndent(ref noElement);
				return noElement;
			}

			string everyContent = string.Empty;
			foreach (var p in option.Properties)
			{
				everyContent += p.GetWriterSerialize(syncType);
			}
			CodeFormat.AddIndent(ref everyContent);
			everyContent = string.Format(ObjectFormat.SerializeEveryProperty,
										 option.Modifier,
										 option.SerializeFunctionName,
										 everyContent) + NewLine;
			CodeFormat.AddIndent(ref everyContent);
			return everyContent;
		}

		#endregion

		#region Remote Side

		private static string Remote_GenDeclaration(GenerateOption option, SyncType syncType, SyncDirection direction)
		{
			string declarationContent = string.Empty;
			foreach (var prop in option.Properties)
			{
				declarationContent += prop.GenerateRemotePropertyDeclaration(syncType, direction) + NewLine + NewLine;
			}
			foreach (var func in option.Functions)
			{
				declarationContent += func.GetPartialDeclaraction(syncType, direction) + NewLine + NewLine;
			}
			CodeFormat.AddIndent(ref declarationContent);
			return declarationContent;
		}

		private static string Remote_GenPropertyDeserializeContent(GenerateOption option, SyncType syncType)
		{
			// If there are no elements to sync
			if (!option.HasSyncElement)
			{
				var noElement = string.Format(ObjectFormat.DeserializeSyncNoElement,
											  option.Modifier, option.DeserializeFunctionName);
				CodeFormat.AddIndent(ref noElement);
				return noElement + NewLine;
			}

			// Property deserialize group content
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
				string deserializePropContent = prop.GeneratetPropertyDeserializeIfDirty(dirtyBitName, curPropIndex, syncType);
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
				propDeserialize += string.Format(ObjectFormat.PropertyDeserializeGroup, i,
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
				funcDeserialize += string.Format(ObjectFormat.FunctionDeserializeGroup, i + 4, nameof(BitmaskByte), dirtyBitName, contentGroup);
			}
			CodeFormat.AddIndent(ref funcDeserialize);

			// Combind
			var deserializeContent = string.Format(ObjectFormat.DeserializeSync,
												   option.Modifier,
												   option.DeserializeFunctionName,
												   option.BitmaskTypeName,
												   propDeserialize,
												   funcDeserialize);
			CodeFormat.AddIndent(ref deserializeContent);

			return deserializeContent;
		}

		private static string Remote_GenMasterDeserializeEveryProperty(GenerateOption option, SyncType syncType)
		{
			// If there are no elements to sync
			if (!option.HasProperties)
			{
				string noElement = string.Format(ObjectFormat.DeserializeSyncNoElement,
												 option.Modifier,
												 option.DeserializeFunctionName) + NewLine;
				CodeFormat.AddIndent(ref noElement);
				return noElement;
			}

			string everyContent = string.Empty;
			foreach (var p in option.Properties)
			{
				everyContent += p.GetReadDeserialize(syncType);
			}
			CodeFormat.AddIndent(ref everyContent);
			everyContent = string.Format(ObjectFormat.DeserializeEveryProperty,
										 option.Modifier,
										 option.DeserializeFunctionName,
										 everyContent) + NewLine;
			CodeFormat.AddIndent(ref everyContent);
			return everyContent;
		}

		#endregion
	}
}