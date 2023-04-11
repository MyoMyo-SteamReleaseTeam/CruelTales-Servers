using System;
using System.Collections.Generic;
using System.IO;
using CT.CorePatcher.FilePatch;
using CT.CorePatcher.Packets;
using CT.Tools.GetOpt;

namespace CT.CorePatcher
{
	public static class OptionParserExtension
	{
		public static bool TryApplyArguments(this OptionParser op, string[] args)
		{
			try
			{
				op.OnArguments(args);
				return true;
			}
			catch (Exception e)
			{
				PatcherConsole.PrintError(e.GetType().Name);
				Console.WriteLine();
				PatcherConsole.PrintError(e.Message);
				return false;
			}
		}
	}

	internal class MainProcess
	{
		public static void Main(string[] args)
		{
			try
			{
				// Run packet generator
				if (PacketGenerator.Run(args) == false)
				{
					PatcherConsole.PrintError($"{nameof(PacketGenerator)} error!");
					return;
				}

				// Check copy count
				StringArgument patchCountArg = new();
				OptionParser filePatchOp = new OptionParser();
				OptionParser.BindArgument(filePatchOp, "patchCount", 2, patchCountArg);
				if (!filePatchOp.TryApplyArguments(args))
				{
					PatcherConsole.PrintError($"Copy count argument parse error!");
					return;
				}

				if (!int.TryParse(patchCountArg.Argument, out int patchCount))
				{
					PatcherConsole.PrintError($"Copy count parse error! Argument : {patchCountArg.Argument}");
				}
				if (patchCount < 0)
				{
					PatcherConsole.PrintWarm($"Zero copy count!");
					return;
				}

				// File patch
				List<StringArgument> sourcePathList = new();
				List<StringArgument> targetPathList = new();

				OptionParser op = new OptionParser();
				for (int i = 0; i < patchCount; i++)
				{
					sourcePathList.Add(new StringArgument());
					targetPathList.Add(new StringArgument());

					OptionParser.BindArgument(op, $"source_{i}", 2, sourcePathList[i]);
					OptionParser.BindArgument(op, $"target_{i}", 2, targetPathList[i]);
				}
				if (!op.TryApplyArguments(args))
				{
					return;
				}

				Console.WriteLine($"Patch count : {patchCount}");

				for (int i = 0; i < patchCount; i++)
				{
					string source = sourcePathList[i].Argument;
					string target = targetPathList[i].Argument;

					Console.WriteLine($"Patch from : {source}");
					Console.WriteLine($"Patch to   : {target}");

					if (!FilePatcherRunner.Run(source, target))
					{
						PatcherConsole.PrintError($"{nameof(FilePatcherRunner)} error!");
						PatcherConsole.PrintError($"Project source : {source}");
						PatcherConsole.PrintError($"Project target : {target}");
						return;
					}
				}
			}
			catch
			{

			}
			finally
			{
				Console.WriteLine("Finished");
				Console.Read();
			}
		}
	}
}