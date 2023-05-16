using CT.Common.DataType;
using CT.Common.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.NetworkCore
{
	[TestClass]
	public class Test_NetArray
	{
		[TestMethod]
		public void NetArrayTest_ValueTypes()
		{
			NetFloatArray floatArray = new NetFloatArray();
			float multiply = 1.123f;
			int testCount = 33;
			for (int i = 0; i < testCount; i++)
			{
				floatArray.Add(i * multiply);
			}

			ByteBuffer segment = new(33 * 4);
			IPacketWriter writer = segment;
			IPacketReader reader = segment;

			floatArray.Serialize(writer);

			NetFloatArray newFloatArray = new NetFloatArray();
			newFloatArray.Deserialize(reader);

			for (int i = 0; i <  newFloatArray.Count; i++)
			{
				Assert.AreEqual(floatArray[i], newFloatArray[i]);
			}
		}

		class FlexibleSizeData : IPacketSerializable
		{
			public NetString Name;
			public int Age;

			public FlexibleSizeData() {}

			public FlexibleSizeData(string name, int age)
			{
				Name = name;
				Age = age;
			}

			public int SerializeSize => sizeof(int) + Name.SerializeSize;

			public void Serialize(IPacketWriter writer)
			{
				writer.Put(Name);
				writer.Put(Age);
			}

			public void Deserialize(IPacketReader reader)
			{
				Name = reader.ReadNetString();
				Age = reader.ReadInt32();
			}

			public void Ignore(IPacketReader reader)
			{
				throw new System.NotImplementedException();
			}

			public static void IgnoreStatic(IPacketReader reader)
			{
				throw new System.NotImplementedException();
			}
		}

		[TestMethod]
		public void NetArrayTest_Flexible()
		{
			NetArray<FlexibleSizeData> refArray = new()
			{
				new FlexibleSizeData("Test", 12),
				new FlexibleSizeData("WOW", 15),
				new FlexibleSizeData("ABCDE한글", 777),
			};

			ByteBuffer segment = new(200);
			IPacketWriter writer = segment;
			IPacketReader reader = segment;

			refArray.Serialize(writer);

			NetFloatArray newFloatArray = new NetFloatArray();
			newFloatArray.Deserialize(reader);

			for (int i = 0; i < refArray.Count; i++)
			{
				Assert.AreEqual(refArray[i].Name, refArray[i].Name);
				Assert.AreEqual(refArray[i].Age, refArray[i].Age);
			}
		}

		enum TestType
		{
			None = 0,
			A, B, C, D, E, F, G, H, I
		}

		class FixedSizeData : IPacketSerializable
		{
			public TestType Type;
			public int Age;
			public int Height;

			public int SerializeSize => 9;

			public void Serialize(IPacketWriter writer)
			{
				writer.Put((byte)Type);
				writer.Put(Age);
				writer.Put(Height);
			}

			public void Deserialize(IPacketReader reader)
			{
				Type = (TestType)reader.ReadByte();
				Age = reader.ReadInt32();
				Height = reader.ReadInt32();
			}

			public void Ignore(IPacketReader reader)
			{
				throw new System.NotImplementedException();
			}

			public static void IgnoreStatic(IPacketReader reader)
			{
				throw new System.NotImplementedException();
			}
		}

		[TestMethod]
		public void NetArrayTest_Fixed()
		{
			NetFixedArray<FixedSizeData> fixedArray = new()
			{
				new FixedSizeData() { Type = TestType.A,  Age = 15, Height = 120 },
				new FixedSizeData() { Type = TestType.B,  Age = 55, Height = 2333 },
				new FixedSizeData() { Type = TestType.C,  Age = 33, Height = 1551 },
				new FixedSizeData() { Type = TestType.G,  Age = 656, Height = 123 },
			};

			ByteBuffer segment = new(9 * fixedArray.Count);
			IPacketWriter writer = segment;
			IPacketReader reader = segment;

			fixedArray.Serialize(writer);

			NetFloatArray newFloatArray = new NetFloatArray();
			newFloatArray.Deserialize(reader);

			for (int i = 0; i < fixedArray.Count; i++)
			{
				Assert.AreEqual(fixedArray[i].Type, fixedArray[i].Type);
				Assert.AreEqual(fixedArray[i].Age, fixedArray[i].Age);
				Assert.AreEqual(fixedArray[i].Height, fixedArray[i].Height);
			}
		}
	}
}
