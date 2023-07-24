using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace KaNet.Physics.RigidBodies
{
	/// <summary>회전 가능한 상자 강체 입니다.</summary>
	public class BoxOBBRigidBody : KaRigidBody
	{
		/// <summary>너비</summary>
		public float Width { get; private set; }

		/// <summary>높이</summary>
		public float Height { get; private set; }

		/// <summary>OBB의 바운더리 반지름입니다.</summary>
		public float BoundaryRadius { get; private set; }

		/// <summary>OBB의 바운더리 반지름입니다.</summary>
		public float BoundaryDiameter { get; private set; }

		/// <summary>정점 배열</summary>
		[AllowNull] public readonly Vector2[] _vertices;

		/// <summary>변환이 적용된 정점 배열</summary>
		[AllowNull] public readonly Vector2[] _transformedVertices;

		public BoxOBBRigidBody(float width, float height, float rotation,
							   bool isStatic, PhysicsLayerMask layerMask)
			: base(KaPhysicsShapeType.Box_OBB, layerMask, rotation, isStatic)
		{
			Width = width;
			Height = height;

			_vertices = new Vector2[4];
			_transformedVertices = new Vector2[4];

			CalcurateProperties();
		}

		public override void CalcurateProperties()
		{
			float hw = Width * 0.5f;
			float hh = Height * 0.5f;
			_vertices[0] = new Vector2(-hw, hh);
			_vertices[1] = new Vector2(hw, hh);
			_vertices[2] = new Vector2(hw, -hh);
			_vertices[3] = new Vector2(-hw, -hh);

			BoundaryRadius = new Vector2(hw, hh).Length();
			BoundaryDiameter = BoundaryRadius * 2;

			KaPhysics.ComputeTransform(_vertices, _transformedVertices, Position, Rotation);
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
			KaPhysics.ComputeTransform(_vertices, _transformedVertices, Position, Rotation);
			return _transformedVertices;
		}

		public override bool IsCollideWith(KaRigidBody otherBody, out Vector2 normal, out float depth)
		{
			bool result;

			switch (otherBody.ShapeType)
			{
				case KaPhysicsShapeType.Box_AABB:
					result = KaPhysics.IsCollideAABBOBB((BoxAABBRigidBody)otherBody, this,
													  out normal, out depth);
					normal = -normal;
					break;

				case KaPhysicsShapeType.Box_OBB:
					result = KaPhysics.IsCollideOBBs(this, (BoxOBBRigidBody)otherBody,
												   out normal, out depth);
					break;

				case KaPhysicsShapeType.Circle:
					result = KaPhysics.IsCollideCircleOBB((CircleRigidBody)otherBody, this,
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
