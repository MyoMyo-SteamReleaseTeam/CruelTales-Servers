namespace CT.Common.Gameplay
{
	public enum Faction : byte
	{
		None = 0,

		Red = 1,
		Blue = 2,
		Green = 3,

		/// <summary>시스템입니다.</summary>
		System,

		/// <summary>중립입니다.</summary>
		Neutral,

		/// <summary>관전자입니다.</summary>
		Speculator,
	}

	public static class FactionExtension
	{
		public static bool IsSameFaction(this Faction value, Faction other)
		{
			return value == other;
		}
	}
}
