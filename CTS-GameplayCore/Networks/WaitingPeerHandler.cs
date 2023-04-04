using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using CT.Network.Core;
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

		public ulong GetHashCodeByIpPort()
		{
			return WaitingSession.GetHashCodeByIpPort(Peer);
		}

		[StructLayout(LayoutKind.Explicit)]
		readonly struct EndpointHashHelper
		{
			[FieldOffset(0)] public readonly ulong EndpointHash;
			[FieldOffset(0)] public readonly int IpEndpointHash;
			[FieldOffset(4)] public readonly int Value;

			public EndpointHashHelper(int ipEndPointHash, int value)
			{
				IpEndpointHash = ipEndPointHash;
				Value = value;
			}
		}

		public static ulong GetHashCodeByIpPort(NetPeer peer)
		{
			int endPointHash = peer.EndPoint.GetHashCode();
			EndpointHashHelper hashHelper = new(endPointHash, peer.Id);
			return hashHelper.EndpointHash;
		}
	}

	public class WaitingPeerHandler : FrameRunner
	{
		// Waiting Session
		public const int WAITING_TIMEOUT_SEC = 5;
		public readonly TimeSpan WAITING_TIMEOUT = new TimeSpan(0, 0, 0, WAITING_TIMEOUT_SEC);
		public int WaitingSessionCount
		{
			get
			{
				lock (_waitingLock)
				{
					return _waitingSessionByHash.Count;
				}
			}
		}
		private readonly object _waitingLock = new object();

		private readonly Dictionary<ulong, WaitingSession> _waitingSessionByHash = new();
		private readonly List<ulong> _kickSessionHashList = new();

		public WaitingPeerHandler(TickTimer serverTimer) 
			: base(WAITING_TIMEOUT_SEC * 900, serverTimer,
				   LogManager.GetLogger(typeof(WaitingPeerHandler)))
		{
			
		}

		public void AddNewWaitingPeer(NetPeer peer)
		{
			lock (_waitingLock)
			{
				WaitingSession ws = new WaitingSession(peer, DateTime.UtcNow);
				ulong peerHashCode = ws.GetHashCodeByIpPort();
				if (_waitingSessionByHash.ContainsKey(peerHashCode))
				{
					_log.Fatal($"Peer {peer.EndPoint} already waiting in waiting queue!\n" +
						$"Disconnect peer and remove from waiting queue!");
					peer.Disconnect();
					_waitingSessionByHash.Remove(peerHashCode);
					return;
				}

				_waitingSessionByHash.Add(peerHashCode, ws);
			}
		}

		public void KickTimeoutWaitingPeer()
		{
			lock (_waitingLock)
			{
				DateTime currentTime = DateTime.UtcNow;

				foreach (var currentPeer in _waitingSessionByHash.Values)
				{
					var elapsed = currentTime - currentPeer.ConnectTime;
					if (elapsed < WAITING_TIMEOUT)
					{
						continue;
					}

					_kickSessionHashList.Add(currentPeer.GetHashCodeByIpPort());
				}

				int kickCount = _kickSessionHashList.Count;
				for (int i = 0; i < kickCount; i++)
				{
					ulong kickPeerHash = _kickSessionHashList[i];
					if (!_waitingSessionByHash.TryGetValue(kickPeerHash, out var kickSession))
					{
						_log.Fatal($"There is no session to kick! Session hash code : {kickPeerHash}");
						continue;
					}

					kickSession.Peer.Disconnect();
					_waitingSessionByHash.Remove(kickPeerHash);
				}

				_kickSessionHashList.Clear();
			}
		}

		protected override void OnUpdate(float deltaTime)
		{
			KickTimeoutWaitingPeer();
			if (this.WaitingSessionCount > 0)
			{
				_log.Info($"Current waiting user count : {this.WaitingSessionCount}");
			}
		}
	}
}