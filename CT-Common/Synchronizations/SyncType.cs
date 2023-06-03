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
		ReliableOrUnreliable,

		/// <summary>
		/// 목표 대상이 있는 신뢰성 RPC 타입입니다.
		/// RPC 에서만 부여할 수 있습니다.
		/// </summary>
		ReliableTarget,

		/// <summary>
		/// 목표 대상이 있는 비신뢰성 RPC 타입입니다.
		/// RPC 에서만 부여할 수 있습니다.
		/// </summary>
		UnreliableTarget,

		/// <summary>
		/// Cold data 여부입니다. Cold 데이터는 최초 1회 Spawn 및 Enter에서만 동기화되고 이후 동기화되지 않습니다.<br/>
		/// Cold 데이터는 최초 1회 동기화 된 뒤 RPC나 다른 값 동기화를 통해 간접적으로 시뮬레이션 되는 대상에 적합합니다.
		/// </summary>
		ColdData,
	}
}
