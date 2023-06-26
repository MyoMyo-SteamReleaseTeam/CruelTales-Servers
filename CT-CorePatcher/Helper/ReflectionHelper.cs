using System;
using System.Collections.Generic;
using System.Reflection;
using CT.Common.DataType;
using CT.Common.Tools.Collections;

namespace CT.CorePatcher.Helper
{
	public static class ReflectionHelper
	{
		private static readonly HashSet<string> _nativeStructSet = new()
		{
			"Vector2", "Vector3"
		};

		public static bool IsNativeStruct(string typeName)
		{
			return _nativeStructSet.Contains(typeName);
		}

		private static readonly BidirectionalMap<string, string> _primitiyTypeByCLR = new()
		{
			{ "bool", "Boolean" },
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
			//{ "NetString", "NetString" },
			//{ "NetStringShort", "NetStringShort" },
		};

		public static bool IsCLRPrimitiveType(string typeName)
		{
			return _primitiyTypeByCLR.TryGetForward(typeName, out _) ||
				   _primitiyTypeByCLR.TryGetReverse(typeName, out _);
		}

		public static bool IsNetString(string typeName)
		{
			return typeName == nameof(NetString) || typeName == nameof(NetStringShort);
		}

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

		private static readonly Dictionary<string, int> _dataSizeByTypeName = new()
		{
			{ "Boolean", sizeof(byte) },
			{ "boolean", sizeof(byte) },
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

			{ "Bool",   sizeof(byte) },
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
