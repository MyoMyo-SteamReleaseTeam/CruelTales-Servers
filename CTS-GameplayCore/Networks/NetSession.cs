using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT.Network.Serialization;
using CTS.Instance;
using LiteNetLib;

namespace CT.Network.Core
{
	public class NetSession
	{
		private NetworkService _netService;
		private int _peerId;

		public NetSession(NetworkService netService)
		{
			_netService = netService;
		}

		public void Initialize(NetPeer peer)
		{
			_peerId = peer.Id;
		}

		public void SendReliableOrdered(PacketWriter writer)
		{
			_peer.Send(writer.Buffer.Array, 
					   writer.Buffer.Offset,
					   writer.Position,
					   DeliveryMethod.ReliableOrdered);
		}
	}
}
