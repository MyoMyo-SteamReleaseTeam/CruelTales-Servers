using System.Threading.Tasks;
using CT.Network.DataType;
using CT.Network.Serialization;
using CTS.Instance.Gameplay;
using CTS.Instance.Packets;
using LiteNetLib;
using log4net;

namespace CTS.Instance.Networks
{
	public enum NetSessionState
	{
		NoConnection = 0,
		WaitForJoinRequest,
		WaitForJoinTheGame,
		InGame,
	}

	public class ClientSession
	{
		// Constant
		public const int WAITING_TIMEOUT_SEC = 5;

		// Log
		public static ILog _log = LogManager.GetLogger(typeof(ClientSession));

		// Gameplay
		public GameInstance? GameInstance { get; private set; }

		// Services
		private readonly SessionManager _sessionManager;
		private readonly NetworkManager _networkManager;
		private readonly GameInstanceManager _gameInstanceManager;

		// Network
		private NetPeer? _peer;
		public int PeerId { get; private set; }

		// State
		public NetSessionState CurrentState { get; private set; }

		// Verifications
		public ClientId ClientId { get; private set; }
		public ClientToken ClientToken { get; private set; }
		public GameInstanceGuid GameInstanceGuid { get; private set; }

		// Lock
		private object _clientSessionLock = new object();

		public ClientSession(SessionManager sessionManager,
							 NetworkManager networkManager)
		{
			_sessionManager = sessionManager;
			_networkManager = networkManager;
			_gameInstanceManager = _networkManager.GameplayInstanceManager;
		}

		public void OnDisconnected(DisconnectInfo disconnectInfo)
		{
			lock (_clientSessionLock)
			{
				disconnectInternal();
			}

			_log.Info($"Client {this} disconnected. Reason : {disconnectInfo.Reason}");
		}

		public void Disconnect()
		{
			lock (_clientSessionLock)
			{
				disconnectInternal();
			}
		}

		private void disconnectInternal()
		{
			if (CurrentState == NetSessionState.NoConnection)
				return;

			CurrentState = NetSessionState.NoConnection;
			_peer?.Disconnect();
			_sessionManager.Remove(this);
			//GameInstance?.OnPlayerDisconnected(this); // TODO : Add disconnected job
		}

		public bool TryConnected(NetPeer peer)
		{
			lock (_clientSessionLock)
			{
				if (CurrentState != NetSessionState.NoConnection)
					return false;
				CurrentState = NetSessionState.WaitForJoinRequest;

				_peer = peer;
				PeerId = peer.Id;
				return true;
			}
		}

		public void OnReceive(PacketBase packet)
		{
			PacketDispatcher.Dispatch(packet, this);
		}

		public void SendReliable(PacketWriter writer)
		{

		}

		public void SendUnreliable(PacketWriter writer)
		{

		}

		public bool WaitForJoinRequest()
		{
			lock (_clientSessionLock)
			{
				if (CurrentState != NetSessionState.WaitForJoinRequest)
					return false;
				CurrentState = NetSessionState.WaitForJoinRequest;
			}

			var t = waitAuthenticationAsync();
			return true;
		}

		private async ValueTask waitAuthenticationAsync()
		{
			for (int i = 0; i < WAITING_TIMEOUT_SEC; i++)
			{
				await Task.Delay(1000);
				lock (_clientSessionLock)
				{
					if (CurrentState == NetSessionState.WaitForJoinTheGame)
						return;
				}
			}

			Disconnect();
		}

		public void OnReqTryJoinGameInstance(ClientId id, ClientToken token, GameInstanceGuid roomGuid)
		{
			lock (_clientSessionLock)
			{
				if (CurrentState != NetSessionState.WaitForJoinTheGame)
				{
					disconnectInternal();
					return;
				}

				ClientId = id;
				ClientToken = token;
				GameInstanceGuid = roomGuid;

				if (_gameInstanceManager.TryGetGameInstanceBy(GameInstanceGuid, out var instance))
				{
					if (instance.TryJoinSession(this))
					{
						CurrentState = NetSessionState.InGame;
						GameInstance = instance;
					}

					disconnectInternal();
					_log.Error($"Client {this} fail to join GameInstance {GameInstanceGuid}");
				}
				else
				{
					disconnectInternal();
					_log.Error($"There is no GameInstance with GUID {GameInstanceGuid}");
				}
			}

			_log.Info($"Client {ClientId} has been verified. Token({ClientToken}) MatchEndPoint({GameInstanceGuid})");
		}

		public override string ToString()
		{
			return ClientId.ToString();
		}
	}
}
