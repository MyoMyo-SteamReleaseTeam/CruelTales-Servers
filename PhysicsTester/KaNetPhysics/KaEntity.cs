using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using KaNet.Physics.RigidBodies;
using PhysicsTester;

namespace KaNet.Physics
{
	internal class KaEntity
	{
		[AllowNull]
		public KaPhysicsWorld World { get; private set; }
		public int ID { get; private set; }

		[AllowNull]
		public readonly KaRigidBody Body;
		public Color Color { get; set; }
		private static int[] _vertexIndices = new int[6] { 0, 1, 2, 0, 2, 3 };

		private int CollideID = -1;

		private KaEntity() { }

		/// <summary>원 RigidBody를 가진 Entity를 생성합니다.</summary>
		private KaEntity(KaPhysicsWorld world, float radius,
						 bool isStatic, PhysicsLayerMask layerMask,
						 Vector2 position = default, float rotation = 0)
		{
			World = world;

			Body = World.CreateCircle(radius, isStatic, layerMask);
			Body.MoveTo(position);
			Color = WinformRandomHelper.RandomColor();
		}

		/// <summary>사각형 RigidBody를 가진 Entity를 생성합니다.</summary>
		private KaEntity(KaPhysicsWorld world, float width, float height,
						 bool isStatic, PhysicsLayerMask layerMask,
						 Vector2 position = default, float rotation = 0)
		{
			World = world;
			Body = rotation == 0 ?
				World.CreateBoxAABB(width, height, isStatic, layerMask) :
				World.CreateBoxOBB(width, height, rotation, isStatic, layerMask);
			Body.MoveTo(position);
			Color = WinformRandomHelper.RandomColor();
		}

		/// <summary>원 RigidBody를 가진 Entity를 생성합니다.</summary>
		public static KaEntity CreateCircleEntity(KaPhysicsWorld world, float radius,
												  bool isStatic, PhysicsLayerMask layerMask,
												  Vector2 position = default)
		{
			return new KaEntity(world, radius, isStatic, layerMask, position);
		}

		/// <summary>축 정렬된 사각형 RigidBody를 가진 Entity를 생성합니다.</summary>
		public static KaEntity CreateAABBEntity(KaPhysicsWorld world, float width, float height,
												bool isStatic, PhysicsLayerMask layerMask,
												Vector2 position = default)
		{
			return new KaEntity(world, width, height, isStatic, layerMask, position);
		}

		/// <summary>회전 가능한 사각형 RigidBody를 가진 Entity를 생성합니다.</summary>
		public static KaEntity CreateOBBEntity(KaPhysicsWorld world, float width, float height,
											   float rotation, bool isStatic, 
											   PhysicsLayerMask layerMask, Vector2 position = default)
		{
			return new KaEntity(world, width, height, isStatic, layerMask, position, rotation);
		}

		public void SetID(int id)
		{
			this.ID = id;
			Body.Initialize(ID, OnCollisionWith);
		}

		private void OnCollisionWith(int obj)
		{
			CollideID = obj;
		}

		public void Draw(Renderer renderer)
		{
			Color fillColor = Color;

			if (CollideID >= 0)
			{
				fillColor = Color.Red;
			}

			Vector2 position = Body.Position;

			switch (Body.ShapeType)
			{
				case KaPhysicsShapeType.None:
					break;

				case KaPhysicsShapeType.Box_AABB:
					{
						BoxAABBRigidBody body = (BoxAABBRigidBody)Body;
						renderer.DrawBoxFill(body.Position, body.Width, body.Height, fillColor);
						renderer.DrawBox(body.Position, body.Width, body.Height, Color.White);
					}
					break;

				case KaPhysicsShapeType.Box_OBB:
					{
						BoxOBBRigidBody body = (BoxOBBRigidBody)Body;
						renderer.DrawPolygonFill(body.GetTransformedVertices(), _vertexIndices, fillColor);
						renderer.DrawPolygon(body.GetTransformedVertices(), _vertexIndices, Color.White);
					}
					break;

				case KaPhysicsShapeType.Circle:
					{
						CircleRigidBody body = (CircleRigidBody)Body;

						float x = -MathF.Sin(body.Rotation);
						float y = MathF.Cos(body.Rotation);
						Vector2 rotationLine = new Vector2(x, y) * body.Radius;

						renderer.DrawCircleFill(position, body.Radius, fillColor);
						renderer.DrawCircle(position, body.Radius, Color.White);
						renderer.DrawLine(position, rotationLine + body.Position, Color.White);
					}
					break;

				default:
					break;
			}

			float centerPivotRadius = 5.0f / renderer.Zoom;

			renderer.DrawLine(position, position + new Vector2(1, 1) * centerPivotRadius, Color.SkyBlue);
			renderer.DrawLine(position, position + new Vector2(1, -1) * centerPivotRadius, Color.SkyBlue);
			renderer.DrawLine(position, position + new Vector2(-1, 1) * centerPivotRadius, Color.SkyBlue);
			renderer.DrawLine(position, position + new Vector2(-1, -1) * centerPivotRadius, Color.SkyBlue);

			renderer.DrawText(Body.Rotation.ToString("F3"), position - new Vector2(0, 1f), Color.Orange,
							  isCenter: true);

			if (CollideID >= 0)
			{
				renderer.DrawText(CollideID.ToString(), position, Color.Black, Color.White,
								  isCenter: true, font: renderer.DefaultFont16);
			}
			else
			{
				renderer.DrawText(this.ID.ToString(), position, Color.LightGreen, Color.Black,
								  isCenter: true, font: renderer.DefaultFont16);
			}

			CollideID = -1;
		}
	}
}
