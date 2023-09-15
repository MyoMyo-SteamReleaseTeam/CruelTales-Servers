/*
 * Generated File : Master_MiniGameControllerBase
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
	public partial class MiniGameControllerBase
	{
		public override NetworkObjectType Type => NetworkObjectType.MiniGameControllerBase;
		[SyncRpc]
		public partial void Server_StartMiniGame();
		[SyncRpc]
		public partial void Server_NextGameStartCountdown(float second);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public virtual partial void Client_ReadyGame(NetworkPlayer player, bool isReady);
		public MiniGameControllerBase()
		{
		}
		public partial void Server_StartMiniGame()
		{
			Server_StartMiniGameCallstackCount++;
			_dirtyReliable_0[3] = true;
			MarkDirtyReliable();
		}
		protected byte Server_StartMiniGameCallstackCount = 0;
		public partial void Server_NextGameStartCountdown(float second)
		{
			Server_NextGameStartCountdownfCallstack.Add(second);
			_dirtyReliable_0[4] = true;
			MarkDirtyReliable();
		}
		protected List<float> Server_NextGameStartCountdownfCallstack = new(4);
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Server_TryLoadSceneAllGCallstack.Clear();
			Server_TryLoadSceneGCallstack.Clear();
			Server_StartMiniGameCallstackCount = 0;
			Server_NextGameStartCountdownfCallstack.Clear();
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
			if (_dirtyReliable_0[3])
			{
				writer.Put((byte)Server_StartMiniGameCallstackCount);
			}
			if (_dirtyReliable_0[4])
			{
				byte count = (byte)Server_NextGameStartCountdownfCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_NextGameStartCountdownfCallstack[i];
					writer.Put(arg);
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
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadBoolean(out bool isReady)) return false;
					Client_ReadyGame(player, isReady);
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
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(1);
				}
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
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(1);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public new static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
