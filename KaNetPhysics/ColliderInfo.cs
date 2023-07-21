using System;
using System.Numerics;
using KaNet.Physics.RigidBodies;

namespace KaNet.Physics
{
	[Serializable]
	public struct ColliderInfo
	{
		public KaPhysicsShapeType PhysicsShapeType;
		public Vector2 Position;
		public float Rotation;
		public float Radius;
		public float Width;
		public float Height;
		public bool IsStatic;

//#if NET
//		public ColliderInfo()
//		{
//			PhysicsShapeType = KaPhysicsShapeType.None;
//			Position = Vector2.Zero;
//			Rotation = 0;
//			Radius = KaPhysics.MIN_COLLIDER_SIZE * 2;
//			Width = KaPhysics.MIN_COLLIDER_SIZE;
//			Height = KaPhysics.MIN_COLLIDER_SIZE;
//			IsStatic = true;
//		}
//#endif

		public ColliderInfo(KaPhysicsShapeType physicsShapeType,
							Vector2 position, float rotation, float radius,
							float width, float height, bool isStatic)
		{
			PhysicsShapeType = physicsShapeType;
			Position = position;
			Rotation = rotation;
			Radius = radius;
			Width = width;
			Height = height;
			IsStatic = isStatic;
		}

		public KaRigidBody CreateRigidBody(bool checkValidation = false)
		{
			KaRigidBody rigidBody;

#if DEBUG
			if (checkValidation)
			{
				if (Width < KaPhysics.MIN_COLLIDER_SIZE)
					throw new ArgumentException($"Too small width : {Width}");

				if (Height < KaPhysics.MIN_COLLIDER_SIZE)
					throw new ArgumentException($"Too small height : {Height}");

				if (Radius * 2 < KaPhysics.MIN_COLLIDER_SIZE)
					throw new ArgumentException($"Too small diameter : {Radius * 2}");
			}
#endif

			if (PhysicsShapeType == KaPhysicsShapeType.Box_AABB)
			{
#if DEBUG
				if (checkValidation && KaPhysics.NearlyNotEqual(Rotation, 0))
				{
					throw new ArgumentException($"This collider is AABB but current rotation is {Rotation}!");
				}
#endif
				rigidBody = new BoxAABBRigidBody(Width, Height, IsStatic);
			}
			else if (PhysicsShapeType == KaPhysicsShapeType.Box_OBB)
			{
#if DEBUG
				if (checkValidation)
				{
					if (KaPhysics.NearlyEqual(Rotation, 0) ||
						KaPhysics.NearlyEqual(Rotation, 90) ||
						KaPhysics.NearlyEqual(Rotation, 180) ||
						KaPhysics.NearlyEqual(Rotation, 270))
					{
						throw new ArgumentException($"This collider is OBB but current rotation is {Rotation}! It's should be assigned OBB!");
					}
				}
#endif
				rigidBody = new BoxOBBRigidBody(Width, Height, Rotation, IsStatic);
			}
			else if (PhysicsShapeType == KaPhysicsShapeType.Circle)
			{
				rigidBody = new CircleRigidBody(Radius, IsStatic);
			}
			else
			{
				throw new ArgumentException($"There is no such shape type : {PhysicsShapeType}");
			}

			rigidBody.MoveTo(Position);
			return rigidBody;
		}
	}
}
