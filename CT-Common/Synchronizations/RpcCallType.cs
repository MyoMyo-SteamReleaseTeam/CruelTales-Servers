namespace CT.Common.Synchronizations
{
	/// <summary>
	/// 원격 함수 호출 타입입니다.
	/// </summary>
	public enum RpcCallType
	{
		None = 0,

		/// <summary>
		/// 모든 유저에게 호출됩니다.
		/// </summary>
		Broadcast,

		/// <summary>
		/// 특정 유저에게 호출합니다.
		/// Remote에서는 사용할 수 없습니다.
		/// </summary>
		Target,
	}
}
