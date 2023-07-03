using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace KaNet.Physics.Legacy
{
	/// <summary>물리 형체 타입</summary>
	public enum Legacy_ShapeType
	{
		/// <summary>원</summary>
		Circle = 0,

		/// <summary>직사각형</summary>
		Box = 1,
	}

	/// <summary>RigidBody</summary>
	public sealed class Legacy_RigidBody
	{
		/// <summary>위치</summary>
		private Vector2 _position;
		public Vector2 Position => _position;

		/// <summary>선속도</summary>
		private Vector2 _linearVelocity;
		public Vector2 LinearVelocity
		{
			get => _linearVelocity;
			internal set => _linearVelocity = value;
		}

		/// <summary>각도</summary>
		private float _angle;
		public float Angle => _angle;

		/// <summary>각속도</summary>
		private float _angularVelocity;
		public float AngularVelocity
		{
			get => _angularVelocity;
			internal set => _angularVelocity = value;
		}

		/// <summary>힘</summary>
		private Vector2 _force;

		/// <summary>모양 타입</summary>
		public readonly Legacy_ShapeType ShapeType;

		/// <summary>밀도</summary>
		public readonly float Density;

		/// <summary>질량</summary>
		public readonly float Mass;
		public readonly float InvMass;

		/// <summary>반발 계수</summary>
		public readonly float Restitution;

		/// <summary>면적</summary>
		public readonly float Area;

		/// <summary>관성 모멘트</summary>
		public readonly float Inertia;
		public readonly float InvInertia;

		/// <summary>정적 객체 여부</summary>
		public readonly bool IsStatic;

		/// <summary>반지름</summary>
		public readonly float Radius;

		/// <summary>너비</summary>
		public readonly float Width;

		/// <summary>높이</summary>
		public readonly float Height;

		/// <summary>정지 마찰력</summary>
		public readonly float StaticFriction;

		/// <summary>동마찰력</summary>
		public readonly float DynamicFriction;

		/// <summary>정점 배열</summary>
		[AllowNull]
		private readonly Vector2[] _vertices;

		/// <summary>폴리곤의 삼각형 인덱스 배열</summary>
		[AllowNull]
		public readonly int[] Triangles; //물리에서 사용하지 않음

		/// <summary>회전 변환이 적용된 정점 배열</summary>
		[AllowNull]
		private Vector2[] _transformedVertices;

		/// <summary>AABB 바운딩 볼륨</summary>
		private Legacy_AabbBoundary _aabb;

		/// <summary>변환이 필요한지 여부</summary>
		private bool _transformUpdateRequired;

		/// <summary>AABB 바운딩 볼륨 수정이 필요한지 여부</summary>
		private bool _aabbUpdateRequired;

		public Legacy_RigidBody(float density, float mass, float inertia, float restitution, float area,
						 bool isStatic, float radius, float width, float height,
						 Vector2[] vertices, Legacy_ShapeType shapeType)
		{
			_position = Vector2.Zero;
			_linearVelocity = Vector2.Zero;
			_angle = 0f;
			_angularVelocity = 0f;

			_force = Vector2.Zero;

			ShapeType = shapeType;
			Density = density;
			Mass = mass;
			InvMass = mass > 0f ? 1f / mass : 0f;
			Inertia = inertia;
			InvInertia = inertia > 0f ? 1f / inertia : 0f;
			Restitution = restitution;
			Area = area;
			IsStatic = isStatic;
			Radius = radius;
			Width = width;
			Height = height;
			StaticFriction = 0.6f;
			DynamicFriction = 0.4f;

			if (ShapeType is Legacy_ShapeType.Box)
			{
				_vertices = vertices;
				Triangles = createBoxTriangles();
				_transformedVertices = new Vector2[_vertices.Length];
			}
			else
			{
				_vertices = null;
				Triangles = null;
				_transformedVertices = null;
			}

			_transformUpdateRequired = true;
			_aabbUpdateRequired = true;
		}

		/// <summary>직사각형의 정점 배열을 생성합니다.</summary>
		private static Vector2[] createBoxVertices(float width, float height)
		{
			float left = -width / 2f;
			float right = left + width;
			float bottom = -height / 2f;
			float top = bottom + height;

			Vector2[] vertices = new Vector2[4];
			vertices[0] = new Vector2(left, top);
			vertices[1] = new Vector2(right, top);
			vertices[2] = new Vector2(right, bottom);
			vertices[3] = new Vector2(left, bottom);

			return vertices;
		}

		/// <summary>직사각형의 삼각형 정점 인덱스를 생성합니다.</summary>
		private static int[] createBoxTriangles()
		{
			int[] triangles = new int[6];
			triangles[0] = 0; // TL
			triangles[1] = 1; // TR
			triangles[2] = 2; // BR
			triangles[3] = 0; // TL
			triangles[4] = 2; // BR
			triangles[5] = 3; // BL
			return triangles;
		}

		/// <summary>회전 행렬이 적용된 정점 배열을 반환합니다.</summary>
		public Vector2[] GetTransformedVertices()
		{
			if (_transformUpdateRequired)
			{
				Legacy_Transform transform = new Legacy_Transform(_position, _angle);

				for (int i = 0; i < _vertices.Length; i++)
				{
					Vector2 v = _vertices[i];
					_transformedVertices[i] = v.Transform(transform);
				}
			}

			_transformUpdateRequired = false;
			return _transformedVertices;
		}

		/// <summary>AABB 바운딩 볼륨을 반환합니다.</summary>
		public Legacy_AabbBoundary GetAABB()
		{
			if (_aabbUpdateRequired)
			{
				float minX = float.MaxValue;
				float minY = float.MaxValue;
				float maxX = float.MinValue;
				float maxY = float.MinValue;

				if (ShapeType is Legacy_ShapeType.Box)
				{
					Vector2[] vertices = GetTransformedVertices();

					for (int i = 0; i < vertices.Length; i++)
					{
						Vector2 v = vertices[i];

						if (v.X < minX) { minX = v.X; }
						if (v.X > maxX) { maxX = v.X; }
						if (v.Y < minY) { minY = v.Y; }
						if (v.Y > maxY) { maxY = v.Y; }
					}
				}
				else if (ShapeType is Legacy_ShapeType.Circle)
				{
					minX = Position.X - Radius;
					minY = Position.Y - Radius;
					maxX = Position.X + Radius;
					maxY = Position.Y + Radius;
				}
				else
				{
					throw new Exception("Unknown ShapeType.");
				}

				_aabb = new Legacy_AabbBoundary(minX, minY, maxX, maxY);
			}

			_aabbUpdateRequired = false;
			return _aabb;
		}

		/// <summary>물리 Step을 진행합니다.</summary>
		public void Step(float stepTime, Vector2 gravity, int iterations)
		{
			if (IsStatic)
			{
				return;
			}

			stepTime /= iterations;

			// Force = Mass * Acc
			// Acc = Force / Mass

			//FlatVector acceleration = force / Mass;
			//linearVelocity += acceleration * time;

			_linearVelocity += gravity * stepTime;
			_position += _linearVelocity * stepTime;

			_angle += _angularVelocity * stepTime;

			_force = Vector2.Zero;

			// 움직인 경우 여러 프로퍼티를 갱신
			_transformUpdateRequired = true;
			_aabbUpdateRequired = true;
		}

		/// <summary>힘을 가합니다.</summary>
		public void AddForce(Vector2 amount)
		{
			_force = amount;
		}

		/// <summary>상대적으로 이동합니다.</summary>
		public void Move(Vector2 amount)
		{
			_position += amount;
			_transformUpdateRequired = true;
			_aabbUpdateRequired = true;
		}

		/// <summary>해당 위치로 이동합니다.</summary>
		public void MoveTo(Vector2 position)
		{
			_position = position;
			_transformUpdateRequired = true;
			_aabbUpdateRequired = true;
		}

		/// <summary>물체를 상대적으로 회전합니다.</summary>
		public void Rotate(float amount)
		{
			_angle += amount;
			_transformUpdateRequired = true;
			_aabbUpdateRequired = true;
		}

		/// <summary>해당 각도로 회전합니다.</summary>
		/// <param name="angle"></param>
		public void RotateTo(float angle)
		{
			_angle = angle;
			_transformUpdateRequired = true;
			_aabbUpdateRequired = true;
		}

		public static bool CreateCircleBody(float radius, float density, bool isStatic, float restitution, out Legacy_RigidBody body)
		{
			body = null;
			float area = radius * radius * MathF.PI;

			if (area < Legacy_PhysicsWorld.MinBodySize)
			{
				throw new Exception($"Circle radius is too small. Min circle area is {Legacy_PhysicsWorld.MinBodySize}");
			}

			if (area > Legacy_PhysicsWorld.MaxBodySize)
			{
				throw new Exception($"Circle radius is too large. Max circle area is {Legacy_PhysicsWorld.MaxBodySize}");
			}

			if (density < Legacy_PhysicsWorld.MinDensity)
			{
				throw new Exception($"Density is too small. Min density is {Legacy_PhysicsWorld.MinDensity}");
			}

			if (density > Legacy_PhysicsWorld.MaxDensity)
			{
				throw new Exception($"Density is too large. Max density is {Legacy_PhysicsWorld.MaxDensity}");
			}

			restitution = KaMath.Clamp(restitution, 0f, 1f);

			float mass = 0f;
			float inertia = 0;

			if (!isStatic)
			{
				// mass = area * depth * density
				mass = area * density;
				inertia = 1f / 2f * mass * radius * radius;
			}

			body = new Legacy_RigidBody(density, mass, inertia, restitution, area, isStatic, radius, 0f, 0f, null, Legacy_ShapeType.Circle);
			return true;
		}

		public static bool CreateBoxBody(float width, float height, float density, bool isStatic, float restitution,
										 out Legacy_RigidBody body, out string errorMessage)
		{
			body = null;
			errorMessage = string.Empty;

			float area = width * height;

			if (area < Legacy_PhysicsWorld.MinBodySize)
			{
				errorMessage = $"Area is too small. Min area is {Legacy_PhysicsWorld.MinBodySize}";
				return false;
			}

			if (area > Legacy_PhysicsWorld.MaxBodySize)
			{
				errorMessage = $"Area is too large. Max area is {Legacy_PhysicsWorld.MaxBodySize}";
				return false;
			}

			if (density < Legacy_PhysicsWorld.MinDensity)
			{
				errorMessage = $"Density is too small. Min density is {Legacy_PhysicsWorld.MinDensity}";
				return false;
			}

			if (density > Legacy_PhysicsWorld.MaxDensity)
			{
				errorMessage = $"Density is too large. Max density is {Legacy_PhysicsWorld.MaxDensity}";
				return false;
			}

			restitution = KaMath.Clamp(restitution, 0f, 1f);

			float mass = 0;
			float inertia = 0;

			if (!isStatic)
			{
				// mass = area * depth * density
				mass = area * density;
				inertia = 1f / 12 * mass * width * width + height * height;
			}

			Vector2[] vertices = createBoxVertices(width, height);

			body = new Legacy_RigidBody(density, mass, inertia, restitution, area, isStatic, 0f, width, height, vertices, Legacy_ShapeType.Box);
			return true;
		}
	}
}
