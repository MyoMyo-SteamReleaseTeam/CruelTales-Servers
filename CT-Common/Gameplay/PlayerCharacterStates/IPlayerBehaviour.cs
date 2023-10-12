using System.Numerics;
using CT.Common.Gameplay.Players;

namespace CT.Common.Gameplay.PlayerCharacterStates
{
	public interface IPlayerBehaviour
	{
		public DokzaAnimationState AnimationState { get; set; }
		public ProxyDirection ProxyDirection { get; set; }
		public Vector2 MoveDirection { get; set; }
		public Vector2 ActionDirection { get; set; }
		public float AnimationTime { get; set; }

		public CharacterStatus Status { get; }

		public void Physics_Stop();
		public void Physics_Move(Vector2 moveDirection, bool isWalk);
		public void Physics_Impluse(Vector2 forceDirection, float power, float forceFriction);
		public void Physics_ResetImpluse();
		
		public void OnAnimationChanged(DokzaAnimationState state);
		public void OnAnimationChanged(DokzaAnimationState state, ProxyDirection direction);
		public void OnProxyDirectionChanged(ProxyDirection direction);
		public void OnDuringAction();
		public void OnKnockbacked(Vector2 pushedDirection);
	}
}
