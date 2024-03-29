/*
 * Generated File : Master_ZTest_OutterTest
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CT.Common;
using CT.Common.DataType;
using CT.Common.Exceptions;
using CT.Common.Gameplay;
using CT.Common.Quantization;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools;
using CT.Common.DataType.Input;
using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Gameplay.PlayerCharacterStates;
using CT.Common.Gameplay.Players;
using CT.Common.Tools.CodeGen;
using CT.Common.Tools.Collections;
using CT.Common.Tools.ConsoleHelper;
using CT.Common.Tools.Data;
using CT.Common.Tools.FSM;
using CT.Common.Tools.GetOpt;
using CT.Common.Tools.SharpJson;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.SyncObjects
{
	[Serializable]
	public partial class ZTest_OutterTest
	{
		[SyncObject]
		private readonly SyncDictionary<NetInt32, NetInt32> _dictionary;
		[SyncObject(dir: SyncDirection.Bidirection)]
		private readonly Synchronizations.SyncObjectList<ZTest_InnerTest> _innerList;
		[SyncObject]
		private readonly Synchronizations.SyncObjectList<ZTest_InnerTestNoTarget> _noTargetList;
		[SyncObject(dir: SyncDirection.Bidirection)]
		private readonly Synchronizations.SyncObjectDictionary<NetworkIdentity, ZTest_InnerTest> _objectDictionary;
		[SyncObject(dir: SyncDirection.Bidirection)]
		private readonly ZTest_InnerTest _inner;
		[SyncVar]
		private int _testInt;
		private Action<Synchronizations.SyncObjectList<ZTest_InnerTest>>? _onInnerListChanged;
		public event Action<Synchronizations.SyncObjectList<ZTest_InnerTest>> OnInnerListChanged
		{
			add => _onInnerListChanged += value;
			remove => _onInnerListChanged -= value;
		}
		private Action<Synchronizations.SyncObjectDictionary<NetworkIdentity, ZTest_InnerTest>>? _onObjectDictionaryChanged;
		public event Action<Synchronizations.SyncObjectDictionary<NetworkIdentity, ZTest_InnerTest>> OnObjectDictionaryChanged
		{
			add => _onObjectDictionaryChanged += value;
			remove => _onObjectDictionaryChanged -= value;
		}
		private Action<ZTest_InnerTest>? _onInnerChanged;
		public event Action<ZTest_InnerTest> OnInnerChanged
		{
			add => _onInnerChanged += value;
			remove => _onInnerChanged -= value;
		}
		public ZTest_OutterTest()
		{
			_dictionary = new(this);
			_innerList = new(this, 8);
			_noTargetList = new(this);
			_objectDictionary = new(this);
			_inner = new(this);
		}
		private BitmaskByte _dirtyReliable_0 = new();
		public ZTest_InnerTest Inner => _inner;
		public int TestInt
		{
			get => _testInt;
			set
			{
				if (_testInt == value) return;
				_testInt = value;
				_dirtyReliable_0[5] = true;
				MarkDirtyReliable();
			}
		}
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			_dictionary.ClearDirtyReliable();
			_innerList.ClearDirtyReliable();
			_noTargetList.ClearDirtyReliable();
			_objectDictionary.ClearDirtyReliable();
			_inner.ClearDirtyReliable();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyReliable_0[0] = _dictionary.IsDirtyReliable;
			_dirtyReliable_0[1] = _innerList.IsDirtyReliable;
			_dirtyReliable_0[2] = _noTargetList.IsDirtyReliable;
			_dirtyReliable_0[3] = _objectDictionary.IsDirtyReliable;
			_dirtyReliable_0[4] = _inner.IsDirtyReliable;
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				_dictionary.SerializeSyncReliable(writer);
			}
			if (_dirtyReliable_0[1])
			{
				int curSize = writer.Size;
				_innerList.SerializeSyncReliable(player, writer);
				if (writer.Size == curSize)
				{
					dirtyReliable_0[1] = false;
				}
			}
			if (_dirtyReliable_0[2])
			{
				_noTargetList.SerializeSyncReliable(player, writer);
			}
			if (_dirtyReliable_0[3])
			{
				int curSize = writer.Size;
				_objectDictionary.SerializeSyncReliable(player, writer);
				if (writer.Size == curSize)
				{
					dirtyReliable_0[3] = false;
				}
			}
			if (_dirtyReliable_0[4])
			{
				int curSize = writer.Size;
				_inner.SerializeSyncReliable(player, writer);
				if (writer.Size == curSize)
				{
					dirtyReliable_0[4] = false;
				}
			}
			if (_dirtyReliable_0[5])
			{
				writer.Put(_testInt);
			}
			if (dirtyReliable_0.AnyTrue())
			{
				writer.PutTo(dirtyReliable_0, dirtyReliable_0_pos);
			}
			else
			{
				writer.SetSize(dirtyReliable_0_pos);
			}
		}
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			_dictionary.SerializeEveryProperty(writer);
			_innerList.SerializeEveryProperty(writer);
			_noTargetList.SerializeEveryProperty(writer);
			_objectDictionary.SerializeEveryProperty(writer);
			_inner.SerializeEveryProperty(writer);
			writer.Put(_testInt);
		}
		public override void InitializeMasterProperties()
		{
			_dictionary.InitializeMasterProperties();
			_innerList.InitializeMasterProperties();
			_noTargetList.InitializeMasterProperties();
			_objectDictionary.InitializeMasterProperties();
			_inner.InitializeMasterProperties();
			_testInt = 0;
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!_innerList.TryDeserializeSyncReliable(player, reader)) return false;
				_onInnerListChanged?.Invoke(_innerList);
			}
			if (dirtyReliable_0[1])
			{
				if (!_objectDictionary.TryDeserializeSyncReliable(player, reader)) return false;
				_onObjectDictionaryChanged?.Invoke(_objectDictionary);
			}
			if (dirtyReliable_0[2])
			{
				if (!_inner.TryDeserializeSyncReliable(player, reader)) return false;
				_onInnerChanged?.Invoke(_inner);
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => true;
		public override void InitializeRemoteProperties()
		{
			_innerList.InitializeMasterProperties();
			_objectDictionary.InitializeMasterProperties();
			_inner.InitializeMasterProperties();
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
			}
			if (dirtyReliable_0[1])
			{
			}
			if (dirtyReliable_0[2])
			{
				_inner.IgnoreSyncReliable(reader);
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
			}
			if (dirtyReliable_0[1])
			{
			}
			if (dirtyReliable_0[2])
			{
				ZTest_InnerTest.IgnoreSyncStaticReliable(reader);
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
