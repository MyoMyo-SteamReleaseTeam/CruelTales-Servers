namespace CT.Common.DataType
{
	public enum ClientSessionState
	{
		NoConnection = 0,
		WaitForJoinRequest,
		WaitForJoinGame,
		InGame,
	}
}
