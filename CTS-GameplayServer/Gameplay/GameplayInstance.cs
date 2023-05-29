﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Networks.Runtimes;
using CTS.Instance.Networks;
using log4net;

namespace CTS.Instance.Gameplay
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SynchronizeJob
	{
		public UserId Sender;
		public SyncOperation Operation;
		public IPacketReader SyncDataReader;

		public SynchronizeJob(UserId sender, SyncOperation operation, IPacketReader reader)
		{
			Sender = sender;
			Operation = operation;
			SyncDataReader = reader;
		}
	}

	/// <summary>게임 진행을 위한 옵션입니다.</summary>
	public struct GameplayOption
	{
		public int MaxUser { get; set; }
	}

	public class GameplayInstance
	{
		// Support
		[AllowNull] public readonly static ILog _log = LogManager.GetLogger(typeof(GameplayInstance));

		public TickTimer ServerTimer { get; private set; }

		// Instance property
		public GameInstanceGuid Guid { get; private set; }
		public GameplayOption Option { get; private set; }

		// Handlers
		public UserSessionHandler SessionHandler { get; private set; }

		// Managers
		public GameManager GameManager { get; private set; }
		public WorldManager WorldManager { get; private set; }

		// Job Queue
		private JobQueue<SynchronizeJob> _syncJobQueue;

		public GameplayInstance(TickTimer serverTimer, InstanceInitializeOption option)
		{
			ServerTimer = serverTimer;
			SessionHandler = new UserSessionHandler(this, option);
			WorldManager = new WorldManager(this, option);
			GameManager = new GameManager(this, option);
			_syncJobQueue = new(onSyncJobExecute, option.SyncJobCapacity);
		}

		public void Initialize(GameplayOption option, GameInstanceGuid guid)
		{
			Option = option;
			Guid = guid;
			WorldManager.Clear();

			// TODO : Start game properly
			GameManager.StartGame();
		}

		/// <summary>Update logic</summary>
		/// <param name="deltaTime">Delta Time</param>
		public void Update(float deltaTime)
		{
			// Handle network connections
			SessionHandler.Flush();

			// Sync network object from remote
			_syncJobQueue.Flush();

			// Update world positions and logic
			WorldManager.UpdateNetworkObjects(deltaTime);

			// Update game manager logic
			GameManager.Update(deltaTime);

			// Send sync data to each user
			WorldManager.UpdateVisibilityAndSendData();

			// Update world partitions
			WorldManager.UpdateWorldPartitions();

			// Reset dirtys
			WorldManager.ClearDirtys();

			// Remove objects
			WorldManager.UpdateRemoveObjects();
		}

		public void Shutdown(DisconnectReasonType reason)
		{
			foreach (var ps in SessionHandler.UserList)
			{
				DisconnectPlayer(ps, reason);
			}
		}

		public void DisconnectPlayer(UserSession userSession, DisconnectReasonType reason)
		{
			userSession.Disconnect(reason);
		}

		public void DisconnectPlayer(UserId userId, DisconnectReasonType reason)
		{
			if (SessionHandler.TryGetUserSession(userId, out var userSession))
			{
				userSession.Disconnect(reason);
			}
		}

		#region Synchronize

		public bool TrySync(UserId sender, SyncOperation syncType, IPacketReader packetReader)
		{
			if (!SessionHandler.IsConnected(sender))
			{
				return false;
			}

			_syncJobQueue.Push(new SynchronizeJob(sender, syncType, packetReader));
			return true;
		}

		private void onSyncJobExecute(SynchronizeJob syncJob)
		{
			switch (syncJob.Operation)
			{
				case SyncOperation.Reliable:
					if (!WorldManager.OnDeserializeSyncReliable(syncJob.Sender, syncJob.SyncDataReader))
					{
						DisconnectPlayer(syncJob.Sender, DisconnectReasonType.ServerError_CannotHandlePacket);
					}
					break;

				case SyncOperation.Unreliable:
					if (!WorldManager.OnDeserializeSyncUnreliable(syncJob.Sender, syncJob.SyncDataReader))
					{
						DisconnectPlayer(syncJob.Sender, DisconnectReasonType.ServerError_CannotHandlePacket);
					}
					break;

				case SyncOperation.Creation:
				case SyncOperation.Destruction:
					_log.Fatal($"Client try to manage sync object life cycle! Type : {syncJob.Operation}");
					break;

				default:
					_log.Warn($"There is no such sync operation. Type : {syncJob.Operation}");
					break;
			}
		}

		#endregion

		public override string ToString() => $"[i:{Guid}][CCU:{SessionHandler.MemberCount}]";
	}
}
