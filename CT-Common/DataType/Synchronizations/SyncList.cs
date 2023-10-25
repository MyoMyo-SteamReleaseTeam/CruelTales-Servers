using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CT.Common.Exceptions;
using CT.Common.Serialization;
using CT.Common.Synchronizations;

namespace CT.Common.DataType.Synchronizations
{
	/// <summary>구조체 배열을 동기화하는 Collection 입니다.</summary>
	public class SyncList<T> : IRemoteSynchronizable, IList<T> 
		where T : struct, IPacketSerializable, IEquatable<T>
	{
		private struct SyncToken
		{
			public CollectionSyncType Operation;
			public T Data;
			public byte Index;
		}

		[AllowNull]
		private IDirtyable _owner;
		private List<T> _list;
		private List<SyncToken> _syncOperations;

		public event Action<int>? OnRemoved;
		public event Action<int, T>? OnChanged;
		public event Action<T>? OnAdded;
		public event Action<int, T>? OnInserted;
		public event Action? OnCleared;

		public int Count => _list.Count;
		private bool _isDirtyReliable;
		public bool IsDirtyReliable => _isDirtyReliable;
		private int _duplicatedEventCount;

		//[Obsolete("Owner를 등록할 수 있는 생성자를 사용하세요.")]
		public SyncList(int capacity = 8, int operationCapacity = 4)
		{
			_list = new(capacity);
			_syncOperations = new(operationCapacity);
		}

		public SyncList(IDirtyable owner, int capacity = 8, int operationCapacity = 4)
		{
			BindOwner(owner);
			_list = new List<T>(capacity);
			_syncOperations = new List<SyncToken>(operationCapacity);
		}

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
				MarkDirtyReliable();
				_syncOperations.Add(new SyncToken()
				{
					Operation = CollectionSyncType.Change,
					Data = value,
					Index = (byte)index
				});
			}
		}

		public void Constructor() {}

		public void BindOwner(IDirtyable owner)
		{
			_owner = owner;
		}

		public int IndexOf(T item) => _list.IndexOf(item);

		public void Add(T item)
		{
			_list.Add(item);
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Data = item,
				Operation = CollectionSyncType.Add
			});
		}

		public void Insert(int index, T item)
		{
			_list.Insert(index, item);
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Insert,
				Data = item,
				Index = (byte)index
			});
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Remove,
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
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Remove,
				Index = (byte)removeIndex
			});

			return true;
		}

		public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

		public void Clear()
		{
			_list.Clear();
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Clear,
			});
		}

		public bool Contains(T item) => _list.Contains(item);
		public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

		public void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_syncOperations.Clear();
		}

		public void MarkDirtyReliable()
		{
			_isDirtyReliable = true;
			_owner.MarkDirtyReliable();
		}

		public void SerializeEveryProperty(IPacketWriter writer)
		{
			byte count = (byte)_list.Count;
			writer.Put(count);
			for (int i = 0; i < count; i++)
			{
				_list[i].Serialize(writer);
			}

			// Write duplicated event count
			writer.Put((byte)_syncOperations.Count);
		}

		public void SerializeSyncReliable(IPacketWriter writer)
		{
			writer.Put((byte)_syncOperations.Count);
			byte operationCount = (byte)_syncOperations.Count;
			if (operationCount > 0)
			{
				for (int i = 0; i < operationCount; i++)
				{
					var opToken = _syncOperations[i];
					writer.Put((byte)opToken.Operation);

					switch (opToken.Operation)
					{
						case CollectionSyncType.Clear:
							break;

						case CollectionSyncType.Add:
							opToken.Data.Serialize(writer);
							break;

						case CollectionSyncType.Remove:
							writer.Put(opToken.Index);
							break;

						case CollectionSyncType.Change:
							writer.Put(opToken.Index);
							opToken.Data.Serialize(writer);
							break;

						case CollectionSyncType.Insert:
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

		public bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			byte count = reader.ReadByte();
			for (int i = 0; i < count; i++)
			{
				T item = new();
				if (!item.TryDeserialize(reader))
				{
					return false;
				}
				_list.Add(item);
			}
			_duplicatedEventCount = reader.ReadByte();
			return true;
		}

		public bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			byte operationCount = reader.ReadByte();
			for (int i = 0; i < operationCount; i++)
			{
				/*
				 * 최초 1회 동기화시 변경된 데이터도 똑같이 수신된다면
				 * 중복 호출이 일어날 수 있음.
				 * 이미 반영된 이벤트이기 때문에 무시한다.
				 */
				if (_duplicatedEventCount > 0)
				{
					_duplicatedEventCount--;
					ignoreOperation(reader);
					continue;
				}

				var operation = (CollectionSyncType)reader.ReadByte();
				switch (operation)
				{
					case CollectionSyncType.Clear:
						_list.Clear();
						OnCleared?.Invoke();
						break;

					case CollectionSyncType.Add:
						T addData = new();
						if (!addData.TryDeserialize(reader)) return false;
						_list.Add(addData);
						OnAdded?.Invoke(addData);
						break;

					case CollectionSyncType.Remove:
						byte removeIndex = reader.ReadByte();
						_list.RemoveAt(removeIndex);
						OnRemoved?.Invoke(removeIndex);
						break;

					case CollectionSyncType.Change:
						byte changeIndex = reader.ReadByte();
						if (!_list[i].TryDeserialize(reader)) return false;
						OnChanged?.Invoke(changeIndex, _list[i]);
						break;

					case CollectionSyncType.Insert:
						byte insertIndex = reader.ReadByte();
						T insertData = new();
						if (!insertData.TryDeserialize(reader)) return false;
						_list.Insert(insertIndex, insertData);
						OnInserted?.Invoke(insertIndex, insertData);
						break;

					default:
						Debug.Assert(false);
						break;
				}
			}
			return true;
		}

		public void InitializeMasterProperties()
		{
			_list.Clear();
			_syncOperations.Clear();
		}

		public void InitializeRemoteProperties()
		{
			_list.Clear();
			_syncOperations.Clear();
		}

		public void IgnoreSyncReliable(IPacketReader reader) => IgnoreSyncStaticReliable(reader);
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			byte operationCount = reader.ReadByte();
			for (int i = 0; i < operationCount; i++)
			{
				ignoreOperation(reader);
			}
		}

		public bool IsReadOnly => throw new NotImplementedException();
		public bool IsDirtyUnreliable => throw new WrongSyncType(SyncType.Unreliable);
		public void ClearDirtyUnreliable() => throw new WrongSyncType(SyncType.Unreliable);
		public void MarkDirtyUnreliable() => throw new NotImplementedException();
		public bool TryDeserializeSyncUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public void SerializeSyncUnreliable(IPacketWriter writer) => throw new WrongSyncType(SyncType.Unreliable);
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public void IgnoreSyncUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void ignoreOperation(IPacketReader reader)
		{
			var operation = (CollectionSyncType)reader.ReadByte();
			switch (operation)
			{
				case CollectionSyncType.Clear:
					break;

				case CollectionSyncType.Add:
					ignoreElement(reader);
					break;

				case CollectionSyncType.Remove:
					reader.Ignore(sizeof(byte));
					break;

				case CollectionSyncType.Change:
					reader.Ignore(sizeof(byte));
					break;

				case CollectionSyncType.Insert:
					ignoreElement(reader);
					break;

				default:
					Debug.Assert(false);
					break;
			}
		}

		private static T _ignoreInstance = new();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void ignoreElement(IPacketReader reader)
		{
			_ignoreInstance.Ignore(reader);
		}
	}
}
