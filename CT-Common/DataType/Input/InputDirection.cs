using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CT.Common.DataType.Input
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

	public static class InputDirectionExtension
	{
		public const float INV_SNAP_RAD = 1.0f / (MathF.PI * 0.5f / 8);

		private static readonly Vector2[] _inputDirectionTable = new Vector2[]
		{
			new Vector2(1, 0),
			Vector2.Normalize(new Vector2(1, 1)),
			new Vector2(0, 1),
			Vector2.Normalize(new Vector2(-1, 1)),
			new Vector2(-1, 0),
			Vector2.Normalize(new Vector2(-1, -1)),
			new Vector2(0, -1),
			Vector2.Normalize(new Vector2(1, -1)),
		};

		/// <summary>입력 방향을 방향 벡터로 변환합니다.</summary>
		public static Vector2 ToDirectionVector(this InputDirection inputDirection)
		{
			return _inputDirectionTable[(int)inputDirection];
		}

		/// <summary>방향 벡터를 8방향 입력 enum flag로 변환합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InputDirection ToInputDirection(this Vector2 vec)
		{
			return vec.Y >= 0 ?
				(InputDirection)((MathF.Acos(vec.X) * INV_SNAP_RAD + 1) * 0.5f) :
				(InputDirection)((int)((MathF.Acos(-vec.X) * INV_SNAP_RAD + 9) * 0.5f) % 8);
		}

#if UNITY_2021
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
		
		/// <summary>입력 방향을 방향 벡터로 변환합니다.</summary>
		public static UnityEngine.Vector2 ToUnityDirectionVector(this InputDirection inputDirection)
		{
			return _inputDirectionTableUnity[(int)inputDirection];
		}

		/// <summary>방향 벡터를 8방향 입력 enum flag로 변환합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InputDirection ToInputDirection(this UnityEngine.Vector2 vec)
		{
			return vec.y >= 0 ?
				(InputDirection)((MathF.Acos(vec.x) * INV_SNAP_RAD + 1) * 0.5f) :
				(InputDirection)((int)((MathF.Acos(-vec.x) * INV_SNAP_RAD + 9) * 0.5f) % 8);
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

		/// <summary>축에 수직하거나 수평하는 방향인지 여부입니다.</summary>
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