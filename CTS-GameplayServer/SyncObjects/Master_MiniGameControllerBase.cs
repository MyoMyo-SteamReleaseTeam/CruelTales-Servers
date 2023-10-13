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
	public partial class MiniGameControllerBase
	{
		public override NetworkObjectType Type => NetworkObjectType.MiniGameControllerBase;
		[SyncVar]
		protected GameplayState _gameplayState = new();
		[SyncVar]
		protected float _gameTime;
		[SyncVar(SyncType.ColdData)]
		protected float _currentTime;
		[SyncObject]
		protected readonly SyncList<UserId> _eliminatedPlayers;
		[SyncObject]
		protected readonly MapVoteController _mapVoteController;
		[SyncObject]
		protected readonly SyncDictionary<NetByte, NetInt16> _teamScoreByFaction;
		[SyncRpc]
		public partial void Server_GameStartCountdown(float missionShowTime, float countdown);
		[SyncRpc]
		public partial void Server_GameStart(float timeLeft);
		[SyncRpc]
		public partial void Server_FeverTimeStart();
		[SyncRpc]
		public partial void Server_GameEnd(float freezeTime);
		[SyncRpc]
		public partial void Server_ShowResult(float resultTime);
		[SyncRpc]
		public partial void Server_ShowExecution(ExecutionCutSceneType cutSceneType, float playTime);
		[SyncRpc]
		public partial void Server_StartVoteMap(float mapVoteTime);
		[SyncRpc]
		public partial void Server_ShowVotedNextMap(GameSceneIdentity nextMap, float showTime);
		[SyncRpc]
		public partial void Server_SyncTimer(float timeLeft);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public virtual partial void Client_ReadyGame(NetworkPlayer player, bool isReady);
		public MiniGameControllerBase()
		{
			_eliminatedPlayers = new(this);
			_mapVoteController = new(this);
			_teamScoreByFaction = new(this, capacity: 4);
		}
		protected BitmaskByte _dirtyReliable_1 = new();
		protected BitmaskByte _dirtyReliable_2 = new();
		public GameplayState GameplayState
		{
			get => _gameplayState;
			set
			{
				if (_gameplayState == value) return;
				_gameplayState = value;
				_dirtyReliable_0[3] = true;
				MarkDirtyReliable();
			}
		}
		public float GameTime
		{
			get => _gameTime;
			set
			{
				if (_gameTime == value) return;
				_gameTime = value;
				_dirtyReliable_0[4] = true;
				MarkDirtyReliable();
			}
		}
		public SyncList<UserId> EliminatedPlayers => _eliminatedPlayers;
		public MapVoteController MapVoteController => _mapVoteController;
		public SyncDictionary<NetByte, NetInt16> TeamScoreByFaction => _teamScoreByFaction;
		public partial void Server_GameStartCountdown(float missionShowTime, float countdown)
		{
			Server_GameStartCountdownffCallstack.Add((missionShowTime, countdown));
			_dirtyReliable_1[0] = true;
			MarkDirtyReliable();
		}
		protected List<(float missionShowTime, float countdown)> Server_GameStartCountdownffCallstack = new(4);
		public partial void Server_GameStart(float timeLeft)
		{
			Server_GameStartfCallstack.Add(timeLeft);
			_dirtyReliable_1[1] = true;
			MarkDirtyReliable();
		}
		protected List<float> Server_GameStartfCallstack = new(4);
		public partial void Server_FeverTimeStart()
		{
			Server_FeverTimeStartCallstackCount++;
			_dirtyReliable_1[2] = true;
			MarkDirtyReliable();
		}
		protected byte Server_FeverTimeStartCallstackCount = 0;
		public partial void Server_GameEnd(float freezeTime)
		{
			Server_GameEndfCallstack.Add(freezeTime);
			_dirtyReliable_1[3] = true;
			MarkDirtyReliable();
		}
		protected List<float> Server_GameEndfCallstack = new(4);
		public partial void Server_ShowResult(float resultTime)
		{
			Server_ShowResultfCallstack.Add(resultTime);
			_dirtyReliable_1[4] = true;
			MarkDirtyReliable();
		}
		protected List<float> Server_ShowResultfCallstack = new(4);
		public partial void Server_ShowExecution(ExecutionCutSceneType cutSceneType, float playTime)
		{
			Server_ShowExecutionEfCallstack.Add((cutSceneType, playTime));
			_dirtyReliable_1[5] = true;
			MarkDirtyReliable();
		}
		protected List<(ExecutionCutSceneType cutSceneType, float playTime)> Server_ShowExecutionEfCallstack = new(4);
		public partial void Server_StartVoteMap(float mapVoteTime)
		{
			Server_StartVoteMapfCallstack.Add(mapVoteTime);
			_dirtyReliable_1[6] = true;
			MarkDirtyReliable();
		}
		protected List<float> Server_StartVoteMapfCallstack = new(4);
		public partial void Server_ShowVotedNextMap(GameSceneIdentity nextMap, float showTime)
		{
			Server_ShowVotedNextMapGfCallstack.Add((nextMap, showTime));
			_dirtyReliable_1[7] = true;
			MarkDirtyReliable();
		}
		protected List<(GameSceneIdentity nextMap, float showTime)> Server_ShowVotedNextMapGfCallstack = new(4);
		public partial void Server_SyncTimer(float timeLeft)
		{
			Server_SyncTimerfCallstack.Add(timeLeft);
			_dirtyReliable_2[0] = true;
			MarkDirtyReliable();
		}
		protected List<float> Server_SyncTimerfCallstack = new(4);
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Server_TryLoadSceneAllGCallstack.Clear();
			Server_TryLoadSceneGCallstack.Clear();
			_eliminatedPlayers.ClearDirtyReliable();
			_mapVoteController.ClearDirtyReliable();
			_teamScoreByFaction.ClearDirtyReliable();
			_dirtyReliable_1.Clear();
			Server_GameStartCountdownffCallstack.Clear();
			Server_GameStartfCallstack.Clear();
			Server_FeverTimeStartCallstackCount = 0;
			Server_GameEndfCallstack.Clear();
			Server_ShowResultfCallstack.Clear();
			Server_ShowExecutionEfCallstack.Clear();
			Server_StartVoteMapfCallstack.Clear();
			Server_ShowVotedNextMapGfCallstack.Clear();
			_dirtyReliable_2.Clear();
			Server_SyncTimerfCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			BitmaskByte masterDirty = new BitmaskByte();
			_dirtyReliable_0[5] = _eliminatedPlayers.IsDirtyReliable;
			_dirtyReliable_0[6] = _mapVoteController.IsDirtyReliable;
			_dirtyReliable_0[7] = _teamScoreByFaction.IsDirtyReliable;
			masterDirty[0] = _dirtyReliable_0.AnyTrue();
			masterDirty[1] = _dirtyReliable_1.AnyTrue();
			masterDirty[2] = _dirtyReliable_2.AnyTrue();
			int masterDirty_pos = writer.OffsetSize(sizeof(byte));
			if (masterDirty[0])
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
					_teamScoreByFaction.SerializeSyncReliable(writer);
				}
				if (dirtyReliable_0.AnyTrue())
				{
					writer.PutTo(dirtyReliable_0, dirtyReliable_0_pos);
				}
				else
				{
					writer.SetSize(dirtyReliable_0_pos);
					masterDirty[0] = false;
				}
			}
			if (masterDirty[1])
			{
				_dirtyReliable_1.Serialize(writer);
				if (_dirtyReliable_1[0])
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
				if (_dirtyReliable_1[1])
				{
					byte count = (byte)Server_GameStartfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_GameStartfCallstack[i];
						writer.Put(arg);
					}
				}
				if (_dirtyReliable_1[2])
				{
					writer.Put((byte)Server_FeverTimeStartCallstackCount);
				}
				if (_dirtyReliable_1[3])
				{
					byte count = (byte)Server_GameEndfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_GameEndfCallstack[i];
						writer.Put(arg);
					}
				}
				if (_dirtyReliable_1[4])
				{
					byte count = (byte)Server_ShowResultfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_ShowResultfCallstack[i];
						writer.Put(arg);
					}
				}
				if (_dirtyReliable_1[5])
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
				if (_dirtyReliable_1[6])
				{
					byte count = (byte)Server_StartVoteMapfCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_StartVoteMapfCallstack[i];
						writer.Put(arg);
					}
				}
				if (_dirtyReliable_1[7])
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
			}
			if (masterDirty[2])
			{
				_dirtyReliable_2.Serialize(writer);
				if (_dirtyReliable_2[0])
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
			if (masterDirty.AnyTrue())
			{
				writer.PutTo(masterDirty, masterDirty_pos);
			}
			else
			{
				writer.SetSize(masterDirty_pos);
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
			_teamScoreByFaction.SerializeEveryProperty(writer);
		}
		public override void InitializeMasterProperties()
		{
			_gameSceneIdentity = new();
			_gameplayState = (GameplayState)0;
			_gameTime = 0;
			_currentTime = 0;
			_eliminatedPlayers.InitializeMasterProperties();
			_mapVoteController.InitializeMasterProperties();
			_teamScoreByFaction.InitializeMasterProperties();
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
