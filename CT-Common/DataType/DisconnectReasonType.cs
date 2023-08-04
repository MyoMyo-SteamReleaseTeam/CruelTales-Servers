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

		// Client error
		ClientError_CannotHandlePacket,
		ClientError_FailedToReceivePacket,
		ClientError_WrongConnectionFlow,
		ClientError_NullReference,
		ClientError_WrongClientData,

		// Server error
		ServerError_CannotHandlePacket,
		ServerError_WrongOperation,
		ServerError_YouTryRejoin,
		ServerError_YouAreNotInTheWaitingQueue,
		ServerError_YouAreNotHost,
		ServerError_UnauthorizedBehaviour,

		// Reject from server
		Reject_ThereIsNoSuchGameInstance,
		Reject_GameInstanceIsAlreadyFull,
		Reject_WrongPassword,
		Reject_PermissionDenied,

		// Gameplay
		Client_GameEnd,
	}

	public static class DisconnectReasonTypeExtension
	{
		public static void PutDisconnectReasonType(this IPacketWriter writer,
												   DisconnectReasonType reasonType)
		{
			writer.Put((byte)reasonType);
		}

		public static DisconnectReasonType ReadDisconnectReasonType(this IPacketReader reader)
		{
			return (DisconnectReasonType)reader.ReadByte();
		}
	}
}
