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
		public ProxyDirection CurrentDokzaDirection { get; private set; } = ProxyDirection.RightDown;

		public PlayerCharacterModel(IPlayerBehaviour player)
		{
			Player = player;
		}

		public void UpdateAnimation(DokzaAnimationState animationState)
		{
			AnimationState = animationState;
			AnimationTime = 0;
		}

		public void UpdateDokzaDirection(ProxyDirection direction)
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

		public ProxyDirection GetDokzaDirection()
		{
			ProxyDirection direction = ProxyDirection.None;

			if (MoveDirection.X < 0f)
			{
				direction |= ProxyDirection.Left;
			}
			else if (MoveDirection.X > 0f)
			{
				direction |= ProxyDirection.Right;
			}
			else
			{
				direction |= CurrentDokzaDirection.IsRight() ?
					ProxyDirection.Right : ProxyDirection.Left;
			}

			if (MoveDirection.Y > 0f)
			{
				direction |= ProxyDirection.Up;
			}
			else if (MoveDirection.Y < 0f)
			{
				direction |= ProxyDirection.Down;
			}
			else
			{
				direction |= CurrentDokzaDirection.IsUp() ?
					ProxyDirection.Up : ProxyDirection.Down;
			}

			Debug.Assert(!direction.IsAxisAligned());

			return direction;
		}
	}
}
