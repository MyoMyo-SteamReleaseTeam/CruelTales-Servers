using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class NetworkPlayer
	{
		// Log
		private readonly static ILog _log = LogManager.GetLogger(typeof(NetworkPlayer));

		// Referenece
		public GameManager GameManager { get; private set; }
		public WorldManager WorldManager { get; private set; }

		// Session Info
		public UserSession? Session { get; private set; }
		public UserId UserId { get; private set; }
		public NetStringShort Username { get; private set; }
		public int Costume { get; private set; }

		// Gameplay
		public NetworkTransform? TargetTransform { get; private set; }
		public NetworkTransform ViewTransform { get; private set; }
		public Vector2 HalfViewInSize { get; private set; }
		public Vector2 HalfViewOutSize { get; private set; }
		public Faction Faction { get; private set; }
		public float CameraSpeed { get; private set; } = 10.0f;

		// Visibility
		public VisibilityAuthority VisibilityAuthority { get; private set; }

		public NetworkPlayer(GameManager gameManager,
							 WorldManager worldManager,
							 InstanceInitializeOption option)
		{
			ViewTransform = new NetworkTransform();
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
			Costume = 119;
			_log.Debug($"Player {Username} created!");
		}

		public void OnDestroyed()
		{
			UserId = new UserId(0);
			Username = string.Empty;
			Costume = 0;
			if (Session != null && Session.CurrentState != UserSessionState.NoConnection)
			{
				Session.Disconnect(DisconnectReasonType.Unknown);
			}
			Session = null;
			_log.Debug($"Player {Username} destroyed!");
		}

		public void Update(float deltaTime)
		{
			Vector3 targetPosition = TargetTransform == null ? 
				Vector3.Zero : TargetTransform.Position;

			ViewTransform.Position = Vector3.Lerp(ViewTransform.Position, 
												  targetPosition,
												  CameraSpeed * deltaTime);
			ViewTransform.Velocity = TargetTransform == null ?
				Vector3.Zero : TargetTransform.Velocity;
		}

		public void OnViewTargetChanged(NetworkTransform target)
		{
			TargetTransform = target;
		}

		public override string ToString() => Username;
	}
}
