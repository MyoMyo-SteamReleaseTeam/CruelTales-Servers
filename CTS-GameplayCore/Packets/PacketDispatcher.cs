using System.Collections.Generic;
using CT.Network.Core;
using CT.Network.Serialization;
using CT.Packets;

namespace CTS.Instance.Packets
{
	public delegate void HandlePacket(PacketBase receivedPacket, ClientSession session);

	public static class PacketDispatcher
	{
		public static Dictionary<PacketType, HandlePacket> _dispatcherTable = new()
		{
			{ PacketType.Client_Req_TryJoinMatching, PacketHandler.Handle_Client_Req_TryJoinMatching }
		};

		public static void Dispatch(PacketBase receivedPacket, ClientSession session)
		{
			_dispatcherTable[receivedPacket.PacketType](receivedPacket, session);
		}
	}
}
