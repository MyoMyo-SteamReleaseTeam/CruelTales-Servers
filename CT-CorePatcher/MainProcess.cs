using CT.CorePatcher.FilePatch;
using CT.CorePatcher.Packets;

namespace CT.CorePatcher
{
	internal class MainProcess
	{
		public static void Main(string[] args)
		{
			args = new string[]
			{
				// Packet Generator
				$"--{PacketGenerator.XML_PATH} \"../../../../CT-NetworkCore/PacketDefinition/\"",
				$"--{PacketGenerator.OUTPUT_SERVER} \"../../../../CT-NetworkCore/Packets/\"",
				$"-{PacketGenerator.PACKET_TYPE_NAME} PacketType",
				$"-{PacketGenerator.BASE_NAMESPACE} CT.Packets",
			};
			
			if (PacketGenerator.Run(args) == false)
			{
				PatcherConsole.PrintError($"{nameof(PacketGenerator)} error!");
				return;
			}

			args = new string[]
			{
				// FilePatcher
				$"--{FilePatcherRunner.ORIGIN_PATH} \"../../../../CT-NetworkCore\"",
				$"--{FilePatcherRunner.TARGET_PATH} \"../../../../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-NetworkCore",
				$"-{FilePatcherRunner.GUARD_EXTENSION} .asmdef",
				$"-{FilePatcherRunner.INCLUDE_EXTENSION} .cs .xml .meta",
				$"-{FilePatcherRunner.EXCLUDE_FOLDER} obj bin debug build Legacy",
			};

			if (FilePatcherRunner.Run(args) == false)
			{
				PatcherConsole.PrintError($"{nameof(FilePatcherRunner)} error! Project : CT-NetworkCore");
				return;
			}

			args = new string[]
			{
				// FilePatcher
				$"--{FilePatcherRunner.ORIGIN_PATH} \"../../../../CT-Tools\"",
				$"--{FilePatcherRunner.TARGET_PATH} \"../../../../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-Tools",
				$"-{FilePatcherRunner.GUARD_EXTENSION} .asmdef",
				$"-{FilePatcherRunner.INCLUDE_EXTENSION} .cs .xml .meta",
				$"-{FilePatcherRunner.EXCLUDE_FOLDER} obj bin debug build Legacy",
			};

			if (FilePatcherRunner.Run(args) == false)
			{
				PatcherConsole.PrintError($"{nameof(FilePatcherRunner)} error! Project : CT-Tools");
				return;
			}
		}
	}
}