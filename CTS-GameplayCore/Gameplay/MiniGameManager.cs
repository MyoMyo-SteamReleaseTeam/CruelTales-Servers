using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CTS.Instance.Gameplay.Entities;
using CTS.Instance.Networks;

namespace CTS.Instance.Gameplay
{
	public class GameWorldManager
	{
		private NetEntityId _entityIdCounter;

		private Dictionary<NetEntityId, BaseEntity> _entityById = new();

		public void Clear()
		{
			_entityById.Clear();
		}

		public void AddPlayer(ClientSession session)
		{
			_entityIdCounter = new NetEntityId(_entityIdCounter.ID + 1);
			var playerEntity = new Entity_Player();
			playerEntity.BindClient(session.ClientId);
			_entityById.Add(_entityIdCounter, playerEntity);
		}
	}

	public class MiniGameManager
	{
		

		public event Action OnGameEnd;

		public void StartGame()
		{

		}

		public void CheckEndCondition()
		{

		}
	}
}
