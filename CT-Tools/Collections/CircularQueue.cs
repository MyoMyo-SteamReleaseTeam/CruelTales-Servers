using System;
using System.Collections;
using System.Collections.Generic;

namespace CT.Tools.Collections
{
	/// <summary>원형 큐입니다.</summary>
	public class CircularQueue<T> : IEnumerable<T>
	{
		private T[] _queue;

		public int Count { get; private set; }
		public int Capacity { get; private set; }

		private int _frontIndex;
		private int _tailIndex;

		public CircularQueue(int capacity = 8)
		{
			Capacity = capacity;
			_queue = new T[capacity];

			Clear();
		}

		public void Clear()
		{
			Count = 0;
			_frontIndex = 0;
			_tailIndex = 0;
		}

		public void Reserve(int capacity)
		{
			if (Capacity >= capacity)
			{
				return;
			}

			Capacity = capacity;

			T[] allocate = new T[Capacity];
			Array.Copy(_queue, 0, allocate, 0, _queue.Length);
			_queue = allocate;
		}

		public bool TryEnqueue(T value)
		{
			if (IsFull())
			{
				return false;
			}

			Count++;
			_queue[_tailIndex] = value;
			_tailIndex = (_tailIndex + 1) % Capacity;
			return true;
		}

		public bool TryDequeue(out T? value)
		{
			if (IsEmpty())
			{
				value = default;
				return false;
			}

			Count--;
			value = _queue[_frontIndex];
			_frontIndex = (_frontIndex + 1) % Capacity;
			return true;
		}

		public bool IsEmpty() => Count == 0;

		public bool IsFull() => Count == Capacity;

		public IEnumerator<T> GetEnumerator()
		{
			return (IEnumerator<T>)_queue.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _queue.GetEnumerator();
		}
	}
}
