using System;

namespace CT.Common.Serialization
{
	/// <summary>직/역직렬화 인터페이스입니다.</summary>
	public interface IPacketSerializable
	{
		public int SerializeSize { get; }
		public void Serialize(IPacketWriter writer);
		public bool TryDeserialize(IPacketReader reader);
		public void Ignore(IPacketReader reader);
	}
}
