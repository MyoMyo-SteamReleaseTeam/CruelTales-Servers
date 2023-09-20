using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class NetworkPlayer
	{
		// Log
		private readonly static ILog _log = LogManager.GetLogger(typeof(NetworkPlayer));

		// Referenece
		public GameplayManager GameManager { get; private set; }
		public WorldManager WorldManager { get; private set; }
		public PlayerState? PlayerState { get; private set; }

		// Session Info
		public UserSession? Session { get; private set; }
		private UserId _userId;
		private NetStringShort _username;

		public UserId UserId
		{ 
			get => _userId;
			private set
			{
				_userId = value;
				if (PlayerState != null)
					PlayerState.UserId = _userId;
			}
		}

		public NetStringShort Username
		{
			get => _username;
			private set
			{
				_username = value;
				if (PlayerState != null)
					PlayerState.Username = _username;
			}
		}

		public bool _isHost = false;
		public bool IsHost
		{
			get => _isHost;
			set
			{
				_isHost = value;
				if (PlayerState != null)
					PlayerState.IsHost = _isHost;
			}
		}

		public bool _isReady = false;
		public bool IsReady
		{
			get => _isReady;
			set
			{
				_isReady = value;
				if (PlayerState != null)
					PlayerState.IsReady= _isReady;
			}
		}

		public bool _isMapLoaded = false;
		public bool IsMapLoaded
		{
			get => _isMapLoaded;
			set
			{
				_isMapLoaded = value;
				if (PlayerState != null)
					PlayerState.IsMapLoaded = _isMapLoaded;
			}
		}

		// Gameplay
		public CameraController? CameraController { get; private set; }
		public PlayerCharacter? PlayerCharacter { get; private set; }
		//public NetRigidBody? TargetRigidBody { get; private set; }
		public Vector2 ViewPosition
		{
			get
			{
				if (CameraController == null)
					return Vector2.Zero;

				return CameraController.ViewPosition;
			}
		}
		public Vector2 HalfViewInSize { get; private set; }
		public Vector2 HalfViewOutSize { get; private set; }
		public Faction Faction { get; private set; }

		// Visibility
		public bool CanSeeViewObject { get; set; } = false;

#if DEBUG
#pragma warning disable CS8618
		public NetworkPlayer(UserId userId)
		{
			UserId = userId;
		}
#pragma warning restore CS8618
#endif

		public NetworkPlayer(GameplayManager gameManager,
							 WorldManager worldManager,
							 InstanceInitializeOption option)
		{
			GameManager = gameManager;
			WorldManager = worldManager;
			HalfViewInSize = option.HalfViewInSize;
			HalfViewOutSize = option.HalfViewOutSize;
		}

		public void OnCreated(UserSession userSession)
		{
			// Setup Session
			Session = userSession;
			UserId = Session.UserId;
			Username = Session.Username;

			IsHost = false;
			IsReady = false;
			IsMapLoaded = false;

			// Visibility
			CanSeeViewObject = false;

			_log.Debug($"Player {Username} created!");
		}

		public void OnDestroyed()
		{
			UserId = new UserId(0);
			Username = string.Empty;
			if (Session != null && Session.CurrentState != UserSessionState.NoConnection)
			{
				Session.Disconnect(DisconnectReasonType.Unknown);
			}
			Session = null;
			_log.Debug($"Player {Username} destroyed!");
		}

		public void Update(float deltaTime)
		{
		}

		public void BindCamera(CameraController camera)
		{
			CameraController = camera;
		}

		public void ReleaseCamera(CameraController camera)
		{
			if (CameraController != camera)
				return;

			CameraController = null;
		}

		public void BindCharacter(PlayerCharacter character)
		{
			PlayerCharacter = character;
		}

		public void ReleaseCharacter(PlayerCharacter character)
		{
			if (PlayerCharacter != character)
				return;

			PlayerCharacter = null;
		}

		public void BindPlayerState(PlayerState state)
		{
			PlayerState = state;
			state.UserId = UserId;
			state.Username = new(Username);

			state.IsHost = IsHost;
			state.IsReady = IsReady;
			state.IsMapLoaded = IsMapLoaded;

			state.Costume.Head = RandomHelper.NextInt(20);
			state.Costume.Body = RandomHelper.NextInt(20);
		}

		public void ReleasePlayerState()
		{
			PlayerState = null;
		}

		public override string ToString() => $"{Username}:{UserId}";
	}
}
