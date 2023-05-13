using System;

#if NET
using System.Numerics;
#elif UNITY_2021
using UnityEngine;
#endif

using System.Runtime.CompilerServices;

namespace CT.Common.Quantization
{
	public static class MathExtension
	{
		public const float RADIAN_TO_DEGREE = 180.0f / MathF.PI;
		public const float DEGREE_TO_RADIAN = MathF.PI / 180.0f;
		public const float RADIAN = 180 / MathF.PI;

		public static float DegreeToRadian(float angle)
		{
			return angle * RADIAN_TO_DEGREE;
		}

		public static float RadianToDegree(float angle)
		{
			return angle * DEGREE_TO_RADIAN;
		}
	}

	public static class Quantizer
	{
		public const int PRECISION = 6;

		public const float INVERSE_PI = 1 / MathF.PI;
		public const float QUANTIZE_RAD = INVERSE_PI * 128;
		public const float REMAP_RAD_INVERSE = MathF.PI / 128.0f;

		public static byte QuantizeFloatToInt8(float value, float maxRange)
		{
			return (byte)(value / maxRange * 256);
		}

		public static float DequantizeFloatFromInt8(byte value, float maxRange)
		{
			return value / 256F * maxRange;
		}

		public static short QuantizeFloatToShort(float value, float maxRange)
		{
			return (short)(maxRange / value + maxRange * 0.5f);
		}

		public static float DequantizeFloatFromShort(short value, float maxRange)
		{
			float range = maxRange * 0.5f;
			return 1 / range * value - range;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte Vec2ToRadByte(Vector2 vec2)
		{
#if NET
			return vec2.Y >= 0 ?
				(byte)(MathF.Acos(vec2.X) * QUANTIZE_RAD) :
				(byte)(MathF.Acos(-vec2.X) * QUANTIZE_RAD + 128);
#elif UNITY_2021
			return vec2.y >= 0 ?
				(byte)(MathF.Acos(vec2.x) * QUANTIZE_RAD) : 
				(byte)(MathF.Acos(-vec2.x) * QUANTIZE_RAD + 128);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 RadByteToVec2(byte radByte)
		{
			if (radByte <= 128)
			{
				float x = MathF.Round(MathF.Cos(radByte * REMAP_RAD_INVERSE), PRECISION);
				float y = MathF.Round(MathF.Sqrt(1 - x * x), PRECISION);
				return new Vector2(x, y);
			}
			else
			{
				radByte -= 128;
				float x = MathF.Round(MathF.Cos(radByte * REMAP_RAD_INVERSE), PRECISION);
				float y = MathF.Round(MathF.Sqrt(1 - x * x), PRECISION);
				return new Vector2(-x, -y);
			}
		}
	}
}
