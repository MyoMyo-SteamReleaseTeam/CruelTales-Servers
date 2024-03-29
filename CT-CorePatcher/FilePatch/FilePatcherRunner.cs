﻿namespace CT.CorePatcher.FilePatch
{
	public class FilePatcherRunner
	{
		public static bool Run(string sourcePath, string targetPath)
		{
			PatcherConsole.PrintJobInfo("File Patcher");

			// Check arguments validation
			bool isValidArguments = true;
			if (string.IsNullOrEmpty(sourcePath))
			{
				isValidArguments = false;
				PatcherConsole.PrintError($"There is no source path.");
			}

			if (string.IsNullOrEmpty(targetPath))
			{
				isValidArguments = false;
				PatcherConsole.PrintError($"There is no target path.");
			}

			if (isValidArguments == false)
			{
				return false;
			}

			// Setup patcher
			FilePatcher filePatcher = new FilePatcher();

			filePatcher.SetupGuardExtension(".asmdef");
			filePatcher.SetupIncludeExtension(".cs");
			filePatcher.SetupExcludeFolderName("obj", "bin", "debug", "build", "Legacy", "Definitions");

			filePatcher.SetupSourceDirectory(sourcePath);
			filePatcher.SetupTargetDirectory(targetPath);

			// Exception handling
			if (!filePatcher.HasGuardExtension)
			{
				PatcherConsole.PrintError("Invalid guard extension!");
				return false;
			}

			if (!filePatcher.HasValidExtensionRules)
			{
				PatcherConsole.PrintError("Invalid extension reuls!");
				return false;
			}

			if (!filePatcher.HasValidSourcePaths)
			{
				PatcherConsole.PrintError("Invalid source directory path");
				return false;
			}

			if (!filePatcher.HasValidTargetPaths)
			{
				PatcherConsole.PrintError("Invalid target directory path");
				return false;
			}

			return filePatcher.Excute();
		}
	}
}