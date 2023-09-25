using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Infos;
using CTS.Instance.Gameplay;

namespace CTS.Instance.SyncObjects
{
	public struct DueoksiniItemInfo
	{
		public FieldItemType ItemType;
		public int SpwanCount;
		public int Score;
	}

	public static class DueoksiniHelper
	{
		private readonly static DueoksiniItemInfo[] _itemInfos = new DueoksiniItemInfo[]
		{
			new() { ItemType = FieldItemType.Dueoksini_Rice, SpwanCount = 8, Score = 2 },
			new() { ItemType = FieldItemType.Dueoksini_Kimchi, SpwanCount = 8, Score = 2 },
			new() { ItemType = FieldItemType.Dueoksini_TaroSoup, SpwanCount = 8, Score = 3 },
			new() { ItemType = FieldItemType.Dueoksini_Japchae, SpwanCount = 4, Score = 4 },
			new() { ItemType = FieldItemType.Dueoksini_Jeon, SpwanCount = 4, Score = 4 },
			new() { ItemType = FieldItemType.Dueoksini_Yukjeon, SpwanCount = 4, Score = 5 },
			new() { ItemType = FieldItemType.Dueoksini_RawMeat, SpwanCount = 4, Score = 5 },
			new() { ItemType = FieldItemType.Dueoksini_SteamedSeaBream, SpwanCount = 2, Score = 6 },
			new() { ItemType = FieldItemType.Dueoksini_Galbijjim, SpwanCount = 2, Score = 7 },
			new() { ItemType = FieldItemType.Dueoksini_Gujeolpan, SpwanCount = 2, Score = 7 },
			new() { ItemType = FieldItemType.Dueoksini_Sinseonro, SpwanCount = 1, Score = 10 },
		};

		public static int _totalSpwanItemCount = 0;

		public static int GetTotalSpwanItemCount()
		{
			if (_totalSpwanItemCount > 0)
			{
				return _totalSpwanItemCount;
			}

			_totalSpwanItemCount = 0;

			int count = (int)(FieldItemType.Dueoksini_Gujeolpan - FieldItemType.Dueoksini_Rice);
			for (int i = 0; i < count; i++)
			{
				var info = _itemInfos[i];
				_totalSpwanItemCount += info.SpwanCount;
			}

			return _totalSpwanItemCount;
		}

		public static bool TryGetItemInfo(FieldItemType fieldItemType, out DueoksiniItemInfo itemInfo)
		{
			if (!fieldItemType.IsBaseType(FieldItemBaseType.Dueoksini))
			{
				itemInfo = new();
				return false;
			}

			int index = (int)(fieldItemType - FieldItemType.Dueoksini) - 1;
			itemInfo = _itemInfos[index];
			return true;
		}
	}

	public partial class Dueoksini_MiniGameController : MiniGameControllerBase
	{
		private DueoksiniTable? _redTeamTable;
		private DueoksiniTable? _blueTeamTable;

		private Dictionary<NetworkIdentity, FieldItem> _itemById;

		public override void Constructor()
		{
			base.Constructor();
			_itemById = new(64);
		}

		public override void OnCreated()
		{
			base.OnCreated();

			// Create by pivots
			// Create: red, blue tables
			// Create: high class items
			foreach (var pivot in MapData.PivotInfos)
			{
				Vector2 position = pivot.Position;

				if (pivot.Index == 0)
				{
					_redTeamTable = WorldManager.CreateObject<DueoksiniTable>(position);
					_redTeamTable.Initialize(InteractorInfoExtension.TableInteractorInfo);
					_redTeamTable.InitializeAs(Faction.Red);

				}
				else if (pivot.Index == 1)
				{
					_blueTeamTable = WorldManager.CreateObject<DueoksiniTable>(position);
					_blueTeamTable.Initialize(InteractorInfoExtension.TableInteractorInfo);
					_blueTeamTable.InitializeAs(Faction.Blue);
				}
				else if (pivot.Index == 2)
				{
					SpawnFieldItemBy(FieldItemType.Dueoksini_Sinseonro, position);
				}
				else if (pivot.Index == 3)
				{
					SpawnFieldItemBy(FieldItemType.Dueoksini_Gujeolpan, position);
				}
			}

			// Create Items
			// Step 1: Select random positions
			int areaCount = MapData.AreaInfos.Count;
			float totalArea = 0;
			foreach (var area in MapData.AreaInfos)
			{
				totalArea += area.Area;
			}

			int totalItemSpwanCount = DueoksiniHelper.GetTotalSpwanItemCount();
			int totalItemCount = totalItemSpwanCount;
			int totalCountInArea = totalItemCount - areaCount;
			Span<Vector2> spwanPosList = stackalloc Vector2[totalItemCount];
			totalItemCount--;
			foreach (var areaInfo in MapData.AreaInfos)
			{
				float area = areaInfo.Area;
				int itemCreate = (int)(totalCountInArea * (areaInfo.Area / totalArea));
				totalCountInArea -= itemCreate;
				totalArea -= area;
				// 각 영역에 최소 1개의 아이템을 생성한다.
				for (int i = 0; i < itemCreate + 1; i++)
				{
					Vector2 randPos = areaInfo.GetRandomPosition();
					spwanPosList[totalItemCount--] = randPos;
				}
			}

			Debug.Assert(totalItemCount == -1);
			totalItemCount = 0;

			// Step 2: Create items
			FieldItemType itemIndex = FieldItemType.Dueoksini;
			while (totalItemCount < totalItemSpwanCount)
			{
				itemIndex++;
				DueoksiniHelper.TryGetItemInfo(itemIndex, out var info);
				for (int i = 0; i < info.SpwanCount; i++)
				{
					Vector2 spwanPos = spwanPosList[totalItemCount++];
					SpawnFieldItemBy(info.ItemType, spwanPos);
				}
			}

			// Spawn players
		 	int playerCount = GameplayController.PlayerSet.Count;
			Span<UserId> users = stackalloc UserId[playerCount];
			int u = 0;
			foreach (NetworkPlayer player in GameplayController.PlayerSet)
			{
				users[u++] = player.UserId;
			}
			users.Sort();

			int halfPlayerCount = playerCount / 2;
			foreach (NetworkPlayer player in GameplayController.PlayerSet)
			{
				var character = SpawnPlayerBy<NormalCharacter>(player);
				if (halfPlayerCount > 0)
				{
					character.Faction = Faction.Blue;
					player.Faction = Faction.Blue;
				}
				else
				{
					character.Faction = Faction.Red;
					player.Faction = Faction.Red;
				}

				halfPlayerCount--;
			}
		}
	}
}