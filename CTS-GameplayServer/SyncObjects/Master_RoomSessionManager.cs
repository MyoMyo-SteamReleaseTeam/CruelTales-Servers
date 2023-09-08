/*
 * Generated File : Master_RoomSessionManager
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
	public partial class RoomSessionManager : IMasterSynchronizable
	{
		[SyncVar]
		private NetStringShort _roomName = new();
		[SyncVar]
		private NetStringShort _roomDiscription = new();
		[SyncVar]
		private int _password;
		[SyncVar]
		private int _maxPlayerCount;
		[SyncVar]
		private int _minPlayerCount;
		[SyncObject]
		private readonly SyncObjectDictionary<UserId, PlayerState> _playerStateTable;
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void ServerRoomSetAck_Callback(NetworkPlayer player, RoomSettingResult callback);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetPassword(NetworkPlayer player, int password);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetRoomName(NetworkPlayer player, NetStringShort roomName);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetRoomDiscription(NetworkPlayer player, NetStringShort roomDiscription);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetRoomMaxUser(NetworkPlayer player, int maxUserCount);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetRoomMinUser(NetworkPlayer player, int minUserCount);
		[AllowNull] public IDirtyable _owner;
		public void BindOwner(IDirtyable owner) => _owner = owner;
		public RoomSessionManager()
		{
			_playerStateTable = new(this);
		}
		public RoomSessionManager(IDirtyable owner)
		{
			_owner = owner;
			_playerStateTable = new(this);
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
		public NetStringShort RoomName
		{
			get => _roomName;
			set
			{
				if (_roomName == value) return;
				_roomName = value;
				_dirtyReliable_0[0] = true;
				MarkDirtyReliable();
			}
		}
		public NetStringShort RoomDiscription
		{
			get => _roomDiscription;
			set
			{
				if (_roomDiscription == value) return;
				_roomDiscription = value;
				_dirtyReliable_0[1] = true;
				MarkDirtyReliable();
			}
		}
		public int Password
		{
			get => _password;
			set
			{
				if (_password == value) return;
				_password = value;
				_dirtyReliable_0[2] = true;
				MarkDirtyReliable();
			}
		}
		public int MaxPlayerCount
		{
			get => _maxPlayerCount;
			set
			{
				if (_maxPlayerCount == value) return;
				_maxPlayerCount = value;
				_dirtyReliable_0[3] = true;
				MarkDirtyReliable();
			}
		}
		public int MinPlayerCount
		{
			get => _minPlayerCount;
			set
			{
				if (_minPlayerCount == value) return;
				_minPlayerCount = value;
				_dirtyReliable_0[4] = true;
				MarkDirtyReliable();
			}
		}
		public SyncObjectDictionary<UserId, PlayerState> PlayerStateTable => _playerStateTable;
		public partial void ServerRoomSetAck_Callback(NetworkPlayer player, RoomSettingResult callback)
		{
			ServerRoomSetAck_CallbackRCallstack.Add(player, callback);
			_dirtyReliable_0[6] = true;
			MarkDirtyReliable();
		}
		private TargetCallstack<NetworkPlayer, RoomSettingResult> ServerRoomSetAck_CallbackRCallstack = new(8);
		public void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			_playerStateTable.ClearDirtyReliable();
			ServerRoomSetAck_CallbackRCallstack.Clear();
		}
		public void ClearDirtyUnreliable() { }
		public void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyReliable_0[5] = _playerStateTable.IsDirtyReliable;
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				_roomName.Serialize(writer);
			}
			if (_dirtyReliable_0[1])
			{
				_roomDiscription.Serialize(writer);
			}
			if (_dirtyReliable_0[2])
			{
				writer.Put(_password);
			}
			if (_dirtyReliable_0[3])
			{
				writer.Put(_maxPlayerCount);
			}
			if (_dirtyReliable_0[4])
			{
				writer.Put(_minPlayerCount);
			}
			if (_dirtyReliable_0[5])
			{
				_playerStateTable.SerializeSyncReliable(player, writer);
			}
			if (_dirtyReliable_0[6])
			{
				int ServerRoomSetAck_CallbackRCount = ServerRoomSetAck_CallbackRCallstack.GetCallCount(player);
				if (ServerRoomSetAck_CallbackRCount > 0)
				{
					var ServerRoomSetAck_CallbackRcallList = ServerRoomSetAck_CallbackRCallstack.GetCallList(player);
					writer.Put((byte)ServerRoomSetAck_CallbackRCount);
					for (int i = 0; i < ServerRoomSetAck_CallbackRCount; i++)
					{
						var arg = ServerRoomSetAck_CallbackRcallList[i];
						writer.Put((byte)arg);
					}
				}
				else
				{
					dirtyReliable_0[6] = false;
				}
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
		public void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public void SerializeEveryProperty(IPacketWriter writer)
		{
			_roomName.Serialize(writer);
			_roomDiscription.Serialize(writer);
			writer.Put(_password);
			writer.Put(_maxPlayerCount);
			writer.Put(_minPlayerCount);
			_playerStateTable.SerializeEveryProperty(writer);
		}
		public void InitializeMasterProperties()
		{
			_roomName = new();
			_roomDiscription = new();
			_password = 0;
			_maxPlayerCount = 0;
			_minPlayerCount = 0;
			_playerStateTable.InitializeMasterProperties();
		}
		public bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int password)) return false;
					ClientRoomSetReq_SetPassword(player, password);
				}
			}
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort roomName = new();
					if (!roomName.TryDeserialize(reader)) return false;
					ClientRoomSetReq_SetRoomName(player, roomName);
				}
			}
			if (dirtyReliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort roomDiscription = new();
					if (!roomDiscription.TryDeserialize(reader)) return false;
					ClientRoomSetReq_SetRoomDiscription(player, roomDiscription);
				}
			}
			if (dirtyReliable_0[3])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int maxUserCount)) return false;
					ClientRoomSetReq_SetRoomMaxUser(player, maxUserCount);
				}
			}
			if (dirtyReliable_0[4])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int minUserCount)) return false;
					ClientRoomSetReq_SetRoomMinUser(player, minUserCount);
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
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort.IgnoreStatic(reader);
				}
			}
			if (dirtyReliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort.IgnoreStatic(reader);
				}
			}
			if (dirtyReliable_0[3])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
			if (dirtyReliable_0[4])
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
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort.IgnoreStatic(reader);
				}
			}
			if (dirtyReliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort.IgnoreStatic(reader);
				}
			}
			if (dirtyReliable_0[3])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
			if (dirtyReliable_0[4])
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
