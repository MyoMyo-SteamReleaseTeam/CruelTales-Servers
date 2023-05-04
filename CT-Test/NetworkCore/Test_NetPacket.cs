using System;
using CT.Common.DataType;
using CT.Common.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.NetworkCore
{
	[TestClass]
	public class Test_NetPacket
	{
		[TestMethod]
		public void PacketSerialization()
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
			string shortStringValue = "가나다라abcde12345...!@#$%";

			int byteCount = 123;
			byte[] byteArray = new byte[byteCount];
			for (int i = 0; i < byteArray.Length; i++)
			{
				byteArray[i] = (byte)i;
			}

			PacketSegment ps = new PacketSegment(1000);
			PacketWriter writer = new PacketWriter(ps);
			PacketReader reader = new PacketReader(ps);

			writer.Put(byteValue);
			writer.Put(sbyteValue);
			writer.Put(ushortValue);
			writer.Put(shortValue);
			writer.Put(uintValue);
			writer.Put(intValue);
			writer.Put(ulongValue);
			writer.Put(longValue);

			writer.Put(boolValue);
			writer.Put(floatValue);
			writer.Put(doubleValue);
			writer.Put(new NetString(stringValue));
			writer.Put(new NetStringShort(shortStringValue));
			writer.Put(byteArray);

			Assert.AreEqual(byteValue, reader.ReadByte());
			Assert.AreEqual(sbyteValue, reader.ReadSByte());
			Assert.AreEqual(ushortValue, reader.ReadUInt16());
			Assert.AreEqual(shortValue, reader.ReadInt16());
			Assert.AreEqual(uintValue, reader.ReadUInt32());
			Assert.AreEqual(intValue, reader.ReadInt32());
			Assert.AreEqual(ulongValue, reader.ReadUInt64());
			Assert.AreEqual(longValue, reader.ReadInt64());

			Assert.AreEqual(boolValue, reader.ReadBool());
			Assert.AreEqual(floatValue, reader.ReadSingle());
			Assert.AreEqual(doubleValue, reader.ReadDouble());
			Assert.IsTrue(stringValue == reader.ReadNetString());
			Assert.IsTrue(shortStringValue == reader.ReadNetStringShort());

			int bytesPosition = reader.Position;
			var readBytes = reader.ReadBytes();
			reader.SetPosition(bytesPosition);
			var readBytes2 = new ArraySegment<byte>(new byte[byteCount]);
			reader.ReadBytesCopy(readBytes2, 0);
			for (int i = 0; i < byteCount; i++)
			{
				Assert.AreEqual(byteArray[i], readBytes[i]);
				Assert.AreEqual(byteArray[i], readBytes2[i]);
			}
		}

		[TestMethod]
		public void PacketCanReadWrite()
		{
			PacketSegment ps = new PacketSegment(12);
			PacketWriter writer = new PacketWriter(ps);
			PacketReader reader = new PacketReader(ps);

			int intValue = 123456789;
			double doubleValue = 12345676.124124D;

			Assert.IsTrue(writer.CanPut(sizeof(int)));
			writer.Put(intValue);
			Assert.IsTrue(writer.CanPut(sizeof(double)));
			writer.Put(doubleValue);
			Assert.IsFalse(writer.CanPut(1));

			Assert.IsTrue(reader.CanRead(sizeof(int)));
			Assert.AreEqual(intValue, reader.ReadInt32());
			Assert.IsTrue(reader.CanRead(sizeof(double)));
			Assert.AreEqual(doubleValue, reader.ReadDouble());
			Assert.IsFalse(reader.CanRead(1));
		}

		[TestMethod]
		public void PacketByArraySegment()
		{
			int testCount = 16;
			int testOffset = 4;
			byte[] data = new byte[testCount];

			PacketSegment ps = new PacketSegment(data);
			PacketWriter writer = new PacketWriter(ps);

			writer.Put(10);
			Assert.IsTrue(writer.CanPut(1));
			Assert.IsFalse(writer.IsEnd);

			writer.Put(20);
			Assert.IsTrue(writer.CanPut(1));
			Assert.IsFalse(writer.IsEnd);

			writer.Put(30);
			Assert.IsTrue(writer.CanPut(1));
			Assert.IsFalse(writer.IsEnd);

			writer.Put(40);
			Assert.IsFalse(writer.CanPut(1));
			Assert.IsTrue(writer.IsEnd);

			ArraySegment<byte> segments = new(data, testOffset, testCount - testOffset);
			ps = new PacketSegment(segments);
			PacketReader reader = new PacketReader(ps);

			Assert.AreEqual(20, reader.ReadInt32());
			Assert.IsTrue(reader.CanRead(1));
			Assert.IsFalse(reader.IsEnd);

			Assert.AreEqual(30, reader.ReadInt32());
			Assert.IsTrue(reader.CanRead(1));
			Assert.IsFalse(reader.IsEnd);

			Assert.AreEqual(40, reader.ReadInt32());
			Assert.IsFalse(reader.CanRead(1));
			Assert.IsTrue(reader.IsEnd);
		}

		[TestMethod]
		public void OffsetArraySegment()
		{
			byte[] data = new byte[4];
			data[0] = (byte)1;
			data[1] = (byte)2;
			data[2] = (byte)3;
			data[3] = (byte)4;
			ArraySegment<byte> section = new(data, 2, 2);
			PacketReader reader = new PacketReader(section);

			Assert.AreEqual(3, reader.ReadByte());
			Assert.AreEqual(4, reader.ReadByte());
		}

		[TestMethod]
		public void OffsetArraySegmentString()
		{
			byte[] data = new byte[16];
			PacketWriter writer = new PacketWriter(data);
			writer.Put(0); // 4
			writer.Put(0); // 4
			writer.Put(0); // 4
			writer.Put(new NetStringShort("12"));

			ArraySegment<byte> section = new(data, 12, 3);
			PacketReader reader = new PacketReader(section);

			Assert.AreEqual("12", reader.ReadNetStringShort().Value);
		}

		[TestMethod]
		public void WriteWriter()
		{
			byte[] srcData = new byte[12];
			PacketWriter writer0 = new(new ArraySegment<byte>(srcData, 0, 4));
			PacketWriter writer1 = new(new ArraySegment<byte>(srcData, 4, 4));
			PacketWriter writer2 = new(new ArraySegment<byte>(srcData, 8, 4));
			writer0.Put(1);
			writer1.Put(2);
			writer2.Put(3);

			byte[] destData = new byte[12];
			PacketReader reader = new(destData);
			PacketWriter destWriter = new(destData);
			destWriter.Put(writer0);
			destWriter.Put(writer1);
			destWriter.Put(writer2);

			reader.SetPosition(0);
			Assert.AreEqual(1, reader.ReadInt32());
			Assert.AreEqual(2, reader.ReadInt32());
			Assert.AreEqual(3, reader.ReadInt32());
		}
	}
}
