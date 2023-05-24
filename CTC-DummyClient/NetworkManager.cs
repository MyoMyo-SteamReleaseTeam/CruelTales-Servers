using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Networks;
using CT.Packets;
using CTC.Networks.Packets;
using LiteNetLib;
using log4net;

namespace CTC.Networks
{
	public class NetworkManager
	{
		private static ILog _log = LogManager.GetLogger(typeof(NetworkManager));
		private UserSessionState _sessionState = UserSessionState.NoConnection;
		private EventBasedNetListener _listener;
		private NetManager _netManager;
		private NetPeer? _serverPeer;
		private PacketPool _packetPool = new();

		private RemoteWorldManager _gameSynchronizer;
		public RemoteWorldManager GameSynchronizer => _gameSynchronizer;

		public DummyUserInfo UserInfo { get; private set; }

		public NetworkManager(DummyUserInfo info)
		{
			UserInfo = info;

			_gameSynchronizer = new RemoteWorldManager(this);
			_listener = new EventBasedNetListener();
			_netManager = new NetManager(_listener);
			_netManager.ReuseAddress = true;
			_netManager.DisconnectTimeout = GlobalNetwork.DisconnectTimeout;

			_listener.NetworkReceiveEvent += OnReceived;
			_listener.PeerConnectedEvent += OnConnected;
			_listener.PeerDisconnectedEvent += OnDisconnected;
		}

		public void Start(int clientBindPort)
		{
			_netManager.Start(clientBindPort);
			_sessionState = UserSessionState.NoConnection;
		}

		public void Update(float deltaTime)
		{
			_netManager.PollEvents();
			_gameSynchronizer.Update(deltaTime);
		}

		public void TryConnect(string address, int port)
		{
			_netManager.Connect(address, port, "TestServer");
			_sessionState = UserSessionState.NoConnection;
		}

		public void Disconnect()
		{
			_serverPeer?.Disconnect();
			_netManager.DisconnectAll();
		}

		public void SendReliable(IPacketWriter writer,
								 byte channelNumber = 0)
		{
			_serverPeer?.Send(writer.Buffer.Array,
							  writer.Buffer.Offset,
							  writer.Size,
							  channelNumber,
							  DeliveryMethod.ReliableOrdered);
		}

		public void SendUnreliable(IPacketWriter writer,
								   byte channelNumber = 0)
		{
			_serverPeer?.Send(writer.Buffer.Array,
							  writer.Buffer.Offset,
							  writer.Size,
							  channelNumber,
							  DeliveryMethod.Unreliable);
		}

		private ByteBuffer _receivedPacket = new ByteBuffer();
		private void OnReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
		{
			try
			{
				var packetSegment = reader.GetRemainingBytesSegment();
				_receivedPacket.Initialize(packetSegment, packetSegment.Count);
				while (_receivedPacket.CanRead(sizeof(PacketType)))
				{
					PacketType packetType = _receivedPacket.ReadPacketType();

					if (PacketDispatcher.IsCustomPacket(packetType))
					{
						PacketDispatcher.DispatchRaw(packetType, _receivedPacket, this);
					}
					else
					{
						var packet = _packetPool.ReadPacket(packetType, _receivedPacket);
						PacketDispatcher.Dispatch(packet, this);
						_packetPool.Return(packet);
					}
				}
			}
			catch
			{
				_log.Error($"Disconnect! Failed to handle packet!");
				Disconnect();
			}
		}

		private IPacketWriter _connectPacketWriter = new ByteBuffer(1000);
		private void OnConnected(NetPeer peer)
		{
			_sessionState = UserSessionState.TryConnecting;

			_serverPeer = peer;
			_log.Info($"Success to connect to the server. Server endpoint : {peer.EndPoint}");

			var enterPacket = _packetPool.GetPacket<CS_Req_TryEnterGameInstance>();
			enterPacket.MatchTo = UserInfo.GameInstanceGuid;
			enterPacket.Token = UserInfo.UserToken;
			enterPacket.UserDataInfo = new UserDataInfo()
			{
				UserId = UserInfo.UserId,
				Username = "abc",
				UserCostume = new DokzaCostume()
				{
					Body = 1,
					Head = 2,
				}
			};

			_log.Info($"Send CS_Req_TryEnterGameInstance : {UserInfo}");

			_connectPacketWriter.ResetWriter();
			_connectPacketWriter.Put(enterPacket);
			SendReliable(_connectPacketWriter);

			_packetPool.Return(enterPacket);
		}

		private void OnDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			_sessionState = UserSessionState.NoConnection;

			if (disconnectInfo.AdditionalData.IsNull)
			{
				_log.Error($"Disconnected from the server. Disconnect reason : {disconnectInfo.Reason}");
			}
			else
			{
				var disconnectReason = (DisconnectReasonType)disconnectInfo.AdditionalData.GetByte();
				_log.Warn($"Disconnected from the server. Disconnect reason : {disconnectReason}");
			}
		}

		private IPacketWriter _readyPacketWriter = new ByteBuffer(20);
		internal void ReqTryReadyToSync()
		{
			_log.Info("Try request ready to sync");

			var readyPacket = _packetPool.GetPacket<CS_Req_ReadyToSync>();
			_readyPacketWriter.ResetWriter();
			_readyPacketWriter.Put(readyPacket);
			SendReliable(_readyPacketWriter);
			_packetPool.Return(readyPacket);
		}
	}
}