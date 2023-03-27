using System.IO;
using CT.Network.Serialization.Type;
using System.Reflection;
using System;

namespace CT.PacketGenerator
{
	internal class Program
	{
		private static string _packetDirectory = @"../../../PacketDefinition";

		static void Main(string[] args)
		{
			var targetPath = Path.Combine(_packetDirectory, "TestPacket.xml");

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
			var generatedCode = parser.ParseFromXml(targetPath);

			Console.WriteLine(generatedCode);
		}
	}
}