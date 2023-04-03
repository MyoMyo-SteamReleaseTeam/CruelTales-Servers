using CT.Network.Serialization;
using CTS.Instance;
using CTS.Instance.Gameplay;
using LiteNetLib;

namespace CT.Network.Core
{
	public class NetSession
	{
		public GameInstance? CurrentInstance { get; private set; }
		private NetworkService _netService;
		private int _peerId;

		public NetSession(NetworkService netService)
		{
			_netService = netService;
		}

		public void JoinGameInstance(GameInstance gameInstance)
		{
			CurrentInstance = gameInstance;
		}

		public void OnConnected()
		{

		}

		public void Disconnect()
		{
			CurrentInstance?.Disconnect(this);
		}

		public void Initialize(NetPeer peer)
		{
			_peerId = peer.Id;
		}

		public void SendReliableOrdered(PacketWriter writer)
		{
			//_peer.Send(writer.Buffer.Array, 
			//		   writer.Buffer.Offset,
			//		   writer.Position,
			//		   DeliveryMethod.ReliableOrdered);
		}
	}
}
