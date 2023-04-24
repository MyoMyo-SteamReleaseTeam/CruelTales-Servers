﻿using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CT.Networks.Runtimes;
using CT.Packets;
using CTS.Instance.Networks;
using CTS.Instance.PacketCustom;
using CTS.Instance.Packets;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameInstanceOption
	{
		public int MaxUser { get; set; }
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
		private UserSessionHandler _sessionHandler;
		public UserSessionHandler SessionHandler => _sessionHandler;
		private UserInputHandler _inputHandler;

		// Managers
		private GameManager _gameManager;

		public GameInstance(TickTimer serverTimer)
		{
			ServerTimer = serverTimer;
			_sessionHandler = new UserSessionHandler(this);
			_inputHandler = new UserInputHandler(this);
			_gameManager = new GameManager(this);
		}

		public void Initialize(GameInstanceGuid guid, GameInstanceOption option)
		{
			_log = LogManager.GetLogger($"{nameof(GameInstance)}_{guid}");
			_option = option;

			Guid = guid;

			_sessionHandler.Initialize(_option);
			_inputHandler.Clear();

			// TEST
			_gameManager.StartGame();
		}

		/// <summary>Update logic</summary>
		/// <param name="deltaTime">Delta Time</param>
		public void Update(float deltaTime)
		{
			// Handle network connections
			_sessionHandler.Flush();

			// Handle received input
			_inputHandler.Flush(deltaTime);

			// Update game logic
			_gameManager.Update(deltaTime);
			_gameManager.SyncReliable();
			_gameManager.SyncUnreliable();
		}

		public void Shutdown(DisconnectReasonType reason)
		{
			foreach (var ps in _sessionHandler.UserList)
			{
				DisconnectPlayer(ps, reason);
			}
		}

		public void OnUserInput(UserInputJob job)
		{
			_inputHandler.PushUserInput(job);
		}

		public void DisconnectPlayer(UserSession userSession, DisconnectReasonType reason)
		{
			userSession.Disconnect(reason);
		}

		#region Session

		public void OnUserEnterGame(UserSession userSession)
		{
			_gameManager.OnUserEnter(userSession);
		}

		public void OnUserLeaveGame(UserSession userSession)
		{
			_log.Info($"[Instance:{Guid}] Session {userSession} leave the game");
		}

		#endregion

		public override string ToString()
		{
			return $"[GUID:{Guid}][MemberCount:{_sessionHandler.MemberCount}]";
		}
	}
}
