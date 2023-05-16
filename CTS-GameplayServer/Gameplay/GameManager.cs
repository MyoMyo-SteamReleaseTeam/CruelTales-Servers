using System;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CT.Packets;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.Gameplay
{
	public class GameManager
	{
		private MiniGameMapData _miniGameMapData { get; set; }
		private GameInstance _gameInstance;
		private UserSessionHandler _userSessionHandler;

		// Test buffer
		private byte[] _packetBuffer = new byte[1024 * 64];

		private GameWorldManager _gameWorldManager;

		public GameManager(GameInstance gameInstance)
		{
			_gameInstance = gameInstance;
			_userSessionHandler = gameInstance.SessionHandler;
			_gameWorldManager = new GameWorldManager();

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
			_gameWorldManager.Clear();
		}

		public void StartGame()
		{

		}

		float sendTime = 0;

		public void Update(float deltaTime)
		{
			_gameWorldManager.Update(deltaTime);
			_gameWorldManager.UpdateSerialize();
		}

		public void CheckEndCondition()
		{

		}

		public void OnUserEnter(UserSession userSession)
		{
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

		public void SyncReliable()
		{
			//var netObjs = _worldObject.Values;
			//if (netObjs.Count <= 0)
			//	return;

			//int syncCount = 0;

			//PacketWriter pw = new(_packetBuffer);
			//pw.Put(PacketType.SC_Sync_Reliable);
			//foreach (var netObj in netObjs)
			//{
			//	if (!netObj.IsDirtyReliable)
			//	{
			//		continue;
			//	}
			//	pw.Put(netObj.Identity);
			//	netObj.SerializeSyncReliable(pw);
			//	netObj.ClearDirtyReliable();
			//	syncCount++;
			//}

			//if (syncCount > 0)
			//{
			//	_userSessionHandler.SendReliableToAll(pw);
			//}
		}

		public void SyncUnreliable()
		{
			//var netObjs = _worldObject.Values;
			//if (netObjs.Count <= 0)
			//	return;

			//int syncCount = 0;

			//PacketWriter pw = new(_packetBuffer);
			//pw.Put(PacketType.SC_Sync_Unreliable);
			//foreach (var netObj in netObjs)
			//{
			//	if (!netObj.IsDirtyUnreliable)
			//	{
			//		continue;
			//	}
			//	pw.Put(netObj.Identity);
			//	netObj.SerializeSyncUnreliable(pw);
			//	netObj.ClearDirtyUnreliable();
			//	syncCount++;
			//}

			//if (syncCount > 0)
			//{
			//	_userSessionHandler.SendReliableToAll(pw);
			//}
		}

		public void OnUserTrySync(SyncOperation syncType, IPacketReader packetReader)
		{
			//_gameWorldManager.
		}
	}
}
