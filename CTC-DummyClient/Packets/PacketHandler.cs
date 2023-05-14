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

		internal static void Handle_SC_Ack_TryEnterGameInstance(PacketBase receivedPacket, NetworkManager networkManager)
		{
			networkManager.ReqTryReadyToSync();
		}

		internal static void Handle_SC_Sync_LifeCycle(PacketReader reader, NetworkManager networkManager)
		{
			//Console.WriteLine("Initialize");
			networkManager.GameSynchronizer.OnSyncInitialize(reader);
		}

		internal static void Handle_SC_Sync_MasterLifeCycle(PacketReader receivedPacket, NetworkManager networkManager)
		{
			throw new NotImplementedException();
		}

		internal static void Handle_SC_Sync_MasterReliable(PacketReader receivedPacket, NetworkManager networkManager)
		{
			throw new NotImplementedException();
		}

		internal static void Handle_SC_Sync_MasterUnreliable(PacketReader receivedPacket, NetworkManager networkManager)
		{
			throw new NotImplementedException();
		}

		internal static void Handle_SC_Sync_Reliable(PacketReader reader, NetworkManager networkManager)
		{
			_log.Info("Reliable");
			networkManager.GameSynchronizer.OnDeserializeReliable(reader);
		}
			
		internal static void Handle_SC_Sync_Unreliable(PacketReader reader, NetworkManager networkManager)
		{
			//Console.WriteLine("Unreliable");
			networkManager.GameSynchronizer.OnDeserializeUnreliable(reader);
		}
	}
}
