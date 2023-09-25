using System.Collections;
using System.Collections.Generic;

namespace CT.Common.Tools
{
	public readonly struct EnumIndexPair
	{
		public readonly int EnumIndex;
		public readonly int BaseEnumIndex;

		public EnumIndexPair(int enumIndex, int baseEnumIndex)
		{
			EnumIndex = enumIndex;
			BaseEnumIndex = baseEnumIndex;
		}
	}

	public class PartialEnumTable : IEnumerable<EnumIndexPair>
	{
		public void Add(int enumIndex, int baseEnumIndex)
		{
			_baseEnumTable.Add(new EnumIndexPair(enumIndex, baseEnumIndex));
		}

		private List<EnumIndexPair> _baseEnumTable = new();

		public bool IsMatch(int enumIndex, int baseEnumNumber)
		{
			var curPair = _baseEnumTable[baseEnumNumber];
			if (enumIndex < curPair.BaseEnumIndex)
				return false;

			if (baseEnumNumber + 1 < _baseEnumTable.Count)
			{
				var nextPair = _baseEnumTable[baseEnumNumber + 1];
				return enumIndex < nextPair.BaseEnumIndex;
			}

			return true;
		}

		public int GetBaseTypeIndex(int enumIndex)
		{
			for (int i = _baseEnumTable.Count - 1; i >= 0; i--)
			{
				EnumIndexPair table = _baseEnumTable[i];
				if (enumIndex >= table.EnumIndex)
				{
					return table.BaseEnumIndex;
				}
			}

			return 0;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _baseEnumTable.GetEnumerator();
		}

		IEnumerator<EnumIndexPair> IEnumerable<EnumIndexPair>.GetEnumerator()
		{
			return _baseEnumTable.GetEnumerator();
		}
	}
}
