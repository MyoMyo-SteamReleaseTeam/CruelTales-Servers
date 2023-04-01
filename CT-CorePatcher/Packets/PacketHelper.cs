using System.Collections.Generic;
using System.Xml;
using CT.Tool.Collections;

namespace CT.CorePatcher.Packets
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
		Using,
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
			{ "bool", "Bool" },
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
			{ "NetString", "NetString" },
			{ "NetStringShort", "NetStringShort" },
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

		public static readonly Dictionary<string, int> _dataSizeByTypeName = new()
		{
			{ "bool",   sizeof(byte) },
			{ "byte",   sizeof(byte) },
			{ "sbyte",  sizeof(sbyte) },
			{ "short",  sizeof(short) },
			{ "ushort", sizeof(ushort) },
			{ "int",    sizeof(int) },
			{ "uint",   sizeof(uint) },
			{ "long",   sizeof(long) },
			{ "ulong",  sizeof(ulong) },
			{ "float",  sizeof(float) },
			{ "double", sizeof(double) },

			{ "Bool",    sizeof(byte) },
			{ "Byte",   sizeof(byte) },
			{ "SByte",  sizeof(sbyte) },
			{ "Int16",  sizeof(short) },
			{ "UInt16", sizeof(ushort) },
			{ "Int32",  sizeof(int) },
			{ "UInt32", sizeof(uint) },
			{ "Int64",  sizeof(long) },
			{ "UInt64", sizeof(ulong) },
			{ "Single", sizeof(float) },
			{ "Double", sizeof(double) },
		};

		public static int GetByteSizeByTypeName(string typeName)
		{
			return _dataSizeByTypeName[typeName];
		}

		public static bool CanGetByteSizeByTypeName(string type)
		{
			return _dataSizeByTypeName.ContainsKey(type);
		}
	}
}
