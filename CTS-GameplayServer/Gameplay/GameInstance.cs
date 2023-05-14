using System;
using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
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
		public UserSessionHandler SessionHandler { get; private set; }
		//private UserInputHandler _inputHandler;

		// Managers
		public GameManager GameManager { get; private set; }

		public GameInstance(TickTimer serverTimer)
		{
			ServerTimer = serverTimer;
			SessionHandler = new UserSessionHandler(this);
			//_inputHandler = new UserInputHandler(this);
			GameManager = new GameManager(this);
		}

		public void Initialize(GameInstanceGuid guid, GameInstanceOption option)
		{
			_log = LogManager.GetLogger($"{nameof(GameInstance)}_{guid}");
			_option = option;

			Guid = guid;

			SessionHandler.Initialize(_option);
			GameManager.Initialize();
			//_inputHandler.Clear();

			// TEST
			GameManager.StartGame();
		}

		/// <summary>Update logic</summary>
		/// <param name="deltaTime">Delta Time</param>
		public void Update(float deltaTime)
		{
			// Handle network connections
			SessionHandler.Flush();

			// Handle received input
			//_inputHandler.Flush(deltaTime);

			// Update game logic
			GameManager.Update(deltaTime);
			GameManager.SyncReliable();
			GameManager.SyncUnreliable();
		}

		public void Shutdown(DisconnectReasonType reason)
		{
			foreach (var ps in SessionHandler.UserList)
			{
				DisconnectPlayer(ps, reason);
			}
		}

		//public void OnUserInput(UserInputJob job)
		//{
		//	_inputHandler.PushUserInput(job);
		//}

		public void DisconnectPlayer(UserSession userSession, DisconnectReasonType reason)
		{
			userSession.Disconnect(reason);
		}

		#region Session

		public void OnUserEnterGame(UserSession userSession)
		{
			GameManager.OnUserEnter(userSession);
		}

		public void OnUserLeaveGame(UserSession userSession)
		{
			_log.Info($"[Instance:{Guid}] Session {userSession} leave the game");
			GameManager.OnUserLeave(userSession);
		}

		#endregion

		public override string ToString()
		{
			return $"[GUID:{Guid}][MemberCount:{SessionHandler.MemberCount}]";
		}
	}
}
