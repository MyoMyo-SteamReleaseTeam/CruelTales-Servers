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
	
	public sealed class Client_TryConnect : PacketBase
	{
		public override PacketType PacketType => PacketType.Client_TryConnect;
	
		public ulong Id;
		public NetStringShort Token;
		public MatchEndpoint MatchTo = new();
	
		public override int SerializeSize => Token.SerializeSize + MatchTo.SerializeSize + 10;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(Id);
			writer.Put(Token);
			MatchTo.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			Id = reader.ReadUInt64();
			Token = reader.ReadNetStringShort();
			MatchTo.Deserialize(reader);
		}
	}
	
	public sealed class Server_AckConnect : PacketBase
	{
		public override PacketType PacketType => PacketType.Server_AckConnect;
	
		public ConnectAck Result;
	
		public override int SerializeSize =>  + 3;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(Result);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			Result = reader.ReadConnectAck();
		}
	}
	
	public sealed class Client_TrySendUserProfile : PacketBase
	{
		public override PacketType PacketType => PacketType.Client_TrySendUserProfile;
	
		public UserProfile UserProfile = new();
	
		public override int SerializeSize => UserProfile.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			UserProfile.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			UserProfile.Deserialize(reader);
		}
	}
	
	public sealed class Server_InitialWorldState : PacketBase
	{
		public override PacketType PacketType => PacketType.Server_InitialWorldState;
	
		public NetStringShort MiniGameName;
	
		public override int SerializeSize => MiniGameName.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(MiniGameName);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			MiniGameName = reader.ReadNetStringShort();
		}
	}
	
	public sealed class Server_SpawnEntities : PacketBase
	{
		public override PacketType PacketType => PacketType.Server_SpawnEntities;
	
		public NetArray<EntityPlayerState> SpawnedEntities = new NetArray<EntityPlayerState>();
	
		public override int SerializeSize => SpawnedEntities.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			SpawnedEntities.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			SpawnedEntities.Deserialize(reader);
		}
	}
	
	public sealed class Server_DespawnEntities : PacketBase
	{
		public override PacketType PacketType => PacketType.Server_DespawnEntities;
	
		public NetFixedArray<NetEntityID> DespawnEntities = new NetFixedArray<NetEntityID>();
	
		public override int SerializeSize => DespawnEntities.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			DespawnEntities.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			DespawnEntities.Deserialize(reader);
		}
	}
}