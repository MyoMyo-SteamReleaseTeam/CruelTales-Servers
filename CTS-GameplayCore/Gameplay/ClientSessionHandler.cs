﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CT.Common.DataType;
using CT.Network.Runtimes;
using CT.Tools.Collections;
using CTS.Instance.Networks;
using log4net;

namespace CTS.Instance.Gameplay
{
	public enum ClientSessionJobType
	{
		None = 0,
		TryJoin,
		OnDisconnected,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SessionJob
	{
		public ClientSessionJobType Type;
		public ClientSession ClientSession;
	}

	public class ClientSessionHandler : IJobHandler
	{
		// Log
		private readonly ILog _log = LogManager.GetLogger(typeof(ClientSessionHandler));

		// DI
		private readonly GameInstance _gameInstance;

		// Events
		public event Action<ClientSession> OnClientEnterGame;
		public event Action<ClientSession> OnClientLeaveGame;

		// Settings
		public int MemberCount => _playerById.Count;
		public int MaxClient { get; private set; }

		// Session Map
		private readonly BidirectionalMap<ClientId, ClientSession> _playerById = new();
		private readonly List<ClientSession> _playerList = new();
		public IReadOnlyList<ClientSession> PlayerList => _playerList;

		// Job Queue
		private JobQueue<SessionJob> _jobQueue;

		public ClientSessionHandler(GameInstance gameInstance)
		{
			_gameInstance = gameInstance;
			_jobQueue = new(onJobExecuted);
		}

		public void Initialize(GameInstanceOption option)
		{
			MaxClient = option.MaxClient;
		}

		public void Clear() => _jobQueue.Clear();

		public void Flush() => _jobQueue.Flush();

		private void onJobExecuted(SessionJob job)
		{
			switch (job.Type)
			{
				case ClientSessionJobType.TryJoin: Execute_tryJoinGame(job.ClientSession); break;
				case ClientSessionJobType.OnDisconnected: Execute_onDisconnected(job.ClientSession); break;

				default:
					_log.Error($"Unknown clinet session job detected!");
					Debug.Assert(false);
					break;
			}
		}

		public void Push_TryJoinGame(ClientSession clientSession)
		{
			_jobQueue.Push(new SessionJob()
			{
				Type = ClientSessionJobType.TryJoin,
				ClientSession = clientSession,
			});
		}

		private void Execute_tryJoinGame(ClientSession clientSession)
		{
			if (_playerById.ContainsForward(clientSession.ClientId))
			{
				_log.Warn($"Client {clientSession.ClientId} try to join again");
				return;
			}

			if (MemberCount >= MaxClient)
			{
				_log.Warn($"This game instance is already full!");
				clientSession.Callback_TryJoinGame(false, null, DisconnectReasonType.GameInstanceIsAlreadyFull);
				return;
			}

			_playerById.Add(clientSession.ClientId, clientSession);
			_playerList.Add(clientSession);

			OnClientEnterGame(clientSession);
			clientSession.Callback_TryJoinGame(true, _gameInstance, DisconnectReasonType.None);

			_log.Info($"{this} Session {clientSession} join the game");
		}

		public void Push_OnDisconnected(ClientSession clientSession)
		{
			_jobQueue.Push(new SessionJob()
			{
				Type = ClientSessionJobType.OnDisconnected,
				ClientSession = clientSession,
			});
		}

		private void Execute_onDisconnected(ClientSession clientSession)
		{
			if (_playerById.TryRemove(clientSession.ClientId))
			{
				_log.Info($"[GUID:{_gameInstance.Guid}][Current player:{this.MemberCount}] Session {clientSession} leave the game");
				OnClientLeaveGame(clientSession);
			}
			else
			{
				_log.Error($"Player disconnected warning! There is no client {clientSession.ClientId}!");
			}
		}
	}
}
