using System;
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
		TryJoin,
		OnDisconnected,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SessionJob
	{
		public UserSessionJobType Type;
		public UserSession UserSession;
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
				case UserSessionJobType.TryJoin: Execute_tryJoinGame(job.UserSession); break;
				case UserSessionJobType.OnDisconnected: Execute_onDisconnected(job.UserSession); break;

				default:
					_log.Error($"Unknown user session job detected!");
					Debug.Assert(false);
					break;
			}
		}

		public void Push_TryJoinGame(UserSession userSession)
		{
			_jobQueue.Push(new SessionJob()
			{
				Type = UserSessionJobType.TryJoin,
				UserSession = userSession,
			});
		}

		private void Execute_tryJoinGame(UserSession userSession)
		{
			if (_userById.ContainsForward(userSession.UserId))
			{
				_log.Warn($"User {userSession.UserId} try to join again");
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

			userSession.OnEnterGame(this);
			_gameInstance.OnUserEnterGame(userSession);
		}

		public void Push_OnDisconnected(UserSession userSession)
		{
			_jobQueue.Push(new SessionJob()
			{
				Type = UserSessionJobType.OnDisconnected,
				UserSession = userSession,
			});
		}

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
