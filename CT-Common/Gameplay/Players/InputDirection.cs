using System.Runtime.CompilerServices;

namespace CT.Common.Gameplay.Players
{
	/// <summary>8방향 입력 enum flag입니다.</summary>
	public enum InputDirection : byte
	{
		Right = 0,
		Left = 1,
		Up = 2,
		Down = 3,

		RightDown = 4,
		LeftDown = 5,
		RightUp = 6,
		LeftUp = 7,
	}

	/// <summary>움직임 입력 타입입니다. 2비트입니다.</summary>
	public enum MovementInputType : byte
	{
		NoInput = 0,
		Stop = 1,
		Walk = 2,
		Run = 3,
	}

	public static class InputDirectionExtension
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsRight(this InputDirection direction)
		{
			return direction == InputDirection.Right || 
				direction == InputDirection.RightDown ||
				direction == InputDirection.RightUp;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsLeft(this InputDirection direction)
		{
			return direction == InputDirection.Left ||
				direction == InputDirection.LeftDown ||
				direction == InputDirection.LeftUp;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsUp(this InputDirection direction)
		{
			return direction == InputDirection.Up ||
				direction == InputDirection.RightUp ||
				direction == InputDirection.LeftUp;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsDown(this InputDirection direction)
		{
			return direction == InputDirection.Down ||
				direction == InputDirection.RightDown ||
				direction == InputDirection.LeftDown;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAxisAligned(this InputDirection direction)
		{
			return direction == InputDirection.Right ||
				   direction == InputDirection.Left ||
				   direction == InputDirection.Up ||
				   direction == InputDirection.Down;
		}
	}
}