using System.Numerics;

public static class KaMath
{
	/// <summary>값을 스냅합니다.</summary>
	/// <param name="value">스냅할 값입니다.</param>
	/// <param name="snap">스냅 크기입니다.</param>
	/// <returns>스냅된 값입니다.</returns>
	public static float SnapBy(float value, float snap)
	{
		return (int)(value / snap) * snap;
	}

	/// <summary>값을 스냅합니다.</summary>
	/// <param name="value">스냅할 값입니다.</param>
	/// <param name="snap">스냅 크기입니다.</param>
	/// <returns>스냅된 값입니다.</returns>
	public static Vector2 SnapBy(Vector2 value, float snap)
	{
		float x = SnapBy(value.X, snap);
		float y = SnapBy(value.Y, snap);
		return new Vector2(x, y);
	}

	public static float Remap(float inValue, float inMin, float inMax, float outMin, float outMax)
	{
		return outMin + (inValue - inMin) / (inMax - inMin) * (outMax - outMin);
	}

	public static float Clamp(float value, float min, float max)
	{
		if (value < min)
			return min;

		if (value > max)
			return max;

		return value;
	}

	public static int Clamp(int value, int min, int max)
	{
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

	public static readonly float PhysicsFloatElapsed = 0.001f;

	public static bool NearlyEqual(float a, float b)
	{
		return MathF.Abs(a - b) < PhysicsFloatElapsed;
	}

	public static bool NearlyEqual(Vector2 a, Vector2 b)
	{
		//return Vector2.DistanceSquared(a, b) < PhysicsFloatElapsed * PhysicsFloatElapsed;
		return NearlyEqual(a.X, b.X) && NearlyEqual(a.Y, b.Y);
	}
}
