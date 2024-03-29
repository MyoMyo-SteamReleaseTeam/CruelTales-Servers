﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Tools.Collections;
using CT.Networks.Runtimes;
using CTS.Instance.Networks;
using CTS.Instance.SyncObjects;
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

		// Reference
		private readonly GameplayInstance _gameplayInstance;

		// Option
		private RoomOption _roomOption;

		// Settings
		public int MemberCount => _userById.Count + _waitingTable.Count;

		// Session Map
		public List<UserSession> UserList { get; private set; }
		private readonly BidirectionalMap<UserId, UserSession> _userById;
		private readonly BidirectionalMap<UserId, UserSession> _waitingTable;

		// Job Queue
		private JobQueue<SessionJob> _jobQueue;

		public UserSessionHandler(GameplayInstance gameInstance,
								  InstanceInitializeOption option,
								  RoomOption roomOption)
		{
			_gameplayInstance = gameInstance;
			_jobQueue = new(onJobExecuted, option.SessionJobCapacity);
			_roomOption = roomOption;

			UserList = new List<UserSession>(option.SystemMaxUser);
			_userById = new BidirectionalMap<UserId, UserSession>(option.SystemMaxUser);
			_waitingTable = new BidirectionalMap<UserId, UserSession>(option.SystemMaxUser);
		}

		public bool TryGetUserSession(UserId id, [MaybeNullWhen(false)] out UserSession session)
		{
			return _userById.TryGetValue(id, out session);
		}

		public bool IsConnected(UserId id)
		{
			return _userById.Contains(id);
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
			_waitingTable.TryRemove(userSession.UserId);
			if (_userById.TryRemove(userSession.UserId))
			{
				_log.Debug($"[GUID:{_gameplayInstance.Guid}][Current user:{this.MemberCount}] Session {userSession} leave the game");
				_gameplayInstance.GameplayManager.OnUserLeaveGame(userSession);
			}
			else
			{
				_log.Warn($"Player disconnected warning! There is no user {userSession.UserId}!");
			}
		}

		public void Push_TryEnterGame(UserSession userSession)
		{
			_jobQueue.Push(new(UserSessionJobType.TryEnterGame, userSession));
		}

		/// <summary>유저를 참가시키거나 거부합니다. 참가 후 바로 접속 유저로 집계됩니다.</summary>
		private void Execute_tryEnterGame(UserSession userSession)
		{
			if (_userById.ContainsForward(userSession.UserId) ||
				_waitingTable.ContainsForward(userSession.UserId))
			{
				_log.Warn($"User {userSession} try to join again");
				userSession.Disconnect(DisconnectReasonType.ServerError_YouTryRejoin);
				return;
			}

			if (_roomOption.HasPassword && _roomOption.Password != userSession.RoomPassword)
			{
				userSession.Disconnect(DisconnectReasonType.Reject_WrongPassword);
				return;
			}

			if (MemberCount >= _roomOption.MaxUser)
			{
				_log.Warn($"User {userSession} fail to join GameInstance {_gameplayInstance.Guid}. " +
					$"Reason : {DisconnectReasonType.Reject_GameInstanceIsAlreadyFull}");

				userSession.Disconnect(DisconnectReasonType.Reject_GameInstanceIsAlreadyFull);
				return;
			}

			_waitingTable.Add(userSession.UserId, userSession);

			// Ack response
			_log.Debug($"User {userSession} has been enter the game instance. [GUID:{_gameplayInstance.Guid}]");
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
			if (!_waitingTable.ContainsForward(userSession.UserId))
			{
				_log.Warn($"There is no {userSession} user in waiting table! GUID:{_gameplayInstance.Guid}");
				userSession.Disconnect(DisconnectReasonType.ServerError_YouAreNotInTheWaitingQueue);
				return;
			}

			GameplayController? gameplayController = _gameplayInstance.GameplayManager.GameplayController;
			if (gameplayController == null )
			{
				_log.Fatal($"The server was not initialized! GUID:{_gameplayInstance.Guid}");
				userSession.Disconnect(DisconnectReasonType.ServerError_NotInitialized);
				return;
			}

			if (!gameplayController.CheckIfCanJoin(out DisconnectReasonType reason))
			{
				userSession.Disconnect(reason);
				return;
			}

			_log.Debug($"[Instance:{_gameplayInstance.Guid}] Session {userSession} enter the game");
			userSession.OnEnterGame(this);
			_gameplayInstance.GameplayManager.OnUserEnterGame(userSession);

			if (!_waitingTable.TryRemove(userSession.UserId))
			{
				_log.Error($"Failed to try remove user session from waiting!");
			}
			_userById.Add(userSession.UserId, userSession);
			UserList.Add(userSession);
		}

		public void SendReliableToAll(IPacketWriter writer,
									  byte channelNumber = NetworkManager.CHANNEL_CONNECTION)
		{
			foreach (var user in _userById.ForwardValues)
			{
				user.SendReliable(writer, channelNumber);
			}
		}

		public void SendUnreliableToAll(IPacketWriter writer,
										byte channelNumber = NetworkManager.CHANNEL_CONNECTION)
		{
			foreach (var user in _userById.ForwardValues)
			{
				user.SendUnreliable(writer, channelNumber);
			}
		}

		public void SendReliable(UserId userId, IPacketWriter writer,
								 byte channelNumber = NetworkManager.CHANNEL_CONNECTION)
		{
			if (_userById.TryGetValue(userId, out var userSession))
			{
				userSession.SendReliable(writer, channelNumber);
			}
			else
			{
				_log.Warn($"There is no {userId} to {nameof(SendReliable)} in GI:{_gameplayInstance.Guid}");
			}
		}

		public void SendUnreliable(UserId userId, IPacketWriter writer,
								   byte channelNumber = NetworkManager.CHANNEL_CONNECTION)
		{
			if (_userById.TryGetValue(userId, out var userSession))
			{
				userSession.SendUnreliable(writer, channelNumber);
			}
			else
			{
				_log.Warn($"There is no {userId} to {nameof(SendUnreliable)} in GI:{_gameplayInstance.Guid}");
			}
		}
	}
}
