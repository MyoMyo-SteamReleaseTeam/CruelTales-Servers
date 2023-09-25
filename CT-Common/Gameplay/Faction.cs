﻿namespace CT.Common.Gameplay
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
		Blue,
		Green,
	}

	public static class FactionExtension
	{
		public static bool IsSameFaction(this Faction value, Faction other)
		{
			return value == other;
		}
	}
}