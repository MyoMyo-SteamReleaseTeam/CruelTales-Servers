using System.Numerics;

namespace CT.Common.Gameplay.PlayerCharacterStates
{
	public enum InputEvent
	{
		Movement,
		ActionAxis,
		ActionBtn
	}

	public struct InputInfo
	{
		public InputEvent InputEvent;
		public Vector2 MoveDirection;
		public float Power;

		public bool IsWalk => Power <= 0.5f;
		public bool HasMovementInput => Power > 0;

		public InputInfo(InputEvent inputEvent, Vector2 moveDirection, float power)
		{
			InputEvent = inputEvent;
			MoveDirection = moveDirection;
			Power = power;
		}

		public InputInfo(InputEvent inputEvent, Vector2 moveDirection, bool isWalk)
		{
			InputEvent = inputEvent;
			MoveDirection = moveDirection;
			Power = isWalk ? 0.5f : 1;
		}

		public InputInfo(InputEvent inputEvent)
		{
			InputEvent = inputEvent;
			MoveDirection = Vector2.Zero;
			Power = 0;
		}

#if UNITY_2021
		public System.Numerics.Vector2 GetAxisToNativeVector2()
		{
			return Axis.ToNativeVector2();
		}
#elif NET
		public System.Numerics.Vector2 GetAxisToNativeVector2()
		{
			return MoveDirection;
		}
#endif
	}
}
