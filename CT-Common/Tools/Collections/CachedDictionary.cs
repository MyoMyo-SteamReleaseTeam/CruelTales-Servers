using System;
using System.Collections.Generic;

namespace CT.Common.Tools.Collections
{
	[Obsolete("Need benchmark")]
	public class CachedDictionary<Key, Value>
		where Key : notnull
	{
		private Dictionary<Key, Value> _dictionary = new();
		private List<Key> _keyList = new();
		private List<Value> _valueList = new();

		public IList<Key> Keys => _keyList;
		public IList<Value> Values => _valueList;

		public void Add(Key key, Value value)
		{
			_dictionary.Add(key, value);
			_keyList.Add(key);
			_valueList.Add(value);
		}

		public void Remove(Key key)
		{
			var value = _dictionary[key];
			_dictionary.Remove(key);
			_keyList.Remove(key);
			_valueList.Remove(value);
		}

		public void Clear()
		{
			_dictionary.Clear();
			_keyList.Clear();
			_valueList.Clear();
		}
	}
}
