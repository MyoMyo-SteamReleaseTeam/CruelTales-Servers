#if UNITY_2021_3
using System.Collections.Generic;
#endif
using CT.Network.Serialization;
using CT.Packets;

namespace CT.Network.Packets
{
	public delegate PacketBase CreatePacket(PacketReader reader);

	public static class PacketFactory
	{
		private static Dictionary<PacketType, CreatePacket> _packetFactoryTable = new()
		{
			{ PacketType.Client_Req_TryJoinGameInstance, (r) => r.Read<Client_Req_TryJoinGameInstance>() },
			{ PacketType.Server_Ack_TryJoinGameInstance, (r) => r.Read<Server_Ack_TryJoinGameInstance>() },
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