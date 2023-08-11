using System.Diagnostics;
using System.Numerics;
using CT.Common.Gameplay.Players;

namespace CT.Common.Gameplay.PlayerCharacterStates
{
	public class PlayerCharacterModel
	{
		public IPlayerBehaviour Player { get; private set; }
		public Vector2 ActionAxis { get; set; } = Vector2.Zero;
		public Vector2 MoveDirection { get; set; } = Vector2.Zero;

		public PlayerCharacterModel(IPlayerBehaviour player)
		{
			Player = player;
		}

		public void UpdateAnimation(DokzaAnimationState animationState)
		{
			Player.AnimationState = animationState;
			Player.AnimationTime = 0.0f;
			Player.OnAnimationChanged(Player.AnimationState);
		}

		public void UpdateDokzaDirection(ProxyDirection direction)
		{
			Player.ProxyDirection = direction;
			Player.OnProxyDirectionChanged(Player.ProxyDirection);
		}

		public void UpdateDokzaDirection()
		{
			Player.ProxyDirection = GetProxyDirectionByDokza();
			Player.OnProxyDirectionChanged(Player.ProxyDirection);
		}

		public void Update(float deltaTime)
		{
			Player.AnimationTime += deltaTime;
		}

		public ProxyDirection GetProxyDirectionByDokza()
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
				direction |= Player.ProxyDirection.IsRight() ?
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
				direction |= Player.ProxyDirection.IsUp() ?
					ProxyDirection.Up : ProxyDirection.Down;
			}

			Debug.Assert(!direction.IsAxisAligned());

			return direction;
		}
	}
}
