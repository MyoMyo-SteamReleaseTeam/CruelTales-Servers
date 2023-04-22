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
				declarationContent += prop.GetPraivteDeclaration() + NewLine + NewLine;
			}
			foreach (var func in Functions)
			{
				declarationContent += func.GetPartialDeclaraction() + NewLine + NewLine;
			}

			// Property Synchronization
			string syncContent = string.Empty;

			int propIndex = 0;
			int propDirtyBitsCount = 0;
			foreach (var prop in Properties)
			{
				string dirtyBitname = string.Format(SyncFormat.PropertyDirtyBitname, propDirtyBitsCount);
				syncContent += string.Format(SyncFormat.PropertyGetSet,
											 prop.TypeName,
											 prop.GetPublicPropertyName(),
											 prop.PrivateName,
											 dirtyBitname,
											 propIndex);
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
				string dirtyBitname = string.Format(SyncFormat.FunctionDirtyBitname, funcDirtyBitsCount);
				syncContent += string.Format(SyncFormat.FunctionCallWithStack,
											 func.FunctionName,
											 func.GetParameterContent(),
											 func.GetCallStackTupleContent(),
											 dirtyBitname,
											 funcIndex);

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
				string dirtyBitname = string.Format(SyncFormat.PropertyDirtyBitname, i);
				dirtyBitsContent += string.Format(SyncFormat.DeclarationDirtyBits,
												  nameof(BitmaskByte), dirtyBitname) + NewLine;
			}
			for (int i = 0; i <= funcDirtyBitsCount; i++)
			{
				string dirtyBitname = string.Format(SyncFormat.FunctionDirtyBitname, i);
				dirtyBitsContent += string.Format(SyncFormat.DeclarationDirtyBits,
												  nameof(BitmaskByte), dirtyBitname) + NewLine;
			}

			syncContent = dirtyBitsContent + NewLine + syncContent;

			// Serialize function
			string anyDirtyBitsContent = string.Empty;
			for (int i = 0; i <= propDirtyBitsCount; i++)
			{
				string dirtyBitname = string.Format(SyncFormat.PropertyDirtyBitname, i);
				anyDirtyBitsContent += string.Format(SyncFormat.AnyDirtyBits, i, dirtyBitname) + NewLine;
			}
			for (int i = 0; i <= funcDirtyBitsCount; i++)
			{
				string dirtyBitname = string.Format(SyncFormat.FunctionDirtyBitname, i);
				anyDirtyBitsContent += string.Format(SyncFormat.AnyDirtyBits, i + 4, dirtyBitname) + NewLine;
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

				string dirtyBitname = string.Format(SyncFormat.PropertyDirtyBitname, propDirtyBitsCounter);
				propSerializeGroup[propDirtyBitsCounter] += string.Format(SyncFormat.PropertySerializeIfDirty,
																		  dirtyBitname, curPropIndex, prop.GetWriterSerialize());
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
				string dirtyBitname = string.Format(SyncFormat.PropertyDirtyBitname, i);
				string contentGroup = propSerializeGroup[i];
				CodeFormat.AddIndent(ref contentGroup);
				propSerialize += string.Format(SyncFormat.PropertySerializeGroup, i, dirtyBitname, contentGroup);
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

				var funcSerializeContent = func.GetCallstackSerializeContent();
				CodeFormat.AddIndent(ref funcSerializeContent, 2);
				string dirtyBitname = string.Format(SyncFormat.FunctionDirtyBitname, funcDirtyBitsCounter);
				funcSerializeGroup[funcDirtyBitsCounter] += string.Format(SyncFormat.FunctionSerializeIfDirty,
																		  dirtyBitname, curFuncIndex,
																		  func.FunctionName,
																		  funcSerializeContent);
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
				string dirtyBitname = string.Format(SyncFormat.FunctionDirtyBitname, i);
				string contentGroup = funcSerializeGroup[i];
				CodeFormat.AddIndent(ref contentGroup);
				funcSerialize += string.Format(SyncFormat.FunctionSerializeGroup, i, dirtyBitname, contentGroup);
			}
			CodeFormat.AddIndent(ref funcSerialize);

			// Add serialize funtion
			var serializeContent = string.Format(SyncFormat.SerializeSync,
												 nameof(NetworkObject.SerializeSyncReliable),
												 nameof(BitmaskByte),
												 anyDirtyBitsContent,
												 propSerialize,
												 funcSerialize, "");

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
