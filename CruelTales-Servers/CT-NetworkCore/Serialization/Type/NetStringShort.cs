using System.Diagnostics;
using System.Text;

namespace CT.Network.Serialization.Type
{
	/// <summary>65536이하 길이의 string 입니다.</summary>
	public readonly struct NetString
	{
		public readonly string Value;
		public const int MAX_BYTE_LENGTH = 65536;

		public readonly int ByteSize = 0;
		public int DataSize => ByteSize + 2;

		public static implicit operator string(NetString value) => value.Value;
		public static implicit operator NetString(string value) => new NetString(value);

		public NetString(string value)
		{
			ByteSize = Encoding.UTF8.GetByteCount(value);
			Debug.Assert(ByteSize <= MAX_BYTE_LENGTH);
			Value = value;
		}

		public override string ToString() => Value;
	}

	/// <summary>256이하 길이의 string 입니다.</summary>
	public readonly struct NetStringShort
	{
		public readonly string Value;
		public const int MAX_BYTE_LENGTH = 256;

		public readonly int ByteSize = 0;
		public int DataSize => ByteSize + 1;

		public static implicit operator string(NetStringShort value) => value.Value;
		public static implicit operator NetStringShort(string value) => new NetStringShort(value);

		public NetStringShort(string value)
		{
			ByteSize = Encoding.UTF8.GetByteCount(value);
			Debug.Assert(ByteSize <= MAX_BYTE_LENGTH);
			Value = value;
		}

		public override string ToString() => Value;
	}
}
