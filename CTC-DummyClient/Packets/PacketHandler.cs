using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT.Common.Serialization;

namespace CTC.Networks
{
	public static class PacketHandler
	{
		internal static void Handle_SC_Ack_TryJoinGameInstance(PacketBase receivedPacket, ServerSession session)
		{
		}

		internal static void Handle_SC_ReliableSynchroniation(PacketReader reader, ServerSession session)
		{
			throw new NotImplementedException();
		}

		internal static void Handle_SC_UnreliableSynchroniation(PacketReader reader, ServerSession session)
		{
			throw new NotImplementedException();
		}
	}
}
