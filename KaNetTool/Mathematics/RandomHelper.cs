using System.Runtime.CompilerServices;

public static class RandomHelper
{
	private static Random _random = new Random();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool RandomBollean()
	{
		return RandomInteger(0, 2) == 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int RandomInteger(int min, int max)
	{
		return _random.Next(min, max);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float RandomSingle(float min, float max)
	{
		return KaMath.Lerp(min, max, _random.NextSingle());
	}
}
