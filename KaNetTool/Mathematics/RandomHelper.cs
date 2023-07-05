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
		return NextInteger(0, 2) == 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int NextInteger(int min, int max)
	{
		return _random.Next(min, max);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float NextSingle(float min, float max)
	{
		return KaMath.Lerp(min, max, _random.NextSingle());
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
	public static Vector2 NextVectorTo(Vector2 b)
	{
		return new Vector2(_random.NextSingle() * b.X,
						   _random.NextSingle() * b.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 NextVectorTo(Vector3 b)
	{
		return new Vector3(_random.NextSingle() * b.X,
						   _random.NextSingle() * b.Y,
						   _random.NextSingle() * b.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 NextVectorBetween(Vector2 a, Vector2 b)
	{
		return new Vector2(NextSingle(a.X, b.X),
						   NextSingle(a.Y, b.Y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 NextVectorBetween(Vector3 a, Vector3 b)
	{
		return new Vector3(NextSingle(a.X, b.X),
						   NextSingle(a.Y, b.Y),
						   NextSingle(a.Z, b.Z));
	}
}
