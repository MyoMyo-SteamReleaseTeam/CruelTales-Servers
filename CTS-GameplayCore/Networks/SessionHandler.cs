using System.Collections.Generic;
using CT.Network.Core;
using CT.Network.DataType;
using CT.Tools.Collections;
using LiteNetLib;

namespace CTS.Instance.Networks
{
	public class SessionHandler
	{
		// Ingame Session
		public object _sessionLock = new();
		private BidirectionalMap<int, ClientToken> _tokenByPeerId = new();
		private Dictionary<ClientToken, NetClientSession> _sessionByToken = new();
		private Dictionary<int, NetPeer> _peerByPeerId = new();
		public int SessionCount => _tokenByPeerId.Count;

		public void RemovePeer(NetPeer peer)
		{
			lock (_sessionLock)
			{
				//_peerByPeerId.ContainsKey()
			}
		}

		public bool IsValidToken()
		{
			return true;
		}
	}
}