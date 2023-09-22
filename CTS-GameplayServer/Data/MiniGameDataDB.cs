using System.Collections.Generic;
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
			_miniGameDataByMode.Add(GameModeType.CustomLobby, new()
			{
				ExecutionCutScene = ExecutionCutSceneType.None,
				GameTime = 1000000.0f,
				FeverTime = 1.0f,
			});
			_miniGameDataByMode.Add(GameModeType.RedHood, new()
			{
				ExecutionCutScene = ExecutionCutSceneType.RedHood,
				GameTime = 5.0f,
				FeverTime = 1.0f,
			});
			_miniGameDataByMode.Add(GameModeType.Dueoksini, new()
			{
				ExecutionCutScene = ExecutionCutSceneType.Dueoksini,
				GameTime = 5.0f,
				FeverTime = 1.0f,
			});

			return true;
		}

		public static MiniGameData GetMiniGameData(GameSceneIdentity gameId)
		{
			return _miniGameDataByMode[gameId.Mode];
		}
	}
}
