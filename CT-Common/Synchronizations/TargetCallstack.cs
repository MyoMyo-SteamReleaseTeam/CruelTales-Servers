using System.Collections.Generic;
using CT.Common.Tools;

#if NET

namespace CT.Common.Synchronizations
{
	public class TargetVoidCallstack<Key> where Key : notnull
	{
		private Dictionary<Key, byte> _callstackTable;
		public bool IsDirty => _callstackTable.Count > 0;

		public TargetVoidCallstack(int capacity)
		{
			_callstackTable = new(capacity);
		}

		public void Clear()
		{
			_callstackTable.Clear();
		}

		public void Add(Key key)
		{
			if (!_callstackTable.ContainsKey(key))
			{
				_callstackTable.Add(key, 1);
				return;
			}

			_callstackTable[key]++;
		}

		public int GetCallCount(Key key)
		{
			if (!_callstackTable.ContainsKey(key))
				return 0;

			return _callstackTable[key];
		}
	}

	public class TargetCallstack<Key, Arg>
		where Key : notnull
		where Arg : struct
	{
		private Dictionary<Key, List<Arg>> _callstackTable;
		private ObjectPool<List<Arg>> _pool;
		public bool IsDirty => _callstackTable.Count > 0;

		public TargetCallstack(int capacity)
		{
			_callstackTable = new(capacity);
			_pool = new(getList, capacity);
		}

		public void Clear()
		{
			foreach (var callstackList in _callstackTable.Values)
			{
				callstackList.Clear();
				_pool.Return(callstackList);
			}
			_callstackTable.Clear();
		}

		public int GetCallCount(Key key)
		{
			if (!_callstackTable.ContainsKey(key))
				return 0;

			return _callstackTable[key].Count;
		}

		public void Add(Key key, Arg argument)
		{
			if (!_callstackTable.ContainsKey(key))
			{
				_callstackTable.Add(key, _pool.Get());
			}

			_callstackTable[key].Add(argument);
		}

		public List<Arg> GetCallList(Key key)
		{
			return _callstackTable[key];
		}

		private static List<Arg> getList() => new List<Arg>(4);
	}
}

#endif