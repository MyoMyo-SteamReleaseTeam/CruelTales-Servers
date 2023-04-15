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

		public static void Handle_CS_Req_TryJoinGameInstance(PacketBase receivedPacket, ClientSession session)
		{
			var packet = (CS_Req_TryJoinGameInstance)receivedPacket;
			_log.Info($"Receive {nameof(CS_Req_TryJoinGameInstance)} {packet.Id} {packet.Token} {packet.MatchTo}");

			session.OnReqTryJoinGameInstance(packet.Id, packet.Token, packet.MatchTo);
		}

		internal static void Handle_CS_Req_PlayerInput_Action(PacketBase receivedPacket, ClientSession session)
		{
		}

		internal static void Handle_CS_Req_PlayerInput_Movement(PacketBase receivedPacket, ClientSession session)
		{
		}
	}
}
