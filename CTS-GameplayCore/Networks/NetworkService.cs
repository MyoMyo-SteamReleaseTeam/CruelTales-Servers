using System;
using System.Threading;
using CT.Network.Core;
using CT.Network.DataType;
using CT.Network.Serialization;
using CTS.Instance.Services;
using LiteNetLib;
using log4net;

namespace CTS.Instance.Networks
{
	public class NetworkService
	{
		// Log
		private ILog _log = LogManager.GetLogger(typeof(NetworkService));

		// Manager
		public NetManager _netManager { get; private set; }
		private readonly EventBasedNetListener _listener;

		// Session
		private SessionHandler _sessionHandler = new();
		private WaitingPeerHandler _waitingPeerHandler;

		// Events
		public event Action<NetClientSession, PacketReader>? OnPacketReceived;
		public event Action<NetClientSession>? OnSessionConnected;

		public NetworkService(ServerOption serverOption, TickTimer serverTimer)
		{
			// Manager
			_listener = new EventBasedNetListener();
			_netManager = new NetManager(_listener);
			_waitingPeerHandler = new WaitingPeerHandler(serverTimer);

			// Events
			_listener.ConnectionRequestEvent += onConnectionRequestEvent;
			_listener.PeerConnectedEvent += onPeerConnectedEvent;
			_listener.PeerDisconnectedEvent += onPeerDisconnectedEvent;
			_listener.NetworkReceiveEvent += onNetworkReceiveEvent;
		}

		public void Start(int serverPort)
		{
			_log.Info($"Start network service");
			_netManager.Start(port: serverPort);

			Thread t = new Thread(_waitingPeerHandler.Run);
			t.IsBackground = true;
			t.Start();
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


				//if (!_sessions.TryGetValue(token, out var clientSession) || clientSession == null)
				//{
				//	peer.Disconnect();
				//	return;
				//}

				//OnPacketReceived?.Invoke(clientSession, pr);
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
			_sessionHandler.RemovePeer(peer);
			_log.Info($"[UserCount:{_sessionHandler.SessionCount}] Client disconnect {peer.EndPoint.ToString()}");
		}

		private void onPeerConnectedEvent(NetPeer peer)
		{
			_waitingPeerHandler.AddNewWaitingPeer(peer);
			_log.Info($"[UserCount:{_sessionHandler.SessionCount}] Client connect from {peer.EndPoint.ToString()}");
		}

		/// <summary>클라이언트의 연결 요청시 호출됩니다.</summary>
		/// <param name="request"></param>
		private void onConnectionRequestEvent(ConnectionRequest request)
		{
			// 클라이언트의 Validation 절차는 연결이 종료된 이후 진행합니다.
			if (IsAvailable())
			{
				_log.Info($"Connection request from {request.RemoteEndPoint.ToString()}");
				request.AcceptIfKey("TestServer"); // TODO: version check
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