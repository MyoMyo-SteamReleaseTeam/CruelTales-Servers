using System.Collections.Generic;
using System.IO;
using System.Linq;
using CT.Tools.Data;

namespace CT.CorePatcher.FilePatch
{
	/// <summary>Source에서 대상 Target으로 파일을 패치합니다.</summary>
	public class FilePatcher
	{
		public string TargetDirectory { get; private set; } = string.Empty;
		public string SourceDirectory { get; private set; } = string.Empty;

		private List<string> _guardExtensions = new();
		private List<string> _includeExtensionList = new();
		private List<string> _exceptFolderName = new();

		public bool HasGuardExtension
		{
			get
			{
				if (_guardExtensions == null)
					return false;

				return _guardExtensions.Count > 0;
			}
		}
		public bool HasValidExtensionRules
		{
			get
			{
				if (_includeExtensionList == null)
					return false;

				return _includeExtensionList.Count > 0;
			}
		}
		public bool HasValidSourcePaths => Directory.Exists(SourceDirectory);
		public bool HasValidTargetPaths => Directory.Exists(TargetDirectory);

		/// <summary>복사할 디렉토리를 설정합니다.</summary>
		public bool SetupSourceDirectory(string sourceDirectory)
		{
			SourceDirectory = sourceDirectory;
			if (string.IsNullOrEmpty(SourceDirectory))
			{
				PatcherConsole.PrintError($"유효하지 않은 원본 경로입니다.\n원본 경로 : {SourceDirectory}");
				return false;
			}

			return true;
		}

		/// <summary>붙여넣을 디렉토리를 설정합니다.</summary>
		public bool SetupTargetDirectory(string targetDirectory)
		{
			TargetDirectory = targetDirectory;
			if (string.IsNullOrEmpty(TargetDirectory))
			{
				PatcherConsole.PrintError($"유효하지 않은 대상 경로입니다.\n대상 경로 : {TargetDirectory}");
				return false;
			}

			return true;
		}

		/// <summary>복사할 확장자 목록을 설정합니다.</summary>
		public bool SetupIncludeExtension(params string[] fileExtensions)
		{
			List<string> setupExtensionList = new List<string>();

			foreach (var extension in fileExtensions)
			{
				if (string.IsNullOrEmpty(extension) || extension == ".")
				{
					PatcherConsole.PrintError($"유효하지 않은 포함 확장자 입니다.\n포함 확장자 : {extension}");
					return false;
				}

				string curExtension = extension.ToLower();

				if (curExtension[0] != '.')
					curExtension = $".{curExtension}";

				setupExtensionList.Add(curExtension);
			}

			_includeExtensionList = setupExtensionList;
			return true;
		}

		/// <summary>예외 폴더 목록을 설정합니다. 해당 이름이 포함된 폴더의 하위 목록은 패치하지 않습니다.</summary>
		public bool SetupExcludeFolderName(params string[] folderNames)
		{
			List<string> setupFolderList = new List<string>();

			foreach (var folder in folderNames)
			{
				if (string.IsNullOrEmpty(folder))
				{
					PatcherConsole.PrintError($"유효하지 않은 예외 폴더 이름 입니다.\n폴더 이름 : {folder}");
					return false;
				}

				string curFolder = folder.ToLower();
				setupFolderList.Add(curFolder);
			}

			_exceptFolderName = setupFolderList;
			return true;
		}

		/// <summary>가드 확장자를 설정합니다. 가드 확장자를 가진 파일이 목적 경로에 없다면 패치가 일어나지 않습니다.</summary>
		/// <param name="guardExtension">가드 확장자</param>
		public bool SetupGuardExtension(params string[] guardExtension)
		{
			List<string> guardExtensionList = new List<string>();

			foreach (var g in guardExtension)
			{
				if (string.IsNullOrEmpty(g) || g == ".")
				{
					PatcherConsole.PrintError($"유효하지 않은 가드 확장자 입니다.\n가드 확장자 : {g}");
					return false;
				}

				string curExtension = g.ToLower();

				if (curExtension[0] != '.')
					curExtension = $".{curExtension}";

				guardExtensionList.Add(curExtension);
			}

			_guardExtensions = guardExtensionList;
			return true;
		}

