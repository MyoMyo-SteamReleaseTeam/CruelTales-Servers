using System.Collections;
using System.Collections.Generic;

namespace CT.Common.Tools.Collections
{
	/// <summary>원형 큐입니다.</summary>
	public class CircularQueue<T> : IEnumerable<T>
		where T : struct
	{
		private List<T> _data;
		public IList<T> Data => _data;

		public int Count { get; private set; }
		public int Capacity { get; private set; }

		public bool IsEmpty => Count == 0;
		public bool IsFull => Count == Capacity;

		private int _frontIndex;
		private int _tailIndex;

		public CircularQueue(int capacity = 8)
		{
			Capacity = capacity;
			_data = new List<T>(capacity);
			for (int i = 0; i < capacity; i++)
			{
				_data.Add(default);
			}

			Clear();
		}

		public void Clear()
		{
			Count = 0;
			_frontIndex = 0;
			_tailIndex = 0;
		}

		public bool TryEnqueue(T value)
		{
			if (IsFull)
				return false;

			_data[_tailIndex] = value;
			_tailIndex = (_tailIndex + 1) % Capacity;
			Count++;
			return true;
		}

		public void Enqueue(T value)
		{
			_data[_tailIndex] = value;
			_tailIndex = (_tailIndex + 1) % Capacity;

			if (IsFull)
			{
				_frontIndex = _tailIndex;
				return;
			}

			Count++;
		}

		public bool TryDequeue(out T value)
		{
			if (IsEmpty)
			{
				value = default;
				return false;
			}

			value = _data[_frontIndex];
			_frontIndex = (_frontIndex + 1) % Capacity;
			Count--;
			return true;
		}

		public bool TryPeek(out T value)
		{
			if (IsEmpty)
			{
				value = default;
				return false;
			}

			value = _data[_frontIndex];
			return true;
		}

		public T Dequeue()
		{
			T value = _data[_frontIndex];
			_frontIndex = (_frontIndex + 1) % Capacity;
			Count--;
			return value;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _data.GetEnumerator();
		}
	}
}