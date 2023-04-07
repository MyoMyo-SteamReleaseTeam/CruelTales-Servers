using CT.Network.DataType;
using CT.Packets;
using LiteNetLib;
using log4net;

namespace CTC.DummyClient
{
	public class DummyClientNetworkManager
	{
		private static ILog _log = LogManager.GetLogger(typeof(DummyClientNetworkManager));

		private EventBasedNetListener _listener;
		private NetManager _netManager;

		public DummyClientNetworkManager()
		{
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
			_log.Info($"Success to connect to the server. Server endpoint : {peer.EndPoint}");

			Client_Req_TryJoinGameInstance reqJoinMatch = new Client_Req_TryJoinGameInstance();
			reqJoinMatch.MatchTo = new GameInstanceGuid(0);
			//reqJoinMatch.Id = 
			_log.Info($"Try send match endpoint to server...");
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