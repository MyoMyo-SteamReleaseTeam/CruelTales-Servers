using System;
using System.Threading.Tasks;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Network.Extensions;
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
		WaitForJoinGame,
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
			if (CurrentState == NetSessionState.NoConnection)
				return;
			CurrentState = NetSessionState.NoConnection;

			// Disconnect
			_disconnectReasonBuffer[0] = (byte)disconnectReason;
			_peer?.Disconnect(_disconnectReasonBuffer);

			// Remove from session manager
			_sessionManager.Remove(this);

			// Leave from game instance and clear it's reference
			GameInstance?.Push_OnDisconnected(this); // TODO : Add disconnected job
			GameInstance = null;

			_log.Info($"Client {this} disconnected. Reason : {disconnectReason}");
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
			}

			var t = waitAuthenticationAsync();
			return true;
		}

		public void OnReceive(PacketBase packet)
		{
			PacketDispatcher.Dispatch(packet, this);
		}

		public void SendReliable(PacketWriter writer, int channelNumber)
		{
			//this._peer.Send(writer.Buffer, 0, writer.Count, channelNumber, DeliveryMethod.ReliableSequenced)
		}

		public void SendUnreliable(PacketWriter writer)
		{

		}

		private async ValueTask waitAuthenticationAsync()
		{
			for (int i = 0; i < WAITING_TIMEOUT_SEC; i++)
			{
				await Task.Delay(1000);
				lock (_clientSessionLock)
				{
					if (CurrentState == NetSessionState.InGame || 
						CurrentState == NetSessionState.NoConnection)
					return;
				}
			}

			Disconnect(DisconnectReasonType.AuthenticationTimeout);
		}

		public void OnReqTryJoinGameInstance(ClientId id, ClientToken token, GameInstanceGuid roomGuid)
		{
			lock (_clientSessionLock)
			{
				_log.Info($"Client {ClientId} has been verified. [Token:{ClientToken}][TargetGUID:{GameInstanceGuid}]")
					;
				if (CurrentState != NetSessionState.WaitForJoinRequest)
				{
					_log.Warn($"Client {this} request join game when current state is {CurrentState}");
					return;
				}

				CurrentState = NetSessionState.WaitForJoinGame;

				ClientId = id;
				ClientToken = token;
				GameInstanceGuid = roomGuid;

				if (_gameInstanceManager.TryGetGameInstanceBy(GameInstanceGuid, out var instance))
				{
					instance.Push_TryJoinSession(this);
					return;
				}
				else
				{
					_log.Error($"There is no GameInstance with GUID {GameInstanceGuid}");
					disconnectInternal(DisconnectReasonType.ThereIsNoSuchGameInstance);
					return;
				}
			}
		}

		public void Callback_TryJoinGame(bool isSuccess, GameInstance? instance, DisconnectReasonType rejectReason)
		{
			if (isSuccess)
			{
				CurrentState = NetSessionState.InGame;
				GameInstance = instance;
				return;

			}

			_log.Error($"Client {this} fail to join GameInstance {GameInstanceGuid}");
			disconnectInternal(rejectReason);
			return;
		}

		public override string ToString()
		{
			return ClientId.ToString();
		}
	}
}
