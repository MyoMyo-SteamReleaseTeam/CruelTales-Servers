﻿using System.Collections.Generic;
using System.Diagnostics;
using CT.Common.Exceptions;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using System;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using CT.Common.Tools;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

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
	public class SyncObjectDictionary<TKey, TValue> : IMasterSynchronizable
		where TKey : struct, IPacketSerializable
		where TValue : IMasterSynchronizable, new()
#elif CT_CLIENT
	/// <summary>
	/// 동기화 객체의 배열을 동기화하는 Collection 입니다.
	/// 클라이언트에서 동작합니다.
	/// </summary>
	public class SyncObjectDictionary<TKey, TValue> : IRemoteSynchronizable
		where TKey : IPacketSerializable, new()
		where TValue : IRemoteSynchronizable, new()
#endif
	{
		private struct SyncToken
		{
			public CollectionSyncType Operation;
			public TKey Key;
			public TValue Value;
		}

		[AllowNull] private IDirtyable _owner;
		private Stack<TValue> _objectPool;
		private Dictionary<TKey, TValue> _dictionary;
		private List<SyncToken> _syncOperations;

		public readonly int MaxCapacity;
		public int Count => _dictionary.Count;

		private bool _isDirtyReliable;
		public bool IsDirtyReliable => _isDirtyReliable;
		public int KeySerializeSize;

		public TValue this[TKey key] => _dictionary[key];

		[Obsolete("Owner를 등록할 수 있는 생성자를 사용하세요.")]
		public SyncObjectDictionary(int maxCapacity = 8, int operationCapacity = 4)
		{
			MaxCapacity = maxCapacity;
			_objectPool = new(MaxCapacity);
			_dictionary = new(MaxCapacity);
			for (int i = 0; i < MaxCapacity; i++)
			{
				var netObj = new TValue();
				netObj.BindOwner(this);
				_objectPool.Push(netObj);
			}
			_syncOperations = new(operationCapacity);
			TKey key = new();
			KeySerializeSize = key.SerializeSize;
		}

		public SyncObjectDictionary(IDirtyable owner, int maxCapacity = 8, int operationCapacity = 4)
		{
			BindOwner(owner);
			MaxCapacity = maxCapacity;
			_objectPool = new(MaxCapacity);
			_dictionary = new(MaxCapacity);
			for (int i = 0; i < MaxCapacity; i++)
			{
				var netObj = new TValue();
				netObj.BindOwner(this);
				_objectPool.Push(netObj);
			}
			_syncOperations = new(operationCapacity);
			TKey key = new();
			KeySerializeSize = key.SerializeSize;
		}

		public void BindOwner(IDirtyable owner)
		{
			_owner = owner;
		}

#if CT_SERVER
		/// <summary>객체를 추가합니다.</summary>
		public void Add(TKey key, Action<TValue> onCreated)
		{
			TValue item = _objectPool.Pop();
			item.InitializeMasterProperties();
			item.InitializeRemoteProperties();
			onCreated(item);

			_dictionary.Add(key, item);
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Key = key,
				Value = item,
				Operation = CollectionSyncType.Add
			});
		}

		public void Remove(TKey key)
		{
			TValue item = _dictionary[key];
			_dictionary.Remove(key);
			_objectPool.Push(item);

			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Remove,
				Key = key
			});
		}

		public void Clear()
		{
			internalClear();
			MarkDirtyReliable();
			_syncOperations.Add(new SyncToken()
			{
				Operation = CollectionSyncType.Clear,
			});
		}
#endif

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public void InitializeMasterProperties()
		{
			internalClear();
		}

		public void InitializeRemoteProperties()
		{
			internalClear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void internalClear()
		{
			foreach (TKey key in _dictionary.Keys)
				_objectPool.Push(_dictionary[key]);
			_dictionary.Clear();
		}

		public void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_syncOperations.Clear();

			foreach (TKey key in _dictionary.Keys)
			{
				_dictionary[key].ClearDirtyReliable();
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
			foreach (TKey key in _dictionary.Keys)
			{
				key.Serialize(writer);
				_dictionary[key].SerializeEveryProperty(writer);
			}
		}
#endif

#if CT_CLIENT
		public bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			byte count = reader.ReadByte();
			for (int i = 0; i < count; i++)
			{
				TKey key = new TKey();
				if (!key.TryDeserialize(reader)) return false;

				TValue item = _objectPool.Pop();
				if (!item.TryDeserializeEveryProperty(reader)) return false;

				_dictionary.Add(key, item);
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

			foreach (TValue value in _dictionary.Values)
			{
				if (value.IsDirtyReliable)
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
							opToken.Key.Serialize(writer);
							opToken.Value.SerializeEveryProperty(writer);
							break;

						case CollectionSyncType.Remove:
							opToken.Key.Serialize(writer);
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
				foreach (TKey key in _dictionary.Keys)
				{
					TValue item = _dictionary[key];
					if (item.IsDirtyReliable)
					{
						int indexPos = writer.Size;
						writer.OffsetSize(KeySerializeSize);
#if CT_SERVER
						item.SerializeSyncReliable(player, writer);
#elif CT_CLIENT
						item.SerializeSyncReliable(writer);
#endif
						if (indexPos + KeySerializeSize == writer.Size)
						{
							writer.SetSize(indexPos);
						}
						else
						{
							serializeCount++;
							writer.PutTo(key, indexPos);
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
#elif CT_CLIENT
		public bool TryDeserializeSyncReliable(IPacketReader reader)
#endif
		{
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
							internalClear();
							break;

						case CollectionSyncType.Add:
							{
								TKey key = new();
								if (!key.TryDeserialize(reader)) return false;
								TValue item = _objectPool.Pop();
								item.InitializeMasterProperties();
								item.InitializeRemoteProperties();
								item.ClearDirtyReliable();
								item.ClearDirtyUnreliable();
								if (!item.TryDeserializeEveryProperty(reader)) return false;
								_dictionary.Add(key, item);
							}
							break;

						case CollectionSyncType.Remove:
							{
								TKey key = new();
								if (!key.TryDeserialize(reader)) return false;
								TValue item = _dictionary[key];
								_dictionary.Remove(key);
								_objectPool.Push(item);
							}
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
					TKey key = new();
					if (!key.TryDeserialize(reader)) return false;
#if CT_SERVER
					if (!_dictionary[key].TryDeserializeSyncReliable(player, reader)) return false;
#elif CT_CLIENT
					if (!_dictionary[key].TryDeserializeSyncReliable(reader)) return false;
#endif
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

		public void Constructor() => throw new NotImplementedException();
		public bool IsDirtyUnreliable => throw new WrongSyncType(SyncType.Unreliable);
		public void ClearDirtyUnreliable() => throw new WrongSyncType(SyncType.Unreliable);
		public void MarkDirtyUnreliable() => throw new WrongSyncType(SyncType.Unreliable);
		public static void IgnoreSyncStaticReliable(IPacketReader reader) => throw new NotImplementedException();
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) => throw new NotImplementedException();
		public void IgnoreSyncReliable(IPacketReader reader) => IgnoreSyncStaticReliable(reader);
		public void IgnoreSyncUnreliable(IPacketReader reader) => IgnoreSyncStaticUnreliable(reader);
	}
}
