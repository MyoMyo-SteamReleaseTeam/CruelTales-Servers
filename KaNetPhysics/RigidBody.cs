using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace KaNet.Physics
{
	/// <summary>물리 속성을 가진 강체입니다.</summary>
	public abstract class RigidBody
	{
		/// <summary>물리 형상 타입</summary>
		public PhysicsShapeType ShapeType { get; private set; }

		/// <summary>위치</summary>
		public Vector2 Position { get; private set; }

		/// <summary>각도</summary>
		public float Angle { get; private set; }

		/// <summary>선속도</summary>
		public Vector2 LinearVelocity { get; private set; }

		/// <summary>정적 객체 여부</summary>
		public readonly bool IsStatic;

		/// <summary>변환이 필요한지 여부입니다.</summary>
		protected bool _isTransformDirty = true;

		public RigidBody(PhysicsShapeType shapeType, bool isStatic)
		{
			this.ShapeType = shapeType;
			this.IsStatic = isStatic;
		}

		public void MoveTo(Vector2 position)
		{
			this.Position = position;
			_isTransformDirty = true;
		}

		public void Move(Vector2 direction, float amount)
		{
			this.Position += direction * amount;
			_isTransformDirty = true;
		}

		public void RotateTo(float angle)
		{
			this.Angle = angle;
			_isTransformDirty = true;
		}

		public abstract bool IsCollideWith(RigidBody otherBody, out Vector2 normal, out float depth);
	}

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
			this.Width = width;
			this.Height = height;

			_vertices = new Vector2[4];

			float hw = Width * 0.5f;
			float hh = Height * 0.5f;
			_vertices[0] = new Vector2(-hw, hh);
			_vertices[1] = new Vector2(hw, hh);
			_vertices[2] = new Vector2(hw, -hh);
			_vertices[3] = new Vector2(-hw, -hh);
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
			this.Width = width;
			this.Height = height;

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

	public class CircleRigidBody : RigidBody
	{
		/// <summary>반지름</summary>
		public readonly float Radius;

		public CircleRigidBody(float radius, bool isStatic)
			: base(PhysicsShapeType.Circle, isStatic)
		{
			this.Radius = radius;
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
