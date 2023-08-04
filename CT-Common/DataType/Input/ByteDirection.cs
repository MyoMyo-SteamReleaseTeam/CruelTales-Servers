using System.Numerics;
using CT.Common.Quantization;
using CT.Common.Serialization;

namespace CT.Common.DataType.Input
{
	/// <summary>1 바이트로 양자화된 방향 벡터입니다.</summary>
	public struct ByteDirection : IPacketSerializable
	{
		public byte RawData;
		public int SerializeSize => sizeof(byte);

		public ByteDirection(byte rawData)
		{
			RawData = rawData;
		}

		public ByteDirection(Vector2 direction)
		{
			RawData = Quantizer.Vector2ToByte(direction);
		}

		public void SetDirection(Vector2 direction)
		{
			RawData = Quantizer.Vector2ToByte(direction);
		}

		public Vector2 GetDirection()
		{
			return Quantizer.RadByteToVec2(RawData);
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(RawData);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!reader.TryReadByte(out var value))
				return false;

			RawData = value;
			return true;
		}

		public void Ignore(IPacketReader reader)
		{
			IgnoreStatic(reader);
		}

		public static void IgnoreStatic(IPacketReader reader)
		{
			reader.Ignore(sizeof(byte));
		}
	}
}
