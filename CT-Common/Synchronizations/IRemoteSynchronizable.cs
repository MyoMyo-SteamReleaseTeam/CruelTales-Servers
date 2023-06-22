﻿using CT.Common.Serialization;

namespace CT.Common.Synchronizations
{
	/// <summary>
	/// 동기화 객체를 정의합니다.
	/// </summary>
	public interface IRemoteSynchronizable
	{
		/// <summary>
		/// 동기화가 필요한 신뢰성 요소가 존재하는지 여부입니다.
		/// 일반적으로 내부 프로퍼티나 원격 함수가 호출되었을 때 true로 전환됩니다.
		/// </summary>
		public bool IsDirtyReliable { get; }

		/// <summary>
		/// 동기화가 필요한 비신뢰성 요소가 존재하는지 여부입니다.
		/// 일반적으로 내부 프로퍼티나 원격 함수가 호출되었을 때 true로 전환됩니다.
		/// </summary>
		public bool IsDirtyUnreliable { get; }

		/// <summary>
		/// 신뢰성 속성을 가진 동기화 요소를 직렬화합니다.
		/// </summary>
		public void SerializeSyncReliable(IPacketWriter writer);

		/// <summary>
		/// 비신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
		/// </summary>
		public void SerializeSyncUnreliable(IPacketWriter writer);

		/// <summary>
		/// 모든 동기화 요소를 직렬화합니다.
		/// </summary>
		public void SerializeEveryProperty(IPacketWriter writer);

		/// <summary>
		/// 신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
		/// 역직렬화 된 멤버는 고유한 이벤트를 발생시킵니다.
		/// </summary>
		public bool TryDeserializeSyncReliable(IPacketReader reader);

		/// <summary>
		/// 비신뢰성 속성을 가진 동기화 요소를 역직렬화합니다.
		/// 역직렬화 된 멤버는 고유한 이벤트를 발생시킵니다.
		/// </summary>
		public bool TryDeserializeSyncUnreliable(IPacketReader reader);

		/// <summary>
		/// 모든 동기화 요소를 역직렬화합니다.
		/// 이벤트를 발생시키지 않습니다.
		/// Master 객체가 생성되었을 때 최초 1회 호출됩니다.
		/// </summary>
		public bool TryDeserializeEveryProperty(IPacketReader reader);

		/// <summary>
		/// 신뢰성 Dirty Bits를 초기화합니다.
		/// </summary>
		public void ClearDirtyReliable();

		/// <summary>
		/// 비신뢰성 Dirty Bits를 초기화합니다.
		/// </summary>
		public void ClearDirtyUnreliable();
	}

	public interface INetworkSynchronizable
	{

	}
}