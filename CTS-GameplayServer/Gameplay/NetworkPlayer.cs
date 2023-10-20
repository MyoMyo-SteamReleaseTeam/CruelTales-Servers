using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CTS.Instance.Networks;
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
		private PlayerState? _playerState;
		public InstanceInitializeOption Option { get; private set; }

		// Session Info
		public UserSession? Session { get; private set; }

		private UserId _userId;
		public UserId UserId
		{ 
			get => _userId;
			private set
			{
				_userId = value;
				if (_playerState != null)
					_playerState.UserId = _userId;
			}
		}

		private NetStringShort _username;
		public NetStringShort Username
		{
			get => _username;
			private set
			{
				_username = value;
				if (_playerState != null)
					_playerState.Username = _username;
			}
		}

		private bool _isHost = false;
		public bool IsHost
		{
			get => _isHost;
			set
			{
				_isHost = value;
				if (_playerState != null)
					_playerState.IsHost = _isHost;
			}
		}

		private bool _isReady = false;
		public bool IsReady
		{
			get => _isReady;
			set
			{
				_isReady = value;
				if (_playerState != null)
					_playerState.IsReady= _isReady;
			}
		}

		private bool _isMapLoaded = false;
		public bool IsMapLoaded
		{
			get => _isMapLoaded;
			set
			{
				_isMapLoaded = value;
				if (_playerState != null)
					_playerState.IsMapLoaded = _isMapLoaded;
			}
		}

		private bool _isEliminated = false;
		public bool IsEliminated
		{
			get => _isEliminated;
			set
			{
				_isEliminated = value;
				if (_playerState != null)
					_playerState.IsEliminated = _isEliminated;
			}
		}

		private Faction _faction = Faction.System;
		public Faction Faction
		{
			get => _faction;
			set
			{
				_faction = value;
				if (_playerState != null)
					_playerState.Faction = _faction;
			}
		}

		// Gameplay
		public CameraController? CameraController { get; private set; }
		public PlayerCharacter? PlayerCharacter { get; private set; }
		public Vector2 ViewPosition { get; set; }
		public Vector2 HalfViewInSize { get; private set; }
		public Vector2 HalfViewOutSize { get; private set; }
		public SkinSet CurrentSkin { get; private set; }
		public SkinSet SelectedSkin { get; private set; }

		// Visibility
		public bool CanSeeViewObject { get; set; } = false;

		/// <summary>시야 범위를 벗어나도 볼 수 있습니다.</summary>
		public bool IgnoreViewDistance { get; private set; }

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
			Option = option;
			OptimizeViewSize();
		}

		public void OnCreated(UserSession userSession)
		{
			Initialize();

			// Setup Session
			Session = userSession;
			UserId = Session.UserId;
			Username = Session.Username;

			_log.Debug($"Player {Username} created!");
		}

		public void Initialize()
		{
			// Sessopm
			Session = null;
			UserId = new UserId(0);
			Username = string.Empty;
			_playerState = null;

			// State
			IsHost = false;
			IsReady = false;
			IsMapLoaded = false;
			IsEliminated = false;
			Faction = Faction.System;

			// Gameplay
			CameraController = null;
			PlayerCharacter = null;
			ViewPosition = Vector2.Zero;
			HalfViewInSize = Vector2.Zero;
			HalfViewOutSize = Vector2.Zero;

			// Visibility
			CanSeeViewObject = false;
			IgnoreViewDistance = false;
		}

		public void OnDestroyed()
		{
			if (Session != null && Session.CurrentState != UserSessionState.NoConnection)
			{
				Session.Disconnect(DisconnectReasonType.Unknown);
			}

			Initialize();
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
			OptimizeViewSize();

			if (character is WolfCharacter)
			{
				IgnoreViewDistance = true;
			}
			else
			{
				IgnoreViewDistance = false;
			}
		}

		public void ReleaseCharacter(PlayerCharacter character)
		{
			if (PlayerCharacter != character)
				return;

			PlayerCharacter = null;
			OptimizeViewSize();
		}

		public void BindPlayerState(PlayerState state)
		{
			_playerState = state;
			state.UserId = UserId;
			state.Username = new(Username);

			state.IsHost = IsHost;
			state.IsReady = IsReady;
			state.IsMapLoaded = IsMapLoaded;
			state.IsEliminated = IsEliminated;
		}

		public void OptimizeViewSize()
		{
			HalfViewInSize = Option.HalfViewInSize;
			HalfViewOutSize = Option.HalfViewOutSize;
		}

		public override string ToString() => $"{Username}:{UserId}";
	}
}
