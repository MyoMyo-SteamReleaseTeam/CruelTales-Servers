using System;
using CT.Common.DataType;
using CT.Common.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.NetworkCore
{
	[TestClass]
	public class Test_ByteBuffer
	{
		[TestMethod]
		public void ByteBufferTryPeekTest()
		{
			int testBufferCount = 5;

			ConcurrentByteBufferPool pool = new ConcurrentByteBufferPool(15, testBufferCount, ignoreLOH: true);
			ByteBuffer[] b = new ByteBuffer[testBufferCount];

			for (int i = 0; i < testBufferCount; i++)
			{
				b[i] = pool.Get();
			}

			for (int i = 0; i < testBufferCount; i++)
			{
				for (int j = i + 1; j < testBufferCount; j++)
				{
					Assert.AreNotSame(b[i], b[j]);
				}
			}

			// 15
			byte byteValue = 123;
			ushort ushortValue = 12345;
			uint uintValue = 1234567891;
			ulong ulongValue = 1234567891012345678L;
			b[0].Put(byteValue);
			b[0].Put(ushortValue);
			b[0].Put(uintValue);
			b[0].Put(ulongValue);

			testTryReadPeek(byteValue, b[0].TryPeekByte, b[0].TryReadByte);
			testTryReadPeek(ushortValue, b[0].TryPeekUInt16, b[0].TryReadUInt16);
			testTryReadPeek(uintValue, b[0].TryPeekUInt32, b[0].TryReadUInt32);
			testTryReadPeek(ulongValue, b[0].TryPeekUInt64, b[0].TryReadUInt64);

			// 15 
			sbyte sbyteValue = -123;
			short shortValue = -12345;
			int intValue = -1234567891;
			long longValue = -1234567891012345678L;
			b[1].Put(sbyteValue);
			b[1].Put(shortValue);
			b[1].Put(intValue);
			b[1].Put(longValue);

			testTryReadPeek(sbyteValue, b[1].TryPeekSByte, b[1].TryReadSByte);
			testTryReadPeek(shortValue, b[1].TryPeekInt16, b[1].TryReadInt16);
			testTryReadPeek(intValue, b[1].TryPeekInt32, b[1].TryReadInt32);
			testTryReadPeek(longValue, b[1].TryPeekInt64, b[1].TryReadInt64);

			// 13
			bool boolValue = true;
			float floatValue = -12345.789F;
			double doubleValue = -12345678910.123456789D;
			b[2].Put(boolValue);
			b[2].Put(floatValue);
			b[2].Put(doubleValue);

			testTryReadPeek(boolValue, b[2].TryPeekBool, b[2].TryReadBool);
			testTryReadPeek(floatValue, b[2].TryPeekSingle, b[2].TryReadSingle);
			testTryReadPeek(doubleValue, b[2].TryPeekDouble, b[2].TryReadDouble);

			// 9
			string stringValue = "12515151";
			b[3].Put(new NetStringShort(stringValue));

			Assert.IsTrue(b[3].TryReadNetStringShort(out var rstringValue));
			Assert.AreEqual(stringValue, rstringValue);

			// 15
			string shortStringValue = "가나다라abcd";
			b[4].Put(new NetString(shortStringValue));

			Assert.IsTrue(b[4].TryReadNetString(out var rshortStringValue));
			Assert.AreEqual(shortStringValue, rshortStringValue);
		}

		private void testTryReadPeek<T>(T value,
										FuncTryReadPeek<T> peekFunc,
										FuncTryReadPeek<T> readFunc) where T : struct
		{
			Assert.IsTrue(peekFunc(out var outPeekValue));
			Assert.AreEqual(value, outPeekValue);
			Assert.IsTrue(readFunc(out var outReadValue));
			Assert.AreEqual(value, outReadValue);
		}
	}

	public delegate bool FuncTryReadPeek<T>(out T value);
}