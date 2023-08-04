/*
 * Generated File : Remote_GameplayController
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Numerics;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Players;
using CT.Common.DataType;
using CT.Common.DataType.Input;
using CT.Common.DataType.Synchronizations;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTC.Networks.Synchronizations;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class GameplayController
	{
		public override NetworkObjectType Type => NetworkObjectType.GameplayController;
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_ReadyToSync();
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_OnMapLoaded();
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetPassword(int password);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetRoomName(NetStringShort roomName);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetRoomDiscription(NetStringShort roomDiscription);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetRoomMaxUser(int maxUser);
		[SyncVar]
		private int _currentPlayerCount;
		public int CurrentPlayerCount => _currentPlayerCount;
		private Action<int>? _onCurrentPlayerCountChanged;
		public event Action<int> OnCurrentPlayerCountChanged
		{
			add => _onCurrentPlayerCountChanged += value;
			remove => _onCurrentPlayerCountChanged -= value;
		}
		[SyncVar]
		private NetStringShort _roomName = new();
		public NetStringShort RoomName => _roomName;
		private Action<NetStringShort>? _onRoomNameChanged;
		public event Action<NetStringShort> OnRoomNameChanged
		{
			add => _onRoomNameChanged += value;
			remove => _onRoomNameChanged -= value;
		}
		[SyncVar]
		private NetStringShort _roomDiscription = new();
		public NetStringShort RoomDiscription => _roomDiscription;
		private Action<NetStringShort>? _onRoomDiscriptionChanged;
		public event Action<NetStringShort> OnRoomDiscriptionChanged
		{
			add => _onRoomDiscriptionChanged += value;
			remove => _onRoomDiscriptionChanged -= value;
		}
		[SyncVar]
		private int _password;
		public int Password => _password;
		private Action<int>? _onPasswordChanged;
		public event Action<int> OnPasswordChanged
		{
			add => _onPasswordChanged += value;
			remove => _onPasswordChanged -= value;
		}
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void Server_LoadGame(GameMapType mapType);
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void ServerRoomSetAck_Callback(RoomSettingResult callback);
		private BitmaskByte _dirtyReliable_0 = new();
		public override bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _dirtyReliable_0.AnyTrue();
				return isDirty;
			}
		}
		public override bool IsDirtyUnreliable => false;
		public partial void Client_ReadyToSync()
		{
			Client_ReadyToSyncCallstackCount++;
			_dirtyReliable_0[0] = true;
		}
		private byte Client_ReadyToSyncCallstackCount = 0;
		public partial void Client_OnMapLoaded()
		{
			Client_OnMapLoadedCallstackCount++;
			_dirtyReliable_0[1] = true;
		}
		private byte Client_OnMapLoadedCallstackCount = 0;
		public partial void ClientRoomSetReq_SetPassword(int password)
		{
			ClientRoomSetReq_SetPasswordiCallstack.Add(password);
			_dirtyReliable_0[2] = true;
		}
		private List<int> ClientRoomSetReq_SetPasswordiCallstack = new(4);
		public partial void ClientRoomSetReq_SetRoomName(NetStringShort roomName)
		{
			ClientRoomSetReq_SetRoomNameNCallstack.Add(roomName);
			_dirtyReliable_0[3] = true;
		}
		private List<NetStringShort> ClientRoomSetReq_SetRoomNameNCallstack = new(4);
		public partial void ClientRoomSetReq_SetRoomDiscription(NetStringShort roomDiscription)
		{
			ClientRoomSetReq_SetRoomDiscriptionNCallstack.Add(roomDiscription);
			_dirtyReliable_0[4] = true;
		}
		private List<NetStringShort> ClientRoomSetReq_SetRoomDiscriptionNCallstack = new(4);
		public partial void ClientRoomSetReq_SetRoomMaxUser(int maxUser)
		{
			ClientRoomSetReq_SetRoomMaxUseriCallstack.Add(maxUser);
			_dirtyReliable_0[5] = true;
		}
		private List<int> ClientRoomSetReq_SetRoomMaxUseriCallstack = new(4);
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
			Client_ReadyToSyncCallstackCount = 0;
			Client_OnMapLoadedCallstackCount = 0;
			ClientRoomSetReq_SetPasswordiCallstack.Clear();
			ClientRoomSetReq_SetRoomNameNCallstack.Clear();
			ClientRoomSetReq_SetRoomDiscriptionNCallstack.Clear();
			ClientRoomSetReq_SetRoomMaxUseriCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				writer.Put((byte)Client_ReadyToSyncCallstackCount);
			}
			if (_dirtyReliable_0[1])
			{
				writer.Put((byte)Client_OnMapLoadedCallstackCount);
			}
			if (_dirtyReliable_0[2])
			{
				byte count = (byte)ClientRoomSetReq_SetPasswordiCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = ClientRoomSetReq_SetPasswordiCallstack[i];
					writer.Put(arg);
				}
			}
			if (_dirtyReliable_0[3])
			{
				byte count = (byte)ClientRoomSetReq_SetRoomNameNCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = ClientRoomSetReq_SetRoomNameNCallstack[i];
					arg.Serialize(writer);
				}
			}
			if (_dirtyReliable_0[4])
			{
				byte count = (byte)ClientRoomSetReq_SetRoomDiscriptionNCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = ClientRoomSetReq_SetRoomDiscriptionNCallstack[i];
					arg.Serialize(writer);
				}
			}
			if (_dirtyReliable_0[5])
			{
				byte count = (byte)ClientRoomSetReq_SetRoomMaxUseriCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = ClientRoomSetReq_SetRoomMaxUseriCallstack[i];
					writer.Put(arg);
				}
			}
		}
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer) { }
		public override void InitializeMasterProperties() { }
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!reader.TryReadInt32(out _currentPlayerCount)) return false;
				_onCurrentPlayerCountChanged?.Invoke(_currentPlayerCount);
			}
			if (dirtyReliable_0[1])
			{
				if (!_roomName.TryDeserialize(reader)) return false;
				_onRoomNameChanged?.Invoke(_roomName);
			}
			if (dirtyReliable_0[2])
			{
				if (!_roomDiscription.TryDeserialize(reader)) return false;
				_onRoomDiscriptionChanged?.Invoke(_roomDiscription);
			}
			if (dirtyReliable_0[3])
			{
				if (!reader.TryReadInt32(out _password)) return false;
				_onPasswordChanged?.Invoke(_password);
			}
			if (dirtyReliable_0[4])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadUInt16(out var mapTypeValue)) return false;
					GameMapType mapType = (GameMapType)mapTypeValue;
					Server_LoadGame(mapType);
				}
			}
			if (dirtyReliable_0[5])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadByte(out var callbackValue)) return false;
					RoomSettingResult callback = (RoomSettingResult)callbackValue;
					ServerRoomSetAck_Callback(callback);
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadInt32(out _currentPlayerCount)) return false;
			_onCurrentPlayerCountChanged?.Invoke(_currentPlayerCount);
			if (!_roomName.TryDeserialize(reader)) return false;
			_onRoomNameChanged?.Invoke(_roomName);
			if (!_roomDiscription.TryDeserialize(reader)) return false;
			_onRoomDiscriptionChanged?.Invoke(_roomDiscription);
			if (!reader.TryReadInt32(out _password)) return false;
			_onPasswordChanged?.Invoke(_password);
			return true;
		}
		public override void InitializeRemoteProperties()
		{
			_currentPlayerCount = 0;
			_roomName = new();
			_roomDiscription = new();
			_password = 0;
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[1])
			{
				NetStringShort.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[2])
			{
				NetStringShort.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[4])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(2);
				}
			}
			if (dirtyReliable_0[5])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(1);
				}
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[1])
			{
				NetStringShort.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[2])
			{
				NetStringShort.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[4])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(2);
				}
			}
			if (dirtyReliable_0[5])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(1);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
