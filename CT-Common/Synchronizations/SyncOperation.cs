namespace CT.Common.Synchronizations
{
	/// <summary>
	/// 동기화 작업 타입입니다.
	/// </summary>
	public enum SyncOperation
	{
		None = 0,

		/// <summary>
		/// 신뢰성 객체를 동기화합니다.
		/// </summary>
		Reliable,

		/// <summary>
		/// 비신뢰성 객체를 동기화합니다.
		/// </summary>
		Unreliable,

		/// <summary>
		/// 객체 생성을 동기화합니다.
		/// </summary>
		Creation,

		/// <summary>
		/// 객체 소멸을 동기화합니다.
		/// </summary>
		Destruction,
	}
}
