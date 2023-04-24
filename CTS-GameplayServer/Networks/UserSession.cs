﻿using System.Threading.Tasks;
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

		// Packets
		private PacketPool _packetPool = new();

		public UserSession(SessionManager sessionManager,
							 NetworkManager networkManager)
		{
			_sessionManager = sessionManager;
			_networkManager = networkManager;
			_gameInstanceManager = _networkManager.GameplayInstanceManager;
		}

		public void Reset()
		{
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

		public void OnReceive(PacketBase packet)
		{
			PacketDispatcher.Dispatch(packet, this);
		}

		public void OnReceiveRaw(PacketType packetType, PacketReader reader)
		{
			PacketDispatcher.DispatchRaw(packetType, reader, this);
		}

		public void SendReliable(PacketWriter writer,
								 byte channelNumber = NetworkManager.CHANNEL_CONNECTION)
		{
			_peer?.Send(writer.Buffer.Array,
						writer.Buffer.Offset, 
						writer.Count, 
						channelNumber, 
						DeliveryMethod.ReliableOrdered);
		}

		public void SendUnreliable(PacketWriter writer,
								   byte channelNumber = NetworkManager.CHANNEL_CONNECTION)
		{
			_peer?.Send(writer.Buffer.Array,
						writer.Buffer.Offset,
						writer.Count,
						channelNumber,
						DeliveryMethod.Unreliable);
		}

		/// <summary>유저가 접속 시도중입니다.</summary>
		public bool OnTryConnecting(NetPeer peer)
		{
			lock (_sessionLock)
			{
				if (CurrentState != UserSessionState.NoConnection)
					return false;
				CurrentState = UserSessionState.TryConnecting;

				_peer = peer;
				PeerId = peer.Id;
			}

			_ = waitAuthenticationAsync();
			return true;
		}

		/// <summary>일정 시간동안 접속 시도에 실패하면 유저의 연결을 종료합니다.</summary>
		private async ValueTask waitAuthenticationAsync()
		{
			for (int i = 0; i < WAITING_TIMEOUT_SEC; i++)
			{
				await Task.Delay(1000);
				lock (_sessionLock)
				{
					if (CurrentState == UserSessionState.InGameplay ||
						CurrentState == UserSessionState.NoConnection)
						return;
				}
			}

			var state = CurrentState; // Prevent data race
			DisconnectReasonType disconnectReason = state switch
			{
				UserSessionState.TryConnecting => DisconnectReasonType.Timeout_Authentication,
				UserSessionState.TryEnterGameInstance => DisconnectReasonType.Timeout_FailedToEnterGameInstance,
				UserSessionState.TryReadyToSync => DisconnectReasonType.Timeout_FailedToReadyToSync,
				UserSessionState.InGameplay => DisconnectReasonType.Timeout_GameCannotHandleYourSession,
				_ => DisconnectReasonType.Unknown,
			};

			Disconnect(disconnectReason);
		}

		/// <summary>
		/// 유저가 게임 참가를 요청했습니다.
		/// 게임에 참가할 수 없다면 유저의 연결을 종료합니다.
		/// </summary>
		public void OnReqTryEnterGameInstance(UserDataInfo userDataInfo, UserToken token, GameInstanceGuid roomGuid)
		{
			lock (_sessionLock)
			{
				if (CurrentState != UserSessionState.TryConnecting)
				{
					_log.Warn($"User {this} request {nameof(OnReqTryEnterGameInstance)} when current state is {CurrentState}");
					Disconnect(DisconnectReasonType.WrongOperation);
					return;
				}

				CurrentState = UserSessionState.TryEnterGameInstance;

				UserToken = token;
				UserDataInfo = userDataInfo;
				GameInstanceGuid = roomGuid;

				_log.Info($"User {this} has been verified. Try to enter game [Target GUID:{GameInstanceGuid}]");
				if (_gameInstanceManager.TryGetGameInstanceBy(GameInstanceGuid, out var instance))
				{
					instance.SessionHandler.Push_TryEnterGame(this);
					return;
				}

				_log.Error($"There is no GameInstance with GUID {GameInstanceGuid}");
				Disconnect(DisconnectReasonType.ThereIsNoSuchGameInstance);
				return;
			}
		}
		
		/// <summary>
		/// 게임 참가 응답입니다.
		/// </summary>
		public void AckTryEnterGameInstance()
		{
			lock (_sessionLock)
			{
				if (CurrentState != UserSessionState.TryEnterGameInstance)
				{
					_log.Warn($"Wrong {nameof(AckTryEnterGameInstance)} to user {this}, when current state is {CurrentState}");
					Disconnect(DisconnectReasonType.WrongOperation);
					return;
				}

				CurrentState = UserSessionState.TryReadyToSync;

				// Create Ack packet
				var ackPacket = _packetPool.GetPacket<SC_Ack_TryEnterGameInstance>();
				ackPacket.AckResult = AckJoinMatch.Success;
				PacketWriter writer = new PacketWriter(new byte[20]);
				writer.Put(ackPacket);

				// Send Ack
				SendReliable(writer);
				_packetPool.Return(ackPacket);
			}
		}

		public void OnReqReadyToEnter()
		{
			lock (_sessionLock)
			{
				if (CurrentState != UserSessionState.TryReadyToSync)
				{
					_log.Warn($"User {this} request {nameof(OnReqReadyToEnter)} when current state is {CurrentState}");
					Disconnect(DisconnectReasonType.WrongOperation);
					return;
				}

				if (_gameInstanceManager.TryGetGameInstanceBy(GameInstanceGuid, out var instance))
				{
					_log.Info($"User {this} is ready to sync");
					instance.SessionHandler.Push_TryReadyToSync(this);
					return;
				}

				_log.Error($"There is no GameInstance with GUID {GameInstanceGuid}");
				Disconnect(DisconnectReasonType.ThereIsNoSuchGameInstance);
				return;
			}
		}

		public void OnEnterGame(UserSessionHandler sessionHandler)
		{
			SessionHandler = sessionHandler;
			CurrentState = UserSessionState.InGameplay;
		}

		public override string ToString()
		{
			return $"{UserId}:{Username}";
		}
	}
}
