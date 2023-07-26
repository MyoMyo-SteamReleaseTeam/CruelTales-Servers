using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using KaNet.Physics.RigidBodies;

namespace CTS.Instance.SyncObjects
{
	public partial class PlayerCharacter : MasterNetworkObject
	{
		public override VisibilityType Visibility => VisibilityType.View;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		public float Speed = 5.0f;

		public NetworkPlayer? NetworkPlayer { get; private set; }

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

		public partial void Client_InputMovement(NetworkPlayer player,
												 Vector2 direction, bool isWalk)
		{
			if (_userId != player.UserId)
				return;

			if (direction == Vector2.Zero)
			{
				RigidBody.ChangeVelocity(Vector2.Zero);
			}
			else
			{
				float speed = isWalk ? Speed * 0.5f : Speed;
				Vector2 velocity = Vector2.Normalize(direction) * speed;
				RigidBody.ChangeVelocity(velocity);
			}
		}

		public partial void Client_InputInteraction(NetworkPlayer player,
													NetworkIdentity target,
													Input_InteractType interactType)
		{
			if (!WorldManager.TryGetNetworkObject(target, out var interactObject))
			{
				return;
			}
		}

		public partial void Client_InputAction(NetworkPlayer player,
											   Input_PlayerAction actionType,
											   Vector2 direction)
		{

		}
	}
}
