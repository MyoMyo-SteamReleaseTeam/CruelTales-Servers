using System.Diagnostics;
using System.Numerics;
using CT.Common.Gameplay.Players;

namespace CT.Common.Gameplay.PlayerCharacterStates
{
	public class PlayerCharacterModel
	{
		public IPlayerBehaviour Player { get; private set; }
		public DokzaAnimationState AnimationState { get; private set; } = DokzaAnimationState.Idle;
		public float AnimationTime { get; private set; }
		public Vector2 ActionAxis { get; set; } = Vector2.Zero;
		public Vector2 MoveDirection { get; set; } = Vector2.Zero;
		public DokzaDirection CurrentDokzaDirection { get; private set; } = DokzaDirection.RightDown;

		public PlayerCharacterModel(IPlayerBehaviour player)
		{
			Player = player;
		}

		public void UpdateAnimation(DokzaAnimationState animationState)
		{
			AnimationState = animationState;
			AnimationTime = 0;
		}

		public void UpdateDokzaDirection(DokzaDirection direction)
		{
			CurrentDokzaDirection = direction;
		}

		public void UpdateDokzaDirection()
		{
			CurrentDokzaDirection = GetDokzaDirection();
		}

		public void Update(float deltaTime)
		{
			AnimationTime += deltaTime;
		}

		public DokzaDirection GetDokzaDirection()
		{
			DokzaDirection direction = DokzaDirection.None;

			if (MoveDirection.X < 0f)
			{
				direction |= DokzaDirection.Left;
			}
			else if (MoveDirection.X > 0f)
			{
				direction |= DokzaDirection.Right;
			}
			else
			{
				direction |= CurrentDokzaDirection.IsRight() ?
					DokzaDirection.Right : DokzaDirection.Left;
			}

			if (MoveDirection.Y > 0f)
			{
				direction |= DokzaDirection.Up;
			}
			else if (MoveDirection.Y < 0f)
			{
				direction |= DokzaDirection.Down;
			}
			else
			{
				direction |= CurrentDokzaDirection.IsUp() ?
					DokzaDirection.Up : DokzaDirection.Down;
			}

			Debug.Assert(!direction.IsAxisAligned());

			return direction;
		}
	}
}
