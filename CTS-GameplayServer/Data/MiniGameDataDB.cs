using System.Collections.Generic;
using System.Numerics;
using CT.Common.Gameplay;
using log4net;

namespace CTS.Instance.Data
{
	public static class MiniGameDataDB
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MiniGameDataDB));

		private static readonly Dictionary<GameModeType, MiniGameData> _miniGameDataByMode = new();

		public static bool TryLoad()
		{
			_miniGameDataByMode.Clear();
			_miniGameDataByMode.Add(GameModeType.CustomLobby, new()
			{
				GameTime = 1000000.0f,
				FeverTime = new Vector2(0, 0),
				ExecutionCutScene = ExecutionCutSceneType.None,
				CompetitionType = CompetitionType.None,
			});
			_miniGameDataByMode.Add(GameModeType.RedHood, new()
			{
				GameTime = 240,
				FeverTime = new Vector2(30, 60),
				ExecutionCutScene = ExecutionCutSceneType.RedHood,
				CompetitionType = CompetitionType.Individual,
			});
			_miniGameDataByMode.Add(GameModeType.Dueoksini, new()
			{
				GameTime = 240,
				FeverTime = new Vector2(60, 90),
				ExecutionCutScene = ExecutionCutSceneType.Dueoksini,
				CompetitionType = CompetitionType.Team,
			});

			return true;
		}

		public static MiniGameData GetMiniGameData(GameSceneIdentity gameId)
		{
			return _miniGameDataByMode[gameId.Mode];
		}
	}
}
