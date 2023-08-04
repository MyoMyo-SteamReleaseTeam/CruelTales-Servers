using System;
using System.Numerics;
using System.Runtime.CompilerServices;

public static class KaMath
{
	public const float RADIAN_TO_DEGREE = 180.0f / MathF.PI;
	public const float DEGREE_TO_RADIAN = MathF.PI / 180.0f;
	public const float RADIAN = 180 / MathF.PI;

	/// <summary>각도를 라디안각으로 변환합니다.</summary>
	/// <param name="angle">각도</param>
	/// <returns>라디안각</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DegreeToRadian(float angle)
	{
		return angle * RADIAN_TO_DEGREE;
	}

	/// <summary>라디안각을 각도로 변환합니다.</summary>
	/// <param name="angle">라디안각</param>
	/// <returns>각도</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float RadianToDegree(float angle)
	{
		return angle * DEGREE_TO_RADIAN;
	}

	/// <summary>값을 스냅합니다.</summary>
	/// <param name="value">스냅할 값입니다.</param>
	/// <param name="snap">스냅 크기입니다.</param>
	/// <returns>스냅된 값입니다.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float SnapBy(float value, float snap)
	{
		return (int)(value / snap) * snap;
	}

	/// <summary>값을 스냅합니다.</summary>
	/// <param name="value">스냅할 값입니다.</param>
	/// <param name="snap">스냅 크기입니다.</param>
	/// <returns>스냅된 값입니다.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 SnapBy(Vector2 value, float snap)
	{
		float x = SnapBy(value.X, snap);
		float y = SnapBy(value.Y, snap);
		return new Vector2(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Remap(float inValue, float inMin, float inMax, float outMin, float outMax)
	{
		return outMin + (inValue - inMin) / (inMax - inMin) * (outMax - outMin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Clamp(float value, float min, float max)
	{
		if (value < min)
			return min;

		if (value > max)
			return max;

		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Clamp(int value, int min, int max)
	{
		if (value < min)
			return min;

		if (value > max)
			return max;

		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Lerp(float a, float b, float t)
	{
		return a + (b - a) * t;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Cross(Vector2 a, Vector2 b)
	{
		return a.X * b.Y - a.Y * b.X;
	}
}
