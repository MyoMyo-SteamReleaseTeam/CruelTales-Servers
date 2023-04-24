using System;

namespace CT.Common.Tools.ConsoleHelper
{
	public static class ConsoleHelper
	{
		public static void SetColor(ConsoleColor foreground,
									ConsoleColor background)
		{
			Console.ForegroundColor = foreground;
			Console.BackgroundColor = background;
		}

		public static void WriteLine(string value,
									 ConsoleColor foreground,
									 ConsoleColor background)
		{
			Console.ForegroundColor = foreground;
			Console.BackgroundColor = background;
			Console.WriteLine(value);
			Console.ResetColor();
		}

		public static void WriteLine(string value, ConsoleColor foreground)
		{
			Console.ForegroundColor = foreground;
			Console.WriteLine(value);
			Console.ResetColor();
		}

		public static void Write(string value,
								 ConsoleColor foreground,
								 ConsoleColor background)
		{
			Console.ForegroundColor = foreground;
			Console.BackgroundColor = background;
			Console.Write(value);
			Console.ResetColor();
		}

		public static void Write(string value, ConsoleColor foreground)
		{
			Console.ForegroundColor = foreground;
			Console.Write(value);
			Console.ResetColor();
		}
	}
}
