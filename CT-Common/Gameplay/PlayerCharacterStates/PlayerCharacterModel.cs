using System.Diagnostics;
using System.Numerics;
using CT.Common.Gameplay.Players;

namespace CT.Common.Gameplay.PlayerCharacterStates
{
	public class PlayerCharacterModel
	{
		public IPlayerBehaviour Player { get; private set; }
		public Vector2 ActionAxis { get; set; } = Vector2.Zero;

		public PlayerCharacterModel(IPlayerBehaviour player)
		{
			Player = player;
		}

		/*
		public void UpdateAnimation(DokzaAnimationState animationState)
		{
			Player.AnimationState = animationState;
			Player.AnimationTime = 0.0f;
			Player.OnAnimationChanged(Player.AnimationState);
		}
		*/

		/// <summary>
		/// Dokza의 Animation을 업데이트하고, 클라이언트에게 요청합니다.
		/// </summary>
		/// <param name="animationState"></param>
		/// <param name="proxyDirection"></param>
		public void UpdateAnimation(DokzaAnimationState animationState, ProxyDirection proxyDirection)
		{
			Player.AnimationState = animationState;
			Player.AnimationTime = 0.0f;
			Player.OnAnimationChanged(animationState, proxyDirection);
		}
		
		/// <summary>
		/// 서버의 ProxyDirection만을 수정합니다.
		/// </summary>
		/// <param name="direction"></param>
		public void UpdateProxyDirectionOnly(ProxyDirection direction)
		{
			Player.ProxyDirection = direction;
		}

		/// <summary>
		/// 서버의 MoveDirection만을 수정합니다.
		/// </summary>
		/// <param name="moveDirection"></param>
		public void UpdateMoveDirectionOnly(Vector2 moveDirection)
		{
			Player.MoveDirection = moveDirection;
		}
		
		/// <summary>
		/// Player.MoveDirection을 기반으로 서버의 ProxyDirection만을 변경합니다.
		/// </summary>
		/// <returns>ProxyDirection의 변경 여부를 전달합니다.</returns>
		public bool UpdateProxyDirectionByMoveDirection()
		{
			ProxyDirection direction = ProxyDirection.None;

			if (Player.MoveDirection.X < 0f)
			{
				direction |= ProxyDirection.Left;
			}
			else if (Player.MoveDirection.X > 0f)
			{
				direction |= ProxyDirection.Right;
			}
			else
			{
				direction |= Player.ProxyDirection.IsRight() ?
					ProxyDirection.Right : ProxyDirection.Left;
			}

			if (Player.MoveDirection.Y > 0f)
			{
				direction |= ProxyDirection.Up;
			}
			else if (Player.MoveDirection.Y < 0f)
			{
				direction |= ProxyDirection.Down;
			}
			else
			{
				direction |= Player.ProxyDirection.IsUp() ?
					ProxyDirection.Up : ProxyDirection.Down;
			}

			Debug.Assert(!direction.IsAxisAligned());

			if (direction == Player.ProxyDirection)
			{
				return false;
			}
			
			Player.ProxyDirection = direction;
			return true;
		}

		/// <summary>
		/// 서버의 현재 ProxyDirection을 강제로 앞을 보게 설정합니다.
		/// </summary>
		public void UpdateProxyDirectionToFront()
		{
			if (Player.ProxyDirection == ProxyDirection.LeftUp)
				Player.ProxyDirection = ProxyDirection.LeftDown;
			else if (Player.ProxyDirection == ProxyDirection.RightUp)
				Player.ProxyDirection = ProxyDirection.RightDown;
		}
		
		/// <summary>
		/// 클라이언트에게 현재 서버의 ProxyDirection을 적용시킵니다.
		/// </summary>
		public void SendProxyDirection()
		{
			Player.OnProxyDirectionChanged(Player.ProxyDirection);
		}
		
		/// <summary>
		/// MoveDirection을 기반으로 서버의 ProxyDirection만을 변경합니다.
		/// </summary>
		/// <param name="moveDirection"></param>
		public void UpdateMoveDirectionWithProxy(Vector2 moveDirection)
		{
			if (moveDirection == Player.MoveDirection)
				return;
			
			Player.MoveDirection = moveDirection;
			UpdateProxyDirectionByMoveDirection();
		}
		
		public void Update(float deltaTime)
		{
			Player.AnimationTime += deltaTime;
		}
	}
}
