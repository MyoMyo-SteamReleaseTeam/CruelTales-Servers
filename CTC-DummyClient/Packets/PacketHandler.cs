using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT.Common.Serialization;
using log4net;

namespace CTC.Networks
{
	public static class PacketHandler
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(PacketHandler));

		internal static void Handle_SC_Ack_TryEnterGameInstance(PacketBase receivedPacket, ServerSession session)
		{
			session.ReqTryReadyToSync();
		}

		internal static void Handle_SC_Sync_LifeCycle(PacketReader reader, ServerSession session)
		{
			Console.WriteLine("Initialize");
			session.GameManager.OnSyncInitialize(reader);
		}

		internal static void Handle_SC_Sync_Reliable(PacketReader reader, ServerSession session)
		{
			Console.WriteLine("Reliable");
			session.GameManager.OnDeserializeReliable(reader);
		}
			
		internal static void Handle_SC_Sync_Unreliable(PacketReader reader, ServerSession session)
		{
			Console.WriteLine("Unreliable");
			session.GameManager.OnDeserializeUnreliable(reader);
		}
	}
}
