﻿using System.Collections.Generic;
using CT.Network.Serialization;
using CT.Packets;
using CTS.Instance.Networks;

namespace CTS.Instance.Packets
{
	public delegate void HandlePacket(PacketBase receivedPacket, ClientSession session);

	public static class PacketDispatcher
	{
		public static Dictionary<PacketType, HandlePacket> _dispatcherTable = new()
		{
			{ PacketType.Client_Req_TryJoinGameInstance, PacketHandler.Handle_Client_Req_TryJoinGameInstance }
		};

		public static void Dispatch(PacketBase receivedPacket, ClientSession session)
		{
			_dispatcherTable[receivedPacket.PacketType](receivedPacket, session);
		}
	}
}
