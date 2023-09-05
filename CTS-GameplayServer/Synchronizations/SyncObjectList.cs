using System.Collections.Generic;
using System.Diagnostics;
using CT.Common.Exceptions;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using System;
using System.Diagnostics.CodeAnalysis;

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

		[AllowNull] private IDirtyable _owner;
		private List<T> _list;
		private List<SyncToken> _syncOperations;

		public event Action<int>? OnRemoved;
		public event Action<int, T>? OnChanged;
		public event Action<T>? OnAdded;
		public event Action<int, T>? OnInserted;
		public event Action? OnCleared;

		public readonly int MaxCapacity;
		public int Count { get; private set; } = 0;

		private bool _isDirtyReliable;
		public bool IsDirtyReliable => _isDirtyReliable;

#if CT_CLIENT
		private bool _isInitialSynchronized = false;
		private static T _ignoreInstance = new T();
#endif

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
				{
					throw new ArgumentOutOfRangeException($"Current sync object list count is {Count}. Index : {index}");
				}
				return _list[index];
			}
		}

		[Obsolete("Owner를 등록할 수 있는 생성자를 사용하세요.")]
		public SyncObjectList(int maxCapacity = 8, int operationCapacity = 4)
		{
			MaxCapacity = maxCapacity;
			_list = new(MaxCapacity);
			for (int i = 0; i < MaxCapacity; i++)
			{
				var netObj = new T();
				netObj.BindOwner(this);
				_list.Add(netObj);
			}
			_syncOperations = new(operationCapacity);
		}

		public SyncObjectList(IDirtyable owner, int maxCapacity = 8, int operationCapacity = 4)
		{
			BindOwner(owner);
			MaxCapacity = maxCapacity;
			_list = new(MaxCapacity);
			for (int i = 0; i < MaxCapacity; i++)
			{
				var netObj = new T();
				netObj.BindOwner(this);
				_list.Add(netObj);
			}
			_syncOperations = new(operationCapacity);
		}

		public void BindOwner(IDirtyable owner)
		{
			_owner = owner;
		}

#if CT_SERVER
		public T Add()
		{
			T item = _list[Count];
			item.InitializeMasterProperties();
			item.InitializeRemoteProperties();
			item.ClearDirtyReliable();

			Count++;
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Data = item,
				Operation = CollectionSyncType.Add
			});

			return item;
		}

		public T Add(Action<T> onCreated)
		{
			T item = _list[Count];
			item.InitializeMasterProperties();
			item.InitializeRemoteProperties();
			onCreated(item);
			item.ClearDirtyReliable();

			Count++;
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Data = item,
				Operation = CollectionSyncType.Add
			});

			return item;
		}

		public void Remove(T item)
		{
			for (byte i = 0; i < Count; i++)
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

			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Remove,
				Index = (byte)index
			});
		}

		public void Clear()
		{
			Count = 0;
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Clear,
			});
		}