		public bool Excute()
		{
			if (!HasGuardExtension)
			{
				PatcherConsole.PrintError("가드 확장자가 없습니다.");
				return false;
			}

			if (!existGuardFile(TargetDirectory))
			{
				PatcherConsole.PrintError("가드 확장자를 가진 파일이 없습니다.");
				return false;
			}

			if (tryGetSourceFilePaths(out var sourceFilePaths))
			{
				clearTargerDirectory();
				copyToTargetDirectory(sourceFilePaths);
				return true;
			}

			PatcherConsole.PrintError("실행 오류");
			return false;
		}

		private void copyToTargetDirectory(IList<string> sourceFilePaths)
		{
			PatcherConsole.PrintSeparator();

			foreach (var srcFilePath in sourceFilePaths)
			{
				string relativeFilePath = Path.GetRelativePath(SourceDirectory, srcFilePath);
				string copyFilePath = Path.Combine(TargetDirectory, relativeFilePath);
				string targetDirPath = Path.GetDirectoryName(copyFilePath) ?? "";

				if (!Directory.Exists(targetDirPath))
					Directory.CreateDirectory(targetDirPath);

				var readResult = FileHandler.TryReadText(srcFilePath);
				
				string copyFileName = Path.GetFileName(srcFilePath);
				var copyFileContent = string.Format(FilePatcherFormat.CopyMetadata, copyFileName, readResult.Value);
				FileHandler.TryWriteText(copyFilePath, copyFileContent);
				PatcherConsole.PrintSaveSuccessResult("File copy completed : ",
													  copyFileName, targetDirPath);
			}
		}

		private bool existGuardFile(string path)
		{
			foreach (var g in _guardExtensions)
			{
				var guard = Directory
					.GetFiles(path)
					.FirstOrDefault(p => Path.GetExtension(p) == g);

				if (guard == null)
				{
					return false;
				}
			}

			return true;
		}

		private void clearTargerDirectory()
		{
			// Delete Files
			var targetFiles = Directory.GetFiles(TargetDirectory);

			foreach (var filePath in targetFiles)
			{
				string extension = Path.GetExtension(filePath).ToLower();
				if (_includeExtensionList.Contains(extension))
				{
					File.Delete(filePath);
				}
			}

			// Delete Directories
			var targetDirectories = Directory.GetDirectories(TargetDirectory);

			foreach (var dirPath in targetDirectories)
			{
				Directory.Delete(dirPath, true);
			}
		}

		/// <summary>복사할 확장자를 가진 파일들의 경로 리스트를 반환받습니다.</summary>
		private bool tryGetSourceFilePaths(out List<string> sourceFilePaths)
		{
			sourceFilePaths = new List<string>();

			if (!HasValidSourcePaths || !HasValidTargetPaths)
			{
				PatcherConsole.PrintError("유효하지 않은 파일 경로입니다.");
				return false;
			}

			if (!HasValidExtensionRules)
			{
				PatcherConsole.PrintError("패치할 확장자 목록이 유효하지 않습니다.");
				return false;
			}

			Queue<string> sourceDirectories = new Queue<string>();
			sourceDirectories.Enqueue(SourceDirectory);

			while (sourceDirectories.Count > 0)
			{
				string curDirectory = sourceDirectories.Dequeue();

				// Add source files
				var curFiles = Directory.GetFiles(curDirectory);

				foreach (var filePath in curFiles)
				{
					string curExtension = Path.GetExtension(filePath).ToLower();

					if (_includeExtensionList.Contains(curExtension))
					{
						sourceFilePaths.Add(filePath);
					}
				}

				// Traverse directory
				var curDirectories = Directory.GetDirectories(curDirectory);

				foreach (var newDir in curDirectories)
				{
					var curFolderName = Path.GetFileName(newDir).ToLower();

					if (_exceptFolderName.Contains(curFolderName))
						continue;

					sourceDirectories.Enqueue(newDir);
				}
			}

			return true;
		}
	}
}