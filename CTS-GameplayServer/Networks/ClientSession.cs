using System;
using System.Threading.Tasks;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Serialization.Type;
using CT.Networks.Extensions;
using CTS.Instance.Gameplay;
using CTS.Instance.Packets;
using LiteNetLib;
using log4net;

namespace CTS.Instance.Networks
{
	public class ClientSession
	{
		// Constant
		public const int WAITING_TIMEOUT_SEC = 5;

		// Log
		public static ILog _log = LogManager.GetLogger(typeof(ClientSession));

		// Gameplay
		public ClientSessionHandler? SessionHandler { get; private set; }

		// Services
		private readonly SessionManager _sessionManager;
		private readonly NetworkManager _networkManager;
		private readonly GameInstanceManager _gameInstanceManager;

		// Network
		private NetPeer? _peer;
		public int PeerId { get; private set; }

		// State
		public ClientSessionState CurrentState { get; private set; }

		// Verifications
		public ClientId ClientId { get; private set; }
		public ClientToken ClientToken { get; private set; }
		public NetStringShort ClientName { get; private set; }
		public GameInstanceGuid GameInstanceGuid { get; private set; }

		// Lock
		private object _clientSessionLock = new object();

		// Buffer
		private byte[] _disconnectReasonBuffer = new byte[1];

		public ClientSession(SessionManager sessionManager,
							 NetworkManager networkManager)
		{
			_sessionManager = sessionManager;
			_networkManager = networkManager;
			_gameInstanceManager = _networkManager.GameplayInstanceManager;
		}

		public void OnDisconnected(DisconnectInfo disconnectInfo)
		{
			DisconnectReasonType reason;

			lock (_clientSessionLock)
			{
				reason = LiteNetLibExtension.ConvertEnum(disconnectInfo.Reason);
				disconnectInternal(reason);
			}
		}

		public void Disconnect(DisconnectReasonType disconnectReason)
		{
			lock (_clientSessionLock)
			{
				disconnectInternal(disconnectReason);
			}
		}

		private void disconnectInternal(DisconnectReasonType disconnectReason)
		{
			// Set state
			if (CurrentState == ClientSessionState.NoConnection)
				return;
			CurrentState = ClientSessionState.NoConnection;

			// Disconnect
			_disconnectReasonBuffer[0] = (byte)disconnectReason;
			_peer?.Disconnect(_disconnectReasonBuffer);

			// Remove from session manager
			_sessionManager.Remove(this);

			// Leave from game instance and clear it's reference
			SessionHandler?.Push_OnDisconnected(this);
			SessionHandler = null;

			_log.Info($"Client {this} disconnected. Reason : {disconnectReason}");
		}

		public bool TryConnected(NetPeer peer)
		{
			lock (_clientSessionLock)
			{
				if (CurrentState != ClientSessionState.NoConnection)
					return false;
				CurrentState = ClientSessionState.WaitForJoinRequest;

				_peer = peer;
				PeerId = peer.Id;
			}

			_ = waitAuthenticationAsync();
			return true;
		}

		public void OnReceive(PacketBase packet)
		{
			PacketDispatcher.Dispatch(packet, this);
		}

		public void SendReliable(PacketWriter writer, byte channelNumber)
		{
			_peer?.Send(writer.Buffer.Array,
						writer.Buffer.Offset, 
						writer.Count, 
						channelNumber, 
						DeliveryMethod.ReliableSequenced);
		}

		public void SendUnreliable(PacketWriter writer, byte channelNumber)
		{
			_peer?.Send(writer.Buffer.Array,
						writer.Buffer.Offset,
						writer.Count,
						channelNumber,
						DeliveryMethod.Unreliable);
		}

		private async ValueTask waitAuthenticationAsync()
		{
			for (int i = 0; i < WAITING_TIMEOUT_SEC; i++)
			{
				await Task.Delay(1000);
				lock (_clientSessionLock)
				{
					if (CurrentState == ClientSessionState.InGame || 
						CurrentState == ClientSessionState.NoConnection)
					return;
				}
			}

			Disconnect(DisconnectReasonType.AuthenticationTimeout);
		}

		public void OnReqTryJoinGameInstance(ClientId id, ClientToken token, GameInstanceGuid roomGuid, NetStringShort username)
		{
			lock (_clientSessionLock)
			{
				_log.Info($"Client {ClientId}:{username} has been verified. [Token:{ClientToken}][TargetGUID:{GameInstanceGuid}]");

				if (CurrentState != ClientSessionState.WaitForJoinRequest)
				{
					_log.Warn($"Client {this} request join game when current state is {CurrentState}");
					return;
				}

				CurrentState = ClientSessionState.WaitForJoinGame;

				ClientId = id;
				ClientToken = token;
				ClientName = username;
				GameInstanceGuid = roomGuid;

				if (_gameInstanceManager.TryGetGameInstanceBy(GameInstanceGuid, out var instance))
				{
					instance.SessionHandler.Push_TryJoinGame(this);
					return;
				}
				else
				{
					_log.Error($"There is no GameInstance with GUID {GameInstanceGuid}");
					Disconnect(DisconnectReasonType.ThereIsNoSuchGameInstance);
					return;
				}
			}
		}
		
		public void OnEnterGame(ClientSessionHandler sessionHandler)
		{
			SessionHandler = sessionHandler;
			CurrentState = ClientSessionState.InGame;
		}

		public override string ToString()
		{
			return $"{ClientId}:{ClientName}";
		}
	}
}
