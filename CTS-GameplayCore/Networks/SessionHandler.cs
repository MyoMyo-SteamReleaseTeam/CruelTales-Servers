using System;
using System.Collections.Generic;
using System.Threading;
using CT.Network.Core;
using CT.Network.DataType;
using CT.Network.Serialization;
using CT.Tools.Collections;
using CTS.Instance.Packets;
using CTS.Instance.Services;
using LiteNetLib;
using log4net;

namespace CTS.Instance.Networks
{
	public class SessionHandler
	{
		public static ILog _log = LogManager.GetLogger(typeof(SessionHandler));

		// Ingame Session
		public object _sessionLock = new();
		//private BidirectionalMap<int, ClientToken> _tokenByPeerId = new();
		//public int SessionCount => _tokenByPeerId.Count;
		//private Dictionary<ClientToken, NetSession> _sessionByToken = new();

		private Dictionary<ClientToken, ClientSession> _clientSessionByToken = new();
		public int SessionCount => _clientSessionByToken.Count;

		private Dictionary<int, ClientSession> _sessionByPeerId = new();
		public int AllSessionCount => _sessionByPeerId.Count;

		public SessionHandler(TickTimer serverTimer)
		{
		}

		public void Start()
		{
		}

		public void OnPeerInitialConnected(NetPeer peer)
		{
			lock (_sessionLock)
			{
				if (_sessionByPeerId.ContainsKey(peer.Id))
				{
					_log.Error($"There is duplicated peer eixist! IP endpoint : {peer.EndPoint}");
					peer.Disconnect();
					_sessionByPeerId.Remove(peer.Id);
					return;
				}

				ClientSession session = new(this);
				session.Initialize(peer);
				_sessionByPeerId.Add(peer.Id, session);

				_log.Info($"[UserCount:{SessionCount}] Client connect from {peer.EndPoint.ToString()}");
			}
		}

		public void OnPeerDisconnected(NetPeer peer)
		{
			lock (_sessionLock)
			{
				// TODO : remove from waiting queue
				removePeer(peer);
				_log.Info($"[UserCount:{SessionCount}] Client disconnect {peer.EndPoint.ToString()}");
			}
		}

		public void OnPeerTryToJoinGame()
		{

		}

		public void Disconnect(NetPeer peer)
		{
			lock (_sessionLock)
			{
				peer.Disconnect();
				removePeer(peer);
				_log.Info($"[UserCount:{SessionCount}] Client disconnect {peer.EndPoint.ToString()}");
			}
		}

		private void removePeer(NetPeer peer)
		{
			if (_sessionByPeerId.ContainsKey(peer.Id))
			{
				_sessionByPeerId.Remove(peer.Id);
			}
		}

		public void OnReceivePacket(NetPeer peer, PacketBase packet)
		{
			if (_sessionByPeerId.TryGetValue(peer.Id, out var session))
			{
				PacketDispatcher.Dispatch(packet, session);
				return;
			}

			_log.Warn($"There is no such peer! PeerId({peer.Id}), Endpoint({peer.EndPoint})");
		}
	}
}