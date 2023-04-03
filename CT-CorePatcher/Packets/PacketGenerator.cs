using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CT.Network.Serialization.Type;
using CT.Tools.CodeGen;
using CT.Tools.ConsoleHelper;
using CT.Tools.Data;
using CT.Tools.GetOpt;

namespace CT.CorePatcher.Packets
{
	internal static class PacketGenerator
	{
		public const string BASE_NAMESPACE = $@"baseNamespace";
		public const string PACKET_TYPE_NAME = $@"packetTypeName";
		public const string XML_PATH = @$"xmlpath";
		public const string OUTPUT_SERVER = @$"outputServer";

		public static bool Run(string[] args)
		{
			// Print program info
			PatcherConsole.PrintProgramInfo("Packet Generator");

			// Declare arguments
			StringArgument baseNamespace = new();
			StringArgument packetTypeName = new();
			StringArgument xmlPath = new();
			StringArgument outputServer = new();

			// Parse options
			OptionParser op = new OptionParser();
			op.BindArgument(op, XML_PATH, 2, xmlPath);
			op.BindArgument(op, OUTPUT_SERVER, 2, outputServer);
			op.BindArgument(op, PACKET_TYPE_NAME, 1, packetTypeName);
			op.BindArgument(op, BASE_NAMESPACE, 1, baseNamespace);
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
			if (xmlPath.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no XML path.");

			if (outputServer.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no server output path.");

			if (packetTypeName.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no packet type name.");

			if (baseNamespace.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no base namespace.");

			if (isValidArguments == false)
				return false;

			// Print current options

			Console.WriteLine($"Start generate packets...");
			Console.WriteLine($"");
			Console.Write($"XML path : ");
			ConsoleHelper.WriteLine(xmlPath.Argument, ConsoleColor.White, ConsoleColor.DarkBlue);
			Console.Write($"Output path : ");
			ConsoleHelper.WriteLine(outputServer.Argument, ConsoleColor.White, ConsoleColor.DarkBlue);
			Console.WriteLine($"Packet Type Name : {packetTypeName}");
			Console.WriteLine($"Base Namespace: {baseNamespace}");
			PatcherConsole.PrintSeparator();

			// Start generate packet codes
			try
			{
				generatePacket(xmlPath.Argument,
							   outputServer.Argument,
							   packetTypeName.Argument,
							   baseNamespace.Argument);

				return true;
			}
			catch (Exception e)
			{
				PatcherConsole.PrintException(e);
				return false;
			}
		}

		private static void generatePacket(string xmlPath,
										   string outputServer,
										   string packetTypeName,
										   string baseNamespace)
		{
			List<JobOption> jobOptionList = new List<JobOption>();

			Queue<string> retrieve = new Queue<string>();
			retrieve.Enqueue(xmlPath);

			while (retrieve.Count > 0)
			{
				string path = retrieve.Dequeue();
				foreach (var d in Directory.GetDirectories(path))
				{
					retrieve.Enqueue(d);
				}

				foreach (var f in Directory.GetFiles(path))
				{
					if (Path.GetExtension(f).ToLower() == ".xml")
					{
						JobOption op = new JobOption();
						op.XmlSourcePath = f;
						op.OutputServerPath = outputServer;
						jobOptionList.Add(op);
					}
				}
			}

			List<CodeGenOperation> operation = new List<CodeGenOperation>();
			List<string> packetEnumNames = new List<string>();

			foreach (var job in jobOptionList)
			{
				// Read packet files
				PacketParser parser = new PacketParser();

				parser.NewLine = TextFormat.LF;
				parser.Indent = TextFormat.Indent;
				var netcore = Assembly.GetAssembly(typeof(NetString));
				if (netcore != null)
				{
					parser.ReferenceAssemblys.Add(netcore);
				}
				parser.Initialize();

				// Save generated packet schema code
				try
				{
					parser.ParseFromXml(job.XmlSourcePath, out var generatedCode, out var pakcetNames);
					packetEnumNames.AddRange(pakcetNames);
					var targetPath = job.GetTargetPath();
					operation.Add(new CodeGenOperation()
					{ 
						GeneratedCode = generatedCode,
						TargetPath = targetPath 
					});
				}
				catch (Exception e)
				{
					PatcherConsole.PrintError(job.GetFileNameWithExtension(), e);
				}

			}

			// Save packet enum code
			string enumFileName = packetTypeName + ".cs";
			try
			{
				var enumCode = CodeGenerator_Enumerate
					.Generate(packetTypeName,
								baseNamespace,
								true, true,
								new List<string>(),
								packetEnumNames);

				var enumServer = Path.Combine(outputServer, enumFileName);
				operation.Add(new CodeGenOperation()
				{
					GeneratedCode = enumCode,
					TargetPath = enumServer
				});
			}
			catch (Exception e)
			{
				PatcherConsole.PrintError(enumFileName, e);
			}

			foreach (var op in operation)
			{
				var saveResult = FileHandler.TryWriteText(op.TargetPath, op.GeneratedCode, true);
				string fileName = string.Empty;

				if (saveResult.ResultType == JobResultType.Success)
				{
					fileName = Path.GetFileNameWithoutExtension(op.TargetPath) + ".cs";
					PatcherConsole.PrintSaveSuccessResult("Generate code completed : ",
															fileName, op.TargetPath);
				}
				else
				{
					PatcherConsole.PrintError(fileName, saveResult.Exception);
					return;
				}
			}
		}
	}
}