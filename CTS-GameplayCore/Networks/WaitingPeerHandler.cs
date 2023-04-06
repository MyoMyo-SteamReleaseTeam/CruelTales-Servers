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
	public class WaitingPeerHandler : TickRunner
	{
		public WaitingPeerHandler(TickTimer serverTimer) 
			: base(WAITING_TIMEOUT_SEC * 900, serverTimer,
				   LogManager.GetLogger(typeof(WaitingPeerHandler)))
		{
			
		}

		public void AddNewWaitingPeer(NetPeer peer)
		{
			lock (_waitingLock)
			{
				if (_sessionByPeerId.ContainsKey(peer.Id))
				{
					_log.Fatal($"Peer {peer.EndPoint} already waiting in waiting queue!\n" +
						$"Disconnect peer and remove from waiting queue!");
					peer.Disconnect();
					return;
				}
				WaitingSession ws = new WaitingSession();
				ws.Initialize(peer);

				_sessionByPeerId.Add(ws.PeerId, ws);
			}
		}

		public void KickTimeoutWaitingPeer()
		{
			lock (_waitingLock)
			{
				DateTime currentTime = DateTime.UtcNow;

				foreach (var currentPeer in _sessionByPeerId.Values)
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
					if (!_sessionByPeerId.TryGetValue(kickPeerHash, out var kickSession))
					{
						_log.Fatal($"There is no session to kick! Session hash code : {kickPeerHash}");
						continue;
					}

					kickSession.Peer.Disconnect();
					_sessionByPeerId.Remove(kickPeerHash);
				}

				_kickSessionHashList.Clear();
			}
		}

		public bool TryPopMatchedPeer(NetPeer matchedPeer)
		{
			lock (_waitingLock)
			{
				ulong peerHash = WaitingSession.GetHashCodeByIpPort(matchedPeer);
				if (!_sessionByPeerId.ContainsKey(peerHash))
				{
					return false;
				}

				_sessionByPeerId.Remove(peerHash);
				return true;
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