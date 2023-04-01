using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CT.CorePatcher.Exceptions;
using CT.Network.Serialization.Type;
using CT.Tool.CodeGen;
using CT.Tool.ConsoleHelper;
using CT.Tool.Data;
using CT.Tool.GetOpt;

namespace CT.CorePatcher.Packets
{
	public class StringArgument
	{
		public string Argument = string.Empty;

		public bool HasArgument(ref bool checker)
		{
			bool result = !string.IsNullOrEmpty(Argument);
			checker &= result;
			return result;
		}
	}

	internal static class PacketGenerator
	{
		public const string BASE_NAMESPACE = $@"baseNamespace";
		public const string PACKET_TYPE_NAME = $@"packetTypeName";
		public const string XML_PATH = @$"xmlpath";
		public const string OUTPUT_SERVER = @$"outputServer";
		public const string OUTPUT_CLIENT = @$"outputClient";

		public static void Run(string[] args)
		{
			// Print program info
			PatcherConsole.PrintSeparator();
			ConsoleHelper.SetColor(ConsoleColor.White, ConsoleColor.DarkGreen);
			Console.WriteLine($"Packet Generator");
			Console.ResetColor();
			PatcherConsole.PrintSeparator();

			// Declare arguments
			StringArgument baseNamespace = new();
			StringArgument packetTypeName = new();
			StringArgument xmlPath = new();
			StringArgument outputServer = new();
			StringArgument outputClient = new();

			// Parse options
			OptionParser optionParser = new OptionParser();
			bindArgument(optionParser, XML_PATH, 2, xmlPath);
			bindArgument(optionParser, OUTPUT_SERVER, 2, outputServer);
			bindArgument(optionParser, OUTPUT_CLIENT, 2, outputClient);
			bindArgument(optionParser, PACKET_TYPE_NAME, 1, packetTypeName);
			bindArgument(optionParser, BASE_NAMESPACE, 1, baseNamespace);
			optionParser.OnArguments(args);

			// Check arguments validation
			bool isValidArguments = true;
			if (xmlPath.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no XML path.");

			if (outputServer.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no server output path.");

			if (outputClient.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no client output path.");

			if (packetTypeName.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no packet type name.");

			if (baseNamespace.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no base namespace.");

			if (isValidArguments == false)
				return;

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
			GeneratePacket(xmlPath.Argument,
						   outputServer.Argument, 
						   outputClient.Argument,
						   packetTypeName.Argument, 
						   baseNamespace.Argument);
		}

		private static void bindArgument(OptionParser parser, string argumentName, int level, StringArgument value)
		{
			parser.RegisterEvent(argumentName, level, (options) =>
			{
				try
				{
					value.Argument = options[0];
				}
				catch
				{
					throw new NoProcessArgumentsException(argumentName);
				}
			});
		}

		public static void GeneratePacket(string xmlPath,
										  string outputServer,
										  string outputClient,
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
						op.OutputClientPath = outputClient;
						jobOptionList.Add(op);
					}
				}
			}

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

				List<string> packetEnumNames = new List<string>();

				// Save generated packet schema code
				try
				{
					parser.ParseFromXml(job.XmlSourcePath, out var generatedCode, out var pakcetNames);
					packetEnumNames.AddRange(pakcetNames);
					var serverPath = job.GetServerTargetPath();
					var serverResult = FileHandler.TryWriteText(serverPath, generatedCode, true);
					if (serverResult.ResultType == JobResultType.Success)
					{
						PatcherConsole.PrintSaveResult(job.GetFileNameWithExtension(), serverPath);
					}
					else
					{
						throw serverResult.Exception;
					}

					var clientPath = job.GetClientTargetPath();
					var clientResult = FileHandler.TryWriteText(clientPath, generatedCode, true);
					if (clientResult.ResultType == JobResultType.Success)
					{
						PatcherConsole.PrintSaveResult(job.GetFileNameWithExtension(), clientPath);
					}
					else
					{
						throw clientResult.Exception;
					}
				}
				catch (Exception e)
				{
					PatcherConsole.PrintError(job.GetFileNameWithExtension(), e);
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
					FileHandler.TryWriteText(enumServer, enumCode, true);
					PatcherConsole.PrintSaveResult(enumFileName, enumServer);
					var enumClient = Path.Combine(outputClient, enumFileName);
					FileHandler.TryWriteText(enumClient, enumCode, true);
					PatcherConsole.PrintSaveResult(enumClient, enumServer);
				}
				catch (Exception e)
				{
					PatcherConsole.PrintError(enumFileName, e);
				}
			}
		}
	}
}