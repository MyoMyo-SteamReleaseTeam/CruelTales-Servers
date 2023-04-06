using System;
using System.Collections.Generic;
using System.Threading;
using CT.Network.Core;
using CT.Network.DataType;
using CT.Network.Serialization;
using CT.Packets;
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
		private readonly EventBasedNetListener _listener;
		public NetManager _netManager { get; private set; }
		private SessionHandler _sessionHandler;

		public NetworkService(ServerOption serverOption, TickTimer serverTimer)
		{
			// Manager
			_listener = new EventBasedNetListener();
			_sessionHandler = new SessionHandler(serverTimer);
			_netManager = new NetManager(_listener);

			// Events
			_listener.ConnectionRequestEvent += onConnectionRequestEvent;
			_listener.PeerConnectedEvent += onPeerConnectedEvent;
			_listener.PeerDisconnectedEvent += onPeerDisconnectedEvent;
			_listener.NetworkReceiveEvent += onNetworkReceiveEvent;
		}

		public void Start(int serverPort)
		{
			_log.Info($"Start network service");
			_sessionHandler.Start();
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
				PacketReader packetReader = new PacketReader(reader.GetRemainingBytesSegment());

				// Check peer's session
				// TODO...
				while (!packetReader.IsEnd)
				{
					// Parse packet
					PacketType packetType = packetReader.ReadPacketType();
					var packet = PacketFactory.Create(packetType, packetReader);
					_sessionHandler.OnReceivePacket(peer, packet);
				}
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
			_sessionHandler.OnPeerDisconnected(peer);
		}

		private void onPeerConnectedEvent(NetPeer peer)
		{
			_sessionHandler.OnPeerInitialConnected(peer);
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
	}
}