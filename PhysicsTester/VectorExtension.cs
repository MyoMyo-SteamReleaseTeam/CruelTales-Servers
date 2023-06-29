using System.Numerics;
using FlatPhysics;

namespace PhysicsTester
{
	public static class VectorExtension
	{
		public static Vector2 FlipY(this Vector2 value)
		{
			return new Vector2(value.X, -value.Y);
		}

		public static Vector2 Transform(this Vector2 v, FlatTransform transform)
		{
			float rx = transform.Cos * v.X - transform.Sin * v.Y;
			float ry = transform.Sin * v.X + transform.Cos * v.Y;

			float tx = rx + transform.PositionX;
			float ty = ry + transform.PositionY;

			return new Vector2(tx, ty);

			//return new FlatVector(transform.Cos * v.X - transform.Sin * v.Y + transform.PositionX,
			//					  transform.Sin * v.X + transform.Cos * v.Y + transform.PositionY);
		}
	}
}
