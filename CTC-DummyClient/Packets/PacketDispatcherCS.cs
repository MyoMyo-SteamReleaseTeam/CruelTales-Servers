/*
 * Generated File : PacketDispatcher.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using System.Collections.Generic;
using CT.Packets;
using CT.Common.Serialization;

namespace CTC.Networks.Packets
{
	public delegate void HandlePacket(PacketBase receivedPacket, NetworkManager networkManager);
	public delegate void HandlePacketRaw(PacketReader receivedPacket, NetworkManager networkManager);

	public static class PacketDispatcher
	{
		private static Dictionary<PacketType, HandlePacket> _dispatcherTable = new()
		{
			{ PacketType.SC_Ack_TryEnterGameInstance, PacketHandler.Handle_SC_Ack_TryEnterGameInstance },
			
		};

		private static Dictionary<PacketType, HandlePacketRaw> _dispatcherRawTable = new()
		{
			{ PacketType.SC_Sync_MasterLifeCycle, PacketHandler.Handle_SC_Sync_MasterLifeCycle },
			{ PacketType.SC_Sync_MasterReliable, PacketHandler.Handle_SC_Sync_MasterReliable },
			{ PacketType.SC_Sync_MasterUnreliable, PacketHandler.Handle_SC_Sync_MasterUnreliable },
			
		};

		private static HashSet<PacketType> _customPacketSet = new()
		{
			PacketType.SC_Sync_MasterLifeCycle,
			PacketType.SC_Sync_MasterReliable,
			PacketType.SC_Sync_MasterUnreliable,
			
		};

		public static void Dispatch(PacketBase receivedPacket, NetworkManager networkManager)
		{
			_dispatcherTable[receivedPacket.PacketType](receivedPacket, networkManager);
		}

		public static void DispatchRaw(PacketType packetType, PacketReader receivedPacket, NetworkManager networkManager)
		{
			_dispatcherRawTable[packetType](receivedPacket, networkManager);
		}

		public static bool IsCustomPacket(PacketType packetType)
		{
			return _customPacketSet.Contains(packetType);
		}
	}

}