#define CHECK_PACKET_SIZE

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

		internal static void Handle_SC_Sync_MasterMovement(IPacketReader receivedPacket, NetworkManager networkManager)
		{
#if CHECK_PACKET_SIZE
			_log.Info($"{nameof(Handle_SC_Sync_MasterMovement)} [Size:{receivedPacket.Size}]");
#endif
			networkManager.GameSynchronizer.OnMasterMovement(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterSpawn(IPacketReader receivedPacket, NetworkManager networkManager)
		{
#if CHECK_PACKET_SIZE
			_log.Info($"{nameof(Handle_SC_Sync_MasterSpawn)} [Size:{receivedPacket.Size}]");
#endif
			networkManager.GameSynchronizer.OnMasterSpawn(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterRespawn(IPacketReader receivedPacket, NetworkManager networkManager)
		{
#if CHECK_PACKET_SIZE
			_log.Info($"{nameof(Handle_SC_Sync_MasterRespawn)} [Size:{receivedPacket.Size}]");
#endif
			networkManager.GameSynchronizer.OnMasterRespawn(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterDespawn(IPacketReader receivedPacket, NetworkManager networkManager)
		{
#if CHECK_PACKET_SIZE
			_log.Info($"{nameof(Handle_SC_Sync_MasterDespawn)} [Size:{receivedPacket.Size}]");
#endif
			networkManager.GameSynchronizer.OnMasterDespawn(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterReliable(IPacketReader receivedPacket, NetworkManager networkManager)
		{
#if CHECK_PACKET_SIZE
			_log.Info($"{nameof(Handle_SC_Sync_MasterReliable)} [Size:{receivedPacket.Size}]");
#endif
			networkManager.GameSynchronizer.OnMasterReliable(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterUnreliable(IPacketReader receivedPacket, NetworkManager networkManager)
		{
#if CHECK_PACKET_SIZE
			_log.Info($"{nameof(Handle_SC_Sync_MasterUnreliable)} [Size:{receivedPacket.Size}]");
#endif
			networkManager.GameSynchronizer.OnMasterUnreliable(receivedPacket);
		}
	}
}
