using CT.Common.Serialization;

namespace CT.Common.Synchronizations
{
	/// <summary>
	/// Master 동기화 객체를 대변하는 객체를 정의합니다.
	/// </summary>
	public interface IRemoteSynchronizable
	{
		/// <summary>
		/// 신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
		/// 역직렬화 된 멤버는 고유한 이벤트를 발생시킵니다.
		/// </summary>
		public void DeserializeSyncReliable(PacketReader reader);

		/// <summary>
		/// 비신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
		/// 역직렬화 된 멤버는 고유한 이벤트를 발생시킵니다.
		/// </summary>
		public void DeserializeSyncUnreliable(PacketReader reader);

		/// <summary>
		/// 모든 동기화 요소를 역직렬화합니다.
		/// 이벤트를 발생시키지 않습니다.
		/// Master 객체가 생성되었을 때 최초 1회 호출됩니다.
		/// </summary>
		public void DeserializeEveryProperty(PacketReader reader);
	}
}
