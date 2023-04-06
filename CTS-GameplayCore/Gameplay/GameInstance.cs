using CT.Network.Core;
using CT.Network.DataType;
using CT.Network.Serialization;
using CT.Tools.Collections;

namespace CTS.Instance.Gameplay
{
	public class GameInstance
	{
		private static int IdCounter = 1;
		public int Id { get; private set; }

		private object _sessionLock = new object();
		private BidirectionalMap<ClientToken, NetSession> _clientSessionByToken = new();
		public int SessionCount => _clientSessionByToken.Count;

		public GameInstance(ServerOption serverOption)
		{
			Id = IdCounter++;

		}

		public void OnConnected(ClientToken token, NetSession session)
		{
			lock (_sessionLock)
			{
				_clientSessionByToken.TryAdd(token, session);
			}
		}

		public void Disconnect(NetSession session)
		{
			lock (_sessionLock)
			{
				_clientSessionByToken.TryRemove(session);
			}
		}

		public void Update(float delta)
		{
		}

		public void OnPacketRecevied(ClientToken token, PacketReader reader)
		{

		}

		public void DeserializePackets()
		{
		}

		public void SerializePackets()
		{
		}
	}
}
