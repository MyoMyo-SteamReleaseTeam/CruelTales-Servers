using System.Collections.Generic;
using System.Xml;
using CT.Common.Tools.Collections;

namespace CT.CorePatcher.Packets
{
	internal enum PacketAttributeType
	{
		None = 0,
		Name,
		Type,
		Namespace,
		Custom,
	}

	internal enum PacketDataType
	{
		Other = 0,
		Definition,
		ServerPacket,
		ClientPacket,
		Class,
		Struct,
		Using,
	}

	internal static class PacketHelper
	{
		private static readonly BidirectionalMap<PacketDataType, string> _dataTypeKeywordTable = new()
		{
			{ PacketDataType.Definition, "Definition" },
			{ PacketDataType.ServerPacket, "server" },
			{ PacketDataType.ClientPacket, "client" },
			{ PacketDataType.Struct, "struct" },
			{ PacketDataType.Class, "class" },
			{ PacketDataType.Using, "using" },
		};

		public static PacketDataType GetPacketDataType(XmlReader r)
		{
			if (_dataTypeKeywordTable.TryGetValue(r.Name, out var type))
			{
				return type;
			}

			return PacketDataType.Other;
		}

		private static readonly Dictionary<PacketDataType, string> _declarationByDataType = new()
		{
			{ PacketDataType.ServerPacket, "sealed partial class" },
			{ PacketDataType.ClientPacket, "sealed partial class" },
			{ PacketDataType.Struct, "partial struct" },
			{ PacketDataType.Class, "sealed partial class" },
		};

		public static bool TryGetDeclarationBy(PacketDataType type, out string declaration)
		{
			bool result = _declarationByDataType.TryGetValue(type, out var value);
			declaration = value ?? "";
			return result;
		}
	}
}
