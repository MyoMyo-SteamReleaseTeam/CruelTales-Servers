using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CT.Network.Serialization.Type;
using CT.Tool.Data;
using CT.Tool.GetOpt;

namespace CT.PacketGenerator
{
	public class JobOption
	{
		public string XmlSourcePath = string.Empty;
		public string TargetPath = string.Empty;

		public string GetFileNameWithExtension()
		{
			return Path.GetFileNameWithoutExtension(XmlSourcePath) + ".cs";
		}

		public string GetTargetPath()
		{
			return Path.Combine(TargetPath, GetFileNameWithExtension());
		}
	}

	internal class Program
	{
		private static string _testPacketDirectory = @"../../../PacketDefinition";

		static void Main(string[] args)
		{
			args = new string[]
			{
				@$"--xmlpath ../../../PacketDefinition/TestPacket.xml",
				@$" ../../../PacketDefinition/TestPacket_origin.xml",
				$@"--output ../../../PacketDefinition/",
			};

			List<JobOption> jobList = new();

			OptionParser optionParser = new OptionParser();
			optionParser.RegisterEvent("xmlpath", 2, (options) =>
			{
				for (int i = 0; i < options.Count; i++)
				{
					string op = options[i];

					if (jobList.Count <= i)
						jobList.Add(new JobOption() { XmlSourcePath = op });
					else
						jobList[i].XmlSourcePath = op;
				}
			});
			optionParser.RegisterEvent("output", 2, (options) =>
			{
				for (int i = 0; i < jobList.Count; i++)
				{
					string op = options[0];

					if (jobList.Count <= i)
						jobList.Add(new JobOption() { TargetPath = op });
					else
						jobList[i].TargetPath = op;
				}
			});
			optionParser.OnArguments(args);
			GeneratePacket(jobList);
		}

		public static void GeneratePacket(List<JobOption> jobOptionList)
		{
			foreach (var job in jobOptionList)
			{
				// Read packet files
				PacketParser parser = new PacketParser();

				parser.NewLine = TextFormat.LF;
				parser.Indent = TextFormat.Indent;
				parser.UsingSegments = new()
				{
					$"{nameof(CT)}.{nameof(CT.Network)}.{nameof(CT.Network.Packet)}",
					$"{nameof(CT)}.{nameof(CT.Network)}.{nameof(CT.Network.Serialization)}",
					$"{nameof(CT)}.{nameof(CT.Network)}.{nameof(CT.Network.Serialization)}.{nameof(CT.Network.Serialization.Type)}",
				};
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
						Console.WriteLine($"============================================");
						Console.WriteLine($"   Generate completed : {job.GetFileNameWithExtension()}");
						Console.WriteLine($"============================================");
						Console.WriteLine(generatedCode);
					}
					else
					{
						throw result.Exception;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine($"============================================");
					Console.WriteLine($"   Generate failed!! : {job.GetFileNameWithExtension()}");
					Console.WriteLine($"============================================");
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