using System.Collections.Generic;
using System.Diagnostics;
using CT.Common.Exceptions;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;

namespace CT.Common.DataType.Synchronizations
{
	/// <summary>
	/// 동기화 객체의 배열을 동기화하는 Collection 입니다.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SyncObjectList<T> : ISynchronizable where T : ISynchronizable, new()
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

		public bool IsDirtyReliable
		{
			get
			{
				if (_operationStack.Count > 0)
					return true;

				for (int i = 0; i < _list.Count; i++)
				{
					if (_list[i].IsDirtyReliable)
						return true;
				}

				return false;
			}
		}

		public void Add(T item)
		{
			_list.Add(item);
			_operationStack.Add(new CollectionOperationToken()
			{
				Data = item,
				Operation = CollectionOperationType.Add
			});
		}

		public void Remove(T item)
		{
			byte removeIndex = 0;
			for (byte i = 0; i < _list.Count; i++)
			{
				if (_list[i].Equals(item))
				{
					removeIndex = i;
					break;
				}
			}

			_list.RemoveAt(removeIndex);
			_operationStack.Add(new CollectionOperationToken()
			{
				Operation = CollectionOperationType.Remove,
				Index = removeIndex
			});
		}

		public void Clear()
		{
			_list.Clear();
			_operationStack.Add(new CollectionOperationToken()
			{
				Operation = CollectionOperationType.Clear,
			});
		}

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public void ClearDirtyReliable()
		{
			_operationStack.Clear();
		}

		public void SerializeEveryProperty(IPacketWriter writer)
		{
			writer.Put((byte)_list.Count);
			for (int i = 0; i < _list.Count; i++)
			{
				_list[i].SerializeEveryProperty(writer);
			}
		}

		public void DeserializeEveryProperty(IPacketReader reader)
		{
			byte count = reader.ReadByte();
			for (int i = 0; i < count; i++)
			{
				T item = new();
				item.DeserializeEveryProperty(reader);
				_list.Add(item);
			}
		}

		public void SerializeSyncReliable(IPacketWriter writer)
		{
			BitmaskByte masterDirty = new BitmaskByte();
			masterDirty[0] = _operationStack.Count > 0;
			byte changeCount = 0;
			for (int i = 0; i < _list.Count; i++)
			{
				if (_list[i].IsDirtyReliable)
				{
					changeCount ++;
				}
			}
			masterDirty[1] = changeCount > 0;

			writer.Put(masterDirty);

			// Serialize if there is even a single operation
			if (masterDirty[0])
			{
				byte operationCount = (byte)_operationStack.Count;
				writer.Put(operationCount);
				for (int i = 0; i < operationCount; i++)
				{
					var opToken = _operationStack[i];
					writer.Put((byte)opToken.Operation);

					switch (opToken.Operation)
					{
						case CollectionOperationType.Clear:
							break;

						case CollectionOperationType.Add:
							opToken.Data.SerializeEveryProperty(writer);
							break;

						case CollectionOperationType.Remove:
							writer.Put(opToken.Index);
							break;

						default:
							Debug.Assert(false);
							break;
					}
				}
			}

			// Serialize if dirty objects
			if (masterDirty[1])
			{
				writer.Put(changeCount);
				for (byte i = 0; i < _list.Count; i++)
				{
					if (_list[i].IsDirtyReliable)
					{
						writer.Put(i);
						_list[i].SerializeSyncReliable(writer);
					}
				}
			}
		}

		public void DeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte masterDirty = reader.ReadBitmaskByte();

			// Serialize if there is even a single operation
			if (masterDirty[0])
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
							T data = new T();
							data.DeserializeEveryProperty(reader);
							_list.Add(data);
							break;

						case CollectionOperationType.Remove:
							byte index = reader.ReadByte();
							_list.RemoveAt(index);
							break;

						default:
							Debug.Assert(false);
							break;
					}
				}
			}

			// Serialize if dirty objects
			if (masterDirty[1])
			{
				byte changeCount = reader.ReadByte();
				for (int i = 0; i < changeCount; i++)
				{
					byte index = reader.ReadByte();
					_list[index].DeserializeSyncReliable(reader);
				}
			}
		}

		private static readonly WrongSyncType _exception = new WrongSyncType(SyncType.Unreliable);
		public bool IsDirtyUnreliable => throw _exception;
		public void ClearDirtyUnreliable() => throw _exception;
		public void DeserializeSyncUnreliable(IPacketReader reader) => throw _exception;
		public void SerializeSyncUnreliable(IPacketWriter writer) => throw _exception;
		#if NET
		public static void IgnoreSyncReliable(IPacketReader reader) => throw _exception;
		public static void IgnoreSyncUnreliable(IPacketReader reader) => throw _exception;
#else
		public void IgnoreSyncReliable(IPacketReader reader) => throw _exception;
		public void IgnoreSyncUnreliable(IPacketReader reader) => throw _exception;
#endif
	}
}
