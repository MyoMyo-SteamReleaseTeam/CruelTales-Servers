using System;
using System.Runtime.CompilerServices;

namespace CT.Common.Gameplay.Players
{
	[Flags]
	public enum DokzaDirection : byte
	{
		None = 0,

		Right = 1,
		Left = 1 << 1,
		Up = 1 << 2,
		Down = 1 << 3,

		RightDown = Right | Down,
		LeftDown = Left | Down,
		RightUp = Right | Up,
		LeftUp = Left | Up,
	}

	public static class DokzaDirectionExtension
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsRight(this DokzaDirection direction)
		{
			return (direction & DokzaDirection.Right) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsLeft(this DokzaDirection direction)
		{
			return (direction & DokzaDirection.Left) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsUp(this DokzaDirection direction)
		{
			return (direction & DokzaDirection.Up) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsDown(this DokzaDirection direction)
		{
			return (direction & DokzaDirection.Down) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAxisAligned(this DokzaDirection direction)
		{
			return direction == DokzaDirection.Right ||
				   direction == DokzaDirection.Left ||
				   direction == DokzaDirection.Up ||
				   direction == DokzaDirection.Down;
		}
	}
}