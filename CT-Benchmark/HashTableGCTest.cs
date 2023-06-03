using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using CT.Common.Serialization;
using CT.Common.Tools;

namespace CT.Benchmark
{
	internal class Player
	{
		public int Id;

		public Player(int id)
		{
			Id = id;
		}
	}

	public class TargetVoidCallStack<Key> where Key : notnull
	{
		private Dictionary<Key, byte> _callstackTable;

		public TargetVoidCallStack(int capacity)
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

		public bool HasBeenCalled(Key key)
		{
			if (!_callstackTable.ContainsKey(key))
				return false;

			return _callstackTable[key] > 0;
		}
	}

	public class TargetCallStack<Key, Arg> 
		where Key : notnull
		where Arg : struct
	{
		private Dictionary<Key, List<Arg>> _callstackTable;
		private ObjectPool<List<Arg>> _pool;

		public TargetCallStack(int capacity)
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

		public bool HasBeenCalled(Key key)
		{
			if (!_callstackTable.ContainsKey(key))
				return false;

			return _callstackTable[key].Count > 0;
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

	[MemoryDiagnoser]
	internal class HashTableGCTest
	{
		private const int KeyCount = 8;
		private const int ValueCount = 16;

		private Dictionary<Player, List<int>> _hashTable;

		public HashTableGCTest()
		{
		}

		[Benchmark]
		public void HashTableCapacityTest()
		{

		}
	}
}
