using System.Diagnostics.CodeAnalysis;
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
		public IPacketReader SyncSegment;

		public SynchronizeJob(UserId sender, SyncOperation operation, IPacketReader reader)
		{
			Sender = sender;
			Operation = operation;
			SyncSegment = reader;
		}
	}

	public class GameplayInstance
	{
		// Support
		[AllowNull] public readonly static ILog _log = LogManager.GetLogger(typeof(GameplayInstance));

		// Reference
		public ServerOption ServerOption { get; private set; }
		public TickTimer ServerTimer { get; private set; }
		public InstanceInitializeOption InitializeOption { get; private set; }

		// Instance property
		public GameInstanceGuid Guid { get; private set; }
		public RoomOption RoomOption { get; private set; } = new();

		// Handlers
		public UserSessionHandler SessionHandler { get; private set; }

		// Managers
		public GameplayManager GameplayManager { get; private set; }

		// Job Queue
		private JobQueue<SynchronizeJob> _syncJobQueue;
		private ConcurrentByteBufferPool _syncPacketPool;

		public GameplayInstance(ServerOption serverOption,
								TickTimer serverTimer,
								InstanceInitializeOption option)
		{
			ServerOption = serverOption;
			ServerTimer = serverTimer;
			InitializeOption = option;
			RoomOption.Reset(InitializeOption);
			SessionHandler = new UserSessionHandler(this, InitializeOption, RoomOption);

			GameplayManager = new GameplayManager(this, ServerOption, InitializeOption);

			_syncJobQueue = new(onSyncJobExecute, InitializeOption.SyncJobCapacity);
			_syncPacketPool = new(1024 * 8, InitializeOption.RemotePacketPoolCount);
		}

		public void Initialize(GameInstanceGuid guid)
		{
			Guid = guid;
			RoomOption.Reset(InitializeOption);

			// TODO : Start game properly
			GameplayManager.Reset();
		}

		/// <summary>Update logic</summary>
		/// <param name="deltaTime">Delta Time</param>
		public void Update(float deltaTime)
		{
			// Handle network connections
			SessionHandler.Flush();

			// Sync network object from remote
			_syncJobQueue.Flush();

			// Update game manager logic
			GameplayManager.Update(deltaTime);
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

			ByteBuffer syncSegment = _syncPacketPool.Get();
			syncSegment.CopyFromReader(packetReader);

			_syncJobQueue.Push(new SynchronizeJob(sender, syncType, syncSegment));
			return true;
		}

		private void onSyncJobExecute(SynchronizeJob syncJob)
		{
			ByteBuffer syncSeg = (ByteBuffer)syncJob.SyncSegment;

			switch (syncJob.Operation)
			{
				case SyncOperation.Reliable:
					if (!GameplayManager.WorldManager.OnRemoteReliable(syncJob.Sender, syncJob.SyncSegment))
					{
						DisconnectPlayer(syncJob.Sender, DisconnectReasonType.ServerError_CannotHandlePacket);
					}
					break;

				case SyncOperation.Unreliable:
					if (!GameplayManager.WorldManager.OnRemoteUnreliable(syncJob.Sender, syncJob.SyncSegment))
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

			_syncPacketPool.Return(syncSeg);
		}

		#endregion

		public override string ToString() => $"[i:{Guid}][CCU:{SessionHandler.MemberCount}]";
	}
}
