using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FlatPhysics
{
	public static class FlatMath
	{
		/// <summary>
		/// Equal to 1/2 of a millimeter
		/// </summary>
		public static readonly float VerySmallAmount = 0.001f;

		public static float Clamp(float value, float min, float max)
		{
			if (min == max)
				return min;

			if (min > max)
				throw new ArgumentOutOfRangeException("min is greater than the max.");

			if (value < min)
				return min;

			if (value > max)
				return max;

			return value;
		}

		public static int Clamp(int value, int min, int max)
		{
			if (min == max)
				return min;

			if (min > max)
				throw new ArgumentOutOfRangeException("min is greater than the max.");

			if (value < min)
				return min;

			if (value > max)
				return max;

			return value;
		}

		public static float Length(Vector2 v)
		{
			return MathF.Sqrt(v.X * v.X + v.Y * v.Y);
		}

		public static float LengthSquared(Vector2 v)
		{
			return v.X * v.X + v.Y * v.Y;
		}

		public static float Distance(Vector2 a, Vector2 b)
		{
			float dx = a.X - b.X;
			float dy = a.Y - b.Y;
			return MathF.Sqrt(dx * dx + dy * dy);
		}

		public static float DistanceSquared(Vector2 a, Vector2 b)
		{
			float dx = a.X - b.X;
			float dy = a.Y - b.Y;
			return dx * dx + dy * dy;
		}

		public static Vector2 Normalize(Vector2 v)
		{
			float len = FlatMath.Length(v);
			return new Vector2(v.X / len, v.Y / len);
		}

		public static float Dot(Vector2 a, Vector2 b)
		{
			return a.X * b.X + a.Y * b.Y;
		}

		public static float Cross(Vector2 a, Vector2 b)
		{
			return a.X * b.Y - a.Y * b.X;
		}

		public static float Lerp(float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		public static bool NearlyEqual(float a, float b)
		{
			return MathF.Abs(a - b) < VerySmallAmount;
		}

		public static bool NearlyEqual(Vector2 a, Vector2 b)
		{
			return FlatMath.DistanceSquared(a, b) < VerySmallAmount * VerySmallAmount;
			//return NearlyEqual(a.X, b.X) && NearlyEqual(a.Y, b.Y);
		}
	}
}
