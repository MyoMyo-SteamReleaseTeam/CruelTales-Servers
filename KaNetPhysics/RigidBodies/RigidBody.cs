using System;
using System.Numerics;

namespace KaNet.Physics.RigidBodies
{
	/// <summary>물리 속성을 가진 강체입니다.</summary>
	public abstract class RigidBody
	{
		/// <summary>물리 형상 타입</summary>
		public PhysicsShapeType ShapeType { get; private set; }

		/// <summary>위치</summary>
		public Vector2 Position { get; private set; }

		/// <summary>각도</summary>
		public float Angle { get; private set; }

		/// <summary>선속도</summary>
		public Vector2 LinearVelocity { get; set; }

		/// <summary>힘<summary>
		public Vector2 ForceVelocity { get; set; }

		/// <summary>힘 마찰력</summary>
		public float ForceFriction { get; set; } = 3.0f;

		/// <summary>정적 객체 여부</summary>
		public readonly bool IsStatic;

		/// <summary>변환이 필요한지 여부입니다.</summary>
		protected bool _isTransformDirty = true;

		//private Vector2 _force;

		public RigidBody(PhysicsShapeType shapeType, bool isStatic)
		{
			ShapeType = shapeType;
			IsStatic = isStatic;
		}

		public void Step(float stepTime)
		{
			if (IsStatic)
				return;

			//LinearVelocity += _force * stepTime;
			Position += (LinearVelocity + ForceVelocity) * stepTime;

			if (ForceVelocity.LengthSquared() > 0.001f)
			{
				ForceVelocity = Vector2.Lerp(ForceVelocity, Vector2.Zero, ForceFriction * stepTime);
			}
			else
			{
				ForceVelocity = Vector2.Zero;
			}

			//_force = Vector2.Zero;
			_isTransformDirty = true;
		}

		public void MoveTo(Vector2 position)
		{
			Position = position;
			_isTransformDirty = true;
		}

		public void Move(Vector2 direction, float amount)
		{
			Position += direction * amount;
			_isTransformDirty = true;
		}

		public void RotateTo(float angle)
		{
			Angle = angle;
			_isTransformDirty = true;
		}

		public void Rotate(float angle)
		{
			Angle += angle;
			_isTransformDirty = true;
		}

		public abstract bool IsCollideWith(RigidBody otherBody, out Vector2 normal, out float depth);
	}
}
