﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Tools.Collections;
using CT.Networks.Runtimes;
using CTS.Instance.Networks;
using log4net;

namespace CTS.Instance.Gameplay
{
	public enum UserSessionJobType
	{
		None = 0,
		OnDisconnected,
		TryEnterGame,
		TryReadyToSync,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SessionJob
	{
		public UserSessionJobType Type;
		public UserSession UserSession;

		public SessionJob(UserSessionJobType type, UserSession session)
		{
			Type = type;
			UserSession = session;
		}
	}

	public class UserSessionHandler : IJobHandler
	{
		// Log
		private readonly ILog _log = LogManager.GetLogger(typeof(UserSessionHandler));

		// DI
		private readonly GameInstance _gameInstance;

		// Settings
		public int MemberCount => _userById.Count;
		public int MaxUser { get; private set; }

		// Session Map
		private readonly BidirectionalMap<UserId, UserSession> _userById = new();
		private readonly List<UserSession> _userList = new();
		public IReadOnlyList<UserSession> UserList => _userList;

		// Job Queue
		private JobQueue<SessionJob> _jobQueue;

		public UserSessionHandler(GameInstance gameInstance)
		{
			_gameInstance = gameInstance;
			_jobQueue = new(onJobExecuted);
		}

		public void Initialize(GameInstanceOption option)
		{
			MaxUser = option.MaxUser;
		}

		public void Clear() => _jobQueue.Clear();

		public void Flush() => _jobQueue.Flush();

		private void onJobExecuted(SessionJob job)
		{
			switch (job.Type)
			{
				case UserSessionJobType.OnDisconnected: Execute_onDisconnected(job.UserSession); break;
				case UserSessionJobType.TryEnterGame: Execute_tryEnterGame(job.UserSession); break;
				case UserSessionJobType.TryReadyToSync: Execute_tryReadyToSync(job.UserSession); break;

				default:
					_log.Error($"Unknown user session job detected!");
					Debug.Assert(false);
					break;
			}
		}

		public void Push_OnDisconnected(UserSession userSession)
		{
			_jobQueue.Push(new(UserSessionJobType.OnDisconnected, userSession));
		}

		/// <summary>연결 종료된 유저의 정보를 게임에서 제거합니다.</summary>
		private void Execute_onDisconnected(UserSession userSession)
		{
			if (_userById.TryRemove(userSession.UserId))
			{
				_log.Info($"[GUID:{_gameInstance.Guid}][Current user:{this.MemberCount}] Session {userSession} leave the game");
				_gameInstance.OnUserLeaveGame(userSession);
			}
			else
			{
				_log.Error($"Player disconnected warning! There is no user {userSession.UserId}!");
			}
		}

		public void Push_TryEnterGame(UserSession userSession)
		{
			_jobQueue.Push(new(UserSessionJobType.TryEnterGame, userSession));
		}

		/// <summary>유저를 참가시키거나 거부합니다. 참가 후 바로 접속 유저로 집계됩니다.</summary>
		private void Execute_tryEnterGame(UserSession userSession)
		{
			if (_userById.ContainsForward(userSession.UserId))
			{
				_log.Warn($"User {userSession} try to join again");
				userSession.Disconnect(DisconnectReasonType.WrongOperation);
				return;
			}

			if (MemberCount >= MaxUser)
			{
				_log.Warn($"User {userSession} fail to join GameInstance {_gameInstance.Guid}. " +
					$"Reason : {DisconnectReasonType.GameInstanceIsAlreadyFull}");

				userSession.Disconnect(DisconnectReasonType.GameInstanceIsAlreadyFull);
				return;
			}

			_userById.Add(userSession.UserId, userSession);
			_userList.Add(userSession);

			// Ack response
			_log.Info($"User {userSession} has been enter the game instance. [GUID:{_gameInstance.Guid}]");
			userSession.AckTryEnterGameInstance();
		}

		public void Push_TryReadyToSync(UserSession userSession)
		{
			_jobQueue.Push(new(UserSessionJobType.TryReadyToSync, userSession));
		}

		/// <summary>
		/// 동기화 준비가 된 유저 이벤트를 발생시킵니다.
		/// 월드 메니져에서 초기화 패킷을 전송합니다.
		/// </summary>
		public void Execute_tryReadyToSync(UserSession userSession)
		{
			if (!_userById.ContainsForward(userSession.UserId))
			{
				_log.Warn($"There is no {userSession} user in this game! GUID:{_gameInstance.Guid}");
				userSession.Disconnect(DisconnectReasonType.WrongOperation);
				return;
			}

			_log.Info($"[Instance:{_gameInstance.Guid}] Session {userSession} enter the game");
			userSession.OnEnterGame(this);
			this._gameInstance.OnUserEnterGame(userSession);
		}

		public void SendReliable(UserId userId, PacketWriter writer,
								 byte channelNumber = NetworkManager.CHANNEL_CONNECTION)
		{
			if (_userById.TryGetValue(userId, out var userSession))
			{
				userSession.SendReliable(writer, channelNumber);
			}
			else
			{
				_log.Warn($"There is no {userId} to {nameof(SendReliable)} in GI:{_gameInstance.Guid}");
			}
		}

		public void SendUnreliable(UserId userId, PacketWriter writer,
								   byte channelNumber = NetworkManager.CHANNEL_CONNECTION)
		{
			if (_userById.TryGetValue(userId, out var userSession))
			{
				userSession.SendUnreliable(writer, channelNumber);
			}
			else
			{
				_log.Warn($"There is no {userId} to {nameof(SendUnreliable)} in GI:{_gameInstance.Guid}");
			}
		}
	}
}
