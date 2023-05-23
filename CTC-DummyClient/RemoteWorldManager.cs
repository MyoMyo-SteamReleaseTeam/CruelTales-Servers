using System.Collections.Generic;
using System.Diagnostics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CTC.Networks.Gameplay.ObjectManagements;
using CTC.Networks.Synchronizations;
using CTC.Networks.SyncObjects.TestSyncObjects;
using log4net;

namespace CTC.Networks
{
	public class RemoteWorldManager
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(RemoteWorldManager));

		// Reference
		private NetworkManager _networkManager;

		// Object management
		private Dictionary<NetworkIdentity, RemoteNetworkObject> _worldObjectById = new();
		private List<NetworkIdentity> _destroyIdList = new List<NetworkIdentity>(16);
		private NetworkObjectPoolManager _poolManager = new();

		public RemoteWorldManager(NetworkManager networkManager)
		{
			_networkManager = networkManager;

		}

		public void Update(float deltaTime)
		{
			if (_destroyIdList.Count != 0)
			{
				for (int i = 0; i < _destroyIdList.Count; i++)
				{
					var id = _destroyIdList[i];
					if (_worldObjectById.TryGetValue(id, out var destroyObj))
					{
						destroyObj.OnDestroyed();
						_worldObjectById.Remove(id);
						_poolManager.Return(destroyObj);
					}
				}
				_destroyIdList.Clear();
			}
		}

		public void OnMasterMovement(IPacketReader reader)
		{
			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkIdentity id = new(reader);
				if (_worldObjectById.TryGetValue(id, out var syncObj))
				{
					syncObj.Transform.Deserialize(reader);
				}
				else
				{
					NetworkTransform.Ignore(reader);
				}
			}
		}

		public void OnMasterSpawn(IPacketReader reader)
		{
			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkObjectType type = reader.ReadNetworkObjectType();
				NetworkIdentity id = new(reader);
				var netObj = _poolManager.Create(type);
				netObj.Created(id);
				netObj.Transform.DeserializeSpawnData(reader);
				netObj.DeserializeEveryProperty(reader);
				if (_worldObjectById.ContainsKey(id))
				{
					_poolManager.Return(netObj);
					continue;
				}
				_worldObjectById.Add(id, netObj);
				netObj.OnSpawn();

				_log.Debug($"Spawn {type}:{id}");
			}
		}

		public void OnMasterRespawn(IPacketReader reader)
		{
			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkObjectType type = reader.ReadNetworkObjectType();
				NetworkIdentity id = new(reader);
				var netObj = _poolManager.Create(type);
				netObj.Created(id);
				netObj.Transform.DeserializeSpawnData(reader);
				netObj.DeserializeEveryProperty(reader);
				if (_worldObjectById.ContainsKey(id))
				{
					_poolManager.Return(netObj);
					continue;
				}
				_worldObjectById.Add(id, netObj);
				netObj.OnRespawn();

				_log.Debug($"Respawn {type}:{id}");
			}
		}

		public void OnMasterDespawn(IPacketReader reader)
		{
			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkIdentity id = new(reader);
				if (!_worldObjectById.ContainsKey(id))
				{
					_log.Warn($"OnMasterDespawn error! there is no such id {id}");
					Debug.Assert(false);
					continue;
				}
				_destroyIdList.Add(id);

				_log.Debug($"Despawn {id}");
			}
		}

		public void OnMasterReliable(IPacketReader reader)
		{
			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkIdentity id = new(reader);
				_log.Debug($"OnMasterReliable {id}");
				if (!_worldObjectById.TryGetValue(id, out var netObj))
				{
					reader.IgnoreAll();
					_log.Fatal($"Cannot handle OnMasterReliable! Disconnect!");
					_networkManager.Disconnect();
					return;
				}
				netObj.DeserializeSyncReliable(reader);
			}
		}

		public void OnMasterUnreliable(IPacketReader reader)
		{
			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkIdentity id = new(reader);
				_log.Debug($"OnMasterUnreliable {id}");
				if (!_worldObjectById.TryGetValue(id, out var netObj))
				{
					reader.IgnoreAll();
					_log.Fatal($"Cannot handle OnMasterReliable! Disconnect!");
					_networkManager.Disconnect();
					return;
				}
				netObj.DeserializeSyncUnreliable(reader);
			}
		}
	}
}