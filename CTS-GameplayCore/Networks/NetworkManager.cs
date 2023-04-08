using System;
using System.Threading;
using CT.Network.Packets;
using CT.Network.Serialization;
using CT.Packets;
using CTS.Instance.Gameplay;
using CTS.Instance.Utils;
using LiteNetLib;
using log4net;

namespace CTS.Instance.Networks
{
	public class NetworkManager
	{
		private static ILog _log = LogManager.GetLogger(typeof(NetworkManager));

		private readonly EventBasedNetListener _networkListener;
		private readonly NetManager _netManager;
		private readonly SessionManager _sessionManager;

		// GameInstance
		public readonly GameInstanceManager GameplayInstanceManager;

		private readonly ServerOption _serverOption;
		private readonly TickTimer _serverTimer;

		public NetworkManager(ServerOption serverOption, TickTimer serverTimer)
		{
			_networkListener = new EventBasedNetListener();
			_networkListener.ConnectionRequestEvent += onConnectionRequestEvent;
			_networkListener.PeerConnectedEvent += onPeerConnectedEvent;
			_networkListener.PeerDisconnectedEvent += onPeerDisconnectedEvent;
			_networkListener.NetworkReceiveEvent += onNetworkReceiveEvent;
			_netManager = new NetManager(_networkListener);

			// Gameplay Instance
			_serverOption = serverOption;
			_serverTimer = serverTimer;

			GameplayInstanceManager = new(_serverOption, _serverTimer);
			_sessionManager = new SessionManager(this);
		}

		public void Start()
		{
			_log.Info($"Start NetworkManager...");

			_netManager.Start(_serverOption.Port);

			Thread thread = new Thread(startPollingEvents);
			thread.IsBackground = false;
			thread.Start();

			GameplayInstanceManager.Start();
		}

		private void startPollingEvents()
		{
			_log.Info($"Start network polling events...");
			while (true)
			{
				_netManager.PollEvents();
				Thread.Sleep(10);
			}
		}

		private void onConnectionRequestEvent(ConnectionRequest request)
		{
			request.AcceptIfKey("TestServer"); // TODO: version check
		}

		private void onPeerConnectedEvent(NetPeer peer)
		{
			if (_sessionManager.Contains(peer.Id))
			{
				_log.Info($"Already exist peer try to connect! Endpoint : {peer.EndPoint}");
				return;
			}

			var session = _sessionManager.Create(peer.Id);
			if (!session.TryConnected(peer))
			{
				peer.Disconnect();
				_log.Error($"Connection error from {peer.EndPoint.ToString()}");
				return;
			}

			_log.Info($"[UserCount:{_sessionManager.Count}] Client connect from {peer.EndPoint.ToString()}");
		}

		private void onPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			if (_sessionManager.TryGetSessionBy(peer.Id, out var session))
			{
				session.OnDisconnected(disconnectInfo);
				_log.Info($"[UserCount:{_sessionManager.Count}] Client disconnected {peer.EndPoint.ToString()}");
			}
		}

		private void onNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			try
			{
				if (_sessionManager.TryGetSessionBy(peer.Id, out var session))
				{
					PacketReader packetReader = new PacketReader(reader.GetRemainingBytesSegment());

					while (!packetReader.IsEnd)
					{
						PacketType packetType = packetReader.ReadPacketType();
						var packet = PacketFactory.Create(packetType, packetReader);
						session.OnReceive(packet);
					}
				}
			}
			catch (Exception e)
			{
				_log.Error($"Receive error from : {peer.EndPoint.ToString()}", e);
				peer.Disconnect();
				return;
			}
		}
	}
}
