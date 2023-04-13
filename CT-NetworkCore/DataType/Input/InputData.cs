#if NET7_0_OR_GREATER
using System.Numerics;
using CT.Network.Quantization;
#endif

using CT.Network.Serialization;

namespace CT.Network.DataType.Input
{
	public enum InputType : byte
	{
		None = 0,
		Movement,
		Interaction,
		Action,
	}

	public struct ByteDirection : IPacketSerializable
	{
		public byte Direction;

		public ByteDirection(Vector2 direction) => Direction = Quantizer.Vec2ToRadByte(direction);
		public Vector2 GetDirectionVector2() => Quantizer.RadByteToVec2(Direction);
		public int SerializeSize => sizeof(byte);
		public void Serialize(PacketWriter writer) => writer.Put(Direction);
		public void Deserialize(PacketReader reader) => Direction = reader.ReadByte();
	}

	public struct InputMovementData : IPacketSerializable
	{
		public MovementType MovementType;
		public ByteDirection Direction;

		public int SerializeSize => sizeof(MovementType) + Direction.SerializeSize;

		public void Serialize(PacketWriter writer)
		{
			writer.PutMovementType(MovementType);
			writer.Put(Direction);
		}

		public void Deserialize(PacketReader reader)
		{
			MovementType = reader.ReadMovementType();
			Direction = reader.Read<ByteDirection>();
		}
	}

	public enum MovementType : byte
	{
		None = 0,
		Walk,
		Run,
		Stop,
	}

	public static class MovementTypeExtension
	{
		public static void PutMovementType(this PacketWriter writer, MovementType movementType)
		{
			writer.Put((byte)movementType);
		}

		public static MovementType ReadMovementType(this PacketReader reader)
		{
			return (MovementType)reader.ReadByte();
		}
	}
}
