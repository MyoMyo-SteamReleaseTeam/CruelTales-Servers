namespace CT.Common.Synchronizations
{
	/// <summary>
	/// 동기화 타입입니다. 데이터의 동기화 신뢰성을 결정합니다.
	/// </summary>
	public enum SyncType
	{
		None = 0,

		/// <summary>
		/// 신뢰성 있는 동기화 타입입니다.
		/// 전달을 보장합니다.
		/// 동기화 객체 내부적으로 신뢰성 속성만 존재하는 경우를 나타냅니다.
		/// </summary>
		Reliable,

		/// <summary>
		/// 비신뢰성 동기화 타입입니다.
		/// 전달을 보장하지 않습니다.
		/// 동기화 객체 내부적으로 비신뢰성 속성만 존재하는 경우를 나타냅니다.
		/// </summary>
		Unreliable,

		/// <summary>
		/// 중첩되는 신뢰성 속성을 가진 동기화 타입입니다.
		/// 동기화 객체 내부적으로 신뢰성 속성과 비신뢰성 속성이 함께 존재하는 경우를 나타냅니다.
		/// </summary>
		RelibaleOrUnreliable,
	}
}
