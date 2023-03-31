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
		public string OutputPath = string.Empty;

		public string GetFileNameWithExtension()
		{
			return Path.GetFileNameWithoutExtension(XmlSourcePath) + ".cs";
		}

		public string GetTargetPath()
		{
			return Path.Combine(OutputPath, GetFileNameWithExtension());
		}
	}

	internal class Program
	{
		private static string _baseNamespace = $@"baseNamespace";
		private static string _packetTypeName = $@"packetTypeName";
		private static string _xmlPathArgument = @$"xmlpath";
		private static string _outputArgument = @$"outputPath";

		static void Main(string[] args)
		{
			string baseNamespace = string.Empty;
			string packetTypeName = string.Empty;
			string xmlPath = string.Empty;
			string outputPath = string.Empty;

			args = new string[]
			{
				$"--{_xmlPathArgument} \"../../../../CT-NetworkCore/PacketDefinition/\"",
				$"--{_outputArgument} \"../../../../CT-NetworkCore/Packets/\"",
				$"-{_packetTypeName} PacketType",
				$"-{_baseNamespace} CT.Packets",
			};

			OptionParser optionParser = new OptionParser();
			optionParser.RegisterEvent(_xmlPathArgument, 2, (options) =>
			{
				try
				{
					xmlPath = options[0];
				}
				catch
				{
					throw new NoProcessArgumentsException(_xmlPathArgument);
				}
			});
			optionParser.RegisterEvent(_outputArgument, 2, (options) =>
			{
				try
				{
					outputPath = options[0];
				}
				catch
				{
					throw new NoProcessArgumentsException(_outputArgument);
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

			if (string.IsNullOrEmpty(outputPath))
			{
				PrintError($"There is no output path.");
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
			ConsoleHelper.WriteLine(outputPath, ConsoleColor.White, ConsoleColor.DarkBlue);
			Console.WriteLine($"Packet Type Name : {packetTypeName}");
			Console.WriteLine($"Base Namespace: {baseNamespace}");
			PrintSeparator();

			GeneratePacket(xmlPath, outputPath, packetTypeName, baseNamespace);
		}

		public static void GeneratePacket(string xmlPath,
										  string outputPath, 
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
						op.OutputPath = outputPath;
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
					var targetPath = job.GetTargetPath();
					var result = FileHandler.TryWriteText(targetPath, generatedCode, true);
					if (result.ResultType == JobResultType.Success)
					{
						PrintSaveResult(job.GetFileNameWithExtension(), targetPath);
					}
					else
					{
						throw result.Exception;
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

					var enumCodePath = Path.Combine(outputPath, enumFileName);
					FileHandler.TryWriteText(enumCodePath, enumCode, true);
					PrintSaveResult(enumFileName, enumCodePath);
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