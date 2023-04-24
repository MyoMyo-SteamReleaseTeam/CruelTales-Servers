using System;
using System.Collections.Generic;
using System.IO;
using CT.Common.Synchronizations;
using CT.Common.Tools.CodeGen;
using CT.Common.Tools.Data;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class SyncGenerateOperation
	{
		class CodeGenInfo
		{
			public string ObjectName { get; private set; }
			public string GenCode = string.Empty;
			public string FileName = string.Empty;

			//[Obsolete]
			//public CodeGenInfo(string objectName, bool isMaster)
			//{
			//	ObjectName = objectName;

			//	if (isMaster)
			//		FileName = SyncFormat.MasterPrefix + ObjectName;
			//	else
			//		FileName = SyncFormat.RemotePrefix + ObjectName;
			//}

			public CodeGenInfo(string objectName, SyncDirection direction)
			{
				ObjectName = objectName;

				if (direction == SyncDirection.FromMaster)
					FileName = SyncFormat.MasterPrefix + ObjectName + ".cs";
				else if (direction == SyncDirection.FromRemote)
					FileName = SyncFormat.RemotePrefix + ObjectName + ".cs";
			}
		}

		public List<string> UsingStatements = new();
		public List<SyncObjectInfo> SyncObjects = new();
		public string Namespace = string.Empty;
		public string TargetPath = string.Empty;

		public void Run(SyncDirection direction)
		{
			PatcherConsole.PrintJobInfo($"Generate sync object definitions : {direction}");

			string usingStatements = string.Empty;

			foreach (var u in UsingStatements)
			{
				usingStatements += u + CodeFormat.NewLine;
			}

			// Generate Code
			List<string> enumTypes = new();
			List<CodeGenInfo> genCodes = new();
			foreach (var syncObj in SyncObjects)
			{
				if (syncObj.IsNetworkObject)
				{
					enumTypes.Add(syncObj.OriginObjectName);
				}

				string code;
				if (direction == SyncDirection.FromMaster)
					code = syncObj.GenerateCode(SyncDirection.FromMaster);
				else
					code = syncObj.GenerateCode(SyncDirection.FromRemote);

				CodeGenInfo info = new(syncObj.ObjectName, direction);
				CodeFormat.AddIndent(ref code);
				info.GenCode = string.Format(SyncFormat.FileFormat, usingStatements, Namespace, code);
				info.GenCode = string.Format(CodeFormat.GeneratorMetadata, info.FileName, info.GenCode);
				CodeFormat.ReformCode(ref info.GenCode, startFromNamespace: true);

				genCodes.Add(info);
			}

			PatcherConsole.PrintJobInfo("Create sync object code files");

			// Remove existing files
			if (Directory.Exists(TargetPath))
			{
				foreach (var path in Directory.GetFiles(TargetPath))
				{
					File.Delete(path);
				}
			}

			// Create code files
			foreach (var info in genCodes)
			{
				string targetPath = Path.Combine(TargetPath, info.FileName);
				var result = FileHandler.TryWriteText(targetPath, info.GenCode, makeDirectory: true);
				if (result.ResultType == JobResultType.Success)
				{
					PatcherConsole.PrintSaveSuccessResult("Sync code gen completed : ", info.FileName, targetPath);
				}
				else
				{
					PatcherConsole.PrintError(result.Exception.Message);
				}
			}

			// Create eunm code file
			{
				var enumCodeFileName = SyncFormat.NetworkObjectTypeTypeName + ".cs";
				var enumCode = CodeGenerator_Enumerate.Generate(SyncFormat.NetworkObjectTypeTypeName,
																Namespace, true, true, new string[0], enumTypes,
																addUsingAndSemicolon: false);
				enumCode = string.Format(CodeFormat.GeneratorMetadata, enumCodeFileName, enumCode);

				string targetPath = Path.Combine(TargetPath, enumCodeFileName);
				var result = FileHandler.TryWriteText(targetPath, enumCode, true);
				if (result.ResultType == JobResultType.Success)
				{
					PatcherConsole.PrintSaveSuccessResult("Sync enum code gen completed : ", enumCodeFileName, targetPath);
				}
				else
				{
					PatcherConsole.PrintError(result.Exception.Message);
				}
			}
		}

		//[Obsolete]
		//public void Run(bool isMaster)
		//{
		//	PatcherConsole.PrintJobInfo("Generate sync object definitions");

		//	foreach (var syncObj in SyncObjects)
		//	{
		//		syncObj.SetSyncDirection(isMaster);
		//	}

		//	string usingStatements = string.Empty;

		//	foreach (var u in UsingStatements)
		//	{
		//		usingStatements += u + CodeFormat.NewLine;
		//	}

		//	// Generate Code
		//	List<string> enumTypes = new();
		//	List<CodeGenInfo> genCodes = new();
		//	foreach (var syncObj in SyncObjects)
		//	{
		//		if (syncObj.IsNetworkObject)
		//		{
		//			enumTypes.Add(syncObj.OriginObjectName);
		//		}
		//		CodeGenInfo info = new(syncObj.ObjectName, isMaster);

		//		string code;
		//		if (isMaster)
		//			code = syncObj.GenerateMasterDeclaration();
		//		else
		//			code = syncObj.GenerateRemoteDeclaration();

		//		CodeFormat.AddIndent(ref code);
		//		info.GenCode = string.Format(SyncFormat.FileFormat, usingStatements, Namespace, code);
		//		info.GenCode = string.Format(CodeFormat.GeneratorMetadata, info.FileName, info.GenCode);
		//		CodeFormat.RemoveNewLine(ref info.GenCode, startFromNamespace: true);
				
		//		genCodes.Add(info);
		//	}

		//	PatcherConsole.PrintJobInfo("Create sync object code files");

		//	// Remove existing files
		//	foreach (var path in Directory.GetFiles(TargetPath))
		//	{
		//		File.Delete(path);
		//	}

		//	// Create code files
		//	foreach (var info in genCodes)
		//	{
		//		string targetPath = Path.Combine(TargetPath, info.FileName);
		//		var result = FileHandler.TryWriteText(targetPath, info.GenCode, true);
		//		if (result.ResultType == JobResultType.Success)
		//		{
		//			PatcherConsole.PrintSaveSuccessResult("Sync code gen completed : ", info.FileName, targetPath);
		//		}
		//		else
		//		{
		//			PatcherConsole.PrintError(result.Exception.Message);
		//		}
		//	}

		//	{
		//		var enumCodeFileName = SyncFormat.NetworkObjectTypeTypeName + ".cs";
		//		var enumCode = CodeGenerator_Enumerate.Generate(SyncFormat.NetworkObjectTypeTypeName,
		//														Namespace, true, true, new string[0], enumTypes,
		//														addUsingAndSemicolon: false);
		//		enumCode = string.Format(CodeFormat.GeneratorMetadata, enumCodeFileName, enumCode);

		//		string targetPath = Path.Combine(TargetPath, enumCodeFileName);
		//		var result = FileHandler.TryWriteText(targetPath, enumCode, true);
		//		if (result.ResultType == JobResultType.Success)
		//		{
		//			PatcherConsole.PrintSaveSuccessResult("Sync enum code gen completed : ", enumCodeFileName, targetPath);
		//		}
		//		else
		//		{
		//			PatcherConsole.PrintError(result.Exception.Message);
		//		}
		//	}
		//}
	}
}
