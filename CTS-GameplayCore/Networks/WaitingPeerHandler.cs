using System;
using System.Collections.Generic;
using CTS.Instance.Services;
using LiteNetLib;
using log4net;

namespace CTS.Instance.Networks
{
	public class WaitingSession
	{
		public int PeerId { get; private set; }
		public NetPeer Peer { get; private set; }
		public DateTime ConnectTime { get; private set; }

		public WaitingSession(NetPeer peer, DateTime connectTime)
		{
			Peer = peer;
			PeerId = Peer.Id;
			ConnectTime = connectTime;
		}
	}

	public class WaitingPeerHandler : FrameRunner
	{
		// Waiting Session
		public const int WAITING_TIMEOUT_SEC = 5;
		public readonly TimeSpan WAITING_TIMEOUT = new TimeSpan(0, 0, 0, WAITING_TIMEOUT_SEC);
		public object _waitingLock = new object();
		private List<WaitingSession> _waitingSessions = new();
		public int WaitingSessionCount
		{
			get
			{
				lock (_waitingLock)
				{
					return _waitingSessions.Count;
				}
			}
		}

		public WaitingPeerHandler(TickTimer serverTimer) 
			: base(WAITING_TIMEOUT_SEC * 900, serverTimer,
				   LogManager.GetLogger(typeof(WaitingPeerHandler)))
		{
			
		}

		public void AddNewWaitingPeer(NetPeer peer)
		{
			lock (_waitingLock)
			{
				_waitingSessions.Add(new WaitingSession(peer, DateTime.UtcNow));
			}
		}

		public void KickTimeoutWaitingPeer()
		{
			lock (_waitingLock)
			{
				DateTime currentTime = DateTime.UtcNow;

				for (int i = _waitingSessions.Count - 1; i >= 0; i--)
				{
					var currentPeer = _waitingSessions[i];
					var elapsed = currentTime - currentPeer.ConnectTime;
					if (elapsed < WAITING_TIMEOUT)
					{
						continue;
					}

					_waitingSessions.RemoveAt(i);
					currentPeer.Peer.Disconnect();
				}
			}
		}

		[Obsolete("For test")]
		public void RemoveWaitingPeer(NetPeer peer)
		{
			lock (_waitingLock)
			{
				int count = _waitingSessions.Count;
				for (int i = 0; i < count; i++)
				{
					if (_waitingSessions[i].PeerId == peer.Id)
					{
						_waitingSessions.RemoveAt(i);
						return;
					}
				}
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			_log.Info("Try tick timout waiting peer...");
			KickTimeoutWaitingPeer();
		}
	}
}