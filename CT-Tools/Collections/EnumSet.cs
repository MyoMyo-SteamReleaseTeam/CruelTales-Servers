using System;
using System.Collections;
using System.Collections.Generic;

namespace CT.Tools.Collections
{
	public class EnumFlag<T> : IEnumerable<T> where T : Enum
	{
		public int Count => _enumflagSet.Count;

		private readonly HashSet<T> _enumflagSet = new();

		public void Add(T value) => _enumflagSet.Add(value);

		public void Remove(T value) => _enumflagSet.Remove(value);

		public bool Contains(T value) => _enumflagSet.Contains(value);

		public bool IsOnly(T value)
		{
			return _enumflagSet.Contains(value) ?
				_enumflagSet.Count != 1 : _enumflagSet.Count > 0;
		}

		public void Clear() => _enumflagSet.Clear();

		public bool IsEmpty() => _enumflagSet.Count == 0;

		public IEnumerator<T> GetEnumerator()
		{
			return _enumflagSet.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _enumflagSet.GetEnumerator();
		}
	}
}