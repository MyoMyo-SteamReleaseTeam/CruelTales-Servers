using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CT.Network.Serialization.Type;
using CT.PacketGenerator.Exceptions;
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
		private static string _testPacketDirectory = @"../../../PacketDefinition";
		private static string _xmlPathArgument = @$"xmlpath";
		private static string _outputArgument = @$"xmlpath";

		static void Main(string[] args)
		{
			string xmlPath = string.Empty;
			string outputPath = string.Empty;

			args = new string[]
			{
				@$"--{_xmlPathArgument} ../../../PacketDefinition/",
				$@"--{_outputArgument} ../../../PacketDefinition/",
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

			optionParser.OnArguments(args);
			GeneratePacket(xmlPath, outputPath);
		}

		public static void GeneratePacket(string xmlPath, string outputPath)
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

				try
				{
					var generatedCode = parser.ParseFromXml(job.XmlSourcePath);
					var targetPath = job.GetTargetPath();
					var result = FileHandler.TryWriteText(targetPath, generatedCode, true);
					if (result.ResultType == JobResultType.Success)
					{
						//Console.WriteLine($"==================================================");
						Console.WriteLine($"   Generate completed : {job.GetFileNameWithExtension()}");
						Console.WriteLine($"==================================================");
						//Console.WriteLine(generatedCode);
					}
					else
					{
						throw result.Exception;
					}
				}
				catch (Exception e)
				{
					//Console.WriteLine($"==================================================");
					Console.WriteLine($"   Generate failed!! : {job.GetFileNameWithExtension()}");
					Console.WriteLine($"==================================================");
					Console.WriteLine($"File save failed!");
					Console.WriteLine($"");
					Console.WriteLine($"# {e.GetType().Name} #");
					Console.WriteLine($"");
					Console.WriteLine($"{e.Message}");
				}
			}
		}
	}
}