using System.Numerics;
using System.Runtime.InteropServices;
using CT.Common.Gameplay;
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

	public class GameManager : IJobHandler
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(GameManager));

		private MiniGameMapData _miniGameMapData { get; set; }
		private GameInstance _gameInstance;
		private UserSessionHandler _userSessionHandler;
		public GameWorldManager WorldManager { get; private set; }

		// Job Queue
		private JobQueue<SynchronizeJob> _syncJobQueue;

		public GameManager(GameInstance gameInstance, int syncJobCapacity)
		{
			_gameInstance = gameInstance;
			_userSessionHandler = gameInstance.SessionHandler;
			WorldManager = new GameWorldManager();

			// Job Queue
			_syncJobQueue = new JobQueue<SynchronizeJob>(onSyncJobExecute, syncJobCapacity);

			// Temp
			_miniGameMapData = new()
			{
				MapType = MiniGameMapType.Map_Square_Europe,
				Theme = MiniGameMapTheme.Europe,
				SpawnPosition = new()
				{
					new Vector3(12.27F, 0F, 7.45F),
					new Vector3(13.77F, 0F, 5.75F),
					new Vector3(15.77F, 0F, 5.25F),
					new Vector3(17.77F, 0F, 5.75F),
					new Vector3(19.27F, 0F, 7.45F),
					new Vector3(14.57F, 0F, 7.75F),
					new Vector3(16.97F, 0F, 7.75F),
					new Vector3(15.77F, 0F, 6.55F),
				},
			};
		}

		public void Initialize()
		{
			WorldManager.Clear();
		}

		public void Update(float deltaTime)
		{
			WorldManager.Update(deltaTime);
			WorldManager.UpdateSerialize();
		}

		public void Clear()
		{
		}

		public void Flush()
		{
			_syncJobQueue.Flush();
		}

		public void OnUserEnter(UserSession userSession)
		{
			var playerEntity = WorldManager.CreateObject<NetworkPlayer>();
			playerEntity.BindUser(userSession);
			//PacketWriter pw = new(_packetBuffer);
			//pw.Put(PacketType.SC_Sync_LifeCycle);
			//foreach (var netObj in _worldObject.Values)
			//{
			//	pw.Put(netObj.Type);
			//	pw.Put(netObj.Identity);
			//	netObj.SerializeEveryProperty(pw);
			//}
			//userSession.SendReliable(pw);
		}

		internal void OnUserLeave(UserSession userSession)
		{
		}

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
	}
}
