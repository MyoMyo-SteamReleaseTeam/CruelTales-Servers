﻿using System.Numerics;
using System.Runtime.CompilerServices;
using CT.Common.Serialization;
using KaNet.Physics.RigidBodies;

namespace CT.Common.Gameplay
{
	// TODO : 초기화 구문 최적화
	public class NetRigidBody
	{
		private readonly KaRigidBody _rigidBody;
		public bool IsDirty => Event.EventFlags != PhysicsEventFlag.None;
		public Vector2 Position => _rigidBody.Position;

		public PhysicsEvent Event;

		public NetRigidBody(KaRigidBody rigidBody)
		{
			_rigidBody = rigidBody;
		}

#if CT_SERVER
		public void SerializeInitialSyncData(IPacketWriter writer)
		{
			writer.Put(_rigidBody.Position);
			writer.Put(_rigidBody.LinearVelocity);
			writer.Put(_rigidBody.ForceVelocity);
		}
#endif

		public bool TryDeserializeInitialSyncData(IPacketReader reader)
		{
			if (!reader.TryReadVector2(out var position)) return false;
			if (!reader.TryReadVector2(out var linearVelocity)) return false;
			if (!reader.TryReadVector2(out var forceVelocity)) return false;

			_rigidBody.MoveTo(position);
			_rigidBody.LinearVelocity = linearVelocity;
			_rigidBody.ForceVelocity = forceVelocity;

			return true;
		}

#if CT_SERVER
		public void SerializeEventSyncData(IPacketWriter writer)
		{
			Event.Serialize(writer);
		}
#endif

		public bool TryDeserializeEventSyncData(IPacketReader reader)
		{
			PhysicsEvent rigidBodyEvent = new PhysicsEvent();
			if (!rigidBodyEvent.TryDeserialize(reader))
				return false;

			SynchronizeByEvent(rigidBodyEvent);
			return true;
		}

		public static void IgnoreSyncData(IPacketReader reader)
		{
			PhysicsEvent.IgnoreStatic(reader);
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
			Event.EventFlags = PhysicsEventFlag.None;
		}

		public void SynchronizeByEvent(PhysicsEvent evt)
		{
			if (evt.EventFlags.HasFlag(PhysicsEventFlag.Position))
			{
				_rigidBody.MoveTo(evt.Position);
			}

			if (evt.EventFlags.HasFlag(PhysicsEventFlag.LinearVelocity)) 
			{
				_rigidBody.LinearVelocity = evt.LinearVelocity;
			}

			if (evt.EventFlags.HasFlag(PhysicsEventFlag.ForceVelocity))
			{
				_rigidBody.ForceVelocity = evt.ForceVelocity;
			}

			if (evt.EventFlags.HasFlag(PhysicsEventFlag.Rotation))
			{
				_rigidBody.RotateTo(_rigidBody.Rotation);
			}
		}

		/// <summary>해당 위치로 이동합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MoveTo(Vector2 position)
		{
			_rigidBody.MoveTo(position);

			Event.EventFlags |= PhysicsEventFlag.Position;
			Event.Position = _rigidBody.Position;
		}

		/// <summary>각도와 거리만큼 상대적으로 이동합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Move(Vector2 direction, float distance)
		{
			_rigidBody.Move(direction, distance);

			Event.EventFlags |= PhysicsEventFlag.Position;
			Event.Position = _rigidBody.Position;
		}

		/// <summary>순간적인 힘을 가합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Impulse(Vector2 direction, float power)
		{
			Impulse(direction * power);
		}

		/// <summary>순간적인 힘을 가합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Impulse(Vector2 impluseVelocity)
		{
			_rigidBody.ForceVelocity = impluseVelocity;

			Event.EventFlags |= PhysicsEventFlag.ForceVelocity | PhysicsEventFlag.Position;
			Event.Position = _rigidBody.Position;
			Event.ForceVelocity = _rigidBody.ForceVelocity;
		}

		/// <summary>가속도를 바꿉니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ChangeVelocity(Vector2 linearVelocity)
		{
			_rigidBody.LinearVelocity = linearVelocity;

			Event.EventFlags |= PhysicsEventFlag.LinearVelocity | PhysicsEventFlag.Position;
			Event.Position = _rigidBody.Position;
			Event.LinearVelocity = _rigidBody.LinearVelocity;
		}

		/// <summary>해당 각도로 회전합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RotateTo(float rotation)
		{
			_rigidBody.RotateTo(rotation);

			Event.EventFlags |= PhysicsEventFlag.Rotation;
			Event.Rotation = _rigidBody.Rotation;
		}

		/// <summary>각도만큼 상대적으로 회전합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Rotate(float rotation)
		{
			_rigidBody.Rotate(rotation);

			Event.EventFlags |= PhysicsEventFlag.Rotation;
			Event.Rotation = _rigidBody.Rotation;
		}
	}
}