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

		// TODO : Bind operation as job
		public bool TryJoinSession(ClientSession clientSession,
								   out DisconnectReasonType rejectReason)
		{
			rejectReason = DisconnectReasonType.None;

			if (_playerById.ContainsKey(clientSession.ClientId))
			{
				_log.Warn($"Client {clientSession.ClientId} try to join again");
				return false;
			}

			if (MemberCount >= _option.MaxMember)
			{
				_log.Warn($"This game instance is already full!");
				rejectReason = DisconnectReasonType.GameInstanceIsAlreadyFull;
				return false;
			}

			_playerById.Add(clientSession.ClientId, clientSession);
			_playerList.Add(clientSession);
			_log.Info($"{this} Session {clientSession} join the game");
			return true;
		}

		// TODO : Bind operation as job
		public void OnPlayerDisconnected(ClientSession clientSession)
		{
			if (!_playerById.ContainsKey(clientSession.ClientId))
			{
				_log.Warn($"Player disconnected warning! There is no client {clientSession.ClientId}!");
				return;
			}

			_playerById.Remove(clientSession.ClientId);
			_log.Info($"{this} Session {clientSession} leave the game");
		}

		public override string ToString()
		{
			return $"[GUID:{Guid}][MemberCount:{MemberCount}]";
		}
	}
}
