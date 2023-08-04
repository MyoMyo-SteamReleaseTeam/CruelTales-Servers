using System;
using System.Runtime.CompilerServices;

namespace CT.Common.Quantization
{
	/// <summary>데이터 양자화를 위한 클래스입니다.</summary>
	public static class Quantizer
	{
		/// <summary>소수점 정밀도입니다.</summary>
		public const int PRECISION = 6;

		public const float INVERSE_PI = 1 / MathF.PI;

		/// <summary>1 바이트 양자화 정밀도입니다.</summary>
		public const float QUANTIZE_RAD = INVERSE_PI * 128;

		/// <summary>1 바이트 역양자화 정밀도의 역수입니다.</summary>
		public const float REMAP_RAD_INVERSE = MathF.PI / 128.0f;

		/// <summary>float를 1 바이트로 양자화합니다.</summary>
		/// <param name="value">값</param>
		/// <param name="maxRange">0 부터의 최대 범위</param>
		/// <returns>양자화된 값</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte QuantizeFloatToInt8(float value, float maxRange)
		{
			return (byte)(value / maxRange * 256);
		}

		/// <summary>1 바이트를 float로 역양자화합니다.</summary>
		/// <param name="value">값</param>
		/// <param name="maxRange">0 부터의 최대 범위</param>
		/// <returns>역양자화된 값</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DequantizeFloatFromInt8(byte value, float maxRange)
		{
			return value / 256F * maxRange;
		}

		/// <summary>float를 2 바이트로 양자화합니다.</summary>
		/// <param name="value">값</param>
		/// <param name="maxRange">0 부터의 최대 범위</param>
		/// <returns>양자화된 값</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short QuantizeFloatToShort(float value, float maxRange)
		{
			return (short)(maxRange / value + maxRange * 0.5f);
		}

		/// <summary>2 바이트를 float로 역양자화합니다.</summary>
		/// <param name="value">값</param>
		/// <param name="maxRange">0 부터의 최대 범위</param>
		/// <returns>역양자화된 값</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DequantizeFloatFromShort(short value, float maxRange)
		{
			float range = maxRange * 0.5f;
			return 1 / range * value - range;
		}

		/// <summary>방향 벡터를 1 바이트로 양자화합니다.</summary>
		/// <param name="vec2">방향 벡터</param>
		/// <returns>1 바이트로 양자화된 방향 벡터</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte Vector2ToByte(System.Numerics.Vector2 vec2)
		{
			return vec2.Y >= 0 ?
				(byte)(MathF.Acos(vec2.X) * QUANTIZE_RAD) :
				(byte)(MathF.Acos(-vec2.X) * QUANTIZE_RAD + 128);
		}

#if UNITY_2021
		/// <summary>방향 벡터를 1 바이트로 양자화합니다.</summary>
		/// <param name="vec2">방향 벡터</param>
		/// <returns>1 바이트로 양자화된 방향 벡터</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte Vector2ToByte(UnityEngine.Vector2 vec2)
		{
			return vec2.y >= 0 ?
				(byte)(MathF.Acos(vec2.x) * QUANTIZE_RAD) : 
				(byte)(MathF.Acos(-vec2.x) * QUANTIZE_RAD + 128);
		}
#endif

		/// <summary>1 바이트를 방향벡터로 역양자화합니다.</summary>
		/// <param name="vec2">양자화된 1 바이트 방향 벡터</param>
		/// <returns>역양자화된 방향 벡터</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Numerics.Vector2 RadByteToVec2(byte radByte)
		{
			if (radByte <= 128)
			{
				float x = MathF.Round(MathF.Cos(radByte * REMAP_RAD_INVERSE), PRECISION);
				float y = MathF.Round(MathF.Sqrt(1 - x * x), PRECISION);
				return new System.Numerics.Vector2(x, y);
			}
			else
			{
				radByte -= 128;
				float x = MathF.Round(MathF.Cos(radByte * REMAP_RAD_INVERSE), PRECISION);
				float y = MathF.Round(MathF.Sqrt(1 - x * x), PRECISION);
				return new System.Numerics.Vector2(-x, -y);
			}
		}

#if UNITY_2021
		/// <summary>1 바이트를 방향벡터로 역양자화합니다.</summary>
		/// <param name="vec2">양자화된 1 바이트 방향 벡터</param>
		/// <returns>역양자화된 방향 벡터</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UnityEngine.Vector2 RadByteToUnityVec2(byte radByte)
		{
			if (radByte <= 128)
			{
				float x = MathF.Round(MathF.Cos(radByte * REMAP_RAD_INVERSE), PRECISION);
				float y = MathF.Round(MathF.Sqrt(1 - x * x), PRECISION);
				return new UnityEngine.Vector2(x, y);
			}
			else
			{
				radByte -= 128;
				float x = MathF.Round(MathF.Cos(radByte * REMAP_RAD_INVERSE), PRECISION);
				float y = MathF.Round(MathF.Sqrt(1 - x * x), PRECISION);
				return new UnityEngine.Vector2(-x, -y);
			}
		}
#endif
	}
}
