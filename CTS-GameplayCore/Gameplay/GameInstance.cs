using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CT.Network.Runtimes;
using CT.Packets;
using CTS.Instance.Networks;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameInstanceOption
	{
		public int MaxClient { get; set; }
	}

	public class GameInstance
	{
		// Support
		[AllowNull] public ILog _log;
		[AllowNull] private GameInstanceOption _option;
		public TickTimer ServerTimer {get; private set; }

		// Game Identification
		public GameInstanceGuid Guid { get; private set; }

		// Handlers
		private ClientSessionHandler _sessionHandler;
		public ClientSessionHandler SessionHandler => _sessionHandler;
		private ClientInputHandler _inputHandler;

		// Managers
		private MiniGameManager _miniGameManager;

		public GameInstance(TickTimer serverTimer)
		{
			ServerTimer = serverTimer;
			_sessionHandler = new ClientSessionHandler(this);
			_inputHandler = new ClientInputHandler(this);
			_miniGameManager = new MiniGameManager();
		}

		public void Initialize(GameInstanceGuid guid, GameInstanceOption option)
		{
			_log = LogManager.GetLogger($"{nameof(GameInstance)}_{guid}");
			_option = option;

			Guid = guid;

			_sessionHandler.Initialize(_option);
			_inputHandler.Clear();
		}

		/// <summary>Update logic</summary>
		/// <param name="deltaTime">Delta Time</param>
		public void Update(float deltaTime)
		{
			// Handle network connections
			_sessionHandler.Flush();

			// Handle received input
			_inputHandler.Flush(deltaTime);

			// Update mini game
		}

		public void Shutdown(DisconnectReasonType reason)
		{
			foreach (var ps in _sessionHandler.PlayerList)
			{
				DisconnectPlayer(ps, reason);
			}
		}

		public void OnClientInput(ClientInputJob job)
		{
			_inputHandler.PushClientInput(job);
		}

		public void DisconnectPlayer(ClientSession clientSession, DisconnectReasonType reason)
		{
			clientSession.Disconnect(reason);
		}

		#region Session

		public void OnClientEnterGame(ClientSession clientSession)
		{
			_log.Info($"[Instance:{Guid}] Session {clientSession} enter the game");

			SC_OnClientEnter enter = new SC_OnClientEnter();
			enter.Username = clientSession.ClientName;

			// TODO : Send initial packet
		}

		public void OnClientLeaveGame(ClientSession clientSession)
		{
			_log.Info($"[Instance:{Guid}] Session {clientSession} leave the game");
		}

		#endregion

		public override string ToString()
		{
			return $"[GUID:{Guid}][MemberCount:{_sessionHandler.MemberCount}]";
		}
	}
}
