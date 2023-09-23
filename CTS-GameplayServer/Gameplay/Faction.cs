namespace CTS.Instance.Gameplay
{
	public enum Faction : byte
	{
		/// <summary>시스템입니다.</summary>
		System,

		/// <summary>중립입니다.</summary>
		Neutral,

		/// <summary>관전자입니다.</summary>
		Speculator,

		Red,
		Bule,
		Green,

		// RedHood
		RedHood_RedHood,
		RedHood_Wolf,
		RedHood_Freeman,

		// Dueoksini
		// Horus
	}

	public static class FactionExtension
	{
		public static bool IsSameFaction(this Faction value, Faction other)
		{
			return value == other;
		}
	}
}
