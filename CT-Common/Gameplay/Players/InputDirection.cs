using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CT.Common.Gameplay.Players
{
	/// <summary>8방향 입력 enum flag입니다.</summary>
	public enum InputDirection : byte
	{
		Right = 0,
		RightUp = 1,
		Up = 2,
		LeftUp = 3,
		Left = 4,
		LeftDown = 5,
		Down = 6,
		RightDown = 7,
	}

	/// <summary>움직임 입력 타입입니다. 2비트입니다.</summary>
	public enum MovementInputType : byte
	{
		/// <summary>입력이 존재하지 않습니다.</summary>
		NoInput = 0,
		Stop = 1,
		Walk = 2,
		Run = 3,
	}

	public static class InputDirectionExtension
	{
		public const float INV_SNAP_RAD = 1.0f / (MathF.PI * 0.5f / 8);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InputDirection ToInputDirection(this System.Numerics.Vector2 vec)
		{
			return vec.Y >= 0 ?
				(InputDirection)((MathF.Acos(vec.X) * INV_SNAP_RAD + 1) * 0.5f) :
				(InputDirection)((int)((MathF.Acos(-vec.X) * INV_SNAP_RAD + 9) * 0.5f) % 8);
		}

		private static readonly System.Numerics.Vector2[] _inputDirectionTable = new System.Numerics.Vector2[]
		{
			new System.Numerics.Vector2(1, 0),
			System.Numerics.Vector2.Normalize(new System.Numerics.Vector2(1, 1)),
			new Vector2(0, 1),
			System.Numerics.Vector2.Normalize(new System.Numerics.Vector2(-1, 1)),
			new Vector2(-1, 0),
			System.Numerics.Vector2.Normalize(new System.Numerics.Vector2(-1, -1)),
			new Vector2(0, -1),
			System.Numerics.Vector2.Normalize(new System.Numerics.Vector2(1, -1)),
		};

		public static System.Numerics.Vector2 ToDirectionVector(this InputDirection inputDirection)
		{
			return _inputDirectionTable[(int)inputDirection];
		}

#if UNITY_2021
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InputDirection ToInputDirection(this UnityEngine.Vector2 vec)
		{
			return vec.y >= 0 ?
				(InputDirection)((MathF.Acos(vec.x) * INV_SNAP_RAD + 1) * 0.5f) :
				(InputDirection)((int)((MathF.Acos(-vec.x) * INV_SNAP_RAD + 9) * 0.5f) % 8);
		}

		private static readonly UnityEngine.Vector2[] _inputDirectionTableUnity = new UnityEngine.Vector2[]
		{
			new UnityEngine.Vector2(1, 0),
			new UnityEngine.Vector2(1, 1).normalized,
			new UnityEngine.Vector2(0, 1),
			new UnityEngine.Vector2(-1, 1).normalized,
			new UnityEngine.Vector2(-1, 0),
			new UnityEngine.Vector2(-1, -1).normalized,
			new UnityEngine.Vector2(0, -1),
			new UnityEngine.Vector2(1, -1).normalized,
		};
		
		public static UnityEngine.Vector2 ToUnityDirectionVector(this InputDirection inputDirection)
		{
			return _inputDirectionTableUnity[(int)inputDirection];
		}
#endif

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