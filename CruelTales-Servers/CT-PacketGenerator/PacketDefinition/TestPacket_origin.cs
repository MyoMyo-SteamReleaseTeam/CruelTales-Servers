using CT.Network.Packet;
using CT.Network.Serialization;
using CT.Network.Serialization.Type;

namespace CT.Packets
{
	public partial struct NetworkID : IPacketSerializable
	{
		public ushort ID;
		public TestType Type;
	
		public int SerializeSize =>  + 3;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(ID);
			writer.Put(Type);
		}
	
		public void Deserialize(PacketReader reader)
		{
			ID = reader.ReadUInt16();
			Type = reader.ReadTestType();
		}
	}
	
	public partial class NetworkObject : IPacketSerializable
	{
		public NetworkId ID = new();
		public NetStringShort Name;
		public int Hp;
	
		public int SerializeSize => ID.SerializeSize + Name.SerializeSize + 4;
	
		public void Serialize(PacketWriter writer)
		{
			ID.Serialize(writer);
			writer.Put(Name);
			writer.Put(Hp);
		}
	
		public void Deserialize(PacketReader reader)
		{
			ID.Deserialize(reader);
			Name = reader.ReadNetStringShort();
			Hp = reader.ReadInt32();
		}
	}
	
	public sealed class Server_SpawnPlayer : IPacketSerializable
	{
		public NetArray<NetworkObject> Players = new NetArray<NetworkObject>();
	
		public int SerializeSize => Players.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			Players.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Players.Deserialize(reader);
		}
	}
	
	public sealed class Server_DespawnPlayer : IPacketSerializable
	{
		public NetFixedArray<NetworkID> IDs = new NetFixedArray<NetworkID>();
	
		public int SerializeSize => IDs.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			IDs.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			IDs.Deserialize(reader);
		}
	}
	
	public sealed class Client_TestClientPacket : IPacketSerializable
	{
		public partial class TestInnerClass : IPacketSerializable
		{
			public int IntField;
			public NetStringShort Name;
		
			public int SerializeSize => Name.SerializeSize + 4;
		
			public void Serialize(PacketWriter writer)
			{
				writer.Put(IntField);
				writer.Put(Name);
			}
		
			public void Deserialize(PacketReader reader)
			{
				IntField = reader.ReadInt32();
				Name = reader.ReadNetStringShort();
			}
		}
	
		public partial class TestLateClass1 : IPacketSerializable
		{
			public uint TestUInt32;
			public NetStringShort TestString;
		
			public int SerializeSize => TestString.SerializeSize + 4;
		
			public void Serialize(PacketWriter writer)
			{
				writer.Put(TestUInt32);
				writer.Put(TestString);
			}
		
			public void Deserialize(PacketReader reader)
			{
				TestUInt32 = reader.ReadUInt32();
				TestString = reader.ReadNetStringShort();
			}
		}
	
		public partial class TestLateClass2 : IPacketSerializable
		{
			public uint TestUInt32;
			public NetStringShort TestString;
		
			public int SerializeSize => TestString.SerializeSize + 4;
		
			public void Serialize(PacketWriter writer)
			{
				writer.Put(TestUInt32);
				writer.Put(TestString);
			}
		
			public void Deserialize(PacketReader reader)
			{
				TestUInt32 = reader.ReadUInt32();
				TestString = reader.ReadNetStringShort();
			}
		}
	
		public partial class TestLateClass3 : IPacketSerializable
		{
			public uint TestUInt32;
			public NetStringShort TestString;
		
			public int SerializeSize => TestString.SerializeSize + 4;
		
			public void Serialize(PacketWriter writer)
			{
				writer.Put(TestUInt32);
				writer.Put(TestString);
			}
		
			public void Deserialize(PacketReader reader)
			{
				TestUInt32 = reader.ReadUInt32();
				TestString = reader.ReadNetStringShort();
			}
		}
	
		public TestInnerClass InnerClassField = new();
	
		public int SerializeSize => InnerClassField.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			InnerClassField.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			InnerClassField.Deserialize(reader);
		}
	}
}