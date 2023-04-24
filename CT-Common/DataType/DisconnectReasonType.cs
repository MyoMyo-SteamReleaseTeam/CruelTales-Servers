using CT.Common.Serialization;

namespace CT.Common.DataType
{
	public enum DisconnectReasonType : byte
	{
		None = 0,

		// LiteNetLib's DisconnectReason
		ConnectionFailed,
		Timeout,
		HostUnreachable,
		NetworkUnreachable,
		RemoteConnectionClose,
		DisconnectPeerCalled,
		ConnectionRejected,
		InvalidProtocol,
		UnknownHost,
		Reconnect,
		PeerToPeerConnection,

		// CT Network's DisconnectReason
		Unknown,

		// Authentication Timeout
		Timeout_Authentication,
		Timeout_FailedToEnterGameInstance,
		Timeout_FailedToReadyToSync,
		Timeout_GameCannotHandleYourSession,


		WrongPacket,
		WrongOperation,

		ThereIsNoSuchGameInstance,
		GameInstanceIsAlreadyFull,
	}

	public static class DisconnectReasonTypeExtension
	{
		public static void PutDisconnectReasonType(this PacketWriter writer,
												   DisconnectReasonType reasonType)
		{
			writer.Put((byte)reasonType);
		}

		public static DisconnectReasonType ReadDisconnectReasonType(this PacketReader reader)
		{
			return (DisconnectReasonType)reader.ReadByte();
		}
	}
}
