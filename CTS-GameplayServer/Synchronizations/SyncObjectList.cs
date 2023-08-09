﻿using System.Collections.Generic;
using System.Diagnostics;
using CT.Common.Exceptions;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using System;

#if CT_SERVER
using CTS.Instance.Gameplay;
using CT.Common.DataType.Synchronizations;
#endif

#if CT_SERVER
namespace CTS.Instance.Synchronizations
#elif CT_CLIENT
namespace CT.Common.DataType.Synchronizations
#endif
{
#if CT_SERVER
	/// <summary>
	/// 동기화 객체의 배열을 동기화하는 Collection 입니다.
	/// 서버에서 동작합니다.
	/// </summary>
	public class SyncObjectList<T> : IMasterSynchronizable
		where T : IMasterSynchronizable, new()
#elif CT_CLIENT
	/// <summary>
	/// 동기화 객체의 배열을 동기화하는 Collection 입니다.
	/// 클라이언트에서 동작합니다.
	/// </summary>
	public class SyncObjectList<T> : IRemoteSynchronizable
		where T : IRemoteSynchronizable, new()
#endif
	{
		private struct SyncToken
		{
			public CollectionSyncType Operation;
			public T Data;
			public byte Index;
		}

		private List<T> _list;
		private List<SyncToken> _syncOperation;

		public readonly int MaxCapacity;
		public int Count { get; private set; } = 0;

		public bool IsDirtyReliable
		{
			get
			{
				if (_syncOperation.Count > 0)
					return true;

				for (int i = 0; i < _list.Count; i++)
				{
					if (_list[i].IsDirtyReliable)
						return true;
				}

				return false;
			}
		}

		public T this[int index]
		{
			get => _list[index];
		}

		public SyncObjectList(int maxCapacity = 8)
		{
			MaxCapacity = maxCapacity;
			_list = new(MaxCapacity);
			for (int i = 0; i < MaxCapacity; i++)
			{
				_list.Add(new T());
			}
			_syncOperation = new(4);
		}

#if CT_SERVER
		/// <summary>객체를 추가합니다.</summary>
		public void Add(Action<T> onCreated)
		{
			T item = _list[Count];
			item.InitializeMasterProperties();
			item.InitializeRemoteProperties();
			onCreated(item);

			Count++;
			_syncOperation.Add(new SyncToken()
			{
				Data = item,
				Operation = CollectionSyncType.Add
			});
		}

		public void Remove(T item)
		{
			for (byte i = 0; i < _list.Count; i++)
			{
				if (_list[i].Equals(item))
				{
					RemoveAt(i);
					return;
				}
			}
		}

		public void RemoveAt(int index)
		{
			T temp = _list[index];
			Count--;
			for (int i = index; i < Count; i++)
			{
				_list[i] = _list[i + 1];
			}
			_list[Count] = temp;

			_syncOperation.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Remove,
				Index = (byte)index
			});
		}

		public void Clear()
		{
			Count = 0;
			_syncOperation.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Clear,
			});
		}
#endif

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public void InitializeMasterProperties()
		{
			Count = 0;
		}

		public void InitializeRemoteProperties()
		{
			Count = 0;
		}

		public void ClearDirtyReliable()
		{
			_syncOperation.Clear();

			int count = Count;
			for (int i = 0; i < count; i++)
			{
				if (_list[i].IsDirtyReliable)
				{
					_list[i].ClearDirtyReliable();
				}
			}
		}

#if CT_SERVER
		public void SerializeEveryProperty(IPacketWriter writer)
		{
			writer.Put((byte)Count);
			for (int i = 0; i < Count; i++)
			{
				_list[i].SerializeEveryProperty(writer);
			}
		}
#endif

#if CT_CLIENT
		public bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			Count = reader.ReadByte();
			for (int i = 0; i < Count; i++)
			{
				if (!_list[i].TryDeserializeEveryProperty(reader))
				{
					return false;
				}
			}
			return true;
		}
