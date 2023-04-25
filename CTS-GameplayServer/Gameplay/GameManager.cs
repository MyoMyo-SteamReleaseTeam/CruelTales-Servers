using System.Collections.Generic;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Packets;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Gameplay
{
	public class GameWorldManager
	{
		private Dictionary<NetworkIdentity, MasterNetworkObject> _worldObjectById = new();
		private NetworkIdentity _entityIdCounter;

		public void Clear()
		{
			//_entityById.Clear();
		}

		public void AddPlayer(UserSession session)
		{
			//_entityIdCounter = new NetEntityId(_entityIdCounter.ID + 1);
			//var playerEntity = new Entity_Player();
			//playerEntity.BindClient(session.UserId);
			//_entityById.Add(_entityIdCounter, playerEntity);
		}

		public void Create(MasterNetworkObject netObject)
		{
			_worldObjectById.Add(netObject.Identity, netObject);
		}
	}

	public class GameManager
	{
		private MiniGameMapData MiniGameMapData { get; set; }
		private GameInstance _gameInstance;
		private UserSessionHandler _userSessionHandler;

		// Test buffer
		private byte[] _packetBuffer = new byte[1024 * 64];

		private Dictionary<NetworkIdentity, MasterNetworkObject> _worldObject = new();

		private static ushort _networkIdentityCounter = new();

		public GameManager(GameInstance gameInstance)
		{
			_gameInstance = gameInstance;
			_userSessionHandler = gameInstance.SessionHandler;

			// Temp
			MiniGameMapData = new()
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

		private NetworkIdentity GetNetworkIdentityCounter() 
			=> new NetworkIdentity(++_networkIdentityCounter);

		public void StartGame()
		{
			for (int z = 0; z < 20; z++)
			{
				for (int x = 0; x < 20; x++)
				{
					var netId = GetNetworkIdentityCounter();
					Test_MovingCube cube = new Test_MovingCube();
					cube.OnCreated(netId);
					cube.X = x;
					cube.Y = 5;
					cube.Z = z;
					cube.R = 1;
					cube.G = 1;
					cube.B = 1;
					cube.Dest = 8;
					cube.Speed = 2.5f;
					_worldObject.Add(cube.Identity, cube);
					cube.SetMoveTime((x * z + 1) * 0.0125f);
				}
			}
		}

		float sendTime = 0;

		public void Update(float deltaTime)
		{
			foreach (var netObj in _worldObject.Values)
			{
				if (netObj is Test_MovingCube cube)
				{
					cube.Update(deltaTime);
				}
			}
		}

		public void CheckEndCondition()
		{

		}

		public void OnUserEnter(UserSession userSession)
		{
			PacketWriter pw = new(_packetBuffer);
			pw.Put(PacketType.SC_Sync_LifeCycle);
			foreach (var netObj in _worldObject.Values)
			{
				pw.Put(netObj.Type);
				pw.Put(netObj.Identity);
				netObj.SerializeEveryProperty(pw);
			}
			userSession.SendReliable(pw);
		}

		public void SyncReliable()
		{
			var netObjs = _worldObject.Values;
			if (netObjs.Count <= 0)
				return;

			int syncCount = 0;

			PacketWriter pw = new(_packetBuffer);
			pw.Put(PacketType.SC_Sync_Reliable);
			foreach (var netObj in netObjs)
			{
				if (!netObj.IsDirtyReliable)
				{
					continue;
				}
				pw.Put(netObj.Identity);
				netObj.SerializeSyncReliable(pw);
				netObj.ClearDirtyReliable();
				syncCount++;
			}

			if (syncCount > 0)
			{
				_userSessionHandler.SendReliableToAll(pw);
			}
		}

		public void SyncUnreliable()
		{
			var netObjs = _worldObject.Values;
			if (netObjs.Count <= 0)
				return;

			int syncCount = 0;

			PacketWriter pw = new(_packetBuffer);
			pw.Put(PacketType.SC_Sync_Unreliable);
			foreach (var netObj in netObjs)
			{
				if (!netObj.IsDirtyUnreliable)
				{
					continue;
				}
				pw.Put(netObj.Identity);
				netObj.SerializeSyncUnreliable(pw);
				netObj.ClearDirtyUnreliable();
				syncCount++;
			}

			if (syncCount > 0)
			{
				_userSessionHandler.SendReliableToAll(pw);
			}
		}
	}
}
