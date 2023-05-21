using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CTC.Networks.Synchronizations;
using CTC.Networks.SyncObjects.TestSyncObjects;
using log4net;

namespace CTC.Networks
{
	public class GameSynchronizer
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(GameSynchronizer));

		private NetworkManager _serverSession;

		private Dictionary<NetworkIdentity, RemoteNetworkObject> _worldObjectById = new();
		private List<NetworkIdentity>[,] _netIdByPartition = new List<NetworkIdentity>[10, 20];

		public GameSynchronizer(NetworkManager serverSession)
		{
			_serverSession = serverSession;


		}

		public void OnMasterMovement(IPacketReader receivedPacket)
		{

		}

		public void OnMasterSpawn(IPacketReader receivedPacket)
		{

		}

		public void OnMasterRespawn(IPacketReader receivedPacket)
		{

		}

		public void OnMasterDespawn(IPacketReader receivedPacket)
		{

		}

		public void OnMasterReliable(IPacketReader receviedPacket)
		{

		}

		public void OnMasterUnreliable(IPacketReader receviedPacket)
		{

		}


		//public void OnSyncInitialize(IPacketReader reader)
		//{
		//	//while (reader.CanRead(1))
		//	//{
		//	//	var type = reader.ReadNetworkObjectType();
		//	//	NetworkIdentity id = new NetworkIdentity();
		//	//	id.Deserialize(reader);
		//	//	Test_MovingCube cube = new();
		//	//	cube.OnCreated(id);
		//	//	cube.DeserializeEveryProperty(reader);
		//	//	_worldObjectById.Add(id, cube);
		//	//}
		//}

		//public void OnDeserializeReliable(IPacketReader reader)
		//{
		//	while (reader.CanRead(1))
		//	{
		//		NetworkIdentity id = new NetworkIdentity();
		//		id.Deserialize(reader);
		//		if (_worldObjectById.TryGetValue(id, out var netObj))
		//		{
		//			netObj.DeserializeSyncReliable(reader);
		//		}
		//	}
		//}

		//public void OnDeserializeUnreliable(IPacketReader reader)
		//{
		//	while (reader.CanRead(1))
		//	{
		//		NetworkIdentity id = new NetworkIdentity();
		//		id.Deserialize(reader);
		//		if (_worldObjectById.TryGetValue(id, out var netObj))
		//		{
		//			netObj.DeserializeSyncUnreliable(reader);
		//		}
		//	}
		//}

		private float _showTime = 0;

		public void Update(float deltaTime)
		{
			//_showTime += deltaTime;
			//if (_showTime > 3.0f)
			//{
			//	_showTime = 0;
			//	foreach (var obj in _worldObjectById.Values)
			//	{
			//		if (obj is PlayerCharacter player)
			//		{
			//			//_log.Info($"{player.Identity} : ({player.R},{player.G},{player.B})");
			//		}
			//	}
			//}
		}
	}
}