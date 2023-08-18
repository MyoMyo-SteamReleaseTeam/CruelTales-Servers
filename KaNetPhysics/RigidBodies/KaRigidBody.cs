using System;
using System.Numerics;
using Sirenix.OdinInspector;

namespace KaNet.Physics.RigidBodies
{
	/// <summary>물리 속성을 가진 강체입니다.</summary>
	[Serializable]
	public abstract class KaRigidBody
	{
		/// <summary>물리 식별자입니다.</summary>
		public int ID { get; private set; } = -1;

		/// <summary>
		/// 다른 다이나믹 강체와 부딪혔을 때 발생합니다.
		/// 정적 강체의 충돌은 감지되지 않습니다.
		/// </summary>
		public Action<int>? OnCollisionWith { get; private set; }

		/// <summary>물리 형상 타입</summary>
		[field: UnityEngine.SerializeField]
		public KaPhysicsShapeType ShapeType { get; private set; }

		/// <summary>레이어 마스크</summary>
		[field: UnityEngine.SerializeField]
		public LayerMask LayerMask { get; private set; }

		/// <summary>위치</summary>
		[ShowInInspector]
		public Vector2 Position { get; private set; }

		/// <summary>선속도</summary>
		[ShowInInspector]
		public Vector2 LinearVelocity { get; set; }

		/// <summary>힘<summary>
		[ShowInInspector]
		public Vector2 ForceVelocity { get; set; }

		/// <summary>각도</summary>
		[field: UnityEngine.SerializeField]
		public float Rotation { get; private set; }

		/// <summary>힘 마찰력</summary>
		[field: UnityEngine.SerializeField]
		public float ForceFriction { get; set; }

		/// <summary>정적 객체 여부</summary>
		[field: UnityEngine.SerializeField]
		public readonly bool IsStatic;

		/// <summary>변환이 필요한지 여부입니다.</summary>
		protected bool _isTransformDirty = true;

		/// <summary>바운딩 볼륨의 변환이 필요한지 여부입니다.</summary>
		protected bool _isBoundingBoxDirty = true;

		/// <summary>AABB 볼륨입니다.</summary>
		protected BoundingBox _boundingBox;

		public KaRigidBody(KaPhysicsShapeType shapeType,
						   PhysicsLayerMask layerMask,
						   bool isStatic)
		{
			ShapeType = shapeType;
			IsStatic = isStatic;
			LayerMask = LayerMaskHelper.GetLayerMask(layerMask);
		}

		public KaRigidBody(KaPhysicsShapeType shapeType,
						   PhysicsLayerMask layerMask, 
						   float rotation, bool isStatic)
		{
			ShapeType = shapeType;
			Rotation = rotation;
			IsStatic = isStatic;
			LayerMask = LayerMaskHelper.GetLayerMask(layerMask);
		}

		public void Initialize(int id, Action<int> onCollisionWith)
		{
			ID = id;
			OnCollisionWith = onCollisionWith;
		}

		public void Initialize(int id)
		{
			ID = id;
		}

		public void BindAction(Action<int> onCollisionWith)
		{
			OnCollisionWith = onCollisionWith;
		}

		public void SetLayerMask(PhysicsLayerMask layerMask)
		{
			LayerMask = LayerMaskHelper.GetLayerMask(layerMask);
		}

		public abstract void CalcurateProperties();

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

		public void OnCollided(int id)
		{
			OnCollisionWith?.Invoke(id);
		}
	}
}
