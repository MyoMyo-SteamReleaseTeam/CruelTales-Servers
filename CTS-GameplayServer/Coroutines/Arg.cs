using System.Runtime.InteropServices;

namespace CTS.Instance.Coroutines
{
	/// <summary>
	/// 코루틴 함수의 인자로 사용되는 인자 Union 입니다.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct Arg
	{
		[FieldOffset(0)] public byte Byte = 0;
		//[FieldOffset(0)] public sbyte SByte = 0;
		[FieldOffset(0)] public short Short = 0;
		//[FieldOffset(0)] public ushort UShort = 0;
		[FieldOffset(0)] public int Int = 0;
		//[FieldOffset(0)] public uint UInt = 0;
		[FieldOffset(0)] public long Long = 0;
		//[FieldOffset(0)] public ulong ULong = 0;
		[FieldOffset(0)] public float Float = 0;
		[FieldOffset(0)] public double Double = 0;

		public Arg(byte value) => Byte = value;
		//public ArgUnion(sbyte value) => SByte = value;
		public Arg(short value) => Short = value;
		//public ArgUnion(ushort value) => UShort = value;
		public Arg(int value) => Int = value;
		//public ArgUnion(uint value) => UInt = value;
		public Arg(long value) => Long = value;
		//public ArgUnion(ulong value) => ULong = value;
		public Arg(float value) => Float = value;
		public Arg(double value) => Double = value;
	}
}
