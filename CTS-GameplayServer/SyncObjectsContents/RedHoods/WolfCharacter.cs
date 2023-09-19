using System.Numerics;
using CTS.Instance.Data;
using CTS.Instance.Gameplay.Events;
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

					if (other is NormalCharacter)
						continue;

					if (other is RedHoodCharacter redHood)
					{
						var curScene = GameplayManager.GameplayController.SceneController;
						var eventHandler = curScene as IWolfEventHandler;
						eventHandler?.OnWolfCatch(this, redHood);
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