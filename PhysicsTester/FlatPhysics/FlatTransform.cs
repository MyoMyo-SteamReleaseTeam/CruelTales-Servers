using System.Numerics;

namespace FlatPhysics
{
	public readonly struct FlatTransform
	{
		public readonly float PositionX;
		public readonly float PositionY;
		public readonly float Sin;
		public readonly float Cos;

		public readonly static FlatTransform Zero = new FlatTransform(0f, 0f, 0f);
		public Vector2 Position => new Vector2(PositionX, PositionY);

        public FlatTransform(Vector2 position, float angle)
        {
			this.PositionX = position.X;
			this.PositionY = position.Y;
			this.Sin = MathF.Sin(angle);
			this.Cos = MathF.Cos(angle);
        }

		public FlatTransform(float x, float y, float angle)
		{
			this.PositionX = x;
			this.PositionY = y;
			this.Sin = MathF.Sin(angle);
			this.Cos = MathF.Cos(angle);
		}
	}
}
