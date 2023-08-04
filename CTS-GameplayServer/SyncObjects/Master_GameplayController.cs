/*
 * Generated File : Master_GameplayController
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
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.SyncObjects
{
	[Serializable]
	public partial class GameplayController
	{
		public override NetworkObjectType Type => NetworkObjectType.GameplayController;
		[SyncVar]
		private int _currentPlayerCount;
		[SyncVar]
		private NetStringShort _roomName = new();
		[SyncVar]
		private NetStringShort _roomDiscription = new();
		[SyncVar]
		private int _password;
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void Server_LoadGame(NetworkPlayer player, GameMapType mapType);
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void ServerRoomSetAck_Callback(NetworkPlayer player, RoomSettingResult callback);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_ReadyToSync(NetworkPlayer player);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_OnMapLoaded(NetworkPlayer player);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetPassword(NetworkPlayer player, int password);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetRoomName(NetworkPlayer player, NetStringShort roomName);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetRoomDiscription(NetworkPlayer player, NetStringShort roomDiscription);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void ClientRoomSetReq_SetRoomMaxUser(NetworkPlayer player, int maxUser);
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
		public int CurrentPlayerCount
		{
			get => _currentPlayerCount;
			set
			{
				if (_currentPlayerCount == value) return;
				_currentPlayerCount = value;
				_dirtyReliable_0[0] = true;
			}
		}
		public NetStringShort RoomName
		{
			get => _roomName;
			set
			{
				if (_roomName == value) return;
				_roomName = value;
				_dirtyReliable_0[1] = true;
			}
		}
		public NetStringShort RoomDiscription
		{
			get => _roomDiscription;
			set
			{
				if (_roomDiscription == value) return;
				_roomDiscription = value;
				_dirtyReliable_0[2] = true;
			}
		}
		public int Password
		{
			get => _password;
			set
			{
				if (_password == value) return;
				_password = value;
				_dirtyReliable_0[3] = true;
			}
		}
		public partial void Server_LoadGame(NetworkPlayer player, GameMapType mapType)
		{
			Server_LoadGameGCallstack.Add(player, mapType);
			_dirtyReliable_0[4] = true;
		}
		private TargetCallstack<NetworkPlayer, GameMapType> Server_LoadGameGCallstack = new(8);
		public partial void ServerRoomSetAck_Callback(NetworkPlayer player, RoomSettingResult callback)
		{
			ServerRoomSetAck_CallbackRCallstack.Add(player, callback);
			_dirtyReliable_0[5] = true;
		}
		private TargetCallstack<NetworkPlayer, RoomSettingResult> ServerRoomSetAck_CallbackRCallstack = new(8);
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
			Server_LoadGameGCallstack.Clear();
			ServerRoomSetAck_CallbackRCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				writer.Put(_currentPlayerCount);
			}
			if (_dirtyReliable_0[1])
			{
				_roomName.Serialize(writer);
			}
			if (_dirtyReliable_0[2])
			{
				_roomDiscription.Serialize(writer);
			}
			if (_dirtyReliable_0[3])
			{
				writer.Put(_password);
			}
			if (_dirtyReliable_0[4])
			{
				int Server_LoadGameGCount = Server_LoadGameGCallstack.GetCallCount(player);
				if (Server_LoadGameGCount > 0)
				{
					var Server_LoadGameGcallList = Server_LoadGameGCallstack.GetCallList(player);
					writer.Put((byte)Server_LoadGameGCount);
					for (int i = 0; i < Server_LoadGameGCount; i++)
					{
						var arg = Server_LoadGameGcallList[i];
						writer.Put((ushort)arg);
					}
				}
				else
				{
					dirtyReliable_0[4] = false;
				}
			}
			if (_dirtyReliable_0[5])
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
					dirtyReliable_0[5] = false;
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
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			writer.Put(_currentPlayerCount);
			_roomName.Serialize(writer);
			_roomDiscription.Serialize(writer);
			writer.Put(_password);
		}
		public override void InitializeMasterProperties()
		{
			_currentPlayerCount = 0;
			_roomName = new();
			_roomDiscription = new();
			_password = 0;
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Client_ReadyToSync(player);
				}
			}
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Client_OnMapLoaded(player);
				}
			}
			if (dirtyReliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int password)) return false;
					ClientRoomSetReq_SetPassword(player, password);
				}
			}
			if (dirtyReliable_0[3])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort roomName = new();
					if (!roomName.TryDeserialize(reader)) return false;
					ClientRoomSetReq_SetRoomName(player, roomName);
				}
			}
			if (dirtyReliable_0[4])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort roomDiscription = new();
					if (!roomDiscription.TryDeserialize(reader)) return false;
					ClientRoomSetReq_SetRoomDiscription(player, roomDiscription);
				}
			}
			if (dirtyReliable_0[5])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int maxUser)) return false;
					ClientRoomSetReq_SetRoomMaxUser(player, maxUser);
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
			if (dirtyReliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
			if (dirtyReliable_0[3])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort.IgnoreStatic(reader);
				}
			}
			if (dirtyReliable_0[4])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort.IgnoreStatic(reader);
				}
			}
			if (dirtyReliable_0[5])
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
				reader.Ignore(1);
			}
			if (dirtyReliable_0[1])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
			if (dirtyReliable_0[3])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort.IgnoreStatic(reader);
				}
			}
			if (dirtyReliable_0[4])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort.IgnoreStatic(reader);
				}
			}
			if (dirtyReliable_0[5])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
