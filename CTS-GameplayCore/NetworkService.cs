using System;
using System.Collections.Generic;
using CT.Network.Core;
using CT.Network.DataType;
using CT.Network.Serialization;
using CT.Tools.Collections;
using LiteNetLib;
using log4net;

namespace CTS.Instance
{
	public class SessionHandler
	{
		private BidirectionalMap<int, ClientToken> _tokenByPeerId = new();
		private Dictionary<ClientToken, NetSession> _sessionByToken = new();
		private Dictionary<int, NetPeer> _peerByPeerId = new();
		public int SessionCount => _tokenByPeerId.Count;

		public void AddNewPeer(NetPeer peer)
		{
			_peerByPeerId.Add(peer.Id, peer);
		}

		public void RemovePeer(NetPeer peer)
		{
			_peerByPeerId.ContainsKey()
		}

		public bool IsValidToken()
		{

		}
	}

	public class NetworkService
	{
		// Log
		private ILog _log = LogManager.GetLogger(typeof(NetworkService));

		// Manager
		public NetManager _netManager { get; private set; }
		private readonly EventBasedNetListener _listener;

		// Session
		private SessionHandler _sessionHandler = new();

		// Events
		public event Action<NetSession, PacketReader>? OnPacketReceived;
		public event Action<NetSession>? OnSessionConnected;

		public NetworkService(ServerOption serverOption)
		{
			// Manager
			_listener = new EventBasedNetListener();
			_netManager = new NetManager(_listener);

			// Events
			_listener.ConnectionRequestEvent += onConnectionRequestEvent;
			_listener.PeerDisconnectedEvent += onPeerDisconnectedEvent;
			_listener.PeerConnectedEvent += onPeerConnectedEvent;
			_listener.NetworkReceiveEvent += onNetworkReceiveEvent;
		}

		public void Start(int serverPort)
		{
			_log.Info($"Start network service");
			_netManager.Start(port: serverPort);
		}

		public void PollEvents()
		{
			_netManager.PollEvents();
		}

		private void onNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			try
			{
				PacketSegment ps = new PacketSegment(reader.GetRemainingBytesSegment());
				PacketReader pr = new PacketReader(ps);

				ClientToken token = new ClientToken();
				token.Deserialize(pr);

				reader.Read

				if (!_sessions.TryGetValue(token, out var clientSession) || clientSession == null)
				{
					peer.Disconnect();
					return;
				}

				OnPacketReceived?.Invoke(clientSession, pr);
			}
			catch (Exception e)
			{
				_log.Error($"Receive error from : {peer.EndPoint.ToString()}", e);
				peer.Disconnect();
				return;
			}
		}

		private void onPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			_log.Info($"[UserCount:{_sessionHandler.SessionCount}] Client disconnect {peer.EndPoint.ToString()}");
			_sessionHandler.RemovePeer(peer);
		}

		private void onPeerConnectedEvent(NetPeer peer)
		{
			_log.Info($"[UserCount:{_sessionHandler.SessionCount}] Client connect from {peer.EndPoint.ToString()}");
			_sessionHandler.AddNewPeer(peer);
		}

		/// <summary>클라이언트의 연결 요청시 호출됩니다.</summary>
		/// <param name="request"></param>
		private void onConnectionRequestEvent(ConnectionRequest request)
		{
			// 클라이언트의 Validation 절차는 연결이 종료된 이후 진행합니다.
			if (IsAvailable())
			{
				_log.Info($"Connection request from {request.RemoteEndPoint.ToString()}");
				request.AcceptIfKey("CTMatchmakingTestServer"); // TODO: version check
			}
			else
			{
				_log.Info($"Reject connection from {request.RemoteEndPoint.ToString()}");
				request.Reject();
			}

			bool IsAvailable()
			{
				//return _netManager.GetPeersCount(ConnectionState.Connected) < 5;
				return true;
			}
		}

		public void Stop()
		{
			_netManager.Stop();
		}


		public bool CheckToken(ClientToken token)
		{
			return true;
		}
	}
}