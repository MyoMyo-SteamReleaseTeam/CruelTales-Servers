using System;
using System.Threading.Tasks;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Serialization.Type;
using CT.Networks.Extensions;
using CT.Packets;
using CTS.Instance.Gameplay;
using CTS.Instance.Packets;
using LiteNetLib;
using log4net;

namespace CTS.Instance.Networks
{
	public class UserSession
	{
		// Constant
		public const int WAITING_TIMEOUT_SEC = 5;

		// Log
		public static ILog _log = LogManager.GetLogger(typeof(UserSession));

		// Gameplay
		public UserSessionHandler? SessionHandler { get; private set; }

		// Services
		private readonly SessionManager _sessionManager;
		private readonly NetworkManager _networkManager;
		private readonly GameInstanceManager _gameInstanceManager;

		// Network
		private NetPeer? _peer;
		public int PeerId { get; private set; }

		// State
		public UserSessionState CurrentState { get; private set; }

		// User Data
		public GameInstanceGuid GameInstanceGuid { get; private set; }
		public UserToken UserToken { get; private set; }
		public UserDataInfo UserDataInfo { get; private set; }
		public UserId UserId => UserDataInfo.UserId;
		public NetStringShort Username => UserDataInfo.Username;

		// Lock
		private object _sessionLock = new object();

		// Buffer
		private byte[] _disconnectReasonBuffer = new byte[1];

		public UserSession(SessionManager sessionManager,
							 NetworkManager networkManager)
		{
			_sessionManager = sessionManager;
			_networkManager = networkManager;
			_gameInstanceManager = _networkManager.GameplayInstanceManager;
		}

		public void OnDisconnected(DisconnectInfo disconnectInfo)
		{
			DisconnectReasonType reason;

			lock (_sessionLock)
			{
				reason = LiteNetLibExtension.ConvertEnum(disconnectInfo.Reason);
				disconnectInternal(reason);
			}
		}

		public void Disconnect(DisconnectReasonType disconnectReason)
		{
			lock (_sessionLock)
			{
				disconnectInternal(disconnectReason);
			}
		}

		private void disconnectInternal(DisconnectReasonType disconnectReason)
		{
			// Set state
			if (CurrentState == UserSessionState.NoConnection)
				return;
			CurrentState = UserSessionState.NoConnection;

			// Disconnect
			_disconnectReasonBuffer[0] = (byte)disconnectReason;
			_peer?.Disconnect(_disconnectReasonBuffer);

			// Remove from session manager
			_sessionManager.Remove(this);

			// Leave from game instance and clear it's reference
			SessionHandler?.Push_OnDisconnected(this);
			SessionHandler = null;

			_log.Info($"User {this} disconnected. Reason : {disconnectReason}");
		}

		public bool TryConnected(NetPeer peer)
		{
			lock (_sessionLock)
			{
				if (CurrentState != UserSessionState.NoConnection)
					return false;
				CurrentState = UserSessionState.WaitForJoinRequest;

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
				lock (_sessionLock)
				{
					if (CurrentState == UserSessionState.InGame || 
						CurrentState == UserSessionState.NoConnection)
					return;
				}
			}

			Disconnect(DisconnectReasonType.AuthenticationTimeout);
		}

		public void OnReqTryJoinGameInstance(UserDataInfo userDataInfo, UserToken token, GameInstanceGuid roomGuid)
		{
			lock (_sessionLock)
			{

				if (CurrentState != UserSessionState.WaitForJoinRequest)
				{
					_log.Warn($"User {this} request join game when current state is {CurrentState}");
					return;
				}

				CurrentState = UserSessionState.WaitForJoinGame;

				UserToken = token;
				UserDataInfo = userDataInfo;
				GameInstanceGuid = roomGuid;

				_log.Info($"User {UserId}:{Username} has been verified. [Token:{UserToken}][TargetGUID:{GameInstanceGuid}]");

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
		
		public void OnEnterGame(UserSessionHandler sessionHandler)
		{
			SessionHandler = sessionHandler;
			CurrentState = UserSessionState.InGame;
		}

		public override string ToString()
		{
			return $"{UserId}:{Username}";
		}
	}
}
