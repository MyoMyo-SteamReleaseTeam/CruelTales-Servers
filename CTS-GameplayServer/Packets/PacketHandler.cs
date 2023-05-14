﻿using System;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Packets;
using CTS.Instance.Networks;
using log4net;

namespace CTS.Instance.Packets
{
	public static class PacketHandler
	{
		public static ILog _log = LogManager.GetLogger(typeof(PacketHandler));

		internal static void Handle_CS_Req_TryEnterGameInstance(PacketBase receivedPacket, UserSession session)
		{
			var packet = (CS_Req_TryEnterGameInstance)receivedPacket;
			session.OnReqTryEnterGameInstance(packet.UserDataInfo, packet.Token, packet.MatchTo);
		}

		internal static void Handle_CS_Req_ReadyToSync(PacketBase receivedPacket, UserSession session)
		{
			var packet = (CS_Req_ReadyToSync)receivedPacket;
			session.OnReqReadyToEnter();
		}

		internal static void Handle_CS_Sync_RemoteReliable(PacketReader receivedPacket, UserSession session)
		{
			session.OnTrySync(SyncOperation.Reliable, receivedPacket);
		}

		internal static void Handle_CS_Sync_RemoteUnreliable(PacketReader receivedPacket, UserSession session)
		{
			session.OnTrySync(SyncOperation.Unreliable, receivedPacket);
		}
	}
}
