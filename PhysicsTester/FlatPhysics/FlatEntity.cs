using System.Numerics;
using PhysicsTester;

namespace FlatPhysics
{
	internal class FlatEntity
	{
		public readonly FlatBody Body;
		public readonly Color Color;

		public FlatEntity(FlatBody body)
		{
			Body = body;
			Color = WinformRandomHelper.RandomColor();
		}

		public FlatEntity(FlatBody body, Color color)
		{
			Body = body;
			Color = color;
		}

		public FlatEntity(FlatWorld world, float radius, bool isStatic, Vector2 position)
		{
			if (!FlatBody.CreateCircleBody(radius, 1f, isStatic, 0.5f, out FlatBody body, out string errorMessage))
			{
				throw new Exception(errorMessage);
			}

			body.MoveTo(position);
			Body = body;
			world.AddBody(Body);
			Color = WinformRandomHelper.RandomColor();
		}

		public FlatEntity(FlatWorld world, float width, float height, bool isStatic, Vector2 position)
		{
			if (!FlatBody.CreateBoxBody(width, height, 1f, isStatic, 0.5f, out FlatBody body, out string errorMessage))
			{
				throw new Exception(errorMessage);
			}

			body.MoveTo(position);
			Body = body;
			world.AddBody(Body);
			Color = WinformRandomHelper.RandomColor();
		}

		public void Draw(Renderer renderer)
		{
			Vector2 position = Body.Position;

			if (Body.ShapeType == ShapeType.Circle)
			{
				Vector2 vb = new Vector2(Body.Radius, 0f);
				float x = -MathF.Sin(Body.Angle);
				float y = MathF.Cos(Body.Angle);
				Vector2 rotationLine = new Vector2(x, y) * Body.Radius;

				renderer.DrawCircleFill(position, Body.Radius, Color);
				renderer.DrawCircle(position, Body.Radius, Color.White);
				renderer.DrawText(Body.Angle.ToString("F3"), position, Color.Orange);
				renderer.DrawLine(position, rotationLine + Body.Position, Color.White);
			}
			else if (Body.ShapeType == ShapeType.Box)
			{
				renderer.DrawPolygonFill(Body.GetTransformedVertices(), Body.Triangles, Color);
				renderer.DrawPolygon(Body.GetTransformedVertices(), Body.Triangles, Color.White);
				renderer.DrawText(Body.Angle.ToString("F3"), position, Color.Orange);
			}

			float centerPivotRadius = 5.0f / renderer.Zoom;

			renderer.DrawLine(position, position + new Vector2(1, 1) * centerPivotRadius, Color.SkyBlue);
			renderer.DrawLine(position, position + new Vector2(1, -1) * centerPivotRadius, Color.SkyBlue);
			renderer.DrawLine(position, position + new Vector2(-1, 1) * centerPivotRadius, Color.SkyBlue);
			renderer.DrawLine(position, position + new Vector2(-1, -1) * centerPivotRadius, Color.SkyBlue);
		}
	}
}
