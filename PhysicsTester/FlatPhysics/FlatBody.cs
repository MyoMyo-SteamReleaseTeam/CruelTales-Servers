using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FlatPhysics
{
	public enum ShapeType
	{
		Circle = 0,
		Box = 1,
	}

	/// <summary>RigidBody</summary>
	public sealed class FlatBody
	{
		private FlatVector position;
		private FlatVector linearVelocity;
		private float angle;
		private float angularVelocity;
		private FlatVector force;

		public readonly ShapeType ShapeType;
		public readonly float Density;
		public readonly float Mass;
		public readonly float InvMass;
		/// <summary>반발 계수</summary>
		public readonly float Restitution;
		public readonly float Area;
		/// <summary>관성 모멘트</summary>
		public readonly float Inertia;
		public readonly float InvInertia;
		public readonly bool IsStatic;
		public readonly float Radius;
		public readonly float Width;
		public readonly float Height;
		public readonly float StaticFriction;
		public readonly float DynamicFriction;

		private readonly FlatVector[] vertices;
		[Obsolete("물리에서 사용하지 않음")]
		public readonly int[] Triangles;
		private FlatVector[] transformedVertices;
		private FlatAABB aabb;

		private bool transformUpdateRequired;
		private bool aabbUpdateRequired;

		public FlatVector Position => position;

		public FlatVector LinearVelocity
		{
			get => this.linearVelocity;
			internal set => this.linearVelocity = value;
		}

		public float AngularVelocity
		{
			get => this.angularVelocity;
			internal set => this.angularVelocity = value;
		}

		public float Angle => angle;

        public FlatBody(float density, float mass, float inertia, float restitution, float area,
						bool isStatic, float radius, float width, float height, FlatVector[] vertices, ShapeType shapeType)
        {
			this.position = FlatVector.Zero;
			this.linearVelocity = FlatVector.Zero;
			this.angle = 0f;
			this.angularVelocity = 0f;

			this.force = FlatVector.Zero;

			this.ShapeType = shapeType;
			this.Density = density;
			this.Mass = mass;
			this.InvMass = mass > 0f ? 1f / mass : 0f;
			this.Inertia = inertia;
			this.InvInertia = inertia > 0f ? 1f / inertia : 0f;
			this.Restitution = restitution;
			this.Area = area;
			this.IsStatic = isStatic;
			this.Radius = radius;
			this.Width = width;
			this.Height = height;
			this.StaticFriction = 0.6f;
			this.DynamicFriction = 0.4f;

			if (this.ShapeType is ShapeType.Box)
			{
				this.vertices = vertices;
				Triangles = CreateBoxTriangles();
				this.transformedVertices = new FlatVector[this.vertices.Length];
			}
			else
			{
				this.vertices = null;
				Triangles = null;
				this.transformedVertices = null;
			}

			this.transformUpdateRequired = true;
			this.aabbUpdateRequired = true;
		}

		private static FlatVector[] CreateBoxVertices(float width, float height)
		{
			float left = -width / 2f;
			float right = left + width;
			float bottom = -height / 2f;
			float top = bottom + height;

			FlatVector[] vertices = new FlatVector[4];
			vertices[0] = new FlatVector(left, top);
			vertices[1] = new FlatVector(right, top);
			vertices[2] = new FlatVector(right, bottom);
			vertices[3] = new FlatVector(left, bottom);

			return vertices;
		}

		private static int[] CreateBoxTriangles()
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

		public FlatVector[] GetTransformedVertices()
		{
			if (this.transformUpdateRequired)
			{
				FlatTransform transform = new FlatTransform(this.position, this.angle);

				for (int i = 0; i < this.vertices.Length; i++)
				{
					FlatVector v = this.vertices[i];
					this.transformedVertices[i] = FlatVector.Transform(v, transform);
				}
			}

			this.transformUpdateRequired = false;
			return this.transformedVertices;
		}

		public FlatAABB GetAABB()
		{
			if (this.aabbUpdateRequired)
			{
				float minX = float.MaxValue;
				float minY = float.MaxValue;
				float maxX = float.MinValue;
				float maxY = float.MinValue;

				if (this.ShapeType is ShapeType.Box)
				{
					FlatVector[] vertices = this.GetTransformedVertices();

					for (int i = 0; i < vertices.Length; i++)
					{
						FlatVector v = vertices[i];

						if (v.X < minX) { minX = v.X; }
						if (v.X > maxX) { maxX = v.X; }
						if (v.Y < minY) { minY = v.Y; }
						if (v.Y > maxY) { maxY = v.Y; }
					}
				}
				else if (this.ShapeType is ShapeType.Circle)
				{
					minX = this.Position.X - this.Radius;
					minY = this.Position.Y - this.Radius;
					maxX = this.Position.X + this.Radius;
					maxY = this.Position.Y + this.Radius;
				}
				else
				{
					throw new Exception("Unknown ShapeType.");
				}

				this.aabb = new FlatAABB(minX, minY, maxX, maxY);
			}

			this.aabbUpdateRequired = false;
			return this.aabb;
		}

		public void Step(float time, FlatVector gravity, int iterations)
		{
			if (this.IsStatic)
			{
				return;
			}

			time /= (float)iterations;

			// Force = Mass * Acc
			// Acc = Force / Mass

			//FlatVector acceleration = this.force / this.Mass;
			//this.linearVelocity += acceleration * time;

			this.linearVelocity += gravity * time;
			this.position += this.linearVelocity * time;

			this.angle += this.angularVelocity * time;

			this.force = FlatVector.Zero;

			// 움직인 경우 여러 프로퍼티를 갱신
			this.transformUpdateRequired = true;
			this.aabbUpdateRequired = true;
		}

		public void AddForce(FlatVector amount)
		{
			this.force = amount;
		}

		public void Move(FlatVector amount)
		{
			this.position += amount;
			this.transformUpdateRequired = true;
			this.aabbUpdateRequired = true;
		}

		public void MoveTo(FlatVector position)
		{
			this.position = position;
			this.transformUpdateRequired = true;
			this.aabbUpdateRequired = true;
		}

		public void Rotate(float amount)
		{
			this.angle += amount;
			this.transformUpdateRequired = true;
			this.aabbUpdateRequired = true;
		}

		public void RotateTo(float angle)
		{
			this.angle = angle;
			this.transformUpdateRequired = true;
			this.aabbUpdateRequired = true;
		}

		public static bool CreateCircleBody(float radius, float density, bool isStatic, float restitution, out FlatBody body, out string errorMessage)
		{
			body = null;
			errorMessage = string.Empty;

			float area = radius * radius * MathF.PI;

			if (area < FlatWorld.MinBodySize)
			{
				errorMessage = $"Circle radius is too small. Min circle area is {FlatWorld.MinBodySize}";
				return false;
			}

			if (area > FlatWorld.MaxBodySize)
			{
				errorMessage = $"Circle radius is too large. Max circle area is {FlatWorld.MaxBodySize}";
				return false;
			}

			if (density < FlatWorld.MinDensity)
			{
				errorMessage = $"Density is too small. Min density is {FlatWorld.MinDensity}";
				return false;
			}

			if (density > FlatWorld.MaxDensity)
			{
				errorMessage = $"Density is too large. Max density is {FlatWorld.MaxDensity}";
				return false;
			}

			restitution = FlatMath.Clamp(restitution, 0f, 1f);

			float mass = 0f;
			float inertia = 0;

			if (!isStatic)
			{
				// mass = area * depth * density
				mass = area * density;
				inertia = (1f / 2f) * mass * radius * radius;
			}

			body = new FlatBody(density, mass, inertia, restitution, area, isStatic, radius, 0f, 0f, null, ShapeType.Circle);
			return true;
		}

		public static bool CreateBoxBody(float width, float height, float density, bool isStatic, float restitution, out FlatBody body, out string errorMessage)
		{
			body = null;
			errorMessage = string.Empty;

			float area = width * height;

			if (area < FlatWorld.MinBodySize)
			{
				errorMessage = $"Area is too small. Min area is {FlatWorld.MinBodySize}";
				return false;
			}

			if (area > FlatWorld.MaxBodySize)
			{
				errorMessage = $"Area is too large. Max area is {FlatWorld.MaxBodySize}";
				return false;
			}

			if (density < FlatWorld.MinDensity)
			{
				errorMessage = $"Density is too small. Min density is {FlatWorld.MinDensity}";
				return false;
			}

			if (density > FlatWorld.MaxDensity)
			{
				errorMessage = $"Density is too large. Max density is {FlatWorld.MaxDensity}";
				return false;
			}

			restitution = FlatMath.Clamp(restitution, 0f, 1f);

			float mass = 0;
			float inertia = 0;

			if (!isStatic)
			{
				// mass = area * depth * density
				mass = area * density;
				inertia = (1f / 12) * mass * width * width + height * height;
			}

			FlatVector[] vertices = CreateBoxVertices(width, height);

			body = new FlatBody(density, mass, inertia, restitution, area, isStatic, 0f, width, height, vertices, ShapeType.Box);
			return true;
		}
	}
}
