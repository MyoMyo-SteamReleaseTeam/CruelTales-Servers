using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTS.Instance.Gameplay;

namespace CTS.Instance.Synchronizations
{
	/// <summary>
	/// 동기화 객체를 정의합니다.
	/// </summary>
	public interface IMasterSynchronizable : IDirtyable
	{
		/// <summary>
		/// 신뢰성 속성을 가진 동기화 요소를 직렬화합니다.
		/// </summary>
		public void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer);

		/// <summary>
		/// 비신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
		/// </summary>
		public void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer);

		/// <summary>
		/// 모든 동기화 요소를 직렬화합니다.
		/// </summary>
		public void SerializeEveryProperty(IPacketWriter writer);

		/// <summary>
		/// 신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
		/// 역직렬화 된 멤버는 고유한 이벤트를 발생시킵니다.
		/// </summary>
		public bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader);

		/// <summary>
		/// 비신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
		/// 역직렬화 된 멤버는 고유한 이벤트를 발생시킵니다.
		/// </summary>
		public bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader);

		/// <summary>
		/// Master 프로퍼티를 초기화합니다.
		/// </summary>
		public void InitializeMasterProperties();

		/// <summary>
		/// Remote 프로퍼티를 초기화합니다.
		/// </summary>
		public void InitializeRemoteProperties();

		/// <summary>
		/// 신뢰성 동기화를 무시합니다.
		/// </summary>
		public void IgnoreSyncReliable(IPacketReader reader);

		/// <summary>
		/// 비신뢰성 동기화를 무시합니다.
		/// </summary>
		public void IgnoreSyncUnreliable(IPacketReader reader);
	}
}
