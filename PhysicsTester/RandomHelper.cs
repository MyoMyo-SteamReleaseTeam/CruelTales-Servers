public static class WinformRandomHelper
{
	private static Random _random = new Random();

#if WINFORM
	public static Color RandomColor()
	{
		return Color.FromArgb(_random.Next(255), _random.Next(255), _random.Next(255));
	}
#endif
}
