using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;

namespace CT.Common.Gameplay.Dueoksini
{
	public struct DueoksiniItemInfo
	{
		public FieldItemType ItemType;
		public int TableCount;
		public int SpwanCount;
		public int Score;
	}

	public static class DueoksiniHelper
	{
		private readonly static DueoksiniItemInfo[] _itemInfos = new DueoksiniItemInfo[]
		{
			new() { ItemType = FieldItemType.Dueoksini_Rice,                TableCount = 4, SpwanCount = 8, Score = 2 },
			new() { ItemType = FieldItemType.Dueoksini_Kimchi,              TableCount = 4, SpwanCount = 8, Score = 2 },
			new() { ItemType = FieldItemType.Dueoksini_TaroSoup,            TableCount = 4, SpwanCount = 8, Score = 3 },
			new() { ItemType = FieldItemType.Dueoksini_Japchae,             TableCount = 2, SpwanCount = 4, Score = 4 },
			new() { ItemType = FieldItemType.Dueoksini_Jeon,                TableCount = 2, SpwanCount = 4, Score = 4 },
			new() { ItemType = FieldItemType.Dueoksini_Yukjeon,             TableCount = 2, SpwanCount = 4, Score = 5 },
			new() { ItemType = FieldItemType.Dueoksini_RawMeat,             TableCount = 2, SpwanCount = 4, Score = 5 },
			new() { ItemType = FieldItemType.Dueoksini_SteamedSeaBream,     TableCount = 1, SpwanCount = 2, Score = 6 },
			new() { ItemType = FieldItemType.Dueoksini_Galbijjim,           TableCount = 1, SpwanCount = 2, Score = 7 },
			new() { ItemType = FieldItemType.Dueoksini_Gujeolpan,           TableCount = 1, SpwanCount = 2, Score = 7 },
			new() { ItemType = FieldItemType.Dueoksini_Sinseonro,           TableCount = 1, SpwanCount = 1, Score = 10 },
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

		public static int GetItemScoreSum(SyncDictionary<NetInt32, NetByte> itemTable)
		{
			int sum = 0;
			foreach (var typeIndex in itemTable.Keys)
			{
				FieldItemType itemType = (FieldItemType)typeIndex.Value;
				int count = itemTable[typeIndex];
				if (TryGetItemInfo(itemType, out var info))
				{
					sum += info.Score * count;
				}
			}

			return sum;
		}
	}
}