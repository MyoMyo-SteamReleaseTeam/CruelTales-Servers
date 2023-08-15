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
	public partial class GameplayController
	{
		public override NetworkObjectType Type => NetworkObjectType.GameplayController;
		[SyncObject(dir: SyncDirection.Bidirection)]
		private readonly RoomSessionManager _roomSessionManager;
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void Server_LoadGame(NetworkPlayer player, GameMapType mapType);
		private Action<RoomSessionManager>? _onRoomSessionManagerChanged;
		public event Action<RoomSessionManager> OnRoomSessionManagerChanged
		{
			add => _onRoomSessionManagerChanged += value;
			remove => _onRoomSessionManagerChanged -= value;
		}
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_ReadyToSync(NetworkPlayer player);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_OnMapLoaded(NetworkPlayer player);
		public GameplayController()
		{
			_roomSessionManager = new(this);
		}
		private BitmaskByte _dirtyReliable_0 = new();
		public RoomSessionManager RoomSessionManager => _roomSessionManager;
		public partial void Server_LoadGame(NetworkPlayer player, GameMapType mapType)
		{
			Server_LoadGameGCallstack.Add(player, mapType);
			_dirtyReliable_0[1] = true;
			MarkDirtyReliable();
		}
		private TargetCallstack<NetworkPlayer, GameMapType> Server_LoadGameGCallstack = new(8);
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			_roomSessionManager.ClearDirtyReliable();
			Server_LoadGameGCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyReliable_0[0] = _roomSessionManager.IsDirtyReliable;
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				int curSize = writer.Size;
				_roomSessionManager.SerializeSyncReliable(player, writer);
				if (writer.Size == curSize)
				{
					dirtyReliable_0[0] = false;
				}
			}
			if (_dirtyReliable_0[1])
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
					dirtyReliable_0[1] = false;
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
			_roomSessionManager.SerializeEveryProperty(writer);
		}
		public override void InitializeMasterProperties()
		{
			_roomSessionManager.InitializeMasterProperties();
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!_roomSessionManager.TryDeserializeSyncReliable(player, reader)) return false;
				_onRoomSessionManagerChanged?.Invoke(_roomSessionManager);
			}
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Client_ReadyToSync(player);
				}
			}
			if (dirtyReliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Client_OnMapLoaded(player);
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => true;
		public override void InitializeRemoteProperties()
		{
			_roomSessionManager.InitializeMasterProperties();
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				_roomSessionManager.IgnoreSyncReliable(reader);
			}
			if (dirtyReliable_0[1])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[2])
			{
				reader.Ignore(1);
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				RoomSessionManager.IgnoreSyncStaticReliable(reader);
			}
			if (dirtyReliable_0[1])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[2])
			{
				reader.Ignore(1);
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
