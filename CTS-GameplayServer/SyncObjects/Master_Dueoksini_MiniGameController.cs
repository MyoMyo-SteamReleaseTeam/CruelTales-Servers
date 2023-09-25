/*
 * Generated File : Master_Dueoksini_MiniGameController
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
	public partial class Dueoksini_MiniGameController
	{
		public override NetworkObjectType Type => NetworkObjectType.Dueoksini_MiniGameController;
		public Dueoksini_MiniGameController()
		{
		}
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Server_TryLoadSceneAllGCallstack.Clear();
			Server_TryLoadSceneGCallstack.Clear();
			_eliminatedPlayers.ClearDirtyReliable();
			_mapVoteController.ClearDirtyReliable();
			Server_GameStartCountdownffCallstack.Clear();
			_dirtyReliable_1.Clear();
			Server_GameStartfCallstack.Clear();
			Server_GameEndfCallstack.Clear();
			Server_ShowResultfCallstack.Clear();
			Server_ShowExecutionEfCallstack.Clear();
			Server_StartVoteMapfCallstack.Clear();
			Server_ShowVotedNextMapGfCallstack.Clear();
			Server_SyncTimerfCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			int originSize = writer.Size;
			_dirtyReliable_0[5] = _eliminatedPlayers.IsDirtyReliable;
			_dirtyReliable_0[6] = _mapVoteController.IsDirtyReliable;
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0.AnyTrue())
			{
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
					writer.Put((byte)_gameplayState);
				}
				if (_dirtyReliable_0[4])
				{
					writer.Put(_gameTime);
				}
				if (_dirtyReliable_0[5])
				{
					_eliminatedPlayers.SerializeSyncReliable(writer);
				}
				if (_dirtyReliable_0[6])
				{
					_mapVoteController.SerializeSyncReliable(player, writer);
				}
				if (_dirtyReliable_0[7])
				{
					byte count = (byte)Server_GameStartCountdownffCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_GameStartCountdownffCallstack[i];
						writer.Put(arg.missionShowTime);
						writer.Put(arg.countdown);
					}
				}
			}
			writer.PutTo(dirtyReliable_0, dirtyReliable_0_pos);
			_dirtyReliable_1.Serialize(writer);
			if (_dirtyReliable_1.AnyTrue())
			{
				if (_dirtyReliable_1[0])
				{
					byte count = (byte)Server_GameStartfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_GameStartfCallstack[i];
						writer.Put(arg);
					}
				}
				if (_dirtyReliable_1[1])
				{
					byte count = (byte)Server_GameEndfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_GameEndfCallstack[i];
						writer.Put(arg);
					}
				}
				if (_dirtyReliable_1[2])
				{
					byte count = (byte)Server_ShowResultfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_ShowResultfCallstack[i];
						writer.Put(arg);
					}
				}
				if (_dirtyReliable_1[3])
				{
					byte count = (byte)Server_ShowExecutionEfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_ShowExecutionEfCallstack[i];
						writer.Put((byte)arg.cutSceneType);
						writer.Put(arg.playTime);
					}
				}
				if (_dirtyReliable_1[4])
				{
					byte count = (byte)Server_StartVoteMapfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_StartVoteMapfCallstack[i];
						writer.Put(arg);
					}
				}
				if (_dirtyReliable_1[5])
				{
					byte count = (byte)Server_ShowVotedNextMapGfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_ShowVotedNextMapGfCallstack[i];
						arg.nextMap.Serialize(writer);
						writer.Put(arg.showTime);
					}
				}
				if (_dirtyReliable_1[6])
				{
					byte count = (byte)Server_SyncTimerfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_SyncTimerfCallstack[i];
						writer.Put(arg);
					}
				}
			}
			if (writer.Size == originSize + 2)
			{
				writer.SetSize(originSize);
			}
		}
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			_gameSceneIdentity.Serialize(writer);
			writer.Put((byte)_gameplayState);
			writer.Put(_gameTime);
			writer.Put(_currentTime);
			_eliminatedPlayers.SerializeEveryProperty(writer);
			_mapVoteController.SerializeEveryProperty(writer);
		}
		public override void InitializeMasterProperties()
		{
			_gameSceneIdentity = new();
			_gameplayState = (GameplayState)0;
			_gameTime = 0;
			_currentTime = 0;
			_eliminatedPlayers.InitializeMasterProperties();
			_mapVoteController.InitializeMasterProperties();
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
