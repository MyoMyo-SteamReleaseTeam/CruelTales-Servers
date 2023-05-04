using System;
using System.Collections;
using System.Collections.Generic;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	/// <summary>
	/// 직렬화 크기의 변동이 없는 데이터 타입의 배열입니다.
	/// 고정 크기의 타입에 대해서 최적화되어있습니다.
	/// 직렬화 크기가 유동적이라면 의도하지 않은 버그가 발생합니다.
	/// </summary>
	/// <typeparam name="T">직렬화 크기의 변동이 없는 데이터 타입입니다.</typeparam>
	public class NetFixedArray<T> : IPacketSerializable, IList<T>
		where T : IPacketSerializable, new()
	{
		public const int MAX_COUNT = 255;
		private List<T> _array = new List<T>();

		public T this[int index]
		{
			get => _array[index];
			set => _array[index] = value;
		}

		public int SerializeSize
		{
			get
			{
				int size = sizeof(byte);
				if (size > 0)
				{
					size += _array[0].SerializeSize * Count;
				}
				return size;
			}
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Put((byte)Count);
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				writer.Put(_array[i]);
			}
		}

		public void Deserialize(PacketReader reader)
		{
			Clear();
			int count = reader.ReadByte();
			for (int i = 0; i < count; i++)
			{
				T element = new T();
				reader.ReadTo(element);
				_array.Add(element);
			}
		}

		public int Count
		{
			get
			{
				int count = _array.Count;
				return count > MAX_COUNT ? MAX_COUNT : count;
			}
		}
		public bool IsReadOnly => false;
		public void Add(T item)
		{
			if (Count >= MAX_COUNT)
			{
				throw new ArgumentOutOfRangeException();
			}
			_array.Add(item);
		}
		public void Clear() => _array.Clear();
		public bool Contains(T item) => _array.Contains(item);
		public void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);
		public int IndexOf(T item) => _array.IndexOf(item);
		public void Insert(int index, T item) => _array.Insert(index, item);
		public bool Remove(T item) => _array.Remove(item);
		public void RemoveAt(int index) => _array.RemoveAt(index);
		public IEnumerator<T> GetEnumerator() => _array.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _array.GetEnumerator();
	}

	/// <summary>
	/// 직렬화크기가 유동적인 데이터 타입의 배열입니다.
	/// 고정적인 직렬화 크기를 가지고 있는 타입이라면 NetFixedArray
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class NetArray<T> : IPacketSerializable, IList<T>
		where T : IPacketSerializable, new()
	{
		public const int MAX_COUNT = 255;
		private List<T> _array = new List<T>();

		public T this[int index]
		{
			get => _array[index];
			set => _array[index] = value;
		}

		public int SerializeSize
		{
			get
			{
				int size = sizeof(byte);
				int count = Count;
				for (int i = 0; i < count; i++)
				{
					size += _array[i].SerializeSize;
				}
				return size;
			}
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Put((byte)Count);
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				writer.Put(_array[i]);
			}
		}

		public void Deserialize(PacketReader reader)
		{
			Clear();
			int count = reader.ReadByte();
			for (int i = 0; i < count; i++)
			{
				T element = new T();
				reader.ReadTo(element);
				_array.Add(element);
			}
		}

		public int Count
		{
			get
			{
				int count = _array.Count;
				return count > MAX_COUNT ? MAX_COUNT : count;
			}
		}
		public bool IsReadOnly => false;
		public void Add(T item)
		{
			if (Count >= MAX_COUNT)
			{
				throw new ArgumentOutOfRangeException();
			}
			_array.Add(item);
		}
		public void Clear() => _array.Clear();
		public bool Contains(T item) => _array.Contains(item);
		public void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);
		public int IndexOf(T item) => _array.IndexOf(item);
		public void Insert(int index, T item) => _array.Insert(index, item);
		public bool Remove(T item) => _array.Remove(item);
		public void RemoveAt(int index) => _array.RemoveAt(index);
		public IEnumerator<T> GetEnumerator() => _array.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _array.GetEnumerator();
	}
}
