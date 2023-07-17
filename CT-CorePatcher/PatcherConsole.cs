using System;
using CT.Common.Tools.ConsoleHelper;

namespace CT.CorePatcher
{
	public static class PatcherConsole
	{
		public static void PrintProgramInfo(string programName)
		{
			PrintSeparator('#');
			Console.WriteLine();
			ConsoleHelper.SetColor(ConsoleColor.White, ConsoleColor.DarkBlue);
			Console.Write(DateTime.Now + " : ");
			Console.WriteLine(programName);
			Console.ResetColor();
			Console.WriteLine();
			PrintSeparator('#');
		}

		public static void PrintProgramCompleted(string programName, bool hasError = false)
		{
			PrintSeparator('#');
			Console.WriteLine();
			ConsoleColor backColor = hasError ? ConsoleColor.Red : ConsoleColor.DarkBlue;
			ConsoleHelper.SetColor(ConsoleColor.White, backColor);
			Console.WriteLine($"{programName} ended!");
			if (hasError)
			{
				Console.WriteLine($"Error occur!");
			}
			Console.ResetColor();
			Console.WriteLine();
			PrintSeparator('#');
		}

		public static void PrintJobInfo(string jobName)
		{
			PrintSeparator();
			ConsoleHelper.SetColor(ConsoleColor.White, ConsoleColor.DarkGreen);
			Console.WriteLine(jobName);
			Console.ResetColor();
			PrintSeparator('-');
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
			Console.WriteLine($"{e.StackTrace}");
			PrintSeparator();
		}

		public static void PrintError(string errorMessage)
		{
			ConsoleHelper.SetColor(ConsoleColor.White, ConsoleColor.Red);
			Console.WriteLine(errorMessage);
			Console.ResetColor();
		}

		public static void PrintWarm(string warmMessage)
		{
			ConsoleHelper.SetColor(ConsoleColor.Black, ConsoleColor.DarkYellow);
			Console.WriteLine(warmMessage);
			Console.ResetColor();
		}

		public static void PrintException(Exception e)
		{
			PrintSeparator();
			ConsoleHelper.SetColor(ConsoleColor.White, ConsoleColor.Red);
			Console.WriteLine($"# {e.GetType().Name} #");
			Console.WriteLine($"");
			Console.WriteLine($"{e.Message}");
			Console.WriteLine($"{e.StackTrace}");
			Console.ResetColor();
			PrintSeparator();
		}

		public static void PrintSeparator(char separator = '=')
		{
			Console.WriteLine(new string(separator, 80));
		}
	}
}