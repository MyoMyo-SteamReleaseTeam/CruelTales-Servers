using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using CT.Common.Serialization;
using KaNet.Physics.RigidBodies;

namespace CT.Common.Gameplay
{
	/// <summary>강체의 이벤트 타입입니다.</summary>
	public enum RigidBodyEventType : byte
	{
		None = 0,
		Move = 1,
		MoveTo = 1 << 1,
		Impluse = 1 << 2,
		Velocity = 1 << 3,
		Rotate = 1 << 4,
		RotateTo = 1 << 5,
	}

	/// <summary>강체의 이벤트 정보입니다.</summary>
	public struct RigidBodyEvent : IPacketSerializable
	{
		public RigidBodyEventType Type;

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
			reader.Ignore(sizeof(byte));

			switch (Type)
			{
				case RigidBodyEventType.Move:
				case RigidBodyEventType.MoveTo:
					reader.Ignore(8);
					break;

				case RigidBodyEventType.Impluse:
					reader.Ignore(8);
					reader.Ignore(8);
					break;

				case RigidBodyEventType.Velocity:
					reader.Ignore(8);
					reader.Ignore(8);
					break;

				case RigidBodyEventType.Rotate:
				case RigidBodyEventType.RotateTo:
					reader.Ignore(sizeof(float));
					break;
			}

			throw new NotImplementedException();
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put((byte)Type);

			switch (Type)
			{
				case RigidBodyEventType.Move:
				case RigidBodyEventType.MoveTo:
					writer.Put(Position);
					break;

				case RigidBodyEventType.Impluse:
					writer.Put(Position);
					writer.Put(ForceVelocity);
					break;

				case RigidBodyEventType.Velocity:
					writer.Put(Position);
					writer.Put(LinearVelocity);
					break;

				case RigidBodyEventType.Rotate:
				case RigidBodyEventType.RotateTo:
					writer.Put(Rotation);
					break;
			}
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!reader.TryReadByte(out byte type)) return false;
			Type = (RigidBodyEventType)type;

			switch (Type)
			{
				case RigidBodyEventType.Move:
				case RigidBodyEventType.MoveTo:
					if (!reader.TryReadVector2(out Position)) return false;
					break;

				case RigidBodyEventType.Impluse:
					if (!reader.TryReadVector2(out Position)) return false;
					if (!reader.TryReadVector2(out ForceVelocity)) return false;
					break;

				case RigidBodyEventType.Velocity:
					if (!reader.TryReadVector2(out Position)) return false;
					if (!reader.TryReadVector2(out LinearVelocity)) return false;
					break;

				case RigidBodyEventType.Rotate:
				case RigidBodyEventType.RotateTo:
					if (!reader.TryReadSingle(out Rotation)) return false;
					break;
			}

