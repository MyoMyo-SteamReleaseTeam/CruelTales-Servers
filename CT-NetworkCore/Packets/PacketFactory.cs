/*
 * Generated File : PacketFactory.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using System.Collections.Generic;
using CT.Network.Serialization;
using CT.Packets;

namespace CT.Network.Packets
{
	public delegate PacketBase CreatePacket(PacketReader reader);

	public static class PacketFactory
	{
		private static Dictionary<PacketType, CreatePacket> _packetFactoryTable = new()
		{
			{ PacketType.CS_Req_TryJoinGameInstance, (r) => r.Read<CS_Req_TryJoinGameInstance>() },
			{ PacketType.SC_Ack_TryJoinGameInstance, (r) => r.Read<SC_Ack_TryJoinGameInstance>() },
			{ PacketType.SC_InitialWorldState, (r) => r.Read<SC_InitialWorldState>() },
			{ PacketType.SC_SpawnEntities, (r) => r.Read<SC_SpawnEntities>() },
			{ PacketType.SC_DespawnEntities, (r) => r.Read<SC_DespawnEntities>() },
			{ PacketType.CS_Req_PlayerInput_Movement, (r) => r.Read<CS_Req_PlayerInput_Movement>() },
			{ PacketType.CS_Req_PlayerInput_Action, (r) => r.Read<CS_Req_PlayerInput_Action>() },
			
		};

		public static PacketBase Create(PacketType packetType, PacketReader reader)
		{
			return _packetFactoryTable[packetType](reader);
		}
	}
}