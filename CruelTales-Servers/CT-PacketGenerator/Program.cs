using System.IO;

namespace CT.PacketGenerator
{
	internal class Program
	{
		private static string _packetDirectory = @"../../../PacketDefinition";

		static void Main(string[] args)
		{
			// Read packet files
			PacketParser parser = new PacketParser();
			var targetPath = Path.Combine(_packetDirectory, "TestPacket.xml");
			parser.ParseFromXml(targetPath);
		}
	}
}