using System;
using System.Numerics;
using System.Runtime.CompilerServices;

public static class RandomHelper
{
	private static Random _random = new Random();

#if UNITY_2021
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float NextSingle(this Random random)
	{
		return (float)random.NextDouble();
	}
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NextBollean()
	{
		return NextInt(0, 2) == 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int NextInt(int min, int max)
	{
		return _random.Next(min, max);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int NextInt(int max)
	{
		return _random.Next(0, max);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float NextSingle(float min, float max)
	{
		return KaMath.Lerp(min, max, _random.NextSingle());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float NextSingle(float max)
	{
		return KaMath.Lerp(0, max, _random.NextSingle());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 NextVector2()
	{
		return new Vector2(_random.NextSingle(),
						   _random.NextSingle());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 NextVector3()
	{
		return new Vector3(_random.NextSingle(),
						   _random.NextSingle(),
						   _random.NextSingle());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 NextVector2(Vector2 max)
	{
		return new Vector2(_random.NextSingle() * max.X,
						   _random.NextSingle() * max.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 NextVector3(Vector3 max)
	{
		return new Vector3(_random.NextSingle() * max.X,
						   _random.NextSingle() * max.Y,
						   _random.NextSingle() * max.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 NextVector2(Vector2 min, Vector2 max)
	{
		return new Vector2(NextSingle(min.X, max.X),
						   NextSingle(min.Y, max.Y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 NextVector3(Vector3 min, Vector3 max)
	{
		return new Vector3(NextSingle(min.X, max.X),
						   NextSingle(min.Y, max.Y),
						   NextSingle(min.Z, max.Z));
	}
}
