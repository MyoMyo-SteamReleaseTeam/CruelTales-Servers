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

		/// <summary>OBB의 바운더리 반지름입니다.</summary>
		public readonly float BoundaryRadius;

		/// <summary>OBB의 바운더리 반지름입니다.</summary>
		public readonly float BoundaryDiameter;

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

			BoundaryRadius = new Vector2(hw, hh).Length();
			BoundaryDiameter = BoundaryRadius * 2;

			Physics.ComputeTransform(_vertices, _transformedVertices, Position, Angle);
		}

		public override BoundingBox GetBoundingBox()
		{
			if (_isBoundingBoxDirty)
			{
				_isBoundingBoxDirty = false;
				_boundingBox = new BoundingBox(Position, BoundaryDiameter, BoundaryDiameter);
			}

			return _boundingBox;
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
			bool result;

			switch (otherBody.ShapeType)
			{
				case PhysicsShapeType.Box_AABB:
					result = Physics.IsCollideAABBOBB((BoxAABBRigidBody)otherBody, this,
													  out normal, out depth);
					normal = -normal;
					break;

				case PhysicsShapeType.Box_OBB:
					result = Physics.IsCollideOBBs(this, (BoxOBBRigidBody)otherBody,
												   out normal, out depth);
					break;

				case PhysicsShapeType.Circle:
					result = Physics.IsCollideCircleOBB((CircleRigidBody)otherBody, this,
														out normal, out depth);
					normal = -normal;
					break;

				default:
					throw new ArgumentOutOfRangeException
						($"There is no such physics shape type as {otherBody.ShapeType}");
			}

			return result;
		}
	}
}
