/*
 * Generated File : Master_SceneControllerBase
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
	public partial class SceneControllerBase
	{
		public override NetworkObjectType Type => NetworkObjectType.SceneControllerBase;
		[SyncVar]
		protected GameSceneIdentity _gameSceneIdentity = new();
		[SyncRpc]
		public partial void Server_TryLoadSceneAll(GameSceneIdentity gameScene);
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void Server_TryLoadScene(NetworkPlayer player, GameSceneIdentity gameScene);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public virtual partial void Client_OnSceneLoaded(NetworkPlayer player);
		public SceneControllerBase()
		{
		}
		protected BitmaskByte _dirtyReliable_0 = new();
		public GameSceneIdentity GameSceneIdentity
		{
			get => _gameSceneIdentity;
			set
			{
				if (_gameSceneIdentity == value) return;
				_gameSceneIdentity = value;
				_dirtyReliable_0[0] = true;
				MarkDirtyReliable();
			}
		}
		public partial void Server_TryLoadSceneAll(GameSceneIdentity gameScene)
		{
			Server_TryLoadSceneAllGCallstack.Add(gameScene);
			_dirtyReliable_0[1] = true;
			MarkDirtyReliable();
		}
		protected List<GameSceneIdentity> Server_TryLoadSceneAllGCallstack = new(4);
		public partial void Server_TryLoadScene(NetworkPlayer player, GameSceneIdentity gameScene)
		{
			Server_TryLoadSceneGCallstack.Add(player, gameScene);
			_dirtyReliable_0[2] = true;
			MarkDirtyReliable();
		}
		protected TargetCallstack<NetworkPlayer, GameSceneIdentity> Server_TryLoadSceneGCallstack = new(8);
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Server_TryLoadSceneAllGCallstack.Clear();
			Server_TryLoadSceneGCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				_gameSceneIdentity.Serialize(writer);
			}
			if (_dirtyReliable_0[1])
			{
				byte count = (byte)Server_TryLoadSceneAllGCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_TryLoadSceneAllGCallstack[i];
					arg.Serialize(writer);
				}
			}
			if (_dirtyReliable_0[2])
			{
				int Server_TryLoadSceneGCount = Server_TryLoadSceneGCallstack.GetCallCount(player);
				if (Server_TryLoadSceneGCount > 0)
				{
					var Server_TryLoadSceneGcallList = Server_TryLoadSceneGCallstack.GetCallList(player);
					writer.Put((byte)Server_TryLoadSceneGCount);
					for (int i = 0; i < Server_TryLoadSceneGCount; i++)
					{
						var arg = Server_TryLoadSceneGcallList[i];
						arg.Serialize(writer);
					}
				}
				else
				{
					dirtyReliable_0[2] = false;
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
			_gameSceneIdentity.Serialize(writer);
		}
		public override void InitializeMasterProperties()
		{
			_gameSceneIdentity = new();
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Client_OnSceneLoaded(player);
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
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
