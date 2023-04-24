namespace CT.Common.Synchronizations
{
	/// <summary>
	/// 동기화 흐름 방향입니다.
	/// </summary>
	public enum SyncDirection
	{
		None = 0,

		/// <summary>
		/// Master에서 Remote로 전송합니다.
		/// Server에서 Client로 동기화합니다.
		/// </summary>
		FromMaster,

		/// <summary>
		/// Remote에서 Master로 전송합니다.
		/// Client에서 Server로 동기화합니다.
		/// </summary>
		FromRemote,
	}
}
