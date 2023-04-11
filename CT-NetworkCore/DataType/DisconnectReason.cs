namespace CT.Network.DataType
{
	public enum DisconnectReasonType
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
		AuthenticationTimeout,
		WrongPacket,

		ThereIsNoSuchGameInstance,
		GameInstanceIsAlreadyFull,
	}
}
