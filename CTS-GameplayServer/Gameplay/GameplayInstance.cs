using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Networks.Runtimes;
using CTS.Instance.Networks;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Gameplay
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SynchronizeJob
	{
		public SyncOperation Operation;
		public IPacketReader SyncDataReader;

		public SynchronizeJob(SyncOperation operation, IPacketReader reader)
		{
			Operation = operation;
			SyncDataReader = reader;
		}
	}

	/// <summary>서버의 초기화 옵션입니다.</summary>
	public struct InstanceInitializeOption
	{
		public int SystemMaxUser { get; set; }
		public int SyncJobCapacity => SystemMaxUser * 20;
		public int SessionJobCapacity => (int)(SystemMaxUser * 1.5f);
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
		public WorldManager WorldManager { get; private set; }

		// Job Queue
		private JobQueue<SynchronizeJob> _syncJobQueue;

		public GameplayInstance(TickTimer serverTimer, InstanceInitializeOption option)
		{
			ServerTimer = serverTimer;
			SessionHandler = new UserSessionHandler(this, option);
			WorldManager = new WorldManager();
			_syncJobQueue = new(onSyncJobExecute, option.SyncJobCapacity);
		}

		public void Initialize(GameplayOption option, GameInstanceGuid guid)
		{
			Option = option;
			Guid = guid;
			WorldManager.Clear();
		}

		/// <summary>Update logic</summary>
		/// <param name="deltaTime">Delta Time</param>
		public void Update(float deltaTime)
		{
			// Handle network connections
			SessionHandler.Flush();

			// Update game logic
			_syncJobQueue.Flush();
			WorldManager.Update(deltaTime);
			WorldManager.UpdateSerialize();
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

		#region Session

		public void OnUserEnterGame(UserSession userSession)
		{
			var playerEntity = WorldManager.CreateObject<NetworkPlayer>();
			playerEntity.BindUser(userSession);
		}

		public void OnUserLeaveGame(UserSession userSession)
		{
			_log.Info($"[Instance:{Guid}] Session {userSession} leave the game");
		}

		#endregion

		#region Synchronize

		public void OnUserTrySync(SyncOperation syncType, IPacketReader packetReader)
		{
			_syncJobQueue.Push(new SynchronizeJob(syncType, packetReader));
		}

		private void onSyncJobExecute(SynchronizeJob syncJob)
		{
			switch (syncJob.Operation)
			{
				case SyncOperation.Reliable:
					WorldManager.OnDeserializeSyncReliable(syncJob.SyncDataReader);
					break;

				case SyncOperation.Unreliable:
					WorldManager.OnDeserializeSyncUnreliable(syncJob.SyncDataReader);
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

		public override string ToString()
		{
			return $"[GUID:{Guid}][MemberCount:{SessionHandler.MemberCount}]";
		}
	}
}
