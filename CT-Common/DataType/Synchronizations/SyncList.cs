using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using CT.Common.Exceptions;
using CT.Common.Serialization;
using CT.Common.Synchronizations;

namespace CT.Common.DataType.Synchronizations
{
	/// <summary>
	/// 동기화 객체의 배열을 동기화하는 Collection 입니다.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SyncList<T> : ISynchronizable, IList<T> where T : struct, IPacketSerializable, IEquatable<T>
	{
		private struct CollectionOperationToken
		{
			public CollectionOperationType Operation;
			public T Data;
			public byte Index;
		}

		private List<T> _list = new(8);
		private List<CollectionOperationToken> _operationStack = new();

		public int Count => _list.Count;

		public bool IsDirtyReliable => _operationStack.Count > 0;

		public T this[int index]
		{
			get => _list[index];
			set
			{
				if (_list[index].Equals(value))
				{
					return;
				}

				_list[index] = value;
				_operationStack.Add(new CollectionOperationToken()
				{
					Operation = CollectionOperationType.Change,
					Data = value,
					Index = (byte)index
				});
			}
		}

		public int IndexOf(T item) => _list.IndexOf(item);

		public void Add(T item)
		{
			_list.Add(item);
			_operationStack.Add(new CollectionOperationToken()
			{
				Data = item,
				Operation = CollectionOperationType.Add
			});
		}

		public void Insert(int index, T item)
		{
			_list.Insert(index, item);
			_operationStack.Add(new CollectionOperationToken()
			{
				Operation = CollectionOperationType.Insert,
				Data = item,
				Index = (byte)index
			});
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
			_operationStack.Add(new CollectionOperationToken()
			{
				Operation = CollectionOperationType.Remove,
				Index = (byte)index
			});
		}

		public bool Remove(T item)
		{
			int removeIndex = -1;
			for (byte i = 0; i < _list.Count; i++)
			{
				if (_list[i].Equals(item))
				{
					removeIndex = i;
					break;
				}
			}

			if (removeIndex < 0)
				return false;

			_list.RemoveAt(removeIndex);
			_operationStack.Add(new CollectionOperationToken()
			{
				Operation = CollectionOperationType.Remove,
				Index = (byte)removeIndex
			});

			return true;
		}

		public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

		public void Clear()
		{
			_list.Clear();
			_operationStack.Add(new CollectionOperationToken()
			{
				Operation = CollectionOperationType.Clear,
			});
		}

		public bool Contains(T item) => _list.Contains(item);
		public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

		public void ClearDirtyReliable()
		{
			_operationStack.Clear();
		}

		public void SerializeEveryProperty(PacketWriter writer)
		{
			writer.Put((byte)_list.Count);
			for (int i = 0; i < _list.Count; i++)
			{
				_list[i].Serialize(writer);
			}
		}

		public void SerializeSyncReliable(PacketWriter writer)
		{
			writer.Put((byte)_operationStack.Count);
			byte operationCount = (byte)_operationStack.Count;
			if (operationCount > 0)
			{
				for (int i = 0; i < operationCount; i++)
				{
					var opToken = _operationStack[i];
					writer.Put((byte)opToken.Operation);

					switch (opToken.Operation)
					{
						case CollectionOperationType.Clear:
							break;

						case CollectionOperationType.Add:
							opToken.Data.Serialize(writer);
							break;

						case CollectionOperationType.Remove:
							writer.Put(opToken.Index);
							break;

						case CollectionOperationType.Change:
							writer.Put(opToken.Index);
							opToken.Data.Serialize(writer);
							break;

						case CollectionOperationType.Insert:
							writer.Put(opToken.Index);
							opToken.Data.Serialize(writer);
							break;

						default:
							Debug.Assert(false);
							break;
					}
				}
			}
		}

		public void DeserializeEveryProperty(PacketReader reader)
		{
			byte count = reader.ReadByte();
			for (int i = 0; i < count; i++)
			{
				T item = new();
				item.Deserialize(reader);
				_list.Add(item);
			}
		}

		public void DeserializeSyncReliable(PacketReader reader)
		{
			byte operationCount = reader.ReadByte();
			for (int i = 0; i < operationCount; i++)
			{
				var operation = (CollectionOperationType)reader.ReadByte();
				switch (operation)
				{
					case CollectionOperationType.Clear:
						_list.Clear();
						break;

					case CollectionOperationType.Add:
						T addData = new();
						addData.Deserialize(reader);
						_list.Add(addData);
						break;

					case CollectionOperationType.Remove:
						byte removeIndex = reader.ReadByte();
						_list.RemoveAt(removeIndex);
						break;

					case CollectionOperationType.Change:
						byte changeIndex = reader.ReadByte();
						_list[i].Deserialize(reader);
						break;

					case CollectionOperationType.Insert:
						byte insertIndex = reader.ReadByte();
						T insertData = new();
						insertData.Deserialize(reader);
						_list.Insert(insertIndex, insertData);
						break;

					default:
						Debug.Assert(false);
						break;
				}
			}
		}

		public static void IgnoreSyncReliable(PacketReader reader)
		{
			byte operationCount = reader.ReadByte();
			for (int i = 0; i < operationCount; i++)
			{
				var operation = (CollectionOperationType)reader.ReadByte();
				switch (operation)
				{
					case CollectionOperationType.Clear:
						break;

					case CollectionOperationType.Add:
						T.Ignore(reader);
						break;

					case CollectionOperationType.Remove:
						reader.Ignore(sizeof(byte));
						break;

					case CollectionOperationType.Change:
						reader.Ignore(sizeof(byte));
						break;

					case CollectionOperationType.Insert:
						reader.Ignore(sizeof(byte));
						T.Ignore(reader);
						break;

					default:
						Debug.Assert(false);
						break;
				}
			}
		}

		public bool IsReadOnly => throw new NotImplementedException();
		private static readonly WrongSyncType _exception = new WrongSyncType(SyncType.Unreliable);
		public bool IsDirtyUnreliable => throw _exception;
		public void ClearDirtyUnreliable() => throw _exception;
		public void DeserializeSyncUnreliable(PacketReader reader) => throw _exception;
		public void SerializeSyncUnreliable(PacketWriter writer) => throw _exception;
		public static void IgnoreSyncUnreliable(PacketReader reader) => throw _exception;
	}
}