#endif

		public bool IsDirtyUnreliable => throw new WrongSyncType(SyncType.Unreliable);
		public void ClearDirtyUnreliable() => throw new WrongSyncType(SyncType.Unreliable);

#if CT_SERVER
		public void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
#elif CT_CLIENT
		public void SerializeSyncReliable(IPacketWriter writer)
#endif
		{
			BitmaskByte masterDirty = new BitmaskByte();
			masterDirty[0] = _syncOperation.Count > 0;
			byte changeCount = 0;
			for (int i = 0; i < Count; i++)
			{
				if (_list[i].IsDirtyReliable)
				{
					changeCount++;
				}
			}
			masterDirty[1] = changeCount > 0;

			writer.Put(masterDirty);

			// Serialize if there is even a single operation
			if (masterDirty[0])
			{
#if CT_SERVER
				byte operationCount = (byte)_syncOperation.Count;
				writer.Put(operationCount);
				for (int i = 0; i < operationCount; i++)
				{
					var opToken = _syncOperation[i];
					writer.Put((byte)opToken.Operation);

					switch (opToken.Operation)
					{
						case CollectionSyncType.Clear:
							break;

						case CollectionSyncType.Add:
							opToken.Data.SerializeEveryProperty(writer);
							break;

						case CollectionSyncType.Remove:
							writer.Put(opToken.Index);
							break;

						default:
							Debug.Assert(false);
							break;
					}
				}
#elif CT_CLIENT
				/// Remote cannot run collection operation.
				Debug.Assert(false);
#endif
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
#if CT_SERVER
						_list[i].SerializeSyncReliable(player, writer);
#elif CT_CLIENT
						_list[i].SerializeSyncReliable(writer);
#endif
					}
				}
			}
		}

#if CT_SERVER
		public bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
#elif CT_CLIENT
		public bool TryDeserializeSyncReliable(IPacketReader reader)
#endif
		{
			BitmaskByte masterDirty = reader.ReadBitmaskByte();

			// Serialize if there is even a single operation
			if (masterDirty[0])
			{
#if CT_SERVER
				/// Remote cannot run collection operation.
				Debug.Assert(false);
#elif CT_CLIENT
				byte operationCount = reader.ReadByte();
				for (int i = 0; i < operationCount; i++)
				{
					var operation = (CollectionSyncType)reader.ReadByte();
					switch (operation)
					{
						case CollectionSyncType.Clear:
							_list.Clear();
							break;

						case CollectionSyncType.Add:
							if (!_list[Count].TryDeserializeEveryProperty(reader))
							{
								return false;
							}
							Count++;
							break;

						case CollectionSyncType.Remove:
							byte index = reader.ReadByte();
							_list.RemoveAt(index);
							break;

						default:
							Debug.Assert(false);
							break;
					}
				}
#endif
			}

			// Serialize if dirty objects
			if (masterDirty[1])
			{
				byte changeCount = reader.ReadByte();
				for (int i = 0; i < changeCount; i++)
				{
					byte index = reader.ReadByte();

#if CT_SERVER
					if (!_list[index].TryDeserializeSyncReliable(player, reader))
#elif CT_CLIENT
					if (!_list[index].TryDeserializeSyncReliable(reader))
#endif
					{
						return false;
					}
				}
			}

			return true;
		}
#if CT_SERVER
		public bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) => throw new WrongSyncType(SyncType.Unreliable);
#elif CT_CLIENT
		public bool TryDeserializeSyncUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public void SerializeSyncUnreliable(IPacketWriter writer) => throw new WrongSyncType(SyncType.Unreliable);
#endif

#if NET
		public static void IgnoreSyncReliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public static void IgnoreSyncUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
#else
		public void IgnoreSyncReliable(IPacketReader reader) => throw _exception;
		public void IgnoreSyncUnreliable(IPacketReader reader) => throw _exception;
#endif
	}
}
