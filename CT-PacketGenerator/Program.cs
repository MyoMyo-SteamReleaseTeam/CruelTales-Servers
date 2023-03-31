using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CT.Network.Serialization.Type;
using CT.PacketGenerator.Exceptions;
using CT.Tool.CodeGen;
using CT.Tool.ConsoleHelper;
using CT.Tool.Data;
using CT.Tool.GetOpt;

namespace CT.PacketGenerator
{
	public class JobOption
	{
		public string XmlSourcePath = string.Empty;
		public string OutputServerPath = string.Empty;
		public string OutputClientPath = string.Empty;

		public string GetFileNameWithExtension()
		{
			return Path.GetFileNameWithoutExtension(XmlSourcePath) + ".cs";
		}

		public string GetServerTargetPath()
		{
			return Path.Combine(OutputServerPath, GetFileNameWithExtension());
		}

		public string GetClientTargetPath()
		{
			return Path.Combine(OutputClientPath, GetFileNameWithExtension());
		}
	}

	internal class Program
	{
		private static string _baseNamespace = $@"baseNamespace";
		private static string _packetTypeName = $@"packetTypeName";
		private static string _xmlPath = @$"xmlpath";
		private static string _outputServer = @$"outputServer";
		private static string _outputClient = @$"outputClient";

		static void Main(string[] args)
		{
			string baseNamespace = string.Empty;
			string packetTypeName = string.Empty;
			string xmlPath = string.Empty;
			string outputServer = string.Empty;
			string outputClient = string.Empty;

			args = new string[]
			{
				$"--{_xmlPath} \"../../../../CT-NetworkCore/PacketDefinition/\"",
				$"--{_outputServer} \"../../../../CT-NetworkCore/Packets/\"",
				$"--{_outputClient} \"../../../../../CruelTales-Client/CruelTales-Client/Assets/Scripts/Network/Packets/\"",
				$"-{_packetTypeName} PacketType",
				$"-{_baseNamespace} CT.Packets",
			};

			OptionParser optionParser = new OptionParser();
			optionParser.RegisterEvent(_xmlPath, 2, (options) =>
			{
				try
				{
					xmlPath = options[0];
				}
				catch
				{
					throw new NoProcessArgumentsException(_xmlPath);
				}
			});
			optionParser.RegisterEvent(_outputServer, 2, (options) =>
			{
				try
				{
					outputServer = options[0];
				}
				catch
				{
					throw new NoProcessArgumentsException(_outputServer);
				}
			});
			optionParser.RegisterEvent(_outputClient, 2, (options) =>
			{
				try
				{
					outputClient = options[0];
				}
				catch
				{
					throw new NoProcessArgumentsException(_outputClient);
				}
			});
			optionParser.RegisterEvent(_packetTypeName, 1, (options) =>
			{
				try
				{
					packetTypeName = options[0];
				}
				catch
				{
					throw new NoProcessArgumentsException(_packetTypeName);
				}
			});
			optionParser.RegisterEvent(_baseNamespace, 1, (options) =>
			{
				try
				{
					baseNamespace = options[0];
				}
				catch
				{
					throw new NoProcessArgumentsException(_baseNamespace);
				}
			});
			optionParser.OnArguments(args);

			if (string.IsNullOrEmpty(xmlPath))
			{
				PrintError($"There is no XML path.");
				return;
			}

			if (string.IsNullOrEmpty(outputServer))
			{
				PrintError($"There is no server output path.");
				return;
			}

			if (string.IsNullOrEmpty(outputClient))
			{
				PrintError($"There is no client output path.");
				return;
			}

			if (string.IsNullOrEmpty(packetTypeName))
			{
				PrintError($"There is no packet type name.");
				return;
			}

			if (string.IsNullOrEmpty(baseNamespace))
			{
				PrintError($"There is no base namespace.");
				return;
			}

			Console.WriteLine($"Start generate packets...");
			Console.WriteLine($"");
			Console.Write($"XML path : ");
			ConsoleHelper.WriteLine(xmlPath, ConsoleColor.White, ConsoleColor.DarkBlue);
			Console.Write($"Output path : ");
			ConsoleHelper.WriteLine(outputServer, ConsoleColor.White, ConsoleColor.DarkBlue);
			Console.WriteLine($"Packet Type Name : {packetTypeName}");
			Console.WriteLine($"Base Namespace: {baseNamespace}");
			PrintSeparator();

			GeneratePacket(xmlPath, outputServer, outputClient, 
						   packetTypeName, baseNamespace);
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
						PrintSaveResult(job.GetFileNameWithExtension(), serverPath);
					}
					else
					{
						throw serverResult.Exception;
					}

					var clientPath = job.GetClientTargetPath();
					var clientResult = FileHandler.TryWriteText(clientPath, generatedCode, true);
					if (clientResult.ResultType == JobResultType.Success)
					{
						PrintSaveResult(job.GetFileNameWithExtension(), clientPath);
					}
					else
					{
						throw clientResult.Exception;
					}
				}
				catch (Exception e)
				{
					PrintError(job.GetFileNameWithExtension(), e);
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
					PrintSaveResult(enumFileName, enumServer);
					var enumClient = Path.Combine(outputClient, enumFileName);
					FileHandler.TryWriteText(enumClient, enumCode, true);
					PrintSaveResult(enumClient, enumServer);
				}
				catch (Exception e )
				{
					PrintError(enumFileName, e);
				}
			}
		}

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