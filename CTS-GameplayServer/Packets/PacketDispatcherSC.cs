/*
 * Generated File : PacketDispatcher.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using System.Collections.Generic;
using CT.Packets;
using CT.Common.Serialization;
using CTS.Instance.Networks;

namespace CTS.Instance.Packets
{
	public delegate void HandlePacket(PacketBase receivedPacket, UserSession session);
	public delegate void HandlePacketRaw(PacketReader receivedPacket, UserSession session);

	public static class PacketDispatcher
	{
		public static Dictionary<PacketType, HandlePacket> _dispatcherTable = new()
		{
			{ PacketType.CS_Req_TryEnterGameInstance, PacketHandler.Handle_CS_Req_TryEnterGameInstance },
			{ PacketType.CS_Req_ReadyToSync, PacketHandler.Handle_CS_Req_ReadyToSync },
			
		};

		private static Dictionary<PacketType, HandlePacketRaw> _dispatcherRawTable = new()
		{
			{ PacketType.CS_Sync_RemoteUnreliable, PacketHandler.Handle_CS_Sync_RemoteUnreliable },
			{ PacketType.CS_Sync_RemoteReliable, PacketHandler.Handle_CS_Sync_RemoteReliable },
			
		};

		private static HashSet<PacketType> _customPacketSet = new()
		{
			PacketType.CS_Sync_RemoteUnreliable,
			PacketType.CS_Sync_RemoteReliable,
			
		};

		public static void Dispatch(PacketBase receivedPacket, UserSession session)
		{
			_dispatcherTable[receivedPacket.PacketType](receivedPacket, session);
		}

		public static void DispatchRaw(PacketType packetType, PacketReader receivedPacket, UserSession session)
		{
			_dispatcherRawTable[packetType](receivedPacket, session);
		}

		public static bool IsCustomPacket(PacketType packetType)
		{
			return _customPacketSet.Contains(packetType);
		}
	}
}