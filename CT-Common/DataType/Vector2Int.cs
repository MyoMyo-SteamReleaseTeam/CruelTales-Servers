#if NET
using System;

namespace CT.Common.DataType
{
	public readonly struct Vector2Int : IEquatable<Vector2Int>
	{
		public readonly int X;
		public readonly int Y;

		public Vector2Int()
		{
			X = 0;
			Y = 0;
		}

		public Vector2Int(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Vector2Int(float x, float y)
		{
			X = (int)x;
			Y = (int)y;
		}

		public static Vector2Int operator +(Vector2Int lhs, Vector2Int rhs)
		{
			return new Vector2Int(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}

		public static Vector2Int operator -(Vector2Int lhs, Vector2Int rhs)
		{
			return new Vector2Int(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		public static Vector2Int operator *(Vector2Int lhs, int rhs)
		{
			return new Vector2Int(lhs.X * rhs, lhs.Y * rhs);
		}

		public static Vector2Int operator /(Vector2Int lhs, int rhs)
		{
			return new Vector2Int(lhs.X / rhs, lhs.Y / rhs);
		}

		public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
		{
			return lhs.X == rhs.X && lhs.Y == rhs.Y;
		}

		public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
		{
			return lhs.X != rhs.X || lhs.Y != rhs.Y;
		}

		public bool Equals(Vector2Int other)
		{
			return this == other;
		}

		public override bool Equals(object? obj)
		{
			return obj is Vector2Int other && this == other;
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}
	}
}
#endif
