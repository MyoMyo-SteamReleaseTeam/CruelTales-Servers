//#if CT_SERVER

//using CT.Common.Serialization;

//namespace CT.Common.Synchronizations
//{
//	/// <summary>
//	/// 동기화 객체를 정의합니다. 코드 작성을 위해 복사된 코드입니다.
//	/// 클라이언트 측에서는 컴파일되지 않습니다.
//	/// </summary>
//	public interface IMasterSynchronizable
//	{
//		/// <summary>
//		/// 동기화가 필요한 신뢰성 요소가 존재하는지 여부입니다.
//		/// 일반적으로 내부 프로퍼티나 원격 함수가 호출되었을 때 true로 전환됩니다.
//		/// </summary>
//		public bool IsDirtyReliable { get; }

//		/// <summary>
//		/// 동기화가 필요한 비신뢰성 요소가 존재하는지 여부입니다.
//		/// 일반적으로 내부 프로퍼티나 원격 함수가 호출되었을 때 true로 전환됩니다.
//		/// </summary>
//		public bool IsDirtyUnreliable { get; }

//		/// <summary>
//		/// 신뢰성 속성을 가진 동기화 요소를 직렬화합니다.
//		/// </summary>
//		public void SerializeSyncReliable(INetworkPlayer player, IPacketWriter writer);

//		/// <summary>
//		/// 비신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
//		/// </summary>
//		public void SerializeSyncUnreliable(INetworkPlayer player, IPacketWriter writer);

//		/// <summary>
//		/// 모든 동기화 요소를 직렬화합니다.
//		/// </summary>
//		public void SerializeEveryProperty(IPacketWriter writer);

//		/// <summary>
//		/// 신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
//		/// 역직렬화 된 멤버는 고유한 이벤트를 발생시킵니다.
//		/// </summary>
//		public bool TryDeserializeSyncReliable(INetworkPlayer player, IPacketReader reader);

//		/// <summary>
//		/// 비신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
//		/// 역직렬화 된 멤버는 고유한 이벤트를 발생시킵니다.
//		/// </summary>
//		public bool TryDeserializeSyncUnreliable(INetworkPlayer player, IPacketReader reader);

//		/// <summary>
//		/// 신뢰성 Dirty Bits를 초기화합니다.
//		/// </summary>
//		public void ClearDirtyReliable();

//		/// <summary>
//		/// 비신뢰성 Dirty Bits를 초기화합니다.
//		/// </summary>
//		public void ClearDirtyUnreliable();
//	}
//}

//#endif