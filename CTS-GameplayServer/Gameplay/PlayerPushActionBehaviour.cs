using System.Numerics;
using CT.Common.Gameplay;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Gameplay
{
	public static class PlayerPushActionBehaviour
	{
		/// <summary>
		/// 밀치기 행동을 실행합니다.
		/// 밀쳐지는 대상도 밀치기 수행중이라면 서로 밀쳐집니다.
		/// </summary>
		/// <param name="actor">수행자</param>
		/// <param name="other">대상자</param>
		public static void OnPushAction(PlayerCharacter actor, PlayerCharacter other)
		{
			if (other.StateMachine.CurrentState != other.StateMachine.KnockbackState)
			{
				Vector2 direction = Vector2.Normalize(other.Position - actor.Position);
				if (other.StateMachine.CurrentState == other.StateMachine.PushState)
				{
					actor.OnReactionBy(-direction);
				}
				other.OnReactionBy(direction);
				Vector2 hitPos = (actor.Position + other.Position) * 0.5f;
				actor.GameplayController.EffectController.Play(EffectType.HammerHit, hitPos, 2f);
			}
		}
	}
}
