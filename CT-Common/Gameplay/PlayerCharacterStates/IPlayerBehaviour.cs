using System.Numerics;
using CT.Common.Gameplay.Players;

namespace CT.Common.Gameplay.PlayerCharacterStates
{
	public interface IPlayerBehaviour
	{
		public DokzaAnimationState AnimationState { get; set; }
		public ProxyDirection ProxyDirection { get; set; }
		public Vector2 MoveDirection { get; set; }
		public float AnimationTime { get; set; }

		public void StopMove();
		public void Move(Vector2 moveDirection, bool isWalk);

		public void OnAnimationChanged(DokzaAnimationState state);
		public void OnAnimationChanged(DokzaAnimationState state, ProxyDirection direction);
		public void OnProxyDirectionChanged(ProxyDirection direction);
	}
}
