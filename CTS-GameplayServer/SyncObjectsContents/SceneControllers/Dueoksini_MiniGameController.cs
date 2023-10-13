using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Dueoksini;
using CT.Networks;
using CTS.Instance.Gameplay;

namespace CTS.Instance.SyncObjects
{
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

				if (pivot.Index == GlobalNetwork.SYSTEM_PIVOT_INDEX_LIMIT + 0)
				{
					_redTeamTable = WorldManager.CreateObject<DueoksiniTable>(position);
					_redTeamTable.Initialize(InteractorConst.TableInteractorInfo);
					_redTeamTable.InitializeAs(Faction.Red);
					_redTeamTable.BindController(this);

				}
				else if (pivot.Index == GlobalNetwork.SYSTEM_PIVOT_INDEX_LIMIT + 1)
				{
					_blueTeamTable = WorldManager.CreateObject<DueoksiniTable>(position);
					_blueTeamTable.Initialize(InteractorConst.TableInteractorInfo);
					_blueTeamTable.InitializeAs(Faction.Blue);
					_blueTeamTable.BindController(this);
				}
				else if (pivot.Index == GlobalNetwork.SYSTEM_PIVOT_INDEX_LIMIT + 2)
				{
					SpawnFieldItemBy(FieldItemType.Dueoksini_Sinseonro, position);
				}
				else if (pivot.Index == GlobalNetwork.SYSTEM_PIVOT_INDEX_LIMIT + 3)
				{
					SpawnFieldItemBy(FieldItemType.Dueoksini_Gujeolpan, position);
				}
			}

			// Create Items
			// Step 1: Select random positions
			int areaCount = 0;
			float totalArea = 0;
			foreach (var areaInfo in MapData.AreaInfos)
			{
				if (areaInfo.Index != GlobalNetwork.SYSTEM_AREA_INDEX_LIMIT + 0)
					continue;
				totalArea += areaInfo.Area;
				areaCount++;
			}

			int totalItemSpwanCount = DueoksiniHelper.GetTotalSpwanItemCount();
			int totalItemCount = totalItemSpwanCount;
			int totalCountInArea = totalItemCount - areaCount;
			Span<Vector2> spwanPosList = stackalloc Vector2[totalItemCount];
			totalItemCount--;
			foreach (var areaInfo in MapData.AreaInfos)
			{
				if (areaInfo.Index != GlobalNetwork.SYSTEM_AREA_INDEX_LIMIT + 0)
					continue;
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

			spwanPosList.Shuffle();

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

			// Create players
			SpawnPlayersByTeam<NormalCharacter>(null, Faction.Red, Faction.Blue);
		}
	}
}