namespace CT.Common.DataType
{
	public enum UserSessionState
	{
		/// <summary>
		/// 연결이 없는 상태입니다.
		/// </summary>
		NoConnection = 0,

		/// <summary>
		/// 연결 시도중입니다.<br/>
		/// Token과 같은 유효성 검사를 진행합니다.
		/// </summary>
		TryConnecting,
		
		/// <summary>
		/// 게임 입장 대기중입니다.
		/// </summary>
		TryEnterGameInstance,

		/// <summary>
		/// 동기화를 위해 준비중입니다.
		/// </summary>
		TryReadyToSync,

		/// <summary>
		/// 게임 플레이 중입니다.
		/// </summary>
		InGameplay,
	}
}
