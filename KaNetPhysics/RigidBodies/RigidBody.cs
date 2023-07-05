﻿using System.Numerics;

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

		/// <summary>AABB 볼륨입니다.</summary>
		protected BoundingBox _boundingBox;

		public RigidBody(PhysicsShapeType shapeType, bool isStatic)
		{
			ShapeType = shapeType;
			IsStatic = isStatic;
		}

		/// <summary>AABB 볼륨을 반환합니다.</summary>
		public abstract BoundingBox GetBoundingBox();

		/// <summary>물리 스텝을 진행합니다.</summary>
		public void Step(float stepTime)
		{
			if (IsStatic)
				return;

			Position += (LinearVelocity + ForceVelocity) * stepTime;
			ForceVelocity = ForceVelocity.LengthSquared() > Physics.FLOAT_EPSILON ?
				Vector2.Lerp(ForceVelocity, Vector2.Zero, ForceFriction * stepTime) : Vector2.Zero;

			_isTransformDirty = true;
		}

		/// <summary>해당 위치로 이동합니다.</summary>
		public void MoveTo(Vector2 position)
		{
			Position = position;
			_isTransformDirty = true;
		}

		/// <summary>각도와 거리만큼 상대적으로 이동합니다.</summary>
		public void Move(Vector2 direction, float distance)
		{
			Position += direction * distance;
			_isTransformDirty = true;
		}

		/// <summary>해당 각도로 회전합니다.</summary>
		public void RotateTo(float angle)
		{
			Angle = angle;
			_isTransformDirty = true;
		}

		/// <summary>각도만큼 상대적으로 회전합니다.</summary>
		public void Rotate(float angle)
		{
			Angle += angle;
			_isTransformDirty = true;
		}

		/// <summary>대상 RigidBody와 충돌했는지 여부를 반환합니다.</summary>
		/// <param name="otherBody">검사할 대상입니다.</param>
		/// <param name="normal">충돌한 대상이 밀려나야 하는 방향 벡터입니다.</param>
		/// <param name="depth">충돌한 대상과의 접촉 거리입니다.</param>
		/// <returns>충돌 여부입니다.</returns>
		public abstract bool IsCollideWith(RigidBody otherBody, out Vector2 normal, out float depth);
	}
}
