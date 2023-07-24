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
		public GameplayManager GameManager { get; private set; }
		public WorldManager WorldManager { get; private set; }

		// Session Info
		public UserSession? Session { get; private set; }
		public UserId UserId { get; private set; }
		public NetStringShort Username { get; private set; }
		public int Costume { get; private set; }
		
		// Matchmaking
		public bool IsHost { get; set; } = false;

		// Gameplay
		public NetRigidBody? TargetRigidBody { get; private set; }
		public Vector2 ViewPosition { get; private set; }
		public bool HasViewTarget => TargetRigidBody != null;
		public Vector2 HalfViewInSize { get; private set; }
		public Vector2 HalfViewOutSize { get; private set; }
		public Faction Faction { get; private set; }
		public float CameraSpeed { get; private set; } = 10.0f;

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
			Costume = 119;

			// Visibility
			CanSeeViewObject = false;

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
			Vector2 targetPosition = TargetRigidBody == null ?
				Vector2.Zero : TargetRigidBody.Position;

			ViewPosition = Vector2.Lerp(ViewPosition,
										targetPosition,
										CameraSpeed * deltaTime);
		}

		public void BindViewTarget(NetRigidBody target)
		{
			TargetRigidBody = target;
		}

		public void ReleaseViewTarget()
		{
			TargetRigidBody = null;
		}

		public override string ToString() => Username;
	}
}
