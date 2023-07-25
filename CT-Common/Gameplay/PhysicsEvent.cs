using System;
using System.Numerics;
using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	/// <summary>강체의 이벤트 타입입니다.</summary>
	[Flags]
	public enum PhysicsEventFlag : byte
	{
		None = 0,
		Position = 1,
		LinearVelocity = 1 << 1,
		ForceVelocity = 1 << 2,
		Rotation = 1 << 3,
	}

	public static class PhysicsEventFlagExtension
	{
		public static bool HasFlag(this PhysicsEventFlag value, PhysicsEventFlag flag)
		{
			return (value & flag) != 0;
		}
	}

	/// <summary>강체의 이벤트 정보입니다.</summary>
	public struct PhysicsEvent : IPacketSerializable
	{
		// Flags
		public PhysicsEventFlag EventFlags;

		// Move
		public Vector2 Direction;
		public float Amount;

		// MoveTo
		public Vector2 Position;

		// Velocity
		public Vector2 LinearVelocity;

		// Impluse
		public Vector2 ForceVelocity;

		// Rotate, RotateTo
		public float Rotation;

		public int SerializeSize => throw new System.NotImplementedException();

		public void Ignore(IPacketReader reader)
		{
			var eventFlags = (PhysicsEventFlag)reader.ReadByte();

			if (eventFlags.HasFlag(PhysicsEventFlag.Position))
				reader.Ignore(8);

			if (eventFlags.HasFlag(PhysicsEventFlag.LinearVelocity))
				reader.Ignore(8);

			if (eventFlags.HasFlag(PhysicsEventFlag.ForceVelocity))
				reader.Ignore(8);

			if (eventFlags.HasFlag(PhysicsEventFlag.Rotation))
				reader.Ignore(sizeof(float));

			throw new NotImplementedException();
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put((byte)EventFlags);

			if (EventFlags.HasFlag(PhysicsEventFlag.Position))
				writer.Put(Position);

			if (EventFlags.HasFlag(PhysicsEventFlag.LinearVelocity))
				writer.Put(LinearVelocity);

			if (EventFlags.HasFlag(PhysicsEventFlag.ForceVelocity))
				writer.Put(ForceVelocity);

			if (EventFlags.HasFlag(PhysicsEventFlag.Rotation))
				writer.Put(Rotation);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!reader.TryReadByte(out byte type)) return false;
			EventFlags = (PhysicsEventFlag)type;

			if (EventFlags.HasFlag(PhysicsEventFlag.Position))
			{
				if (!reader.TryReadVector2(out Position)) return false;
			}

			if (EventFlags.HasFlag(PhysicsEventFlag.LinearVelocity))
			{
				if (!reader.TryReadVector2(out LinearVelocity)) return false;
			}

			if (EventFlags.HasFlag(PhysicsEventFlag.ForceVelocity))
			{
				if (!reader.TryReadVector2(out ForceVelocity)) return false;
			}

			if (EventFlags.HasFlag(PhysicsEventFlag.Rotation))
			{
				if (!reader.TryReadSingle(out Rotation)) return false;
			}

			return true;
		}
	}
}