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

		private static Dictionary<MiniGameIdentity, MiniGameMapData> _miniGameMapDataById = new();
		public static bool TryLoad()
		{
			if (!tryLoadByType(GameMapType.Square_Europe)) return false;
			if (!tryLoadByType(GameMapType.MiniGame_RedHood_0)) return false;
			if (!tryLoadByType(GameMapType.MiniGame_Dueoksini_0)) return false;

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
					_miniGameMapDataById.Add(mapData.MiniGameIdentity, mapData);
					return true;
				}

				_log.Fatal(result.Exception.Message);
				return false;
			}
		}

		public static MiniGameMapData GetMiniGameMapData(MiniGameIdentity gameId)
		{
			return _miniGameMapDataById[gameId];
		}

		public static MiniGameControllerBase CreateMiniGameControllerBy(this WorldManager worldManager, MiniGameIdentity gameId)
		{
			return gameId.ModeType switch
			{
				GameModeType.Lobby => worldManager.CreateObject<Lobby_MiniGameController>(),
				GameModeType.RedHood => worldManager.CreateObject<RedHood_MiniGameController>(),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		public static MiniGameIdentity Square = new MiniGameIdentity()
		{
			ModeType = GameModeType.Lobby,
			MapType = GameMapType.Square_Europe,
			Theme = GameMapTheme.Europe_France
		};

		public static MiniGameIdentity RedHood = new MiniGameIdentity()
		{
			ModeType = GameModeType.RedHood,
			MapType = GameMapType.MiniGame_RedHood_0,
			Theme = GameMapTheme.Europe_France
		};

		public static MiniGameIdentity Dueoksini = new MiniGameIdentity()
		{
			ModeType = GameModeType.Dueoksini,
			MapType = GameMapType.MiniGame_Dueoksini_0,
			Theme = GameMapTheme.EastAsia_Korea
		};
	}
}
