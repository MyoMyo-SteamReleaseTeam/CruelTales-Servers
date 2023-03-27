using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CT.Tool.Collections;

namespace CT.PacketGenerator
{
	internal enum PacketAttributeType
	{
		None = 0,
		Name,
		Type,
		Namespace
	}

	internal enum PacketDataType
	{
		Other = 0,
		Definition,
		ServerPacket,
		ClientPacket,
		Class,
		Struct,
	}

	internal static class PacketHelper
	{
		private static readonly BidirectionalMap<PacketDataType, string> _dataTypeKeywordTable = new()
		{
			{ PacketDataType.Definition, "Definition" },
			{ PacketDataType.ServerPacket, "packet-server" },
			{ PacketDataType.ClientPacket, "packet-client" },
			{ PacketDataType.Struct, "struct" },
			{ PacketDataType.Class, "class" },
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
			{ PacketDataType.ServerPacket, "sealed class" },
			{ PacketDataType.ClientPacket, "sealed class" },
			{ PacketDataType.Struct, "partial struct" },
			{ PacketDataType.Class, "partial class" },
		};

		public static bool TryGetDeclarationBy(PacketDataType type, out string declaration)
		{
			bool result = _declarationByDataType.TryGetValue(type, out var value);
			declaration = value ?? "";
			return result;
		}

		private static readonly BidirectionalMap<string, string> _primitiyTypeByCLR = new()
		{
			{ "byte", "Byte" },
			{ "sbyte", "SByte" },
			{ "short", "Int16" },
			{ "ushort", "UInt16" },
			{ "int", "Int32" },
			{ "uint", "UInt32" },
			{ "long", "Int64" },
			{ "ulong", "UInt64" },
			{ "float", "Single" },
			{ "double", "Double" },
			{ "string", "NetString" },
			{ "stringShort", "NetStringShort" },
		};

		public static bool TryGetCLRTypeByPrimitive(string primitiveType, out string CLRtype)
		{
			bool result = _primitiyTypeByCLR.TryGetForward(primitiveType, out var value);
			CLRtype = value ?? "";
			return result;
		}

		public static bool TryGetTypeByCLRType(string CLRtype, out string primitiveType)
		{
			bool result = _primitiyTypeByCLR.TryGetReverse(CLRtype, out var value);
			primitiveType = value ?? "";
			return result;
		}
	}
}
