using System;
using CT.Common.Serialization;
using CT.Packets;
using CTS.Instance.Networks;
using log4net;

namespace CTS.Instance.Packets
{
	public static class PacketHandler
	{
		public static ILog _log = LogManager.GetLogger(typeof(PacketHandler));

		internal static void Handle_CS_Req_TryJoinGameInstance(PacketBase receivedPacket, ClientSession session)
		{
			var packet = (CS_Req_TryJoinGameInstance)receivedPacket;
			session.OnReqTryJoinGameInstance(packet.Id, packet.Token, packet.MatchTo, packet.Username);
		}

		internal static void Handle_CS_Req_PlayerInput_Action(PacketBase receivedPacket, ClientSession session)
		{
		}

		internal static void Handle_CS_Req_PlayerInput_Movement(PacketBase receivedPacket, ClientSession session)
		{
		}
	}
}
