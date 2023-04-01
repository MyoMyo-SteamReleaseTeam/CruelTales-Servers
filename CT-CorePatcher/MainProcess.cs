using CT.CorePatcher.Packets;

namespace CT.CorePatcher
{
	internal class MainProcess
	{
		public static void Main(string[] args)
		{
			args = new string[]
			{
				$"--{PacketGenerator.XML_PATH} \"../../../../CT-NetworkCore/PacketDefinition/\"",
				$"--{PacketGenerator.OUTPUT_SERVER} \"../../../../CT-NetworkCore/Packets/\"",
				$"-{PacketGenerator.PACKET_TYPE_NAME} PacketType",
				$"-{PacketGenerator.BASE_NAMESPACE} CT.Packets",
			};
			PacketGenerator.Run(args);
		}
	}
}
