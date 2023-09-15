using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Common.Tools.Data;
using CTS.Instance.Gameplay;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Data
{
	public static class MiniGameMapDataDB
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MiniGameMapDataDB));

		private static Dictionary<GameSceneIdentity, GameSceneMapData> _miniGameMapDataById = new();
		public static bool TryLoad()
		{
			if (!tryLoadByType(GameMapType.Square_Europe)) return false;
			if (!tryLoadByType(GameMapType.RedHood_0)) return false;
			if (!tryLoadByType(GameMapType.Dueoksini_0)) return false;

			return true;

			bool tryLoadByType(GameMapType mapType)
			{
				_log.Info($"Try load map data {mapType}");
				string path = $"Data/MiniGameMapData/{mapType}.json";
				var result = JsonHandler.TryReadObject<GameSceneMapData>(path);
				if (result.ResultType == JobResultType.Success)
				{
					GameSceneMapData mapData = result.Value;
					mapData.Initialize();
					_miniGameMapDataById.Add(mapData.GameSceneIdentity, mapData);
					return true;
				}

				_log.Fatal(result.Exception.Message);
				return false;
			}
		}

		public static GameSceneMapData GetMiniGameMapData(GameSceneIdentity gameId)
		{
			return _miniGameMapDataById[gameId];
		}

		public static SceneControllerBase CreateSceneControllerBy(this WorldManager worldManager, GameSceneIdentity gameId)
		{
			return gameId.Mode switch
			{
				GameModeType.CustomLobby => worldManager.CreateObject<CustomLobby_SceneController>(),
				GameModeType.RedHood => worldManager.CreateObject<RedHood_MiniGameController>(),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		public static GameSceneIdentity Square = new GameSceneIdentity()
		{
			Mode = GameModeType.CustomLobby,
			Map = GameMapType.Square_Europe,
			Theme = GameMapThemeType.Europe_France
		};

		public static GameSceneIdentity RedHood = new GameSceneIdentity()
		{
			Mode = GameModeType.RedHood,
			Map = GameMapType.RedHood_0,
			Theme = GameMapThemeType.Europe_France
		};

		public static GameSceneIdentity Dueoksini = new GameSceneIdentity()
		{
			Mode = GameModeType.Dueoksini,
			Map = GameMapType.Dueoksini_0,
			Theme = GameMapThemeType.EastAsia_Korea
		};
	}
}
