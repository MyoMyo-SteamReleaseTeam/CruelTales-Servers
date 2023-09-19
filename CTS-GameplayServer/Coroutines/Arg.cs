using System.Numerics;
using System.Runtime.InteropServices;

namespace CTS.Instance.Coroutines
{
	/// <summary>
	/// 코루틴 함수의 인자로 사용되는 인자 Union 입니다.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct Arg
	{
		[FieldOffset(0)] public bool Bool = false;
		[FieldOffset(0)] public byte Byte = 0;
		[FieldOffset(0)] public sbyte SByte = 0;
		[FieldOffset(0)] public short Int16 = 0;
		[FieldOffset(0)] public ushort UInt16 = 0;
		[FieldOffset(0)] public int Int32 = 0;
		[FieldOffset(0)] public uint UInt32 = 0;
		[FieldOffset(0)] public long Int64 = 0;
		[FieldOffset(0)] public ulong UInt64 = 0;
		[FieldOffset(0)] public float Single = 0;
		[FieldOffset(0)] public double Double = 0;

		[FieldOffset(0)] public Vector2 Vector2 = new Vector2();

		public Arg(bool value) => Bool = value;
		public Arg(byte value) => Byte = value;
		public Arg(sbyte value) => SByte = value;
		public Arg(short value) => Int16 = value;
		public Arg(ushort value) => UInt16 = value;
		public Arg(int value) => Int32 = value;
		public Arg(uint value) => UInt32 = value;
		public Arg(long value) => Int64 = value;
		public Arg(ulong value) => UInt64 = value;
		public Arg(float value) => Single = value;
		public Arg(double value) => Double = value;

		public Arg(Vector2 value) => Vector2 = value;
	}
}
