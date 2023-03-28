using System;
using System.IO;
using System.Reflection;
using CT.Network.Serialization.Type;
using CT.Tool.Data;

namespace CT.PacketGenerator
{
	internal struct PacketParseJob
	{
		public string SourcePath;
		public string TargetPath;
	}

	internal class Program
	{
		private static string _packetDirectory = @"..\..\..\PacketDefinition";

		static void Main(string[] args)
		{
			//List<string> 

			//OptionParser optionParser = new OptionParser();
			//optionParser.RegisterEvent("xmlpath", 2, (argumentList) =>
			//{
			//	foreach (var arg in argumentList)
			//	{

			//	}
			//});
			//optionParser.RegisterEvent("targetpath", 2, (argumentList) =>
			//{

			//});

			var sourcePath = Path.Combine(_packetDirectory, "TestPacket.xml");

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
			if (netcore != null )
			{
				parser.ReferenceAssemblys.Add(netcore);
			}
			parser.Initialize();

			var generatedCode = parser.ParseFromXml(sourcePath);

			var targetPath = Path.Combine(_packetDirectory, "TestPacket.cs");

			Console.WriteLine($"Packet generacted : {targetPath}");

			var result = FileHandler.TryWriteText(targetPath, generatedCode);
			Console.WriteLine(result.Exception);
		}
	}
}