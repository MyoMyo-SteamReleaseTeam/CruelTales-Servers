using KaNet.Physics;
using System.Numerics;

namespace CTS.Instance.SyncObjects
{
	public partial class WolfCharacter : PlayerCharacter
	{
		public override void OnDuringAction()
		{
			if (PhysicsWorld.Raycast(RigidBody.Position,
				    ActionRadius, out var hits,
				    PhysicsLayerMask.Player))
			{
				foreach (var id in hits)
				{
					if (Identity.Id == id )
						continue;

					if (!WorldManager.TryGetNetworkObject(new(id), out var netObj))
						continue;

					if (netObj is not PlayerCharacter other)
						continue;

					if (other is not WolfCharacter)
					{
						other.Stop();
						other.ResetImpluse();
						this.Stop();
						this.ResetImpluse();
						
						other.ChangePlayerTypeTo<WolfCharacter>();
						other.StateMachine.ChangeState(other.StateMachine.IdleState);
						this.ChangePlayerTypeTo<PlayerCharacter>();
						this.StateMachine.ChangeState(this.StateMachine.IdleState);
					}
					else
					{
						Vector2 direction = Vector2.Normalize(other.Position - Position);
						other.OnReactionBy(direction);
						this.OnReactionBy(-direction);
					}
					
					break;
				}
			}
		}
	}
}