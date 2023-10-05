namespace CT.Common.Gameplay
{
	public enum GameplayState : byte
	{
		None = 0,

		/// <summary>준비중인 상태입니다.</summary>
		Ready,

		/// <summary>게임 시작 카운트 다운중입니다.</summary>
		StartCountdown,

		/// <summary>게임중입니다.</summary>
		Gameplay,

		/// <summary>피버 타임입니다.</summary>
		Gameplay_FeverTime,
		
		/// <summary>게임이 종료되었습니다.</summary>
		GameEnd,

		/// <summary>결과를 보는 중입니다.</summary>
		Result,

		/// <summary>탈락자 결과를 보는 중입니다.</summary>
		Execute,

		/// <summary>맵 투표중입니다.</summary>
		VoteMap,
	}

	public static class GameplayStateExtension
	{
		public static bool IsGameplay(this GameplayState value)
		{
			return value >= GameplayState.Gameplay && value < GameplayState.GameEnd;
		}
	}
}
