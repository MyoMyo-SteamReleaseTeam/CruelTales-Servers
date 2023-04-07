using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CT.Network.DataType;
using CTS.Instance.Networks;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameInstanceOption
	{
		public int MaxMember { get; set; }
	}

	public class GameInstance
	{
		// Log
		[AllowNull]
		public ILog _log;

		// Game Identification
		public GameInstanceGuid Guid { get; private set; }

		[AllowNull]
		private GameInstanceOption _option;
		private readonly Dictionary<ClientId, ClientSession> _playerById = new();
		private readonly List<ClientSession> _playerList = new();
		public int MemberCount => _playerById.Count;

		public void Initialize(GameInstanceGuid guid, GameInstanceOption option)
		{
			_log = LogManager.GetLogger($"{nameof(GameInstance)}_{guid}");

			Guid = guid;
			_option = option;
		}

		public void Update(float deltaTime)
		{

		}

		public bool TryJoinSession(ClientSession clientSession)
		{
			if (_playerById.ContainsKey(clientSession.ClientId))
			{
				_log.Warn($"Client {clientSession.ClientId} try to join again");
				return false;
			}

			if (MemberCount >= _option.MaxMember)
			{
				_log.Warn($"This game instance is already full!");
			}

			_playerById.Add(clientSession.ClientId, clientSession);
			_playerList.Add(clientSession);
			return true;
		}

		public void OnPlayerDisconnected(ClientSession clientSession)
		{

		}
	}
}
