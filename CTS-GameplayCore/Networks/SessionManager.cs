using System.Diagnostics.CodeAnalysis;
using CT.Tools.Collections;
using log4net;

namespace CTS.Instance.Networks
{
	public class SessionManager
	{
		public int Count => _sessionById.Count;

		private static ILog _log = LogManager.GetLogger(typeof(SessionManager));
		private static int _counter = 0;
		private object _sessionManagerLock = new object();
		private BidirectionalMap<int, ClientSession> _sessionById = new();
		private NetworkManager _networkManager;

		public SessionManager(NetworkManager networkManager)
		{
			_networkManager = networkManager;
		}

		public ClientSession Create()
		{
			ClientSession session = new(this, _networkManager);
			lock (_sessionManagerLock)
			{
				_sessionById.Add(_counter, session);
				_counter++;
				return session;
			}
		}

		public void Remove(ClientSession session)
		{
			lock (_sessionManagerLock)
			{
				if (_sessionById.TryRemove(session))
				{
					return;
				}
			}
			_log.Error($"There is no session to remove. Session : {session.ToString()}");
		}

		public bool TryGetSessionBy(int sessionId, [MaybeNullWhen(false)] out ClientSession session)
		{
			lock (_sessionManagerLock)
			{
				return _sessionById.TryGetValue(sessionId, out session);
			}
		}

		public bool Contains(int sessionId)
		{
			lock (_sessionManagerLock)
			{
				return _sessionById.Contains(sessionId);
			}
		}
	}
}
