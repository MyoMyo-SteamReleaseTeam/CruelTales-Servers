using System.Numerics;
using KaNet.Physics.RigidBodies;
using PhysicsTester;

namespace KaNet.Physics
{
	internal class KaEntity
	{
		public PhysicsWorld World { get; private set; }
		public int Id { get; private set; }

		public readonly RigidBody Body;
		public Color Color { get; set; }
		private static int[] _vertexIndices = new int[6] { 0, 1, 2, 0, 2, 3 };

		public KaEntity(PhysicsWorld world, float radius, bool isStatic,
						Vector2 position = default, float rotation = 0)
		{
			World = world;

			Body = World.CreateCircle(radius, isStatic);
			Body.MoveTo(position);
			Color = WinformRandomHelper.RandomColor();
		}

		public KaEntity(PhysicsWorld world, float width, float height, bool isStatic,
						Vector2 position = default, float rotation = 0)
		{
			World = world;

			if (rotation == 0)
			{
				Body = World.CreateBoxAABB(width, height, isStatic);
			}
			else
			{
				Body = World.CreateBoxOBB(width, height, rotation, isStatic);
			}
			Body.MoveTo(position);
			Color = WinformRandomHelper.RandomColor();
		}

		public void SetID(int id)
		{
			this.Id = id;
		}

		public void Draw(Renderer renderer)
		{
			Vector2 position = Body.Position;

			switch (Body.ShapeType)
			{
				case PhysicsShapeType.None:
					break;

				case PhysicsShapeType.Box_AABB:
					{
						BoxAABBRigidBody body = (BoxAABBRigidBody)Body;
						renderer.DrawBoxFill(body.Position, body.Width, body.Height, Color);
						renderer.DrawBox(body.Position, body.Width, body.Height, Color.White);
					}
					break;

				case PhysicsShapeType.Box_OBB:
					{
						BoxOBBRigidBody body = (BoxOBBRigidBody)Body;
						renderer.DrawPolygonFill(body.GetTransformedVertices(), _vertexIndices, Color);
						renderer.DrawPolygon(body.GetTransformedVertices(), _vertexIndices, Color.White);
					}
					break;

				case PhysicsShapeType.Circle:
					{
						CircleRigidBody body = (CircleRigidBody)Body;

						float x = -MathF.Sin(body.Angle);
						float y = MathF.Cos(body.Angle);
						Vector2 rotationLine = new Vector2(x, y) * body.Radius;

						renderer.DrawCircleFill(position, body.Radius, Color);
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

			//renderer.DrawText(Body.Angle.ToString("F3"), position, Color.Orange);
			renderer.DrawText(this.Id.ToString(), position, Color.LightGreen, Color.Black,
							  isCenter: true, font: renderer.DefaultFont16);
		}
	}
}
