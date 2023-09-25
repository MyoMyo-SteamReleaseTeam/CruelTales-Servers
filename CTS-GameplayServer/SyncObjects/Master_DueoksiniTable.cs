/*
 * Generated File : Master_DueoksiniTable
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
using CT.Common.Gameplay.Infos;
using CT.Common.Gameplay.MiniGames;
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
	public partial class DueoksiniTable
	{
		public override NetworkObjectType Type => NetworkObjectType.DueoksiniTable;
		[SyncVar]
		protected Faction _faction = new();
		[SyncObject]
		protected readonly SyncDictionary<NetInt32, NetByte> _itemCountByType;
		public DueoksiniTable()
		{
			_itemCountByType = new(this);
		}
		protected BitmaskByte _dirtyReliable_1 = new();
		public Faction Faction
		{
			get => _faction;
			set
			{
				if (_faction == value) return;
				_faction = value;
				_dirtyReliable_0[7] = true;
				MarkDirtyReliable();
			}
		}
		public SyncDictionary<NetInt32, NetByte> ItemCountByType => _itemCountByType;
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Server_InteractResultICallstack.Clear();
			Server_TestPositionTickByTickVCallstack.Clear();
			_dirtyReliable_1.Clear();
			_itemCountByType.ClearDirtyReliable();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0.AnyTrue())
			{
				if (_dirtyReliable_0[0])
				{
					writer.Put((byte)_behaviourType);
				}
				if (_dirtyReliable_0[1])
				{
					_size.Serialize(writer);
				}
				if (_dirtyReliable_0[2])
				{
					writer.Put(_prograssTime);
				}
				if (_dirtyReliable_0[3])
				{
					writer.Put(_cooltime);
				}
				if (_dirtyReliable_0[4])
				{
					writer.Put(_interactable);
				}
				if (_dirtyReliable_0[5])
				{
					byte count = (byte)Server_InteractResultICallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_InteractResultICallstack[i];
						writer.Put((byte)arg);
					}
				}
				if (_dirtyReliable_0[6])
				{
					byte count = (byte)Server_TestPositionTickByTickVCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_TestPositionTickByTickVCallstack[i];
						arg.Serialize(writer);
					}
				}
				if (_dirtyReliable_0[7])
				{
					writer.Put((byte)_faction);
				}
			}
			_dirtyReliable_1[0] = _itemCountByType.IsDirtyReliable;
			_dirtyReliable_1.Serialize(writer);
			if (_dirtyReliable_1.AnyTrue())
			{
				if (_dirtyReliable_1[0])
				{
					_itemCountByType.SerializeSyncReliable(writer);
				}
			}
		}
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			writer.Put((byte)_behaviourType);
			_size.Serialize(writer);
			writer.Put(_prograssTime);
			writer.Put(_cooltime);
			writer.Put(_interactable);
			writer.Put((byte)_faction);
			_itemCountByType.SerializeEveryProperty(writer);
		}
		public override void InitializeMasterProperties()
		{
			_behaviourType = (InteractionBehaviourType)0;
			_size = new();
			_prograssTime = 0;
			_cooltime = 0;
			_interactable = false;
			_faction = (Faction)0;
			_itemCountByType.InitializeMasterProperties();
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Client_TryInteract(player);
				}
			}
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Client_TryCancel(player);
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => true;
		public override void InitializeRemoteProperties() { }
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[1])
			{
				reader.Ignore(1);
			}
		}
		public new static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[1])
			{
				reader.Ignore(1);
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public new static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
