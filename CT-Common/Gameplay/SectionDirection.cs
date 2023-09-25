using System;

namespace CT.Common.Gameplay
{
	[Serializable]
	public struct SectionDirection
	{
		public byte From;
		public byte To;

		public ushort GetCombinedValue()
		{
			return (ushort)(From << 8 | To);
		}

		public static void ParseTo(int value, out byte from, out byte to)
		{
			to = (byte)(value & 0b_1111_1111);
			from = (byte)(value >> 8);
		}
	}
}
