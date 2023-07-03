using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace KaNet.Physics
{
	/// <summary>물리 속성을 가진 강체입니다.</summary>
	public class KaRigidBody
	{
		/// <summary>위치</summary>
		public Vector2 Position { get; private set; }

		/// <summary>각도</summary>
		public float Angle { get; private set; }

		/// <summary>선속도</summary>
		public Vector2 LinearVelocity { get; private set; }

		/// <summary>정적 객체 여부</summary>
		public readonly bool IsStatic;

		/// <summary>물리 형상 타입</summary>
		public readonly PhysicsShapeType ShapeType;

		/// <summary>반지름</summary>
		public readonly float Radius;

		/// <summary>너비</summary>
		public readonly float Width;

		/// <summary>높이</summary>
		public readonly float Height;

		/// <summary>정점 배열</summary>
		[AllowNull] public readonly Vector2[] _vertices;

		/// <summary>변환이 적용된 정점 배열</summary>
		[AllowNull] public readonly Vector2[] _transformedVertices;

		/// <summary>변환이 필요한지 여부입니다.</summary>
		private bool _isTransformDirty;

		internal KaRigidBody(PhysicsShapeType shapeType,
							 float radius,
							 float width,
							 float height,
							 float angle,
							 Vector2[]? vertices,
							 bool isStatic)
		{
			this.ShapeType = shapeType;
			this.Radius = radius;
			this.Width = width;
			this.Height = height;
			this.Angle = angle;
			if (vertices != null )
			{
				_vertices = vertices;
				_transformedVertices = new Vector2[vertices.Length];
			}
			_isTransformDirty = true;
			this.IsStatic = isStatic;
		}

		public Vector2[] GetTransformedVertices()
		{
			if (!_isTransformDirty)
				return _transformedVertices;
			
			_isTransformDirty = false;

			float cos = MathF.Cos(Angle);
			float sin = MathF.Sin(Angle);
			Matrix3x2 r = new Matrix3x2(cos, sin, -sin, cos, 0, 0);
			int length = _vertices.Length;

			for (int i = 0; i < length; i++)
			{
				_transformedVertices[i] = Vector2.Transform(_vertices[i], r) + Position;
			}

			return _transformedVertices;
		}

		public void MoveTo(Vector2 position)
		{
			this.Position = position;
			_isTransformDirty = true;
		}

		public void Rotate(float angle)
		{
			this.Angle = angle;
			_isTransformDirty = true;
		}
	}
}
