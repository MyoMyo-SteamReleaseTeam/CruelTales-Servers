using System;
using System.Numerics;

namespace KaNet.Physics.RigidBodies
{
	public class CircleRigidBody : RigidBody
	{
		/// <summary>반지름</summary>
		public readonly float Radius;
		public readonly float Diameter;

		public CircleRigidBody(float radius, bool isStatic)
			: base(PhysicsShapeType.Circle, isStatic)
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

		public override bool IsCollideWith(RigidBody otherBody, out Vector2 normal, out float depth)
		{
			bool result;

			switch (otherBody.ShapeType)
			{
				case PhysicsShapeType.Box_AABB:
					result = Physics.IsCollideCircleAABB(this, (BoxAABBRigidBody)otherBody,
														 out normal, out depth);
					break;

				case PhysicsShapeType.Box_OBB:
					result = Physics.IsCollideCircleOBB(this, (BoxOBBRigidBody)otherBody,
														out normal, out depth);
					break;


				case PhysicsShapeType.Circle:
					result = Physics.IsCollideCircles(this, (CircleRigidBody)otherBody,
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
