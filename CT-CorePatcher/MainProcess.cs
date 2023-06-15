using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using CT.Common.Tools.GetOpt;
using CT.CorePatcher.FilePatch;
using CT.CorePatcher.Packets;
using CT.CorePatcher.SynchronizationsCodeGen;

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
				PatcherConsole.PrintException(e);
				return false;
			}
		}
	}

	[SupportedOSPlatform("windows")]
	internal class MainProcess
	{
		private const string PROGRAM_NAME = "programName";

		private const string PROGRAM_SYNC = "programSync";
		private const string PROGRAM_XML = "programXml";
		private const string PROGRAM_FILEPATCH = "programFilePatch";

		public static bool IsDebug { get; private set; } = false;

		public static void Main(string[] args)
		{
			try
			{
				Console.SetWindowSize(100, 70);
			}
			catch
			{

			}

			if (args.Length == 0)
			{
				IsDebug = true;
			}

			if (IsDebug)
			{
				//RunXmlPacketSystemPatch(args);
				RunSynchronizerGenerator(args);
				return;
			}

			StringArgumentArray programs = new();

			OptionParser op = new OptionParser();
			OptionParser.BindArgumentArray(op, PROGRAM_NAME, 2, programs);
			if (!op.TryApplyArguments(args))
			{
				Console.Read();
				return;
			}

			if (programs.ArgumentArray.Contains(PROGRAM_SYNC))
				RunSynchronizerGenerator(args);

			if (programs.ArgumentArray.Contains(PROGRAM_XML))
				RunXmlPacketSystemPatch(args);

			if (programs.ArgumentArray.Contains(PROGRAM_FILEPATCH))
				RunFilePatch(args);

			Console.Read();
		}

		public static void RunSynchronizerGenerator(string[] args)
		{
			string programName = nameof(RunSynchronizerGenerator);
			PatcherConsole.PrintProgramInfo(programName);

			try
			{
				SynchronizerGenerator syncCodeGen = new();
				syncCodeGen.GenerateCode(args);
			}
			catch (Exception e)
			{
				PatcherConsole.PrintException(e);
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
				// Run packet generator
				if (PacketGenerator.Run(args) == false)
				{
					PatcherConsole.PrintError($"{nameof(PacketGenerator)} error!");
					return;
				}
			}
			catch (Exception e)
			{
				PatcherConsole.PrintException(e);
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
				PatcherConsole.PrintException(e);
				PatcherConsole.PrintProgramCompleted(programName, true);
			}
			finally
			{
				PatcherConsole.PrintProgramCompleted(programName);
			}
		}
	}
}