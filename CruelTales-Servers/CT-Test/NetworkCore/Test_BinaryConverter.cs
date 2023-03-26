using System;
using CT.Network.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.NetworkCore
{
	[TestClass]
	public class Test_BinaryConverter
	{
		[TestMethod]
		public void Serialization()
		{
			// Define test values
			byte byteValue = 123;
			sbyte sbyteValue = -123;
			ushort ushortValue = 12345;
			short shortValue = -12345;
			uint uintValue = 1234567891;
			int intValue = -1234567891;
			ulong ulongValue = 1234567891012345678L;
			long longValue = -1234567891012345678L;

			bool boolValue = true;
			float floatValue = -12345.789F;
			double doubleValue = -12345678910.123456789D;
			string stringValue = "12515151";

			int byteCount = 123;
			byte[] byteArray = new byte[byteCount];
			for (int i = 0; i < byteArray.Length; i++)
			{
				byteArray[i] = (byte)i;
			}

			// Create test buffer
			int bufferSize = 1000;
			byte[] buffer = new byte[bufferSize];
			ArraySegment<byte> bufferRef = new ArraySegment<byte>(buffer);
			int pos;

			// Wrtie test
			pos = 0;
			BinaryConverter.WriteByte(bufferRef, pos, byteValue); pos += sizeof(byte);
			BinaryConverter.WriteSByte(bufferRef, pos, sbyteValue); pos += sizeof(sbyte);

			BinaryConverter.WriteUInt16(bufferRef, pos, ushortValue); pos += sizeof(ushort);
			BinaryConverter.WriteInt16(bufferRef, pos, shortValue); pos += sizeof(short);

			BinaryConverter.WriteUInt32(bufferRef, pos, uintValue); pos += sizeof(uint);
			BinaryConverter.WriteInt32(bufferRef, pos, intValue); pos += sizeof(int);

			BinaryConverter.WriteUInt64(bufferRef, pos, ulongValue); pos += sizeof(ulong);
			BinaryConverter.WriteInt64(bufferRef, pos, longValue); pos += sizeof(long);

			BinaryConverter.WriteBool(bufferRef, pos, boolValue); pos += 1;

			BinaryConverter.WriteFloat(bufferRef, pos, floatValue); pos += sizeof(float);
			BinaryConverter.WriteDouble(bufferRef, pos, doubleValue); pos += sizeof(double);

			pos += BinaryConverter.WriteBytes(bufferRef, pos, byteArray);
			pos += BinaryConverter.WriteString(bufferRef, pos, stringValue);

			// Read test
			pos = 0;
			Assert.AreEqual(byteValue, BinaryConverter.ReadByte(buffer, pos)); pos += sizeof(byte);
			Assert.AreEqual(sbyteValue, BinaryConverter.ReadSByte(buffer, pos)); pos += sizeof(sbyte);

			Assert.AreEqual(ushortValue, BinaryConverter.ReadUInt16(buffer, pos)); pos += sizeof(ushort);
			Assert.AreEqual(shortValue, BinaryConverter.ReadInt16(buffer, pos)); pos += sizeof(short);

			Assert.AreEqual(uintValue, BinaryConverter.ReadUInt32(buffer, pos)); pos += sizeof(uint);
			Assert.AreEqual(intValue, BinaryConverter.ReadInt32(buffer, pos)); pos += sizeof(int);

			Assert.AreEqual(ulongValue, BinaryConverter.ReadUInt64(buffer, pos)); pos += sizeof(ulong);
			Assert.AreEqual(longValue, BinaryConverter.ReadInt64(buffer, pos)); pos += sizeof(long);

			Assert.AreEqual(boolValue, BinaryConverter.ReadBool(buffer, pos)); pos += 1;

			Assert.AreEqual(floatValue, BinaryConverter.ReadFloat(buffer, pos)); pos += sizeof(float);
			Assert.AreEqual(doubleValue, BinaryConverter.ReadDouble(buffer, pos)); pos += sizeof(double);

			byte[] copyArray = new byte[byteCount];
			BinaryConverter.ReadBytesCopy(buffer, pos, copyArray, 0);
			var copyArray2 = BinaryConverter.ReadBytes(buffer, pos, out int arrayRead);
			pos += arrayRead;

			for (int i = 0; i < byteCount; i++)
			{
				Assert.AreEqual(byteArray[i], copyArray[i]);
				Assert.AreEqual(byteArray[i], copyArray2[i]);
			}

			Assert.AreEqual(stringValue, BinaryConverter.ReadString(buffer, pos, out _));

			// unsafe
			byte[] unsafeBuffer = new byte[123];
			ArraySegment<byte> unsafeBufferRef = new ArraySegment<byte>(unsafeBuffer);
			string testString = "12341abac가나다";

			int testBytesSize = 17;
			byte[] testBytes = new byte[testBytesSize];
			for (int i = 0; i <  testBytesSize; i++)
			{
				testBytes[i] = (byte)i;
			}

			int testStringSize = BinaryConverter.WriteStringUnsafe(unsafeBufferRef, 0, testString);
			BinaryConverter.WriteBytesUnsafe(unsafeBufferRef, testStringSize, testBytes);

			Assert.AreEqual(testString, BinaryConverter.ReadStringByLength(unsafeBufferRef, 0, testStringSize));
			var resultBytes = BinaryConverter.ReadByteCopyByLength(unsafeBufferRef, testStringSize, testBytesSize);

			byte[] resultBytes2 = new byte[123];
			BinaryConverter.ReadBytesByLength(unsafeBufferRef, testStringSize, resultBytes2, 0, testBytesSize);

			for (int i = 0; i < testBytesSize; i++)
			{
				Assert.AreEqual(testBytes[i], resultBytes[i]);
				Assert.AreEqual(testBytes[i], resultBytes2[i]);
			}
		}
	}
}