using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CT.CorePatcher.FilePatch;
using CT.CorePatcher.Packets;
using CT.CorePatcher.SynchronizationsCodeGen;
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
		private const string PROGRAM_NAME = "programName";

		private const string programSync = "programSync";
		private const string programXml = "programXml";
		private const string programFilePatch = "programFilePatch";

		public static void Main(string[] args)
		{
			StringArgumentArray programs = new();

			OptionParser op = new OptionParser();
			OptionParser.BindArgumentArray(op, PROGRAM_NAME, 2, programs);
			if (!op.TryApplyArguments(args))
			{
				Console.Read();
				return;
			}

			if (programs.ArgumentArray.Contains(programSync))
				RunSynchronizerGenerator(args);

			if (programs.ArgumentArray.Contains(programXml))
				RunFilePatch(args);

			if (programs.ArgumentArray.Contains(programFilePatch))
				RunXmlPacketSystemPatch(args);

			Console.Read();
		}

		public static void RunSynchronizerGenerator(string[] args)
		{
			string programName = nameof(RunSynchronizerGenerator);
			PatcherConsole.PrintProgramInfo(programName);

			try
			{
				SynchronizerGenerator syncCodeGen = new();
				Console.WriteLine(syncCodeGen.ParseCode());
			}
			catch (Exception e)
			{
				PatcherConsole.PrintError(e.Message);
				PatcherConsole.PrintProgramCompleted(programName, true);
			}
			finally
			{
				PatcherConsole.PrintProgramCompleted(programName);
			}
		}

		public static void RunFilePatch(string[] args)
		{
			string programName = nameof(RunFilePatch);
			PatcherConsole.PrintProgramInfo(programName);

			try
			{
				// Run packet generator
				if (PacketGenerator.Run(args) == false)
				{
					PatcherConsole.PrintError($"{nameof(PacketGenerator)} error!");
					return;
				}
			}
			catch (Exception e)
			{
				PatcherConsole.PrintError(e.Message);
				PatcherConsole.PrintProgramCompleted(programName, true);
			}
			finally
			{
				PatcherConsole.PrintProgramCompleted(programName);
			}
		}

		public static void RunXmlPacketSystemPatch(string[] args)
		{
			string programName = nameof(RunXmlPacketSystemPatch);
			PatcherConsole.PrintProgramInfo(programName);

			try
			{
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
			catch (Exception e)
			{
				PatcherConsole.PrintError(e.Message);
				PatcherConsole.PrintProgramCompleted(programName, true);
			}
			finally
			{
				PatcherConsole.PrintProgramCompleted(programName);
			}
		}
	}
}