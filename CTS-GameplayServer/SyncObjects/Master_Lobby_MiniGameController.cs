/*
 * Generated File : Master_Lobby_MiniGameController
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
	public partial class Lobby_MiniGameController
	{
		public override NetworkObjectType Type => NetworkObjectType.Lobby_MiniGameController;
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void Server_TryStartGameCallback(NetworkPlayer player, StartGameResultType result);
		[SyncRpc]
		public partial void Server_StartGameCountdown(float second);
		[SyncRpc]
		public partial void Server_CancelStartGameCountdown();
		public Lobby_MiniGameController()
		{
		}
		public partial void Server_TryStartGameCallback(NetworkPlayer player, StartGameResultType result)
		{
			Server_TryStartGameCallbackSCallstack.Add(player, result);
			_dirtyReliable_0[4] = true;
			MarkDirtyReliable();
		}
		protected TargetCallstack<NetworkPlayer, StartGameResultType> Server_TryStartGameCallbackSCallstack = new(8);
		public partial void Server_StartGameCountdown(float second)
		{
			Server_StartGameCountdownfCallstack.Add(second);
			_dirtyReliable_0[5] = true;
			MarkDirtyReliable();
		}
		protected List<float> Server_StartGameCountdownfCallstack = new(4);
		public partial void Server_CancelStartGameCountdown()
		{
			Server_CancelStartGameCountdownCallstackCount++;
			_dirtyReliable_0[6] = true;
			MarkDirtyReliable();
		}
		protected byte Server_CancelStartGameCountdownCallstackCount = 0;
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Server_LoadMiniGameMCallstack.Clear();
			Server_StartMiniGameCallstackCount = 0;
			Server_NextGameStartCountdownfCallstack.Clear();
			Server_TryStartGameCallbackSCallstack.Clear();
			Server_StartGameCountdownfCallstack.Clear();
			Server_CancelStartGameCountdownCallstackCount = 0;
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				_miniGameIdentity.Serialize(writer);
			}
			if (_dirtyReliable_0[1])
			{
				int Server_LoadMiniGameMCount = Server_LoadMiniGameMCallstack.GetCallCount(player);
				if (Server_LoadMiniGameMCount > 0)
				{
					var Server_LoadMiniGameMcallList = Server_LoadMiniGameMCallstack.GetCallList(player);
					writer.Put((byte)Server_LoadMiniGameMCount);
					for (int i = 0; i < Server_LoadMiniGameMCount; i++)
					{
						var arg = Server_LoadMiniGameMcallList[i];
						arg.Serialize(writer);
					}
				}
				else
				{
					dirtyReliable_0[1] = false;
				}
			}
			if (_dirtyReliable_0[2])
			{
				writer.Put((byte)Server_StartMiniGameCallstackCount);
			}
			if (_dirtyReliable_0[3])
			{
				byte count = (byte)Server_NextGameStartCountdownfCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_NextGameStartCountdownfCallstack[i];
					writer.Put(arg);
				}
			}
			if (_dirtyReliable_0[4])
			{
				int Server_TryStartGameCallbackSCount = Server_TryStartGameCallbackSCallstack.GetCallCount(player);
				if (Server_TryStartGameCallbackSCount > 0)
				{
					var Server_TryStartGameCallbackScallList = Server_TryStartGameCallbackSCallstack.GetCallList(player);
					writer.Put((byte)Server_TryStartGameCallbackSCount);
					for (int i = 0; i < Server_TryStartGameCallbackSCount; i++)
					{
						var arg = Server_TryStartGameCallbackScallList[i];
						writer.Put((byte)arg);
					}
				}
				else
				{
					dirtyReliable_0[4] = false;
				}
			}
			if (_dirtyReliable_0[5])
			{
				byte count = (byte)Server_StartGameCountdownfCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_StartGameCountdownfCallstack[i];
					writer.Put(arg);
				}
			}
			if (_dirtyReliable_0[6])
			{
				writer.Put((byte)Server_CancelStartGameCountdownCallstackCount);
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
			_miniGameIdentity.Serialize(writer);
		}
		public override void InitializeMasterProperties()
		{
			_miniGameIdentity = new();
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Client_OnMiniGameLoaded(player);
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
