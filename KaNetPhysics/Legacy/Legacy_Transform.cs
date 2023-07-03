using System.Numerics;

namespace KaNet.Physics.Legacy
{
	public readonly struct Legacy_Transform
	{
		public readonly float PositionX;
		public readonly float PositionY;
		public readonly float Sin;
		public readonly float Cos;

		public readonly static Legacy_Transform Zero = new Legacy_Transform(0f, 0f, 0f);
		public Vector2 Position => new Vector2(PositionX, PositionY);

		public Legacy_Transform(Vector2 position, float angle)
		{
			PositionX = position.X;
			PositionY = position.Y;
			Sin = MathF.Sin(angle);
			Cos = MathF.Cos(angle);
		}

		public Legacy_Transform(float x, float y, float angle)
		{
			PositionX = x;
			PositionY = y;
			Sin = MathF.Sin(angle);
			Cos = MathF.Cos(angle);
		}
	}
}
