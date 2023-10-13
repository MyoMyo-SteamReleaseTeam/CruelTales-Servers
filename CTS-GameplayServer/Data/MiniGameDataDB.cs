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
				FeverTimeRatioRange = new Vector2(0, 0),
				ExecutionCutScene = ExecutionCutSceneType.None,
				CompetitionType = CompetitionType.None,
			});
			_miniGameDataByMode.Add(GameModeType.RedHood, new()
			{
				GameTime = 500,
				FeverTimeRatioRange = new Vector2(0.3f, 0.5f),
				ExecutionCutScene = ExecutionCutSceneType.RedHood,
				CompetitionType = CompetitionType.Individual,
			});
			_miniGameDataByMode.Add(GameModeType.Dueoksini, new()
			{
				GameTime = 500,
				FeverTimeRatioRange = new Vector2(0.15f, 0.3f),
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
