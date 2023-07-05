using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace KaNet.Physics.RigidBodies
{
	public class BoxAABBRigidBody : RigidBody
	{
		/// <summary>너비</summary>
		public readonly float Width;

		/// <summary>높이</summary>
		public readonly float Height;

		/// <summary>정점 배열</summary>
		[AllowNull] public readonly Vector2[] _vertices;

		public BoxAABBRigidBody(float width, float height, bool isStatic)
			: base(PhysicsShapeType.Box_AABB, isStatic)
		{
			Width = width;
			Height = height;

			_vertices = new Vector2[4];

			float hw = Width * 0.5f;
			float hh = Height * 0.5f;
			_vertices[0] = new Vector2(-hw, hh);
			_vertices[1] = new Vector2(hw, hh);
			_vertices[2] = new Vector2(hw, -hh);
			_vertices[3] = new Vector2(-hw, -hh);

			_boundingBox = new BoundingBox(Position, Width, Height);
		}

		public override BoundingBox GetBoundingBox()
		{
			if (_isTransformDirty)
			{
				_boundingBox = new BoundingBox(Position, Width, Height);
			}

			return _boundingBox;
		}

		public Vector2[] GetTransformedVertices()
		{
			return _vertices;
		}

		public override bool IsCollideWith(RigidBody otherBody, out Vector2 normal, out float depth)
		{
			switch (otherBody.ShapeType)
			{
				case PhysicsShapeType.Box_AABB:
					return Physics.IsCollideAABBs(this, (BoxAABBRigidBody)otherBody,
													 out normal, out depth);

				case PhysicsShapeType.Box_OBB:
					return Physics.IsCollideAABBOBB(this, (BoxOBBRigidBody)otherBody,
													   out normal, out depth);

				case PhysicsShapeType.Circle:
					return Physics.IsCollideCircleAABB((CircleRigidBody)otherBody, this,
														  out normal, out depth);

				default:
					throw new ArgumentOutOfRangeException
						($"There is no such physics shape type as {otherBody.ShapeType}");
			}
		}
	}
}
