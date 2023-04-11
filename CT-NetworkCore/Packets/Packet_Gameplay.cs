/*
 * Generated File : Packet_Gameplay.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

/*
 * Generated File : Packet_Gameplay.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using CT.Network.DataType;
using CT.Network.Serialization;
using CT.Network.Serialization.Type;

namespace CT.Packets
{
	public sealed class SC_InitialWorldState : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_InitialWorldState;
	
		public NetStringShort MiniGameName;
	
		public override int SerializeSize => MiniGameName.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			writer.Put(MiniGameName);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			MiniGameName = reader.ReadNetStringShort();
		}
	}
	
	public sealed class SC_SpawnEntities : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_SpawnEntities;
	
		public NetArray<EntityPlayerState> SpawnedEntities = new NetArray<EntityPlayerState>();
	
		public override int SerializeSize => SpawnedEntities.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			SpawnedEntities.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			SpawnedEntities.Deserialize(reader);
		}
	}
	
	public sealed class SC_DespawnEntities : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_DespawnEntities;
	
		public NetFixedArray<NetEntityID> DespawnEntities = new NetFixedArray<NetEntityID>();
	
		public override int SerializeSize => DespawnEntities.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			DespawnEntities.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			DespawnEntities.Deserialize(reader);
		}
	}
}