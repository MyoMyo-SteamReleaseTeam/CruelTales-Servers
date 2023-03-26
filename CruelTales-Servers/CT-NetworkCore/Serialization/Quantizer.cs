namespace CT.Network.Serialization
{
	public static class Quantizer
	{
		public const int POS_X_QUANTIZE_RANGE = 65536;
		public const int POS_Y_QUANTIZE_RANGE = 65536;
		public const int POS_Z_QUANTIZE_RANGE = 32768;

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
	}
}
