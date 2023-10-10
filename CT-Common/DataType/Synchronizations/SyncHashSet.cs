using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CT.Common.Exceptions;
using CT.Common.Serialization;
using CT.Common.Synchronizations;

namespace CT.Common.DataType.Synchronizations
{
	public class SyncHashSet<T> : IRemoteSynchronizable, ISet<T>
		where T : struct, IPacketSerializable, IEquatable<T>
	{
		private struct SyncToken
		{
			public CollectionSyncType Operation;
			public T Data;
		}

		[AllowNull]
		private IDirtyable _owner;
		private HashSet<T> _set;
		private List<SyncToken> _syncOperations;

		public event Action<T>? OnRemoved;
		public event Action<T>? OnAdded;
		public event Action? OnCleared;

		public int Count => _set.Count;
		private bool _isDirtyReliable;
		public bool IsDirtyReliable => _isDirtyReliable;
		private bool _isInitialSynchronized = false;

		//[Obsolete("Owner를 등록할 수 있는 생성자를 사용하세요.")]
		public SyncHashSet(int capacity = 8, int operationCapacity = 4)
		{
			_set = new(capacity);
			_syncOperations = new(operationCapacity);
		}

		public SyncHashSet(IDirtyable owner, int capacity = 8, int operationCapacity = 4)
		{
			BindOwner(owner);
			_set = new(capacity);
			_syncOperations = new List<SyncToken>(operationCapacity);
		}

		public void Constructor() { }

		public void BindOwner(IDirtyable owner)
		{
			_owner = owner;
		}

		void ICollection<T>.Add(T item)
		{
			_set.Add(item);
		}

		public bool Add(T item)
		{
			bool result = _set.Add(item);
			if (result)
			{
				MarkDirtyReliable();
				_syncOperations.Add(new SyncToken()
				{
					Data = item,
					Operation = CollectionSyncType.Add
				});
			}
			return result;
		}

		public bool Remove(T item)
		{
			bool result = _set.Remove(item);
			if (result)
			{
				MarkDirtyReliable();
				_syncOperations.Add(new SyncToken()
				{
					Operation = CollectionSyncType.Remove,
					Data = item,
				});
			}
			return result;
		}

		public void Clear()
		{
			_set.Clear();
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Clear,
			});
		}

		public bool Contains(T item) => _set.Contains(item);
		public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _set.GetEnumerator();

		#region Set operations

		public void ExceptWith(IEnumerable<T> other) => _set.ExceptWith(other);
		public void IntersectWith(IEnumerable<T> other) => _set.IntersectWith(other);
		public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);
		public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);
		public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);
		public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);
		public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);
		public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);
		public void SymmetricExceptWith(IEnumerable<T> other) => _set.SymmetricExceptWith(other);
		public void UnionWith(IEnumerable<T> other) => _set.UnionWith(other);
		public void CopyTo(T[] array, int arrayIndex) => _set.CopyTo(array, arrayIndex);

		#endregion

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
			byte count = (byte)_set.Count;
			writer.Put(count);
			foreach (T item in _set)
			{
				item.Serialize(writer);
			}
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
						case CollectionSyncType.Remove:
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
			_isInitialSynchronized = true;
			byte count = reader.ReadByte();
			for (int i = 0; i < count; i++)
			{
				T item = new();
				if (!item.TryDeserialize(reader))
				{
					return false;
				}
				_set.Add(item);
			}
			return true;
		}

		public bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			/*
			 * 최초 1회 동기화시 변경된 데이터도 똑같이 수신된다면
			 * 중복 호출이 일어날 수 있음.
			 * 이미 반영된 이벤트이기 때문에 무시한다.
			 */
			if (_isInitialSynchronized)
			{
				IgnoreSyncReliable(reader);
				_isInitialSynchronized = false;
				return true;
			}

			byte operationCount = reader.ReadByte();
			for (int i = 0; i < operationCount; i++)
			{
				var operation = (CollectionSyncType)reader.ReadByte();
				switch (operation)
				{
					case CollectionSyncType.Clear:
						_set.Clear();
						OnCleared?.Invoke();
						break;

					case CollectionSyncType.Add:
						T addData = new();
						if (!addData.TryDeserialize(reader)) return false;
						_set.Add(addData);
						OnAdded?.Invoke(addData);
						break;

					case CollectionSyncType.Remove:
						T removeData = new();
						if (!removeData.TryDeserialize(reader)) return false;
						_set.Remove(removeData);
						OnRemoved?.Invoke(removeData);
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
			_set.Clear();
			_syncOperations.Clear();
			_isInitialSynchronized = false;
		}

		public void InitializeRemoteProperties()
		{
			_set.Clear();
			_syncOperations.Clear();
			_isInitialSynchronized = false;
		}

		public void IgnoreSyncReliable(IPacketReader reader) => IgnoreSyncStaticReliable(reader);
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			byte operationCount = reader.ReadByte();
			for (int i = 0; i < operationCount; i++)
			{
				var operation = (CollectionSyncType)reader.ReadByte();
				switch (operation)
				{
					case CollectionSyncType.Clear:
						break;

					case CollectionSyncType.Add:
					case CollectionSyncType.Remove:
						ignoreElement(reader);
						break;

					default:
						Debug.Assert(false);
						break;
				}
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
		private static void ignoreElement(IPacketReader reader)
		{
			T temp = new();
			temp.Ignore(reader);
		}
	}
}
