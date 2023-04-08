using System;
using System.Collections;
using System.Collections.Generic;

namespace CT.Network.Serialization.Type
{
	public class NetIntArray : IList<int>
	{
		public const int MAX_COUNT = 255;
		private readonly List<int> _array = new List<int>();

		public int this[int index]
		{
			get => _array[index];
			set => _array[index] = value;
		}

		public int SerializeSize => sizeof(byte) + sizeof(int) * Count;

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
				_array.Add(reader.ReadInt32());
			}
		}

		public int Count => _array.Count;
		public bool IsReadOnly => false;
		public void Add(int item)
		{
			if (this.Count >= MAX_COUNT)
			{
				throw new ArgumentOutOfRangeException();
			}
			_array.Add(item);
		}
		public void Clear() => _array.Clear();
		public bool Contains(int item) => _array.Contains(item);
		public void CopyTo(int[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);
		public int IndexOf(int item) => _array.IndexOf(item);
		public void Insert(int index, int item) => _array.Insert(index, item);
		public bool Remove(int item) => _array.Remove(item);
		public void RemoveAt(int index) => _array.RemoveAt(index);
		public IEnumerator<int> GetEnumerator() => _array.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _array.GetEnumerator();
	}

	public class NetFloatArray : IList<float>
	{
		public const int MAX_COUNT = 255;
		private readonly List<float> _array = new List<float>();

		public float this[int index]
		{
			get => _array[index];
			set => _array[index] = value;
		}

		public int SerializeSize => sizeof(byte) + sizeof(float) * Count;

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
				_array.Add(reader.ReadSingle());
			}
		}

		public int Count => _array.Count;
		public bool IsReadOnly => false;
		public void Add(float item)
		{
			if (this.Count >= MAX_COUNT)
			{
				throw new ArgumentOutOfRangeException();
			}
			_array.Add(item);
		}
		public void Clear() => _array.Clear();
		public bool Contains(float item) => _array.Contains(item);
		public void CopyTo(float[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);
		public int IndexOf(float item) => _array.IndexOf(item);
		public void Insert(int index, float item) => _array.Insert(index, item);
		public bool Remove(float item) => _array.Remove(item);
		public void RemoveAt(int index) => _array.RemoveAt(index);
		public IEnumerator<float> GetEnumerator() => _array.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _array.GetEnumerator();
	}
}
