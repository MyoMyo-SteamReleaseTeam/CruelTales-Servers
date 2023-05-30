using System;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Networks;
using CT.Networks.Runtimes;
using CT.Packets;
using CTS.Instance.Gameplay;
using CTS.Instance.Packets;
using LiteNetLib;
using log4net;

namespace CTS.Instance.Networks
{
	public class NetworkManager
	{
		private static ILog _log = LogManager.GetLogger(typeof(NetworkManager));

		// Managers
		private readonly EventBasedNetListener _networkListener;
		private readonly NetManager _netManager;
		private readonly SessionManager _sessionManager;
		public NetManager NetManager => _netManager;

		// GameInstance
		public readonly GameplayInstanceManager GameplayInstanceManager;

		// References
		private readonly ServerOption _serverOption;
		private readonly TickTimer _serverTimer;

		// Constant
		public const byte CHANNEL_CONNECTION = 0;

		// Packets
		private readonly PacketPool _packetPool = new PacketPool();
		private readonly byte[] _wrongPacketDisconnectInfo = new byte[1] { (byte)DisconnectReasonType.ServerError_CannotHandlePacket };

		public NetworkManager(ServerOption serverOption, TickTimer serverTimer)
		{
			_networkListener = new EventBasedNetListener();
			_networkListener.ConnectionRequestEvent += onConnectionRequestEvent;
			_networkListener.PeerConnectedEvent += onPeerConnectedEvent;
			_networkListener.PeerDisconnectedEvent += onPeerDisconnectedEvent;
			_networkListener.NetworkReceiveEvent += onNetworkReceiveEvent;
			_netManager = new NetManager(_networkListener);
			_netManager.PacketPoolSize = 50000;
			_netManager.MaxConnectAttempts = 100;
			_netManager.UnsyncedEvents = true;
			_netManager.UnsyncedReceiveEvent = true;
			_netManager.UnsyncedDeliveryEvent = true;
			//_netManager.AutoRecycle = true;
			_netManager.DisconnectTimeout = GlobalNetwork.DisconnectTimeout;

			// Gameplay Instance
			_serverOption = serverOption;
			_serverTimer = serverTimer;

			GameplayInstanceManager = new(this, _serverOption, _serverTimer);
			_sessionManager = new SessionManager(this, (int)(_serverOption.GameCount * 7 * 1.15f));
		}

		public void Start()
		{
			_log.Info($"Start NetworkManager...");

			_netManager.Start(_serverOption.Port);

			//Thread thread = new Thread(startPollingEvents);
			//thread.IsBackground = false;
			//thread.Start();

			GameplayInstanceManager.Start();
		}

		//private void startPollingEvents()
		//{
		//	_log.Info($"Start network polling events...");
		//	while (true)
		//	{
		//		_netManager.PollEvents();
		//		Thread.Sleep(10);
		//	}
		//}

		private void onConnectionRequestEvent(ConnectionRequest request)
		{
			request.AcceptIfKey("TestServer"); // TODO: version check
		}

		private void onPeerConnectedEvent(NetPeer peer)
		{
			if (_sessionManager.Contains(peer.Id))
			{
				_log.Warn($"Already exist peer try to connect! Endpoint : {peer.EndPoint}");
				return;
			}

			var session = _sessionManager.Create(peer.Id);
			if (!session.OnTryConnecting(peer))
			{
				peer.Disconnect();
				_log.Error($"Connection error from {peer.EndPoint.ToString()}");
				return;
			}

			_log.Debug($"[UserCount:{_sessionManager.Count}] Client connect from {peer.EndPoint.ToString()}");
		}

		private void onPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			if (_sessionManager.TryGetSessionBy(peer.Id, out var session))
			{
				session.OnDisconnected(disconnectInfo);
				_log.Debug($"[UserCount:{_sessionManager.Count}] Client disconnected {peer.EndPoint.ToString()}");
			}
		}

		private ByteBuffer _packetReader = new ByteBuffer();
		private void onNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
		{
			UserSession? session = null;

			try
			{
				if (_sessionManager.TryGetSessionBy(peer.Id, out session))
				{
					var packetSegment = reader.GetRemainingBytesSegment();
					_packetReader.Initialize(packetSegment, packetSegment.Count);

					while (_packetReader.CanRead(sizeof(PacketType)))
					{
						PacketType packetType = _packetReader.ReadPacketType();
						if (PacketDispatcher.IsCustomPacket(packetType))
						{
							session.OnReceiveRaw(packetType, _packetReader);
						}
						else
						{
							if (!_packetPool.TryReadPacket(packetType, _packetReader, out var packet))
							{
								session.Disconnect(DisconnectReasonType.ServerError_CannotHandlePacket);
								break;
							}
							session.OnReceive(packet);
							_packetPool.Return(packet);
						}
					}
				}
				reader.Recycle();
			}
			catch (Exception e)
			{
				reader.Recycle();
				_log.Error($"Receive error from : {peer.EndPoint.ToString()}", e);
				if (session == null)
				{
					peer.Disconnect(_wrongPacketDisconnectInfo);
				}
				else
				{
					session.Disconnect(DisconnectReasonType.ServerError_CannotHandlePacket);
				}
				return;
			}
		}
	}
}
