using System;
using System.Numerics;

namespace KaNet.Physics.RigidBodies
{
	public class CircleRigidBody : RigidBody
	{
		/// <summary>반지름</summary>
		public readonly float Radius;

		public CircleRigidBody(float radius, bool isStatic)
			: base(PhysicsShapeType.Circle, isStatic)
		{
			Radius = radius;
		}

		public override bool IsCollideWith(RigidBody otherBody, out Vector2 normal, out float depth)
		{
			switch (otherBody.ShapeType)
			{
				case PhysicsShapeType.Box_AABB:
					return Physics.IsCollideCircleAABB(this, (BoxAABBRigidBody)otherBody,
														  out normal, out depth);

				case PhysicsShapeType.Box_OBB:
					return Physics.IsCollideCircleOBB(this, (BoxOBBRigidBody)otherBody,
														  out normal, out depth);


				case PhysicsShapeType.Circle:
					return Physics.IsCollideCircles(this, (CircleRigidBody)otherBody,
													   out normal, out depth);

				default:
					throw new ArgumentOutOfRangeException
						($"There is no such physics shape type as {otherBody.ShapeType}");
			}
		}
	}
}
