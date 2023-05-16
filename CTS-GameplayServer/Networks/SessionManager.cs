using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CT.Common.Tools.Collections;
using log4net;

namespace CTS.Instance.Networks
{
	public class SessionManager
	{
		public int Count => _sessionById.Count;

		private static ILog _log = LogManager.GetLogger(typeof(SessionManager));
		private object _sessionManagerLock = new object();
		private NetworkManager _networkManager;

		private BidirectionalMap<int, UserSession> _sessionById;
		private Stack<UserSession> _sessionPool;

		public SessionManager(NetworkManager networkManager, int maxUserCapacity)
		{
			_networkManager = networkManager;

			_sessionById = new(maxUserCapacity);
			_sessionPool = new(maxUserCapacity);

			// Warm pool
			for (int i = 0; i < maxUserCapacity; i++)
			{
				_sessionPool.Push(new UserSession(this, _networkManager));
			}
		}

		public UserSession Create(int peerId)
		{
			lock (_sessionManagerLock)
			{
				if (!_sessionPool.TryPop(out var session))
				{
					session = new(this, _networkManager);
				}
				session.Reset();
				_sessionById.Add(peerId, session);
				return session;
			}
		}

		public void Remove(UserSession session)
		{
			lock (_sessionManagerLock)
			{
				_sessionPool.Push(session);
				if (_sessionById.TryRemove(session))
				{
					return;
				}
			}
			_log.Error($"There is no session to remove. Session : {session.ToString()}");
		}

		public bool TryGetSessionBy(int peerId, [MaybeNullWhen(false)] out UserSession session)
		{
			lock (_sessionManagerLock)
			{
				return _sessionById.TryGetValue(peerId, out session);
			}
		}

		public bool Contains(int peerId)
		{
			lock (_sessionManagerLock)
			{
				return _sessionById.Contains(peerId);
			}
		}
	}
}
