/*
 * Generated File : Master_MapVoteController
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
	public partial class MapVoteController : IMasterSynchronizable
	{
		[SyncObject]
		private readonly SyncList<GameSceneIdentity> _nextMapVoteList;
		[SyncVar]
		private GameSceneIdentity _nextMap = new();
		[SyncObject]
		private readonly SyncDictionary<UserId, NetInt32> _mapIndexByUserId;
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_VoteMap(NetworkPlayer player, int mapIndex);
		[AllowNull] public IDirtyable _owner;
		public void BindOwner(IDirtyable owner) => _owner = owner;
		public MapVoteController()
		{
			_nextMapVoteList = new(this);
			_mapIndexByUserId = new(this, capacity: 8);
		}
		public MapVoteController(IDirtyable owner)
		{
			_owner = owner;
			_nextMapVoteList = new(this);
			_mapIndexByUserId = new(this, capacity: 8);
		}
		private BitmaskByte _dirtyReliable_0 = new();
		protected bool _isDirtyReliable;
		public bool IsDirtyReliable => _isDirtyReliable;
		public void MarkDirtyReliable()
		{
			_isDirtyReliable = true;
			_owner.MarkDirtyReliable();
		}
		protected bool _isDirtyUnreliable;
		public bool IsDirtyUnreliable => _isDirtyUnreliable;
		public void MarkDirtyUnreliable()
		{
			_isDirtyUnreliable = true;
			_owner.MarkDirtyUnreliable();
		}
		public SyncList<GameSceneIdentity> NextMapVoteList => _nextMapVoteList;
		public GameSceneIdentity NextMap
		{
			get => _nextMap;
			set
			{
				if (_nextMap == value) return;
				_nextMap = value;
				_dirtyReliable_0[1] = true;
				MarkDirtyReliable();
			}
		}
		public SyncDictionary<UserId, NetInt32> MapIndexByUserId => _mapIndexByUserId;
		public void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			_nextMapVoteList.ClearDirtyReliable();
			_mapIndexByUserId.ClearDirtyReliable();
		}
		public void ClearDirtyUnreliable() { }
		public void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyReliable_0[0] = _nextMapVoteList.IsDirtyReliable;
			_dirtyReliable_0[2] = _mapIndexByUserId.IsDirtyReliable;
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				_nextMapVoteList.SerializeSyncReliable(writer);
			}
			if (_dirtyReliable_0[1])
			{
				_nextMap.Serialize(writer);
			}
			if (_dirtyReliable_0[2])
			{
				_mapIndexByUserId.SerializeSyncReliable(writer);
			}
		}
		public void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public void SerializeEveryProperty(IPacketWriter writer)
		{
			_nextMapVoteList.SerializeEveryProperty(writer);
			_nextMap.Serialize(writer);
			_mapIndexByUserId.SerializeEveryProperty(writer);
		}
		public void InitializeMasterProperties()
		{
			_nextMapVoteList.InitializeMasterProperties();
			_nextMap = new();
			_mapIndexByUserId.InitializeMasterProperties();
		}
		public bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int mapIndex)) return false;
					Client_VoteMap(player, mapIndex);
				}
			}
			return true;
		}
		public bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => true;
		public void InitializeRemoteProperties() { }
		public void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
		}
		public void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
