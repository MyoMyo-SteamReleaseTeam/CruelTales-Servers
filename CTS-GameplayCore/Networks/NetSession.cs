using System.Threading;
using System.Threading.Tasks;
using CT.Network.DataType;
using CT.Network.Serialization;
using CTS.Instance.Networks;
using LiteNetLib;
using log4net;

namespace CT.Network.Core
{
	public enum NetSessionState
	{
		None = 0,
		TryGetAuthentication,
		TryToJoinGame,
		Gameplay,
	}

	public abstract class NetSession
	{
		public const int WAITING_TIMEOUT_SEC = 5;
		protected NetPeer? _peer;
		public int PeerId => _peer == null ? -1 : _peer.Id;

		public virtual void Initialize(NetPeer peer)
		{
			_peer = peer;
		}

		public abstract void Disconnect();

		public void SendReliableOrdered(PacketWriter writer)
		{
			_peer?.Send(writer.Buffer.Array,
						writer.Buffer.Offset,
						writer.Position,
						DeliveryMethod.ReliableOrdered);
		}
	}

	public class ClientSession : NetSession
	{
		private object _sessionLock = new object();
		public static ILog _log = LogManager.GetLogger(typeof(ClientSession));

		public NetSessionState CurrentState { get; private set; }
		private SessionHandler _sessionHandler;

		// Client Authentication
		public ClientId ClientId { get; private set; }
		public ClientToken ClientToken { get; private set; }
		public RoomGuid MatchEndpoint { get; private set; }

		public ClientSession(SessionHandler sessionHandler)
		{
			_sessionHandler = sessionHandler;
		}

		public bool TryWaitForAuthentication()
		{
			lock (_sessionLock)
			{
				if (CurrentState != NetSessionState.None)
				{
					return false;
				}
				CurrentState = NetSessionState.TryGetAuthentication;
			}
			var t = waitAuthenticationAsync();
			return true;
		}

		private async ValueTask waitAuthenticationAsync()
		{
			for (int i = 0; i < WAITING_TIMEOUT_SEC; i++)
			{
				await Task.Delay(1000);
				lock (_sessionLock)
				{
					if (CurrentState == NetSessionState.TryToJoinGame)
						return;
				}
			}

			Disconnect();
		}

		public void WaitForMatch(ClientId id, ClientToken token, RoomGuid roomGuid)
		{
			lock (_sessionLock)
			{
				CurrentState = NetSessionState.TryToJoinGame;

				ClientId = id;
				ClientToken = token;
				MatchEndpoint = roomGuid;
			}

			_log.Info($"Client {ClientId} has been verified. Token({ClientToken}) MatchEndPoint({MatchEndpoint})");
		}

		public override void Disconnect()
		{
			lock (_sessionLock)
			{
				CurrentState = NetSessionState.None;
			}

			if (_peer == null)
				return;

			_sessionHandler.Disconnect(_peer);
		}
	}
}
