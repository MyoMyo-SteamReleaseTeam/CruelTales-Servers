using System;
using System.Collections.Generic;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.Gameplay
{
	public class GameWorldManager
	{
		private Dictionary<NetworkIdentity, MasterNetworkObject> _worldObject = new();
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
			_worldObject.Add(netObject.Identity, netObject);
		}
	}

	public class GameManager
	{
		private MiniGameMapData MiniGameMapData { get; set; }

		public GameManager()
		{
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

		public void Update(float deltaTime)
		{

		}

		public void StartGame()
		{

		}

		public void CheckEndCondition()
		{

		}

		public void OnUserEnter(UserSession userSession)
		{

		}


	}
}
