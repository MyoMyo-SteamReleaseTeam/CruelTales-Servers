using System.Numerics;

internal static class KaMath
{
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

	public static float Lerp(float a, float b, float t)
	{
		return a + (b - a) * t;
	}

	public static float Cross(Vector2 a, Vector2 b)
	{
		return a.X * b.Y - a.Y * b.X;
	}

	/// <summary>
	/// Equal to 1/2 of a millimeter
	/// </summary>
	public static readonly float VerySmallAmount = 0.001f;

	public static bool NearlyEqual(float a, float b)
	{
		return MathF.Abs(a - b) < VerySmallAmount;
	}

	public static bool NearlyEqual(Vector2 a, Vector2 b)
	{
		return Vector2.DistanceSquared(a, b) < VerySmallAmount * VerySmallAmount;
		//return NearlyEqual(a.X, b.X) && NearlyEqual(a.Y, b.Y);
	}
}
