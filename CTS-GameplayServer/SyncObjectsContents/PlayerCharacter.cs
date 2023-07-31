using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Gameplay.PlayerCharacterStates;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class PlayerCharacter : MasterNetworkObject, IPlayerBehaviour
	{
		public override VisibilityType Visibility => VisibilityType.View;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		public float Speed = 5.0f;

		public NetworkPlayer? NetworkPlayer { get; private set; }

		// States
		private PlayerCharacterModel PlayerModel;
		private PlayerCharacterStateMachine StateMachine;

		public PlayerCharacter() : base()
		{
			PlayerModel = new(this);
			StateMachine = new(PlayerModel);
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

		public void UpdateRigid(Vector2 moveDirection, bool isWalk)
		{
			float speed = isWalk ? Speed * 0.5f : Speed;
			Vector2 velocity = Vector2.Normalize(moveDirection) * speed;
			RigidBody.ChangeVelocity(velocity);
		}

		public void UpdateRigidStop()
		{
			RigidBody.ChangeVelocity(Vector2.Zero);
		}

		#region Sync

		public partial void Client_InputMovement(NetworkPlayer player,
												 Vector2 direction, bool isWalk)
		{
			if (_userId != player.UserId)
				return;

			InputInfo info;
			if (direction == Vector2.Zero)
			{
				info = new InputInfo(InputEvent.Movement);
			}
			else
			{
				info = new InputInfo(InputEvent.Movement, direction, isWalk);
			}

			StateMachine.OnInputEvent(info);
		}

		#endregion
	}
}
