using System.Numerics;
using System.Security.Cryptography;
using PhysicsTester;

namespace KaNet.Physics
{
	internal class KaEntity
	{
		public readonly KaRigidBody Body;
		public Color Color { get; set; }
		private static int[] _vertexIndices = new int[6] { 0, 1, 2, 0, 2, 3 };

		public KaEntity(KaRigidBody body)
		{
			Body = body;
			Color = WinformRandomHelper.RandomColor();
		}

		public KaEntity(KaRigidBody body, Color color)
		{
			Body = body;
			Color = color;
		}

		public KaEntity(KaPhysicsWorld world, float radius, bool isStatic,
						Vector2 position = default, float rotation = 0)
		{
			Body = KaPhysicsWorld.CreateCircle(radius, isStatic);
			Body.MoveTo(position);
			Body.Rotate(rotation);
			world.AddRigidBody(Body);
			Color = WinformRandomHelper.RandomColor();
		}

		public KaEntity(KaPhysicsWorld world, float width, float height, bool isStatic,
						Vector2 position = default, float rotation = 0)
		{
			if (rotation == 0)
			{
				Body = KaPhysicsWorld.CreateBox(width, height, isStatic);
			}
			else
			{
				Body = KaPhysicsWorld.CreateBoxOBB(width, height, rotation, isStatic);
			}
			Body.MoveTo(position);
			world.AddRigidBody(Body);
			Color = WinformRandomHelper.RandomColor();
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
						renderer.DrawBoxFill(Body.Position, Body.Width, Body.Height, Color);
						renderer.DrawBox(Body.Position, Body.Width, Body.Height, Color.White);
						renderer.DrawText(Body.Angle.ToString("F3"), position, Color.Orange);
					}
					break;

				case PhysicsShapeType.Box_OBB:
					{
						renderer.DrawPolygonFill(Body.GetTransformedVertices(), _vertexIndices, Color);
						renderer.DrawPolygon(Body.GetTransformedVertices(), _vertexIndices, Color.White);
						renderer.DrawText(Body.Angle.ToString("F3"), position, Color.Orange);
					}
					break;

				case PhysicsShapeType.Circle:
					{
						float x = -MathF.Sin(Body.Angle);
						float y = MathF.Cos(Body.Angle);
						Vector2 rotationLine = new Vector2(x, y) * Body.Radius;

						renderer.DrawCircleFill(position, Body.Radius, Color);
						renderer.DrawCircle(position, Body.Radius, Color.White);
						renderer.DrawText(Body.Angle.ToString("F3"), position, Color.Orange);
						renderer.DrawLine(position, rotationLine + Body.Position, Color.White);
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
		}
	}
}
