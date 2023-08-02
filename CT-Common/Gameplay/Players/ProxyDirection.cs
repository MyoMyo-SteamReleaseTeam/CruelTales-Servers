using System;
using System.Runtime.CompilerServices;

namespace CT.Common.Gameplay.Players
{
	[Flags]
	public enum ProxyDirection : byte
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

	public static class ProxyDirectionExtension
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsRight(this ProxyDirection direction)
		{
			return (direction & ProxyDirection.Right) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsLeft(this ProxyDirection direction)
		{
			return (direction & ProxyDirection.Left) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsUp(this ProxyDirection direction)
		{
			return (direction & ProxyDirection.Up) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsDown(this ProxyDirection direction)
		{
			return (direction & ProxyDirection.Down) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAxisAligned(this ProxyDirection direction)
		{
			return direction == ProxyDirection.Right ||
				   direction == ProxyDirection.Left ||
				   direction == ProxyDirection.Up ||
				   direction == ProxyDirection.Down;
		}
	}
}