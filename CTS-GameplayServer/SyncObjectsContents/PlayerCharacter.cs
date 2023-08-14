using System.Numerics;
using CT.Common.DataType;
using CT.Common.DataType.Input;
using CT.Common.Gameplay.PlayerCharacterStates;
using CT.Common.Gameplay.Players;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using KaNet.Physics;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class PlayerCharacter : MasterNetworkObject, IPlayerBehaviour
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(PlayerCharacter));

		public override VisibilityType Visibility => VisibilityType.View;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		public float Speed = 5.0f;

		public NetworkPlayer? NetworkPlayer { get; private set; }

		// States
		private PlayerCharacterModel PlayerModel;
		private PlayerCharacterStateMachine StateMachine;

		#region Getter Setter

		public DokzaAnimationState AnimationState
		{
			get => _animationState;
			set => _animationState = value;
		}

		public ProxyDirection ProxyDirection
		{
			get => _proxyDirection;
			set => _proxyDirection = value;
		}

		public Vector2 MoveDirection
		{
			get;
			set;
		}

		public float AnimationTime
		{
			get => _animationTime;
			set => _animationTime = value;
		}

		#endregion

		public PlayerCharacter() : base()
		{
			PlayerModel = new(this);
			StateMachine = new(PlayerModel);
		}

		public override void OnCreated()
		{
			_animationState = DokzaAnimationState.Idle;
			_proxyDirection = ProxyDirection.RightDown;
		}

		public void BindNetworkPlayer(NetworkPlayer player)
		{
			this.UserId = player.UserId;
			this.Username = player.Username;
			NetworkPlayer = player;
			NetworkPlayer.BindViewTarget(RigidBody);
		}

		public override void OnDestroyed()
		{
			NetworkPlayer?.ReleaseViewTarget();
		}

		public void Move(Vector2 moveDirection, bool isWalk)
		{
			if (KaPhysics.NearlyEqual(moveDirection.Length(), 0))
			{
				RigidBody.ChangeVelocity(moveDirection);
				return;
			}

			float speed = isWalk ? Speed * 0.5f : Speed;
			Vector2 velocity = Vector2.Normalize(moveDirection) * speed;
			RigidBody.ChangeVelocity(velocity);
		}

		public void StopMove()
		{
			RigidBody.ChangeVelocity(Vector2.Zero);
		}

		#region Sync

		public void OnAnimationChanged(DokzaAnimationState state)
		{
			this.Server_OnAnimationChanged(state);
		}

		public void OnAnimationChanged(DokzaAnimationState state, ProxyDirection direction)
		{
			this.Server_OnAnimationChanged(state, direction);
		}

		public void OnProxyDirectionChanged(ProxyDirection direction)
		{
			this.Server_OnProxyDirectionChanged(direction);
		}

		public partial void Client_RequestInput(NetworkPlayer player, InputData inputData)
		{
			if (_userId != player.UserId)
			{
				player.Session?.Disconnect(DisconnectReasonType.ServerError_UnauthorizedBehaviour);
				return;
			}

			StateMachine.OnInputEvent(inputData);
		}

		#endregion
	}
}
