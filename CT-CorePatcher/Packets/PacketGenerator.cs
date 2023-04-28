using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CT.Common.Serialization;
using CT.Common.Serialization.Type;
using CT.Common.Tools.CodeGen;
using CT.Common.Tools.ConsoleHelper;
using CT.Common.Tools.Data;
using CT.Common.Tools.GetOpt;

namespace CT.CorePatcher.Packets
{
	internal static class PacketGenerator
	{
		public const string BASE_NAMESPACE = $@"baseNamespace";
		public const string PACKET_TYPE_NAME = $@"packetTypeName";
		public const string XML_PATH = @$"xmlPath";
		public const string OUTPUT_SERVER = @$"outputServer";

		public const string PACKET_TYPE_PATH = @"packetTypePath";
		public const string FACTORY_SERVER_PATH = @"factoryServerPath";
		public const string FACTORY_CLIENT_PATH = @"factoryClientPath";
		public const string SERVER_DISPATCHER = @"sdispatcher";
		public const string CLIENT_DISPATCHER = @"cdispatcher";

		public static bool Run(string[] args)
		{
#pragma warning disable CA1416
			if (MainProcess.IsDebug)
			{
				return PacketGenerator.DebugRun();
			}
#pragma warning restore CA1416

			// Print program info
			PatcherConsole.PrintJobInfo("Packet Generator");

			// Declare arguments
			StringArgument baseNamespace = new();
			StringArgument packetTypeName = new();
			StringArgument xmlPath = new();
			StringArgument outputServer = new();

			StringArgument packetTypePath = new();
			StringArgument factoryServerPath = new();
			StringArgument factoryClientPath = new();
			StringArgumentArray serverDispatcherPaths = new();
			StringArgumentArray clientDispatcherPaths = new();

			// Parse options
			OptionParser op = new OptionParser();
			OptionParser.BindArgument(op, XML_PATH, 2, xmlPath);
			OptionParser.BindArgument(op, OUTPUT_SERVER, 2, outputServer);
			OptionParser.BindArgument(op, PACKET_TYPE_NAME, 1, packetTypeName);
			OptionParser.BindArgument(op, BASE_NAMESPACE, 1, baseNamespace);

			OptionParser.BindArgument(op, PACKET_TYPE_PATH, 2, packetTypePath);
			OptionParser.BindArgument(op, FACTORY_SERVER_PATH, 2, factoryServerPath);
			OptionParser.BindArgument(op, FACTORY_CLIENT_PATH, 2, factoryClientPath);
			OptionParser.BindArgumentArray(op, SERVER_DISPATCHER, 2, serverDispatcherPaths);
			OptionParser.BindArgumentArray(op, CLIENT_DISPATCHER, 2, clientDispatcherPaths);

			if (!op.TryApplyArguments(args))
			{
				return false;
			}

			// Check arguments validation
			bool isValidArguments = true;
			if (xmlPath.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no {nameof(xmlPath)}.");
			if (outputServer.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no {nameof(outputServer)} path.");
			if (packetTypeName.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no {nameof(packetTypeName)}.");
			if (baseNamespace.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no {nameof(baseNamespace)}.");

			if (factoryServerPath.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no {nameof(factoryServerPath)}.");
			if (serverDispatcherPaths.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no {nameof(serverDispatcherPaths)}.");
			if (clientDispatcherPaths.HasArgument(ref isValidArguments) == false)
				PatcherConsole.PrintError($"There is no {nameof(clientDispatcherPaths)}.");

			if (isValidArguments == false)
				return false;

			// Print current options

			Console.WriteLine($"Start generate packets...");
			Console.WriteLine($"");
			Console.Write($"XML path : ");
			ConsoleHelper.WriteLine(xmlPath.Argument, ConsoleColor.White, ConsoleColor.DarkBlue);
			Console.Write($"Output path : ");
			ConsoleHelper.WriteLine(outputServer.Argument, ConsoleColor.White, ConsoleColor.DarkBlue);
			Console.WriteLine($"Packet Type Name : {packetTypeName.Argument}");
			Console.WriteLine($"Base Namespace: {baseNamespace.Argument}");
			PatcherConsole.PrintSeparator();

			// Start generate packet codes
			List<PacketInfo> packetInfos;
			Console.WriteLine($"# Generate data types...");
			try
			{
				generatePacket(xmlPath.Argument,
							   packetTypePath.Argument,
							   outputServer.Argument,
							   packetTypeName.Argument,
							   baseNamespace.Argument,
							   out packetInfos);
			}
			catch (Exception e)
			{
				PatcherConsole.PrintException(e);
				return false;
			}

			Console.WriteLine($"# Generate packet helpers...");
			// Start generate 
			try
			{
				generatePacketHelpers(packetInfos,
									  serverDispatcherPaths.ArgumentArray,
									  clientDispatcherPaths.ArgumentArray,
									  factoryServerPath.Argument,
									  factoryClientPath.Argument);
			}
			catch (Exception e)
			{
				PatcherConsole.PrintException(e);
				return false;
			}

			return true;
		}

		public static bool DebugRun()
		{
			string xmlPath = "../../../Packets/Test/";
			string packetTypePath = "XMLTest/";
			string outputServer = "XMLTest/Server";
			string packetTypeName = "PacketType";
			string baseNamespace = "CT.Packets";

			string[] serverDispatcherPaths = new string[]
			{
				"XMLTest/Server",
			};
			string[] clientDispatcherPaths = new string[]
			{
				"XMLTest/Client",
			};

			string factoryServerPath = "XMLTest/Server";
			string factoryClientPath = "XMLTest/Client";


			// Start generate packet codes
			List<PacketInfo> packetInfos;
			Console.WriteLine($"# Generate data types...");
			try
			{
				generatePacket(xmlPath,
							   packetTypePath,
							   outputServer,
							   packetTypeName,
							   baseNamespace,
							   out packetInfos);
			}
			catch (Exception e)
			{
				PatcherConsole.PrintException(e);
				return false;
			}

			Console.WriteLine($"# Generate packet helpers...");
			// Start generate 
			try
			{
				generatePacketHelpers(packetInfos,
									  serverDispatcherPaths,
									  clientDispatcherPaths,
									  factoryServerPath,
									  factoryClientPath);
			}
			catch (Exception e)
			{
				PatcherConsole.PrintException(e);
				return false;
			}

			return true;
		}

		private static void generatePacket(string xmlPath,
										   string packetTypePath,
										   string outputServer,
										   string packetTypeName,
										   string baseNamespace,
										   out List<PacketInfo> packetInfos)
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
			packetInfos = new List<PacketInfo>();

			// Create packet codes
			foreach (var job in jobOptionList)
			{
				// Read packet files
				PacketParser parser = new PacketParser();

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
					packetInfos.AddRange(pakcetNames);
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

			// Create packet enum codes
			string enumFileName = packetTypeName + ".cs";
			try
			{
				var enumCode = CodeGenerator_Enumerate
					.Generate(packetTypeName,
							  baseNamespace,
							  hasNone: true,
							  useTab: true,
							  usingList: new List<string>(),
							  packetInfos.Select(p => p.PacketName).ToList(),
							  addUsingAndSemicolon: true,
							  PacketTypeExtension.SIZE_TYPE);

				enumCode = string.Format(CodeFormat.GeneratorMetadata, enumFileName, enumCode);
				var packetTypeTarget = Path.Combine(packetTypePath, enumFileName);
				operation.Add(new CodeGenOperation()
				{
					GeneratedCode = enumCode,
					TargetPath = packetTypeTarget
				});
			}
			catch (Exception e)
			{
				PatcherConsole.PrintError(enumFileName, e);
				return;
			}

			// Generate codes to files
			FileHandler.TryDeleteFilesIn(outputServer, out _);

			foreach (var op in operation)
			{
				string fileName = Path.GetFileName(op.TargetPath);
				var saveResult = FileHandler.TryWriteText(op.TargetPath, op.GeneratedCode, true);

				if (saveResult.ResultType == JobResultType.Success)
				{
					//fileName = Path.GetFileNameWithoutExtension(op.TargetPath) + ".cs";
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

		struct DispatcherJob
		{
			public string TargetPath;
			public string FileName;
			public bool IsClient;
		}

		private static void generatePacketHelpers(List<PacketInfo> packetInfos,
												  string[] serverDispatcherPaths,
												  string[] clientDispatcherPaths,
												  string factoryServerPath,
												  string factoryClientPath)
		{
			PacketParser parser = new PacketParser();

			// Dispatchers
			string dispatcherFileName = PacketFormat.PacketDispatcherFileName + ".cs";

			List<DispatcherJob> jobList = new List<DispatcherJob>();

			foreach (var path in serverDispatcherPaths)
			{
				jobList.Add(new DispatcherJob()
				{
					TargetPath = path, 
					FileName = PacketFormat.PacketDispatcherFileName +
							   PacketFormat.ServerSidePacketPrefix + ".cs",
					IsClient = false
				});
			}

			foreach (var path in clientDispatcherPaths)
			{
				jobList.Add(new DispatcherJob()
				{
					TargetPath = path,
					FileName = PacketFormat.PacketDispatcherFileName +
							   PacketFormat.ClientSidePacketPrefix + ".cs",
					IsClient = true
				});
			}

			foreach (var job in jobList)
			{
				try
				{
					parser.GenerateDispatcherCode(packetInfos, out var dispatcherCode, job.IsClient, dispatcherFileName);
					var targetPath = Path.Combine(job.TargetPath, job.FileName);
					var saveResult = FileHandler.TryWriteText(targetPath, dispatcherCode, makeDirectory: true);
					if (saveResult.ResultType == JobResultType.Success)
					{
						PatcherConsole.PrintSaveSuccessResult("Generate code completed : ",
															  job.FileName, targetPath);
					}
					else
					{
						PatcherConsole.PrintError(job.FileName, saveResult.Exception);
						return;
					}
				}
				catch (Exception e)
				{
					PatcherConsole.PrintError(job.FileName, e);
				}
			}

			// Factory file
			string factoryFileName = PacketFormat.PacketFactoryFileName + ".cs";
			try
			{
				// Server side
				parser.GenerateFactoryCode(packetInfos, out var factoryServerCode, factoryFileName, isServer: true);
				factoryServerPath = Path.Combine(factoryServerPath, factoryFileName);
				var saveResultServer = FileHandler.TryWriteText(factoryServerPath, factoryServerCode, makeDirectory: true);
				if (saveResultServer.ResultType == JobResultType.Success)
				{
					PatcherConsole.PrintSaveSuccessResult("Generate code completed : ",
														  factoryFileName, factoryServerPath);
				}
				else
				{
					PatcherConsole.PrintError(factoryFileName, saveResultServer.Exception);
					return;
				}

				// Client side
				parser.GenerateFactoryCode(packetInfos, out var factoryClientCode, factoryFileName, isServer: false);
				factoryClientPath = Path.Combine(factoryClientPath, factoryFileName);
				var saveResultClient = FileHandler.TryWriteText(factoryClientPath, factoryClientCode, makeDirectory: true);
				if (saveResultClient.ResultType == JobResultType.Success)
				{
					PatcherConsole.PrintSaveSuccessResult("Generate code completed : ",
														  factoryFileName, factoryClientPath);
				}
				else
				{
					PatcherConsole.PrintError(factoryFileName, saveResultClient.Exception);
					return;
				}
			}
			catch (Exception e)
			{
				PatcherConsole.PrintError(factoryFileName, e);
			}
		}
	}
}