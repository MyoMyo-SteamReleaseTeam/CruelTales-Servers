using CT.Packets;

namespace CT.Network.Serialization
{
	public delegate PacketBase CreatePacket(PacketReader reader);

	public static class PacketFactory
	{
		private static Dictionary<PacketType, CreatePacket> _packetFactoryTable = new()
		{
			{ PacketType.Client_Req_TryJoinMatching, (r) => r.Read<Client_Req_TryJoinMatching>() },
			{ PacketType.Server_Ack_TryJoinMatching, (r) => r.Read<Server_Ack_TryJoinMatching>() },
			{ PacketType.Server_InitialWorldState, (r) => r.Read<Server_InitialWorldState>() },
			{ PacketType.Server_SpawnEntities, (r) => r.Read<Server_SpawnEntities>() },
			{ PacketType.Server_DespawnEntities, (r) => r.Read<Server_DespawnEntities>() },
		};

		public static PacketBase Create(PacketType packetType, PacketReader reader)
		{
			return _packetFactoryTable[packetType](reader);
		}
	}
}