			return true;
		}
	}

	// TODO : 초기화 구문 최적화
	public class NetRigidBody
	{
		private readonly KaRigidBody _rigidBody;
		public bool IsDirty => Events.Count != 0;
		public Vector2 Position => _rigidBody.Position;

		public readonly List<RigidBodyEvent> Events = new(4);
		public readonly List<RigidBodyEvent> ReceivedEvents = new(4);

		public NetRigidBody(KaRigidBody rigidBody)
		{
			_rigidBody = rigidBody;
		}

#if CT_SERVER
		public void SerializeInitialSyncData(IPacketWriter writer)
		{
			writer.Put(_rigidBody.Position);
			writer.Put(_rigidBody.LinearVelocity);
			writer.Put(_rigidBody.ForceFriction);
		}
#endif

		public void DeserializeInitialSyncData(IPacketReader reader)
		{
			_rigidBody.MoveTo(reader.ReadVector2());
			_rigidBody.LinearVelocity = reader.ReadVector2();
			_rigidBody.ForceVelocity = reader.ReadVector2();
		}

#if CT_SERVER
		public void SerializeEventSyncData(IPacketWriter writer)
		{
			byte count = (byte)Events.Count;
			writer.Put(count);
			for (int i = 0; i < count; i++)
			{
				writer.Put(Events[i]);
			}
		}
#endif

		public bool TryDeserializeEventSyncData(IPacketReader reader)
		{
			if (!reader.TryReadByte(out var count)) return false;

			for (int i = 0; i < count; i++)
			{
				RigidBodyEvent r = new();
				if (!r.TryDeserialize(reader)) return false;
				ReceivedEvents.Add(r);
			}

			return true;
		}

		public void Reset()
		{
			_rigidBody.MoveTo(Vector2.Zero);
			_rigidBody.RotateTo(0);
			_rigidBody.LinearVelocity = Vector2.Zero;
			_rigidBody.ForceVelocity = Vector2.Zero;
		}

		public void ClearDirty()
		{
			Events.Clear();
		}

		public void SynchronizeByEvent()
		{
			foreach (var r in ReceivedEvents)
			{
				switch (r.Type)
				{
					case RigidBodyEventType.Move:
						_rigidBody.Move(r.Direction, r.Amount);
						break;

					case RigidBodyEventType.MoveTo:
						_rigidBody.MoveTo(r.Position);
						break;

					case RigidBodyEventType.Impluse:
						_rigidBody.ForceVelocity = r.ForceVelocity;
						break;

					case RigidBodyEventType.Velocity:
						_rigidBody.LinearVelocity = r.LinearVelocity;
						break;

					case RigidBodyEventType.Rotate:
						_rigidBody.Rotate(r.Rotation);
						break;

					case RigidBodyEventType.RotateTo:
						_rigidBody.RotateTo(r.Rotation);
						break;
				}
			}
		}

		/// <summary>해당 위치로 이동합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MoveTo(Vector2 position)
		{
			_rigidBody.MoveTo(position);

			RigidBodyEvent r = new();
			r.Type = RigidBodyEventType.MoveTo;
			r.Position = position;
			Events.Add(r);
		}

		/// <summary>각도와 거리만큼 상대적으로 이동합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Move(Vector2 direction, float distance)
		{
			_rigidBody.Move(direction, distance);

			RigidBodyEvent r = new();
			r.Type = RigidBodyEventType.Move;
			r.Position = _rigidBody.Position;
			Events.Add(r);
		}

		/// <summary>순간적인 힘을 가합니다.</summary>
		public void Impulse(Vector2 direction, float power)
		{
			_rigidBody.ForceVelocity = direction * power;

			RigidBodyEvent r = new();
			r.Type = RigidBodyEventType.Impluse;
			r.ForceVelocity = _rigidBody.ForceVelocity;
			Events.Add(r);
		}

		/// <summary>순간적인 힘을 가합니다.</summary>
		public void Impulse(Vector2 impluseVelocity)
		{
			_rigidBody.ForceVelocity = impluseVelocity;

			RigidBodyEvent r = new();
			r.Type = RigidBodyEventType.Impluse;
			r.ForceVelocity = _rigidBody.ForceVelocity;
			Events.Add(r);
		}

		/// <summary>가속도를 바꿉니다.</summary>
		public void ChangeVelocity(Vector2 linearVelocity)
		{
			_rigidBody.LinearVelocity = linearVelocity;

			RigidBodyEvent r = new();
			r.Type = RigidBodyEventType.Velocity;
			r.LinearVelocity = _rigidBody.LinearVelocity;
			Events.Add(r);
		}

		/// <summary>해당 각도로 회전합니다.</summary>
		public void RotateTo(float rotation)
		{
			_rigidBody.RotateTo(rotation);

			RigidBodyEvent r = new();
			r.Type = RigidBodyEventType.RotateTo;
			r.Rotation = _rigidBody.Rotation;
			Events.Add(r);
		}

		/// <summary>각도만큼 상대적으로 회전합니다.</summary>
		public void Rotate(float rotation)
		{
			_rigidBody.Rotate(rotation);

			RigidBodyEvent r = new();
			r.Type = RigidBodyEventType.Rotate;
			r.Rotation = _rigidBody.Rotation;
			Events.Add(r);
		}
	}
}