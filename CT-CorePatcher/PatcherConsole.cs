using System;
using CT.Tools.ConsoleHelper;

namespace CT.CorePatcher
{
	public static class PatcherConsole
	{
		public static void PrintProgramInfo(string programName)
		{
			PrintSeparator();
			ConsoleHelper.SetColor(ConsoleColor.White, ConsoleColor.DarkGreen);
			Console.WriteLine(programName);
			Console.ResetColor();
			PrintSeparator();
		}

		public static void PrintSaveSuccessResult(string message, string fileName, string targetPath)
		{
			Console.Write("[");
			ConsoleHelper.Write("Success", ConsoleColor.Green);
			Console.Write($"] {message} : ");
			ConsoleHelper.Write(fileName, ConsoleColor.White, ConsoleColor.DarkBlue);
			Console.Write($"\nPath : ");
			ConsoleHelper.WriteLine(targetPath, ConsoleColor.Green);
			PrintSeparator();
		}

		public static void PrintError(string fileName, Exception e)
		{
			PrintError($"Generate failed!! : {fileName}");
			PrintSeparator();
			Console.WriteLine($"File save failed!");
			Console.WriteLine($"");
			Console.WriteLine($"# {e.GetType().Name} #");
			Console.WriteLine($"");
			Console.WriteLine($"{e.Message}");
			PrintSeparator();
		}

		public static void PrintError(string errorMessage)
		{
			ConsoleHelper.SetColor(ConsoleColor.White, ConsoleColor.Red);
			Console.WriteLine(errorMessage);
			Console.ResetColor();
		}

		public static void PrintException(Exception e)
		{
			PrintSeparator();
			Console.WriteLine($"# {e.GetType().Name} #");
			Console.WriteLine($"");
			Console.WriteLine($"{e.Message}");
			PrintSeparator();
		}

		public static void PrintSeparator()
		{
			Console.WriteLine(new string('=', 80));
		}
	}
}