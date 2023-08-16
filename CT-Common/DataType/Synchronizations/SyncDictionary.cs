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
	/// <summary>구조체 배열을 동기화하는 Collection 입니다.</summary>
	public class SyncDictionary<TKey, TValue> : IRemoteSynchronizable, IDictionary<TKey, TValue>
		where TKey : struct, IEquatable<TKey>, IPacketSerializable
		where TValue : struct, IEquatable<TValue>, IPacketSerializable
	{
		private struct SyncToken
		{
			public CollectionSyncType Operation;
			public TKey Key;
			public TValue Value;
		}

		[AllowNull]
		private IDirtyable _owner;
		private Dictionary<TKey, TValue> _dictionary;
		private List<SyncToken> _syncOperations;

		public event Action<TKey>? OnRemoved;
		public event Action<TKey, TValue>? OnChanged;
		public event Action<TKey, TValue>? OnAdded;
		public event Action? OnCleared;

		public ICollection<TKey> Keys => _dictionary.Keys;
		public ICollection<TValue> Values => _dictionary.Values;

		public int Count => _dictionary.Count;
		private bool _isDirtyReliable;
		public bool IsDirtyReliable => _isDirtyReliable;

		[Obsolete("Owner를 등록할 수 있는 생성자를 사용하세요.")]
		public SyncDictionary(int capacity = 8, int operationCapacity = 4)
		{
			_dictionary = new(capacity);
			_syncOperations = new(operationCapacity);
		}

		public SyncDictionary(IDirtyable owner, int capacity = 8, int operationCapacity = 4)
		{
			BindOwner(owner);
			_dictionary = new(capacity);
			_syncOperations = new(operationCapacity);
		}

		public TValue this[TKey key]
		{
			get
			{
				return _dictionary[key];
			}
			set
			{
				var originValue = _dictionary[key];
				if (originValue.Equals(value))
					return;

				_dictionary[key] = value;
				MarkDirtyReliable();
				_syncOperations.Add(new SyncToken()
				{
					Operation = CollectionSyncType.Change,
					Key = key,
					Value = value
				});
			}
		}

		public void Constructor() { }

		public void BindOwner(IDirtyable owner)
		{
			_owner = owner;
		}

		public void Add(TKey key, TValue value)
		{
			_dictionary.Add(key, value);
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Add,
				Key = key,
				Value = value
			});
		}

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			TKey key = item.Key;

			if (_dictionary.TryGetValue(key, out var value))
			{
				if (item.Value.Equals(value))
				{
					return true;
				}
			}

			return false;
		}

		public bool Remove(TKey key)
		{
			if (!_dictionary.Remove(key))
				return false;

			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Remove,
				Key = key
			});

			return true;
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			TKey key = item.Key;

			if (_dictionary.TryGetValue(key, out var value))
			{
				if (item.Value.Equals(value))
				{
					_dictionary.Remove(key);
					MarkDirtyReliable();
					_syncOperations.Add(new SyncToken()
					{
						Operation = CollectionSyncType.Remove,
						Key = key
					});

					return true;
				}
			}

			return false;
		}

		public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			_dictionary.Add(item.Key, item.Value);
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Add,
				Key = item.Key,
				Value = item.Value
			});
		}

		public void Clear()
		{
			_dictionary.Clear();
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Clear
			});
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		public void InitializeMasterProperties()
		{
			_dictionary.Clear();
			_syncOperations.Clear();
		}

		public void InitializeRemoteProperties()
		{
			_dictionary.Clear();
			_syncOperations.Clear();
		}

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
			byte count = (byte)_dictionary.Count;
			writer.Put(count);
			foreach (TKey key in _dictionary.Keys)
			{
				key.Serialize(writer);
				_dictionary[key].Serialize(writer);
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
							opToken.Key.Serialize(writer);
							opToken.Value.Serialize(writer);
							break;

						case CollectionSyncType.Remove:
							opToken.Key.Serialize(writer);
							break;

						case CollectionSyncType.Change:
							opToken.Key.Serialize(writer);
							opToken.Value.Serialize(writer);
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
				TKey key = new();
				TValue value = new();
				if (!key.TryDeserialize(reader)) return false;
				if (!value.TryDeserialize(reader)) return false;
				_dictionary.Add(key, value);
			}
			return true;
		}

		public bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			byte operationCount = reader.ReadByte();
			for (int i = 0; i < operationCount; i++)
			{
				var operation = (CollectionSyncType)reader.ReadByte();
				switch (operation)
				{
					case CollectionSyncType.Clear:
						_dictionary.Clear();
						OnCleared?.Invoke();
						break;

					case CollectionSyncType.Add:
						{
							TKey key = new();
							TValue value = new();
							if (!key.TryDeserialize(reader)) return false;
							if (!value.TryDeserialize(reader)) return false;
							_dictionary.Add(key, value);
							OnAdded?.Invoke(key, value);
						}
						break;

					case CollectionSyncType.Remove:
						{
							TKey key = new();
							if (!key.TryDeserialize(reader)) return false;
							_dictionary.Remove(key);
							OnRemoved?.Invoke(key);
						}
						break;

					case CollectionSyncType.Change:
						{
							TKey key = new();
							TValue value = new();
							if (!key.TryDeserialize(reader)) return false;
							if (!value.TryDeserialize(reader)) return false;
							_dictionary[key] = value;
							OnChanged?.Invoke(key, _dictionary[key]);
						}
						break;

					default:
						Debug.Assert(false);
						break;
				}
			}
			return true;
		}

		public static void IgnoreSyncStaticReliable(IPacketReader reader) => throw new NotImplementedException();
		public void IgnoreSyncReliable(IPacketReader reader) => IgnoreSyncStaticReliable(reader);

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotImplementedException();
		public bool IsReadOnly => throw new NotImplementedException();
		public bool IsDirtyUnreliable => throw new WrongSyncType(SyncType.Unreliable);
		public void ClearDirtyUnreliable() => throw new WrongSyncType(SyncType.Unreliable);
		public void MarkDirtyUnreliable() => throw new NotImplementedException();
		public bool TryDeserializeSyncUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public void SerializeSyncUnreliable(IPacketWriter writer) => throw new WrongSyncType(SyncType.Unreliable);
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public void IgnoreSyncUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
	}
}
