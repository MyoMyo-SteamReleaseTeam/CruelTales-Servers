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
			_log.Info(nameof(Handle_SC_Sync_MasterMovement));
			receivedPacket.IgnoreAll();
			networkManager.GameSynchronizer.OnMasterMovement(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterSpawn(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			_log.Info(nameof(Handle_SC_Sync_MasterSpawn));
			receivedPacket.IgnoreAll();
			networkManager.GameSynchronizer.OnMasterSpawn(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterRespawn(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			_log.Info(nameof(Handle_SC_Sync_MasterRespawn));
			receivedPacket.IgnoreAll();
			networkManager.GameSynchronizer.OnMasterRespawn(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterDespawn(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			_log.Info(nameof(Handle_SC_Sync_MasterDespawn));
			receivedPacket.IgnoreAll();
			networkManager.GameSynchronizer.OnMasterDespawn(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterReliable(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			_log.Info(nameof(Handle_SC_Sync_MasterReliable));
			receivedPacket.IgnoreAll();
			networkManager.GameSynchronizer.OnMasterReliable(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterUnreliable(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			_log.Info(nameof(Handle_SC_Sync_MasterUnreliable));
			receivedPacket.IgnoreAll();
			networkManager.GameSynchronizer.OnMasterUnreliable(receivedPacket);
		}
	}
}
