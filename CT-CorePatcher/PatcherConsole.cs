using System;
using CT.Tool.ConsoleHelper;

namespace CT.CorePatcher
{
	public static class PatcherConsole
	{
		public static void PrintSaveResult(string fileName, string targetPath)
		{
			Console.Write("[");
			ConsoleHelper.Write("Success", ConsoleColor.Green);
			Console.Write($"] Generate completed : ");
			ConsoleHelper.WriteLine(fileName, ConsoleColor.White, ConsoleColor.DarkBlue);
			Console.Write($"Greate file at : ");
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

		public static void PrintSeparator()
		{
			Console.WriteLine(new string('=', 80));
		}
	}
}