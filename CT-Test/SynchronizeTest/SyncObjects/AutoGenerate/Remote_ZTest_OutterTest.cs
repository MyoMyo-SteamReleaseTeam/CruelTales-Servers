/*
 * Generated File : Remote_ZTest_OutterTest
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Numerics;
using System.Collections.Generic;
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


namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class ZTest_OutterTest
	{
		[SyncObject(dir: SyncDirection.FromRemote)]
		private readonly ZTest_InnerTest _inner;
		[SyncObject]
		private readonly Synchronizations.SyncDictionary<NetInt32, NetInt32> _dictionary = new();
		public Synchronizations.SyncDictionary<NetInt32, NetInt32> Dictionary => _dictionary;
		private Action<Synchronizations.SyncDictionary<NetInt32, NetInt32>>? _onDictionaryChanged;
		public event Action<Synchronizations.SyncDictionary<NetInt32, NetInt32>> OnDictionaryChanged
		{
			add => _onDictionaryChanged += value;
			remove => _onDictionaryChanged -= value;
		}
		[SyncObject]
		private readonly Synchronizations.SyncObjectList<ZTest_InnerTest> _innerList = new();
		public Synchronizations.SyncObjectList<ZTest_InnerTest> InnerList => _innerList;
		private Action<Synchronizations.SyncObjectList<ZTest_InnerTest>>? _onInnerListChanged;
		public event Action<Synchronizations.SyncObjectList<ZTest_InnerTest>> OnInnerListChanged
		{
			add => _onInnerListChanged += value;
			remove => _onInnerListChanged -= value;
		}
		[SyncObject]
		private readonly Synchronizations.SyncObjectList<ZTest_InnerTestNoTarget> _noTargetList = new();
		public Synchronizations.SyncObjectList<ZTest_InnerTestNoTarget> NoTargetList => _noTargetList;
		private Action<Synchronizations.SyncObjectList<ZTest_InnerTestNoTarget>>? _onNoTargetListChanged;
		public event Action<Synchronizations.SyncObjectList<ZTest_InnerTestNoTarget>> OnNoTargetListChanged
		{
			add => _onNoTargetListChanged += value;
			remove => _onNoTargetListChanged -= value;
		}
		private Action<ZTest_InnerTest>? _onInnerChanged;
		public event Action<ZTest_InnerTest> OnInnerChanged
		{
			add => _onInnerChanged += value;
			remove => _onInnerChanged -= value;
		}
		public ZTest_OutterTest()
		{
			_inner = new();
		}
		private BitmaskByte _dirtyReliable_0 = new();
		public override bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _inner.IsDirtyReliable;
				isDirty |= _dirtyReliable_0.AnyTrue();
				return isDirty;
			}
		}
		public override bool IsDirtyUnreliable => false;
		public ZTest_InnerTest Inner => _inner;
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
			_inner.ClearDirtyReliable();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer)
		{
			_dirtyReliable_0[0] = _inner.IsDirtyReliable;
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				_inner.SerializeSyncReliable(writer);
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
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void InitializeMasterProperties()
		{
			_inner.InitializeRemoteProperties();
		}
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!_dictionary.TryDeserializeSyncReliable(reader)) return false;
				_onDictionaryChanged?.Invoke(_dictionary);
			}
			if (dirtyReliable_0[1])
			{
				if (!_innerList.TryDeserializeSyncReliable(reader)) return false;
				_onInnerListChanged?.Invoke(_innerList);
			}
			if (dirtyReliable_0[2])
			{
				if (!_noTargetList.TryDeserializeSyncReliable(reader)) return false;
				_onNoTargetListChanged?.Invoke(_noTargetList);
			}
			if (dirtyReliable_0[3])
			{
				if (!_inner.TryDeserializeSyncReliable(reader)) return false;
				_onInnerChanged?.Invoke(_inner);
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!_dictionary.TryDeserializeEveryProperty(reader)) return false;
			_onDictionaryChanged?.Invoke(_dictionary);
			if (!_innerList.TryDeserializeEveryProperty(reader)) return false;
			_onInnerListChanged?.Invoke(_innerList);
			if (!_noTargetList.TryDeserializeEveryProperty(reader)) return false;
			_onNoTargetListChanged?.Invoke(_noTargetList);
			if (!_inner.TryDeserializeEveryProperty(reader)) return false;
			_onInnerChanged?.Invoke(_inner);
			return true;
		}
		public override void InitializeRemoteProperties()
		{
			_dictionary.InitializeRemoteProperties();
			_innerList.InitializeRemoteProperties();
			_noTargetList.InitializeRemoteProperties();
			_inner.InitializeRemoteProperties();
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
			}
			if (dirtyReliable_0[3])
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
			}
			if (dirtyReliable_0[3])
			{
				ZTest_InnerTest.IgnoreSyncStaticReliable(reader);
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
