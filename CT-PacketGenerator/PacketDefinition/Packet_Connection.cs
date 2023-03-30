using CT.Network.Packets;
using CT.Network.Serialization;
using CT.Network.Serialization.Type;

namespace CT.Packets.Connection
{
	public partial struct NetworkId : IPacketSerializable
	{
		public ulong Id;
	
		public int SerializeSize =>  + 8;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(Id);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Id = reader.ReadUInt64();
		}
	}
	
	public partial struct Position3D : IPacketSerializable
	{
		public float X;
		public float Y;
		public float Z;
	
		public int SerializeSize =>  + 12;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(X);
			writer.Put(Y);
			writer.Put(Z);
		}
	
		public void Deserialize(PacketReader reader)
		{
			X = reader.ReadSingle();
			Y = reader.ReadSingle();
			Z = reader.ReadSingle();
		}
	}
	
	public partial struct CoordInt : IPacketSerializable
	{
		public int X;
		public int Y;
		public int Z;
	
		public int SerializeSize =>  + 12;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(X);
			writer.Put(Y);
			writer.Put(Z);
		}
	
		public void Deserialize(PacketReader reader)
		{
			X = reader.ReadInt32();
			Y = reader.ReadInt32();
			Z = reader.ReadInt32();
		}
	}
	
	public partial class Entity : IPacketSerializable
	{
		public EntityType Type;
		public Position3D Position3D = new();
		public NetStringShort Name;
		public CoordInt Coord = new();
		public int Hp;
		public int Level;
		public NetIntArray Data = new();
	
		public int SerializeSize => Position3D.SerializeSize + Name.SerializeSize + Coord.SerializeSize + Data.SerializeSize + 9;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(Type);
			Position3D.Serialize(writer);
			writer.Put(Name);
			Coord.Serialize(writer);
			writer.Put(Hp);
			writer.Put(Level);
			Data.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Type = reader.ReadEntityType();
			Position3D.Deserialize(reader);
			Name = reader.ReadNetStringShort();
			Coord.Deserialize(reader);
			Hp = reader.ReadInt32();
			Level = reader.ReadInt32();
			Data.Deserialize(reader);
		}
	}
	
	public sealed class Server_Okay : IPacketSerializable
	{
		public bool IsOkay;
		public NetStringShort WelcomeMeesage;
	
		public int SerializeSize => WelcomeMeesage.SerializeSize + 1;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(IsOkay);
			writer.Put(WelcomeMeesage);
		}
	
		public void Deserialize(PacketReader reader)
		{
			IsOkay = reader.ReadBool();
			WelcomeMeesage = reader.ReadNetStringShort();
		}
	}
	
	public sealed class Client_TryConnect : IPacketSerializable
	{
		public NetStringShort Username;
	
		public int SerializeSize => Username.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(Username);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Username = reader.ReadNetStringShort();
		}
	}
}