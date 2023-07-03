using System.Numerics;
using KaNet.Physics.Legacy;

public static class Legacy_VectorExtension
{
	public static Vector2 Transform(this Vector2 v, Legacy_Transform transform)
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
