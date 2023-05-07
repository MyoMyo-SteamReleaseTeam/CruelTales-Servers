namespace CT.Common.Serialization
{
	/// <summary>
	/// 직/역직렬화 인터페이스입니다.
	/// 구조체가 직렬화 인터페이스를 상속받는 경우<br/>PacketReader의
	/// 인스턴스를 인자로 하는 void 반환형 Ignore 전역 함수를 제작해야합니다.
	/// </summary>
	public interface IPacketSerializable
	{
		public int SerializeSize { get; }
		public void Serialize(PacketWriter writer);
		public void Deserialize(PacketReader reader);
		public abstract static void Ignore(PacketReader reader);
	}
}
