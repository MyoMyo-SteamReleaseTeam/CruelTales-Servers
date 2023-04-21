namespace CT.Common.DataType
{
	public enum UserSessionState
	{
		NoConnection = 0,
		WaitForJoinRequest,
		WaitForJoinGame,
		InGame,
	}
}
