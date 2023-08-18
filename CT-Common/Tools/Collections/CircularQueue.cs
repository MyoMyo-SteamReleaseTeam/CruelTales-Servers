using System.Collections;
using System.Collections.Generic;

namespace CT.Common.Tools.Collections
{
	/// <summary>원형 큐입니다.</summary>
	public class CircularQueue<T> : IEnumerable<T>
	{
		private List<T> _queue;

		public int Count { get; private set; }
		public int Capacity { get; private set; }

		public bool IsEmpty => Count == 0;
		public bool IsFull => Count == Capacity;

		private int _frontIndex;
		private int _tailIndex;

		public CircularQueue(int capacity = 8)
		{
			Capacity = capacity;
			_queue = new List<T>(capacity);

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

			_queue[_tailIndex] = value;
			_tailIndex = (_tailIndex + 1) % Capacity;
			Count++;
			return true;
		}

		public void Enqueue(T value)
		{
			_queue[_tailIndex] = value;
			_tailIndex = (_tailIndex + 1) % Capacity;

			if (IsFull)
			{
				_frontIndex = _tailIndex;
				return;
			}

			Count++;
		}

		public bool TryDequeue(out T? value)
		{
			if (IsEmpty)
			{
				value = default;
				return false;
			}

			value = _queue[_frontIndex];
			_frontIndex = (_frontIndex + 1) % Capacity;
			Count--;
			return true;
		}

		public T Dequeue()
		{
			T value = _queue[_frontIndex];
			_frontIndex = (_frontIndex + 1) % Capacity;
			Count--;
			return value;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _queue.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _queue.GetEnumerator();
		}
	}
}