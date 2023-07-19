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

		public KaRigidBody OnCreatedByTester()
		{
			KaRigidBody rigidBody;

#if DEBUG
			if (Width < KaPhysics.MIN_COLLIDER_SIZE)
				throw new ArgumentException($"Too small width : {Width}");

			if (Height < KaPhysics.MIN_COLLIDER_SIZE)
				throw new ArgumentException($"Too small height : {Height}");

			if (Radius * 2 < KaPhysics.MIN_COLLIDER_SIZE)
				throw new ArgumentException($"Too small diameter : {Radius * 2}");
#endif

			if (PhysicsShapeType == KaPhysicsShapeType.Box_AABB)
			{
#if DEBUG
				if (KaPhysics.NearlyNotEqual(Rotation, 0))
				{
					throw new ArgumentException($"This collider is AABB but current rotation is {Rotation}!");
				}
#endif
				rigidBody = new BoxAABBRigidBody(Width, Height, IsStatic);
			}
			else if (PhysicsShapeType == KaPhysicsShapeType.Box_OBB)
			{
#if DEBUG
				if (KaPhysics.NearlyNotEqual(Rotation, 0))
				{
					throw new ArgumentException($"This collider is AABB but current rotation is {Rotation}!");
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
