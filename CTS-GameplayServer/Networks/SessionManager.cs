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
		private BidirectionalMap<int, UserSession> _sessionById = new();
		private NetworkManager _networkManager;
		private Stack<UserSession> _sessionPool = new();

		public SessionManager(NetworkManager networkManager)
		{
			_networkManager = networkManager;
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
