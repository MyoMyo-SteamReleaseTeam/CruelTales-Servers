using System;
using System.Numerics;
using Sirenix.OdinInspector;

namespace KaNet.Physics.RigidBodies
{
	/// <summary>물리 속성을 가진 강체입니다.</summary>
	[Serializable]
	public abstract class KaRigidBody
	{
		/// <summary>물리 형상 타입</summary>
		[field: UnityEngine.SerializeField]
		public KaPhysicsShapeType ShapeType { get; private set; }

		/// <summary>위치</summary>
		[ShowInInspector]
		public Vector2 Position { get; private set; }

		/// <summary>선속도</summary>
		[ShowInInspector]
		public Vector2 LinearVelocity { get; set; }

		/// <summary>힘<summary>
		[ShowInInspector]
		public Vector2 ForceVelocity { get; private set; }

		/// <summary>각도</summary>
		[field: UnityEngine.SerializeField]
		public float Rotation { get; private set; }

		/// <summary>힘 마찰력</summary>
		[field: UnityEngine.SerializeField]
		public float ForceFriction { get; set; } = 3.0f;

		/// <summary>정적 객체 여부</summary>
		[field: UnityEngine.SerializeField]
		public readonly bool IsStatic;

		/// <summary>변환이 필요한지 여부입니다.</summary>
		protected bool _isTransformDirty = true;

		/// <summary>바운딩 볼륨의 변환이 필요한지 여부입니다.</summary>
		protected bool _isBoundingBoxDirty = true;

		/// <summary>AABB 볼륨입니다.</summary>
		protected BoundingBox _boundingBox;

		public KaRigidBody(KaPhysicsShapeType shapeType, bool isStatic)
		{
			ShapeType = shapeType;
			IsStatic = isStatic;
		}

		public KaRigidBody(KaPhysicsShapeType shapeType, float rotation, bool isStatic)
		{
			ShapeType = shapeType;
			Rotation = rotation;
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
			ForceVelocity = ForceVelocity.LengthSquared() > KaPhysics.FLOAT_EPSILON ?
				Vector2.Lerp(ForceVelocity, Vector2.Zero, ForceFriction * stepTime) : Vector2.Zero;

			_isTransformDirty = true;
			_isBoundingBoxDirty = true;
		}

		/// <summary>해당 위치로 이동합니다.</summary>
		public void MoveTo(Vector2 position)
		{
			Position = position;
			_isTransformDirty = true;
			_isBoundingBoxDirty = true;
		}

		/// <summary>각도와 거리만큼 상대적으로 이동합니다.</summary>
		public void Move(Vector2 direction, float distance)
		{
			Position += direction * distance;
			_isTransformDirty = true;
			_isBoundingBoxDirty = true;
		}

		/// <summary>순간적인 힘을 가합니다.</summary>
		public void Impulse(Vector2 direction, float power)
		{
			ForceVelocity = direction * power;
		}

		/// <summary>순간적인 힘을 가합니다.</summary>
		public void Impulse(Vector2 impluseVelocity)
		{
			ForceVelocity = impluseVelocity;
		}

		/// <summary>해당 각도로 회전합니다.</summary>
		public void RotateTo(float rotation)
		{
			Rotation = rotation;
			_isTransformDirty = true;
		}

		/// <summary>각도만큼 상대적으로 회전합니다.</summary>
		public void Rotate(float rotation)
		{
			Rotation += rotation;
			_isTransformDirty = true;
		}

		/// <summary>대상 RigidBody와 충돌했는지 여부를 반환합니다.</summary>
		/// <param name="otherBody">검사할 대상입니다.</param>
		/// <param name="normal">충돌한 대상이 밀려나야 하는 방향 벡터입니다.</param>
		/// <param name="depth">충돌한 대상과의 접촉 거리입니다.</param>
		/// <returns>충돌 여부입니다.</returns>
		public abstract bool IsCollideWith(KaRigidBody otherBody, out Vector2 normal, out float depth);
	}
}
