using System;
using System.Linq;
using CT.Tools.GetOpt;

namespace CT.CorePatcher.FilePatch
{
	public class FilePatcherRunner
	{
		public const string ORIGIN_PATH = "originPath";
		public const string TARGET_PATH = "targetPath";

		public const string GUARD_EXTENSION = "fp_guard_ext";
		public const string INCLUDE_EXTENSION = "fp_include_ext";
		public const string EXCLUDE_FOLDER = "fp_exclude_folder";

		public static bool Run(string[] args)
		{
			PatcherConsole.PrintProgramInfo("File Patcher");

			StringArgument originPath = new();
			StringArgument targetPath = new();
			string[] guardExtensionList = new string[0];
			string[] includeExtensionList = new string[0];
			string[] excludeFolderList = new string[0];

			OptionParser op = new OptionParser();
			op.BindArgument(op, ORIGIN_PATH, 2, originPath);
			op.BindArgument(op, TARGET_PATH, 2, targetPath);
			op.RegisterEvent(GUARD_EXTENSION, 1, (options) =>
			{
				try
				{
					guardExtensionList = options.ToArray();
				}
				catch
				{
					throw new NoProcessArgumentsException(GUARD_EXTENSION);
				}
			});
			op.RegisterEvent(INCLUDE_EXTENSION, 1, (options) =>
			{
				try
				{
					includeExtensionList = options.ToArray();
				}
				catch
				{
					throw new NoProcessArgumentsException(INCLUDE_EXTENSION);
				}
			});
			op.RegisterEvent(EXCLUDE_FOLDER, 1, (options) =>
			{
				try
				{
					excludeFolderList = options.ToArray();
				}
				catch
				{
					throw new NoProcessArgumentsException(EXCLUDE_FOLDER);
				}
			});
			try
			{
				op.OnArguments(args);
			}
			catch (Exception e)
			{
				PatcherConsole.PrintError(e.GetType().Name);
				Console.WriteLine();
				PatcherConsole.PrintError(e.Message);
				return false;
			}

			// Check arguments validation
			bool isValidArguments = true;
			if (originPath.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no origin path.");

			if (targetPath.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no target path.");

			// Setup patcher
			FilePatcher filePatcher = new FilePatcher();

			filePatcher.SetupGuardExtension(guardExtensionList);
			filePatcher.SetupIncludeExtension(includeExtensionList);
			filePatcher.SetupExcludeFolderName(excludeFolderList);

			filePatcher.SetupSourceDirectory(originPath.Argument);
			filePatcher.SetupTargetDirectory(targetPath.Argument);

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