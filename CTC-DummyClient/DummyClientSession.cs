using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Packets;
using LiteNetLib;
using log4net;

namespace CTC.Networks
{
	public class DummyClientSession
	{
		private static ILog _log = LogManager.GetLogger(typeof(DummyClientSession));

		private EventBasedNetListener _listener;
		private NetManager _netManager;
		private NetPeer? _serverPeer;

		public DummyUserInfo UserInfo { get; private set; }

		public DummyClientSession(DummyUserInfo info)
		{
			UserInfo = info;

			_listener = new EventBasedNetListener();
			_netManager = new NetManager(_listener);

			_listener.NetworkReceiveEvent += OnReceived;
			_listener.PeerConnectedEvent += OnConnected;
			_listener.PeerDisconnectedEvent += OnDisconnected;
		}

		public void Start(int clientBindPort)
		{
			_netManager.Start(clientBindPort);
		}

		public void TryConnect(string address, int port)
		{
			_netManager.Connect(address, port, "TestServer");
		}

		private void OnConnected(NetPeer peer)
		{
			_serverPeer = peer;
			_log.Info($"Success to connect to the server. Server endpoint : {peer.EndPoint}");

			CS_Req_TryJoinGameInstance reqJoinGame = new CS_Req_TryJoinGameInstance();

			reqJoinGame.MatchTo = UserInfo.GameInstanceGuid;
			reqJoinGame.Id = UserInfo.ClientId;
			reqJoinGame.Token = UserInfo.ClientToken;
			reqJoinGame.Username = "abc";

			_log.Info($"Try request join game to server... : {UserInfo}");

			PacketWriter pw = new PacketWriter(new PacketSegment(1000));
			pw.Put(reqJoinGame);

			_serverPeer.Send(pw.Buffer.Array, 0, pw.Count, DeliveryMethod.ReliableOrdered);
		}

		private void OnDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
		{
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

		private void OnReceived(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			_log.Info($"Packet received. Size : {reader.UserDataSize}");
		}

		public void Disconnect()
		{
			_serverPeer?.Disconnect();
			_netManager.DisconnectAll();
		}

		public void PollEvents()
		{
			_netManager.PollEvents();
		}
	}
}