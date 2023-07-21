﻿using System;
using System.Numerics;

namespace KaNet.Physics.RigidBodies
{
	/// <summary>원형 강체입니다.</summary>
	public class CircleRigidBody : KaRigidBody
	{
		/// <summary>반지름</summary>
		public readonly float Radius;
		public readonly float Diameter;

		public CircleRigidBody(float radius, bool isStatic)
			: base(KaPhysicsShapeType.Circle, isStatic)
		{
			Radius = radius;
			Diameter = radius * 2;
		}

		public override BoundingBox GetBoundingBox()
		{
			if (_isBoundingBoxDirty)
			{
				_isBoundingBoxDirty = false;
				_boundingBox = new BoundingBox(Position, Diameter, Diameter);
			}

			return _boundingBox;
		}

		public override bool IsCollideWith(KaRigidBody otherBody, out Vector2 normal, out float depth)
		{
			bool result;

			switch (otherBody.ShapeType)
			{
				case KaPhysicsShapeType.Box_AABB:
					result = KaPhysics.IsCollideCircleAABB(this, (BoxAABBRigidBody)otherBody,
														 out normal, out depth);
					break;

				case KaPhysicsShapeType.Box_OBB:
					result = KaPhysics.IsCollideCircleOBB(this, (BoxOBBRigidBody)otherBody,
														out normal, out depth);
					break;


				case KaPhysicsShapeType.Circle:
					result = KaPhysics.IsCollideCircles(this, (CircleRigidBody)otherBody,
													  out normal, out depth);
					break;

				default:
					throw new ArgumentOutOfRangeException
						($"There is no such physics shape type as {otherBody.ShapeType}");
			}

			return result;
		}
	}
}
