using CT.Network.DataType;
using CT.Network.Serialization;
using CT.Network.Serialization.Type;

namespace CT.Packets
{
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