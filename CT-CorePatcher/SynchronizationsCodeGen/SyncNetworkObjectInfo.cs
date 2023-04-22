using System.Collections.Generic;
using System.Globalization;
using CT.CorePatcher.Synchronizations;
using CT.Tools.Collections;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class SyncNetworkObjectInfo
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		public string ObjectName { get; private set; } = string.Empty;

		public List<SyncPropertyToken> Properties { get; private set; } = new();
		public List<SyncFunctionToken> Functions { get; private set; } = new();

		public SyncNetworkObjectInfo(string objectName)
		{
			ObjectName = objectName;
		}

		public string GenerateServerDeclaration()
		{
			// Declaration
			string declarationContent = string.Empty;

			foreach (var prop in Properties)
			{
				declarationContent += prop.GeneratePraivteDeclaration() + NewLine + NewLine;
			}
			foreach (var func in Functions)
			{
				declarationContent += func.GeneratePartialDeclaraction() + NewLine + NewLine;
			}

			// Property Synchronization
			string syncContent = string.Empty;

			int propIndex = 0;
			int propDirtyBitsCount = 0;
			foreach (var prop in Properties)
			{
				string dirtyBitName = string.Format(SyncFormat.PropertyDirtyBitName, propDirtyBitsCount);
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
			foreach (var func in Functions)
			{
				string dirtyBitName = string.Format(SyncFormat.FunctionDirtyBitName, funcDirtyBitsCount);
				syncContent += func.GenerateFunctionCallWithStack(dirtyBitName, funcIndex);

				funcIndex++;
				if (funcIndex >= 8)
				{
					funcIndex = 0;
					funcDirtyBitsCount++;
				}
			}
			
			// Declaration dirty bits
			string dirtyBitsContent = string.Empty;
			for (int i = 0; i <= propDirtyBitsCount; i++)
			{
				string dirtyBitName = string.Format(SyncFormat.PropertyDirtyBitName, i);
				dirtyBitsContent += string.Format(SyncFormat.DeclarationDirtyBits,
												  nameof(BitmaskByte), dirtyBitName) + NewLine;
			}
			for (int i = 0; i <= funcDirtyBitsCount; i++)
			{
				string dirtyBitName = string.Format(SyncFormat.FunctionDirtyBitName, i);
				dirtyBitsContent += string.Format(SyncFormat.DeclarationDirtyBits,
												  nameof(BitmaskByte), dirtyBitName) + NewLine;
			}

			syncContent = dirtyBitsContent + NewLine + syncContent;

			// Serialize function
			string anyDirtyBitsContent = string.Empty;
			for (int i = 0; i <= propDirtyBitsCount; i++)
			{
				string dirtyBitName = string.Format(SyncFormat.PropertyDirtyBitName, i);
				anyDirtyBitsContent += string.Format(SyncFormat.AnyDirtyBits, i, dirtyBitName) + NewLine;
			}
			for (int i = 0; i <= funcDirtyBitsCount; i++)
			{
				string dirtyBitName = string.Format(SyncFormat.FunctionDirtyBitName, i);
				anyDirtyBitsContent += string.Format(SyncFormat.AnyDirtyBits, i + 4, dirtyBitName) + NewLine;
			}
			CodeFormat.AddIndent(ref anyDirtyBitsContent);

			// Property serialize group content
			List<string> propSerializeGroup = new();
			int propDirtyBitsCounter = 0;
			int curPropIndex = 0;
			for (int i = 0; i < Properties.Count; i++)
			{
				if (propSerializeGroup.Count <= propDirtyBitsCounter)
				{
					propSerializeGroup.Add(string.Empty);
				}
				var prop = Properties[i];

				string dirtyBitName = string.Format(SyncFormat.PropertyDirtyBitName, propDirtyBitsCounter);
				propSerializeGroup[propDirtyBitsCounter] += prop.GeneratetPropertySerializeIfDirty(dirtyBitName, curPropIndex);
				curPropIndex++;
				if (curPropIndex >= 8)
				{
					curPropIndex = 0;
					propDirtyBitsCounter++;
				}
			}

			string propSerialize = string.Empty;
			for (int i = 0; i < propSerializeGroup.Count; i++)
			{
				string dirtyBitName = string.Format(SyncFormat.PropertyDirtyBitName, i);
				string contentGroup = propSerializeGroup[i];
				CodeFormat.AddIndent(ref contentGroup);
				propSerialize += string.Format(SyncFormat.PropertySerializeGroup, i, dirtyBitName, contentGroup);
			}
			CodeFormat.AddIndent(ref propSerialize);

			// Funtion serilalize group content
			List<string> funcSerializeGroup = new();
			int funcDirtyBitsCounter = 0;
			int curFuncIndex = 0;
			for (int i = 0; i < Functions.Count; i++)
			{
				if (funcSerializeGroup.Count <= funcDirtyBitsCounter)
				{
					funcSerializeGroup.Add(string.Empty);
				}
				var func = Functions[i];

				string dirtyBitName = string.Format(SyncFormat.FunctionDirtyBitName, funcDirtyBitsCounter);
				funcSerializeGroup[funcDirtyBitsCounter] += func.GenerateSerializeIfDirty(dirtyBitName, curFuncIndex);
				curFuncIndex++;
				if (curFuncIndex >= 8)
				{
					curFuncIndex = 0;
					funcDirtyBitsCounter++;
				}
			}

			string funcSerialize = string.Empty;
			for (int i = 0; i < funcSerializeGroup.Count; i++)
			{
				string dirtyBitName = string.Format(SyncFormat.FunctionDirtyBitName, i);
				string contentGroup = funcSerializeGroup[i];
				CodeFormat.AddIndent(ref contentGroup);
				funcSerialize += string.Format(SyncFormat.FunctionSerializeGroup, i, dirtyBitName, contentGroup);
			}
			CodeFormat.AddIndent(ref funcSerialize);

			// Add serialize funtion
			string dirtyBitClearContent = string.Empty;
			for (int i = 0; i <= propDirtyBitsCount; i++)
			{
				string dirtyBitName = string.Format(SyncFormat.PropertyDirtyBitName, propDirtyBitsCounter);
				dirtyBitClearContent += string.Format(SyncFormat.DirtyBitsClear, dirtyBitName) + NewLine;
			}
			for (int i = 0; i <= funcDirtyBitsCount; i++)
			{
				string dirtyBitName = string.Format(SyncFormat.FunctionDirtyBitName, funcDirtyBitsCounter);
				dirtyBitClearContent += string.Format(SyncFormat.DirtyBitsClear, dirtyBitName) + NewLine;
			}
			CodeFormat.AddIndent(ref dirtyBitClearContent);

			// Combind
			var serializeContent = string.Format(SyncFormat.SerializeSync,
												 nameof(NetworkObject.SerializeSyncReliable),
												 nameof(BitmaskByte),
												 anyDirtyBitsContent,
												 propSerialize,
												 funcSerialize, dirtyBitClearContent);

			CodeFormat.AddIndent(ref serializeContent);

			CodeFormat.AddIndent(ref syncContent);
			CodeFormat.AddIndent(ref declarationContent);

			return string.Format(SyncFormat.ServerDeclaration,
								 ObjectName,
								 nameof(NetworkObject),
								 declarationContent,
								 syncContent,
								 serializeContent);
		}
	}
}
