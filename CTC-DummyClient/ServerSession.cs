using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Packets;
using CTC.Networks.Packets;
using CTC.Networks.Synchornizations;
using CTC.Networks.SyncObjects.TestSyncObjects;
using LiteNetLib;
using log4net;

namespace CTC.Networks
{
	public class GameManager
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(GameManager));

		private ServerSession _serverSession;

		private Dictionary<NetworkIdentity, RemoteNetworkObject> _worldObjectById = new();

		public GameManager(ServerSession serverSession)
		{
			_serverSession = serverSession;
		}

		public void OnSyncInitialize(PacketReader reader)
		{
			while (reader.CanRead(1))
			{
				var type = reader.ReadNetworkObjectType();
				NetworkIdentity id = new NetworkIdentity();
				id.Deserialize(reader);
				Test_MovingCube cube = new();
				cube.OnCreated(id);
				cube.DeserializeEveryProperty(reader);
				_worldObjectById.Add(id, cube);
			}
		}

		public void OnDeserializeReliable(PacketReader reader)
		{
			while (reader.CanRead(1))
			{
				NetworkIdentity id = new NetworkIdentity();
				id.Deserialize(reader);
				if (_worldObjectById.TryGetValue(id, out var netObj))
				{
					netObj.DeserializeSyncReliable(reader);
				}
			}
		}

		public void OnDeserializeUnreliable(PacketReader reader)
		{
			while (reader.CanRead(1))
			{
				NetworkIdentity id = new NetworkIdentity();
				id.Deserialize(reader);
				if (_worldObjectById.TryGetValue(id, out var netObj))
				{
					netObj.DeserializeSyncUnreliable(reader);
				}
			}
		}

		private float _showTime = 0;

		public void Update(float deltaTime)
		{
			_showTime += deltaTime;
			if (_showTime > 3.0f)
			{
				_showTime = 0;
				foreach (var obj in _worldObjectById.Values)
				{
					if (obj is Test_MovingCube cube)
					{
						_log.Info($"{cube.Identity} : ({cube.R},{cube.G},{cube.B})");
					}
				}
			}
		}
	}

	public class ServerSession
	{
		private static ILog _log = LogManager.GetLogger(typeof(ServerSession));
		private UserSessionState _sessionState = UserSessionState.NoConnection;
		private EventBasedNetListener _listener;
		private NetManager _netManager;
		private NetPeer? _serverPeer;
		private PacketPool _packetPool = new();

		private GameManager _gameManager;
		public GameManager GameManager => _gameManager;

		public DummyUserInfo UserInfo { get; private set; }

		public ServerSession(DummyUserInfo info)
		{
			UserInfo = info;

			_gameManager = new GameManager(this);
			_listener = new EventBasedNetListener();
			_netManager = new NetManager(_listener);

			_listener.NetworkReceiveEvent += OnReceived;
			_listener.PeerConnectedEvent += OnConnected;
			_listener.PeerDisconnectedEvent += OnDisconnected;
		}

		public void Start(int clientBindPort)
		{
			_netManager.Start(clientBindPort);
			_sessionState = UserSessionState.NoConnection;
		}

		public void TryConnect(string address, int port)
		{
			_netManager.Connect(address, port, "TestServer");
			_sessionState = UserSessionState.NoConnection;
		}

		private void OnReceived(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
		{
			try
			{
				PacketReader packetReader = new PacketReader(reader.GetRemainingBytesSegment());

				while (packetReader.CanRead(sizeof(PacketType)))
				{
					PacketType packetType = packetReader.ReadPacketType();

					if (PacketDispatcher.IsCustomPacket(packetType))
					{
						PacketDispatcher.DispatchRaw(packetType, packetReader, this);
					}
					else
					{
						var packet = _packetPool.ReadPacket(packetType, packetReader);
						PacketDispatcher.Dispatch(packet, this);
						_packetPool.Return(packet);
					}
				}
			}
			catch
			{
				System.Console.WriteLine($"Disconnect! Failed to handle packet!");
				//Disconnect();
			}
		}

		public void SendReliable(PacketWriter writer,
								 byte channelNumber = 0)
		{
			_serverPeer?.Send(writer.Buffer.Array,
							  writer.Buffer.Offset,
							  writer.Count,
							  channelNumber,
							  DeliveryMethod.ReliableOrdered);
		}

		public void SendUnreliable(PacketWriter writer,
								   byte channelNumber = 0)
		{
			_serverPeer?.Send(writer.Buffer.Array,
							  writer.Buffer.Offset,
							  writer.Count,
							  channelNumber,
							  DeliveryMethod.Unreliable);
		}

		private void OnConnected(NetPeer peer)
		{
			_sessionState = UserSessionState.TryConnecting;

			_serverPeer = peer;
			_log.Info($"Success to connect to the server. Server endpoint : {peer.EndPoint}");

			var enterPacket = _packetPool.GetPacket<CS_Req_TryEnterGameInstance>();
			enterPacket.MatchTo = UserInfo.GameInstanceGuid;
			enterPacket.Token = UserInfo.UserToken;
			enterPacket.UserDataInfo = new UserDataInfo()
			{
				UserId = UserInfo.UserId,
				Username = "abc",
				UserCostume = new DokzaCostume()
				{
					Body = 1,
					Head = 2,
				}
			};

			_log.Info($"Send CS_Req_TryEnterGameInstance : {UserInfo}");

			PacketWriter pw = new PacketWriter(new PacketSegment(1000));
			pw.Put(enterPacket);

			_serverPeer.Send(pw.Buffer.Array, 0, pw.Count, DeliveryMethod.ReliableOrdered);
			_packetPool.Return(enterPacket);
		}

		private void OnDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			_sessionState = UserSessionState.NoConnection;

			if (disconnectInfo.AdditionalData.IsNull)
			{
				_log.Error($"Disconnected from the server. Disconnect reason : {disconnectInfo.Reason}");
			}
			else
			{
				var disconnectReason = (DisconnectReasonType)disconnectInfo.AdditionalData.GetByte();
				_log.Warn($"Disconnected from the server. Disconnect reason : {disconnectReason}");
			}
		}

		public void Disconnect()
		{
			_serverPeer?.Disconnect();
			_netManager.DisconnectAll();
		}

		public void Update(float deltaTime)
		{
			_netManager.PollEvents();
			_gameManager.Update(deltaTime);
		}

		internal void ReqTryReadyToSync()
		{
			_log.Info("Try request ready to sync");

			var readyPacket = _packetPool.GetPacket<CS_Req_ReadyToSync>();
			PacketWriter writer = new PacketWriter(new byte[20]);
			writer.Put(readyPacket);
			SendReliable(writer);
			_packetPool.Return(readyPacket);
		}
	}
}