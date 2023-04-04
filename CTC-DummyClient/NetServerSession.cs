using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;

namespace CTC.DummyClient
{
	public class NetServerSession
	{
		public int PeerId { get; private set; }
		private NetPeer _peer;

		public NetServerSession(NetPeer serverPeer)
		{
			_peer = serverPeer;
		}

		public void Send()
		{

		}


	}
}
