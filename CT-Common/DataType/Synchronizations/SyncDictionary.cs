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
	public class SyncDictionary<Key, Value> : IRemoteSynchronizable, IDictionary<Key, Value>
		where Key : struct, IEquatable<Key>, IPacketSerializable
		where Value : struct, IEquatable<Value>, IPacketSerializable
	{
		private struct SyncToken
		{
			public CollectionSyncType Operation;
			public Key Key;
			public Value Value;
		}

		private Dictionary<Key, Value> _dictionary;
		private List<SyncToken> _syncOperations;

		public event Action<Key>? OnRemoved;
		public event Action<Key>? OnChanged;
		public event Action<Key, Value>? OnAdded;
		public event Action? OnCleared;

		public ICollection<Key> Keys => _dictionary.Keys;
		public ICollection<Value> Values => _dictionary.Values;

		public int Count => _dictionary.Count;
		public bool IsDirtyReliable => _syncOperations.Count > 0;

		public SyncDictionary(int capacity = 8)
		{
			_dictionary = new(capacity);
			_syncOperations = new(4);
		}

		public Value this[Key key]
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
				_syncOperations.Add(new SyncToken()
				{
					Operation = CollectionSyncType.Change,
					Key = key,
					Value = value
				});
			}
		}

		public void Add(Key key, Value value)
		{
			_dictionary.Add(key, value);
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Add,
				Key = key,
				Value = value
			});
		}

		public bool ContainsKey(Key key)
		{
			return _dictionary.ContainsKey(key);
		}

		public bool Contains(KeyValuePair<Key, Value> item)
		{
			Key key = item.Key;

			if (_dictionary.TryGetValue(key, out var value))
			{
				if (item.Value.Equals(value))
				{
					return true;
				}
			}

			return false;
		}

		public bool Remove(Key key)
		{
			if (!_dictionary.Remove(key))
				return false;

			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Remove,
				Key = key
			});

			return true;
		}

		public bool Remove(KeyValuePair<Key, Value> item)
		{
			Key key = item.Key;

			if (_dictionary.TryGetValue(key, out var value))
			{
				if (item.Value.Equals(value))
				{
					_dictionary.Remove(key);
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

		public bool TryGetValue(Key key, [MaybeNullWhen(false)] out Value value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<Key, Value> item)
		{
			_dictionary.Add(item.Key, item.Value);
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
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Clear
			});
		}

		public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		public void ClearDirtyReliable()
		{
			_syncOperations.Clear();
		}

		public void SerializeEveryProperty(IPacketWriter writer)
		{
			byte count = (byte)_dictionary.Count;
			writer.Put(count);
			foreach (Key key in _dictionary.Keys)
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
				Key key = new();
				Value value = new();
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
							Key key = new();
							Value value = new();
							if (!key.TryDeserialize(reader)) return false;
							if (!value.TryDeserialize(reader)) return false;
							_dictionary.Add(key, value);
							OnAdded?.Invoke(key, value);
						}
						break;

					case CollectionSyncType.Remove:
						{
							Key key = new();
							if (!key.TryDeserialize(reader)) return false;
							_dictionary.Remove(key);
							OnRemoved?.Invoke(key);
						}
						break;

					case CollectionSyncType.Change:
						{
							Key key = new();
							Value value = new();
							if (!key.TryDeserialize(reader)) return false;
							if (!value.TryDeserialize(reader)) return false;
							_dictionary[key] = value;
							OnChanged?.Invoke(key);
						}
						break;

					default:
						Debug.Assert(false);
						break;
				}
			}
			return true;
		}

		public void CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex) => throw new NotImplementedException();
		public bool IsReadOnly => throw new NotImplementedException();
		public bool IsDirtyUnreliable => throw new WrongSyncType(SyncType.Unreliable);
		public void ClearDirtyUnreliable() => throw new WrongSyncType(SyncType.Unreliable);
		public bool TryDeserializeSyncUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public void SerializeSyncUnreliable(IPacketWriter writer) => throw new WrongSyncType(SyncType.Unreliable);
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
		public void IgnoreSyncUnreliable(IPacketReader reader) => throw new WrongSyncType(SyncType.Unreliable);
	}
}
