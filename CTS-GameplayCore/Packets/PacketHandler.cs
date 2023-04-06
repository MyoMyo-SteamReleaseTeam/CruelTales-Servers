using CT.Network.Core;
using CT.Network.Serialization;
using CT.Packets;
using CTS.Instance.Networks;
using log4net;

namespace CTS.Instance.Packets
{
	public static class PacketHandler
	{
		public static ILog _log = LogManager.GetLogger(typeof(PacketHandler));

		public static void Handle_Client_Req_TryJoinMatching(PacketBase receivedPacket, ClientSession session)
		{
			var packet = (Client_Req_TryJoinMatching)receivedPacket;
			_log.Info($"Receive {nameof(Client_Req_TryJoinMatching)} {packet.Id} {packet.Token} {packet.MatchTo}");

			session.WaitForMatch(packet.Id, packet.Token, packet.MatchTo);
		}
	}
}
