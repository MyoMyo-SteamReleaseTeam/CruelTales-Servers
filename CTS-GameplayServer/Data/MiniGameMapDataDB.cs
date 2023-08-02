using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Common.Tools.Data;
using log4net;

namespace CTS.Instance.Data
{
	public static class MiniGameMapDataDB
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MiniGameMapDataDB));

		private static Dictionary<GameMapType, MiniGameMapData> _miniGameMapDataByType = new();

		public static bool TryLoad()
		{
			if (!tryLoadByType(GameMapType.MiniGame_RedHood_0)) return false;

			return true;

			bool tryLoadByType(GameMapType mapType)
			{
				_log.Info($"Try load map data {mapType}");
				string path = $"Data/MiniGameMapData/{mapType}.json";
				var result = JsonHandler.TryReadObject<MiniGameMapData>(path);
				if (result.ResultType == JobResultType.Success)
				{
					MiniGameMapData mapData = result.Value;
					mapData.Initialize();
					_miniGameMapDataByType.Add(mapType, mapData);
					return true;
				}

				_log.Fatal(result.Exception.Message);
				return false;
			}
		}

		public static MiniGameMapData GetMiniGameMapData(GameMapType mapType)
		{
			return _miniGameMapDataByType[mapType];
		}
	}
}
