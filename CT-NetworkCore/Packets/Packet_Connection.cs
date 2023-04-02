using CT.Network.DataType;
using CT.Network.Serialization;
using CT.Network.Serialization.Type;

namespace CT.Packets
{
	public sealed partial class UserProfile : IPacketSerializable
	{
		public NetStringShort Clothes;
	
		public int SerializeSize => Clothes.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(Clothes);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Clothes = reader.ReadNetStringShort();
		}
	}
	
	public sealed class Client_TryConnect : IPacketSerializable
	{
		public ulong Id;
		public NetStringShort Token;
		public MatchEndpoint MatchTo = new();
	
		public int SerializeSize => Token.SerializeSize + MatchTo.SerializeSize + 8;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(Id);
			writer.Put(Token);
			MatchTo.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Id = reader.ReadUInt64();
			Token = reader.ReadNetStringShort();
			MatchTo.Deserialize(reader);
		}
	}
	
	public sealed class Server_AckConnect : IPacketSerializable
	{
		public ConnectAck Result;
	
		public int SerializeSize =>  + 1;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(Result);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Result = reader.ReadConnectAck();
		}
	}
	
	public sealed class Client_TrySendUserProfile : IPacketSerializable
	{
		public UserProfile UserProfile = new();
	
		public int SerializeSize => UserProfile.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			UserProfile.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			UserProfile.Deserialize(reader);
		}
	}
	
	public sealed class Server_InitialWorldState : IPacketSerializable
	{
		public NetStringShort MiniGameName;
	
		public int SerializeSize => MiniGameName.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(MiniGameName);
		}
	
		public void Deserialize(PacketReader reader)
		{
			MiniGameName = reader.ReadNetStringShort();
		}
	}
	
	public sealed class Server_SpawnEntities : IPacketSerializable
	{
		public NetArray<EntityPlayerState> SpawnedEntities = new NetArray<EntityPlayerState>();
	
		public int SerializeSize => SpawnedEntities.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			SpawnedEntities.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			SpawnedEntities.Deserialize(reader);
		}
	}
	
	public sealed class Server_DespawnEntities : IPacketSerializable
	{
		public NetFixedArray<NetEntityID> DespawnEntities = new NetFixedArray<NetEntityID>();
	
		public int SerializeSize => DespawnEntities.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			DespawnEntities.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			DespawnEntities.Deserialize(reader);
		}
	}
}