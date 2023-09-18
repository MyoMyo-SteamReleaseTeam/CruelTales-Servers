using System.Numerics;
using CTS.Instance.Data;
using KaNet.Physics;

namespace CTS.Instance.SyncObjects
{
	public partial class WolfCharacter : PlayerCharacter
	{
		public override void OnCreated()
		{
			base.OnCreated();
			if (RoomSessionManager.PlayerStateTable.TryGetValue(UserId, out var state))
			{
				state.CurrentSkin = SkinSetDataDB.WOLF_SKIN_SET;
			}
		}

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
						
						var otherCreatedCharacter = other.ChangePlayerTypeTo<WolfCharacter>();
						otherCreatedCharacter.StateMachine.ChangeState(otherCreatedCharacter.StateMachine.IdleState);
						otherCreatedCharacter.LoadDefaultPlayerSkin();
						
						var thisCreatedCharacter = this.ChangePlayerTypeTo<PlayerCharacter>();
						thisCreatedCharacter.StateMachine.ChangeState(thisCreatedCharacter.StateMachine.IdleState);
						thisCreatedCharacter.LoadDefaultPlayerSkin();
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

		public override void LoadDefaultPlayerSkin()
		{
			BroadcastOrderTest((int)UserId.Id, 1);
		}
	}
}