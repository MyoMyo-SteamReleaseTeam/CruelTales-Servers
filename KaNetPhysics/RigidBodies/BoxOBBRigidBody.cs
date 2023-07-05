using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace KaNet.Physics.RigidBodies
{
	public class BoxOBBRigidBody : RigidBody
	{
		/// <summary>너비</summary>
		public readonly float Width;

		/// <summary>높이</summary>
		public readonly float Height;

		/// <summary>정점 배열</summary>
		[AllowNull] public readonly Vector2[] _vertices;

		/// <summary>변환이 적용된 정점 배열</summary>
		[AllowNull] public readonly Vector2[] _transformedVertices;

		public BoxOBBRigidBody(float width, float height, bool isStatic)
			: base(PhysicsShapeType.Box_OBB, isStatic)
		{
			Width = width;
			Height = height;

			_vertices = new Vector2[4];
			_transformedVertices = new Vector2[4];

			float hw = Width * 0.5f;
			float hh = Height * 0.5f;
			_vertices[0] = new Vector2(-hw, hh);
			_vertices[1] = new Vector2(hw, hh);
			_vertices[2] = new Vector2(hw, -hh);
			_vertices[3] = new Vector2(-hw, -hh);

			Physics.ComputeTransform(_vertices, _transformedVertices, Position, Angle);
		}

		public Vector2[] GetTransformedVertices()
		{
			if (!_isTransformDirty)
				return _transformedVertices;
			_isTransformDirty = false;
			Physics.ComputeTransform(_vertices, _transformedVertices, Position, Angle);
			return _transformedVertices;
		}

		public override bool IsCollideWith(RigidBody otherBody, out Vector2 normal, out float depth)
		{
			switch (otherBody.ShapeType)
			{
				case PhysicsShapeType.Box_AABB:
					return Physics.IsCollideAABBOBB((BoxAABBRigidBody)otherBody, this,
													   out normal, out depth);

				case PhysicsShapeType.Box_OBB:
					return Physics.IsCollideOBBs(this, (BoxOBBRigidBody)otherBody,
													out normal, out depth);

				case PhysicsShapeType.Circle:
					return Physics.IsCollideCircleOBB((CircleRigidBody)otherBody, this,
														 out normal, out depth);

				default:
					throw new ArgumentOutOfRangeException
						($"There is no such physics shape type as {otherBody.ShapeType}");
			}
		}
	}
}