#endif

		public void InternalClear()
		{
			Count = 0;
		}

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public void InitializeMasterProperties()
		{
			Count = 0;
#if CT_CLIENT
			_isInitialSynchronized = false;
#endif
		}

		public void InitializeRemoteProperties()
		{
			Count = 0;
#if CT_CLIENT
			_isInitialSynchronized = false;
#endif
		}

		public void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_syncOperations.Clear();

			for (int i = 0; i < Count; i++)
			{
				if (_list[i].IsDirtyReliable)
				{
					_list[i].ClearDirtyReliable();
				}
			}
		}

		public void MarkDirtyReliable()
		{
			_isDirtyReliable = true;
			_owner.MarkDirtyReliable();
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
			_isInitialSynchronized = true;
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

#if CT_SERVER
		public void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
#elif CT_CLIENT
		public void SerializeSyncReliable(IPacketWriter writer)
#endif
		{
			BitmaskByte masterDirty = new BitmaskByte();
			masterDirty[0] = _syncOperations.Count > 0;

			for (int i = 0; i < Count; i++)
			{
				if (_list[i].IsDirtyReliable)
				{
					masterDirty[1] = true;
					break;
				}
			}

			int masterDirtyIndex = writer.Size;
			writer.OffsetSize(sizeof(byte));

			// Serialize if there is even a single operation
			if (masterDirty[0])
			{
#if CT_SERVER
				byte operationCount = (byte)_syncOperations.Count;
				writer.Put(operationCount);
				for (int i = 0; i < operationCount; i++)
				{
					var opToken = _syncOperations[i];
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
				int changeIndexPos = writer.Size;
				writer.OffsetSize(sizeof(byte));

				byte serializeCount = 0;
				for (byte i = 0; i < Count; i++)
				{
					if (_list[i].IsDirtyReliable)
					{
						int indexPos = writer.Size;
						writer.OffsetSize(sizeof(byte));
#if CT_SERVER
						_list[i].SerializeSyncReliable(player, writer);
#elif CT_CLIENT
						_list[i].SerializeSyncReliable(writer);
#endif
						if (indexPos + sizeof(byte) == writer.Size)
						{
							writer.SetSize(indexPos);
						}
						else
						{
							serializeCount++;
							writer.PutTo(i, indexPos);
						}
					}
				}

				if (changeIndexPos + sizeof(byte) == writer.Size)
				{
					writer.SetSize(changeIndexPos);
					masterDirty[1] = false;
				}
				else
				{
					writer.PutTo(serializeCount, changeIndexPos);
				}
			}

			if (masterDirty.AnyTrue())
			{
				writer.PutTo(masterDirty, masterDirtyIndex);
			}
			else
			{
				writer.SetSize(masterDirtyIndex);
			}
		}

#if CT_SERVER
		public bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
#elif CT_CLIENT
		public bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			/*
			 * 최초 1회 동기화시 변경된 데이터도 똑같이 수신된다면
			 * 중복 호출이 일어날 수 있음.
			 * 이미 반영된 이벤트이기 때문에 무시한다.
			 */
			if (_isInitialSynchronized)
			{
				IgnoreSyncStaticReliable(reader);
				_isInitialSynchronized = false;
				return true;
			}
#endif

			if (!reader.TryReadBitmaskByte(out BitmaskByte masterDirty)) return false;

			// Serialize if there is even a single operation
			if (masterDirty[0])
			{
#if CT_SERVER
				/// Remote cannot run collection operation.
				Debug.Assert(false);
#elif CT_CLIENT
				if (!reader.TryReadByte(out byte operationCount)) return false;
				for (int i = 0; i < operationCount; i++)
				{
					if (!reader.TryReadByte(out byte operationValue)) return false;
					var operation = (CollectionSyncType)operationValue;
					switch (operation)
					{
						case CollectionSyncType.Clear:
							Count = 0;
							OnCleared?.Invoke();
							break;

						case CollectionSyncType.Add:
							if (!_list[Count].TryDeserializeEveryProperty(reader))
							{
								return false;
							}
							Count++;
							OnAdded?.Invoke(_list[Count]);
							break;

						case CollectionSyncType.Remove:
							if (!reader.TryReadByte(out byte index)) return false;
							T temp = _list[index];
							Count--;
							for (int r = index; r < Count; r++)
							{
								_list[r] = _list[r + 1];
							}
							_list[Count] = temp;
							OnRemoved?.Invoke(index);
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
				if (!reader.TryReadByte(out byte changeCount)) return false;
				for (int i = 0; i < changeCount; i++)
				{
					if (!reader.TryReadByte(out byte index)) return false;

					T item = _list[index];
#if CT_SERVER
					if (!item.TryDeserializeSyncReliable(player, reader)) return false;
#elif CT_CLIENT
					if (!item.TryDeserializeSyncReliable(reader)) return false;
#endif
					OnChanged?.Invoke(index, item);
				}
			}

			return true;
		}
#if CT_SERVER
		public bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) => throw new WrongSyncType(SyncType.Unreliable);
		public static void IgnoreSyncStaticReliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Reliable);
#elif CT_CLIENT
		public bool TryDeserializeSyncUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public void SerializeSyncUnreliable(IPacketWriter writer) => throw new WrongSyncType(SyncType.Unreliable);
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			if (!reader.TryReadBitmaskByte(out BitmaskByte masterDirty)) return;

			if (masterDirty[0])
			{
				if (!reader.TryReadByte(out byte operationCount)) return;
				for (int i = 0; i < operationCount; i++)
				{
					if (!reader.TryReadByte(out byte operationValue)) return;
					var operation = (CollectionSyncType)operationValue;
					switch (operation)
					{
						case CollectionSyncType.Clear:
							break;

						case CollectionSyncType.Add:
							if (!_ignoreInstance.TryDeserializeEveryProperty(reader)) return;
							break;

						case CollectionSyncType.Remove:
							reader.Ignore(sizeof(byte));
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
				if (!reader.TryReadByte(out byte changeCount)) return;
				for (int i = 0; i < changeCount; i++)
				{
					reader.Ignore(sizeof(byte));
					_ignoreInstance.IgnoreSyncReliable(reader);
				}
			}
		}
#endif

		public void Constructor() => throw new NotImplementedException();
		public bool IsDirtyUnreliable => throw new WrongSyncType(SyncType.Unreliable);
		public void ClearDirtyUnreliable() => throw new WrongSyncType(SyncType.Unreliable);
		public void MarkDirtyUnreliable() => throw new WrongSyncType(SyncType.Unreliable);
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) => throw new NotImplementedException();
		public void IgnoreSyncReliable(IPacketReader reader) => IgnoreSyncStaticReliable(reader);
		public void IgnoreSyncUnreliable(IPacketReader reader) => IgnoreSyncStaticUnreliable(reader);
	}
}
