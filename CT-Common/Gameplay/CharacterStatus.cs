using CT.Common.Gameplay.Players;

namespace CT.Common.Gameplay
{
	public class CharacterStatus
	{
		// Basic
		public float MoveSpeed;

		// Animation
		public DokzaAnimationState ActionAnimation;

		// Action gameplay
		public float ActionCoolTime;
		public float ActionRadius;

		// Action physics
		public float ActionPower;
		public float ActionFriction;
		public float ActionDuration;

		// Knockback physics
		public float KnockbackPower;
		public float KnockbackFriction;
		public float KnockbackDuration;

		public CharacterStatus()
		{
			
		}

		public CharacterStatus(CharacterStatus status)
		{
			SetBy(status);
		}

		public void SetBy(CharacterStatus status)
		{
			// Basic
			MoveSpeed = status.MoveSpeed;

			// Animation
			ActionAnimation = status.ActionAnimation;

			// Action gameplay
			ActionCoolTime = status.ActionCoolTime;
			ActionRadius = status.ActionRadius;

			// Action physics
			ActionPower = status.ActionPower;
			ActionFriction = status.ActionFriction;
			ActionDuration = status.ActionDuration;

			// Knockback physics
			KnockbackPower = status.KnockbackPower;
			KnockbackFriction = status.KnockbackFriction;
			KnockbackDuration = status.KnockbackDuration;
		}
	}
}
