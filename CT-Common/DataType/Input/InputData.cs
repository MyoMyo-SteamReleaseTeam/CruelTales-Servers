#if NET
using System.Numerics;
#elif UNITY_2021
using UnityEngine;
#endif
using CT.Common.Quantization;
using CT.Common.Serialization;

namespace CT.Common.DataType.Input
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
		public void Serialize(IPacketWriter writer) => writer.Put(Direction);
		public void Deserialize(IPacketReader reader) => Direction = reader.ReadByte();
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(byte));
	}

	public struct InputMovementData : IPacketSerializable
	{
		public MovementType MovementType;
		public ByteDirection Direction;

		public int SerializeSize => sizeof(MovementType) + Direction.SerializeSize;

		public void Serialize(IPacketWriter writer)
		{
			writer.PutMovementType(MovementType);
			writer.Put(Direction);
		}

		public void Deserialize(IPacketReader reader)
		{
			MovementType = reader.ReadMovementType();
			Direction = reader.Read<ByteDirection>();
		}

		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);

		public static void IgnoreStatic(IPacketReader reader)
		{
			reader.Ignore(sizeof(MovementType));
			ByteDirection.IgnoreStatic(reader);
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
		public static void PutMovementType(this IPacketWriter writer, MovementType movementType)
		{
			writer.Put((byte)movementType);
		}

		public static MovementType ReadMovementType(this IPacketReader reader)
		{
			return (MovementType)reader.ReadByte();
		}
	}
}
