using System.Collections.Generic;
using CT.Network.Serialization;
using CT.Packets;

namespace CTC.Networks.Packets
{
	public delegate void HandlePacket(PacketBase receivedPacket, ServerSession session);

	public static class PacketDispatcher
	{
		public static Dictionary<PacketType, HandlePacket> _dispatcherTable = new()
		{
			{ PacketType.Server_Ack_TryJoinGameInstance, PacketHandler.Handle_Server_Ack_TryJoinGameInstance },
			{ PacketType.Server_InitialWorldState, PacketHandler.Handle_Server_InitialWorldState },
			{ PacketType.Server_SpawnEntities, PacketHandler.Handle_Server_SpawnEntities },
			{ PacketType.Server_DespawnEntities, PacketHandler.Handle_Server_DespawnEntities },
			
		};

		public static void Dispatch(PacketBase receivedPacket, ServerSession session)
		{
			_dispatcherTable[receivedPacket.PacketType](receivedPacket, session);
		}
	}
}