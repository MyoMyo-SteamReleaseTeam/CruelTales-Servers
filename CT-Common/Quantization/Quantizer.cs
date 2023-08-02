using System;
using System.Runtime.CompilerServices;

namespace CT.Common.Quantization
{
	public static class MathExtension
	{
		public static float DegreeToRadian(float angle)
		{
			return angle * KaMath.RADIAN_TO_DEGREE;
		}

		public static float RadianToDegree(float angle)
		{
			return angle * KaMath.DEGREE_TO_RADIAN;
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
		public static byte Vec2ToRadByte(System.Numerics.Vector2 vec2)
		{
			return vec2.Y >= 0 ?
				(byte)(MathF.Acos(vec2.X) * QUANTIZE_RAD) :
				(byte)(MathF.Acos(-vec2.X) * QUANTIZE_RAD + 128);
		}

#if UNITY_2021
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte Vec2ToRadByte(UnityEngine.Vector2 vec2)
		{
			return vec2.y >= 0 ?
				(byte)(MathF.Acos(vec2.x) * QUANTIZE_RAD) : 
				(byte)(MathF.Acos(-vec2.x) * QUANTIZE_RAD + 128);
		}
#endif

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
