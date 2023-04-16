﻿using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CT.Network.Runtimes;
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
		private ClientInputHandler _inputHandler;

		public GameInstance(TickTimer serverTimer)
		{
			ServerTimer = serverTimer;
			_sessionHandler = new ClientSessionHandler(this);
			_sessionHandler.OnClientLeaveGame += onClientLeaveGame;
			_sessionHandler.OnClientEnterGame += onClientEnterGame;
			_inputHandler = new ClientInputHandler(this);
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
			_sessionHandler.Flush();
			_inputHandler.Flush(deltaTime);
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

		private void onClientEnterGame(ClientSession obj)
		{
		}

		private void onClientLeaveGame(ClientSession obj)
		{
		}

		/// <summary>게임 인스턴스에 접속을 시도합니다. 접속 결과는 Callback으로 호출됩니다.</summary>
		/// <param name="clientSession"></param>
		/// <param name="rejectReason"></param>
		/// <returns></returns>
		public void Push_TryJoinSession(ClientSession clientSession)
		{
			_sessionHandler.Push_TryJoinGame(clientSession);
		}

		public void Push_OnDisconnected(ClientSession clientSession)
		{
			_sessionHandler.Push_OnDisconnected(clientSession);
		}

		#endregion

		public override string ToString()
		{
			return $"[GUID:{Guid}][MemberCount:{_sessionHandler.MemberCount}]";
		}
	}
}
