using CT.Network.Serialization;
using CT.Packets;
using LiteNetLib;
using log4net;

namespace CTC.DummyClient
{
	public class DummyClientSession
	{
		private static ILog _log = LogManager.GetLogger(typeof(DummyClientSession));

		private EventBasedNetListener _listener;
		private NetManager _netManager;
		private NetPeer? _serverPeer;

		private DummyUserInfo _userInfo;

		public DummyClientSession(DummyUserInfo info)
		{
			_userInfo = info;

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

			Client_Req_TryJoinGameInstance reqJoinGame = new Client_Req_TryJoinGameInstance();

			reqJoinGame.MatchTo = _userInfo.GameInstanceGuid;
			reqJoinGame.Id = _userInfo.ClientId;
			reqJoinGame.Token = _userInfo.ClientToken;

			_log.Info($"Try request join game to server... : {_userInfo}");

			PacketWriter pw = new PacketWriter(new PacketSegment(1000));
			pw.Put(reqJoinGame);

			_serverPeer.Send(pw.Buffer.Array, 0, pw.Position, DeliveryMethod.ReliableOrdered);
		}

		private void OnDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			_log.Warn($"Disconnected from the server. Disconnect reason : {disconnectInfo.Reason}");
		}

		private void OnReceived(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			_log.Info($"Packet received. Size : {reader.UserDataSize}");
		}

		public void PollEvents()
		{
			_netManager.PollEvents();
		}
	}
}