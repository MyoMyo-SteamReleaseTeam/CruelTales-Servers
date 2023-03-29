using BenchmarkDotNet.Attributes;
using CT.Network.Legacy;

namespace CT.Benchmark
{
	[MemoryDiagnoser]
	public class BitConvertBenchmark
	{
		private byte[] _buffer;
		private ulong _data = 124174812;

		private byte[] _writeBuffer;

		public BitConvertBenchmark()
		{
			_writeBuffer = new byte[8];
			_buffer = new byte[8];
			BinaryConverter_Legacy.WriteUInt64(_buffer, 0, _data);
		}

		[Benchmark]
		unsafe public void WriteViaPointer()
		{
			fixed (byte* ptr = _writeBuffer)
			{
				*(ulong*)(ptr + 0) = _data;
			}
		}

		[Benchmark]
		public void WriteViaBitShift()
		{
			BinaryConverter_Legacy.WriteUInt64(_writeBuffer, 0, _data);
		}

		[Benchmark]
		public void WriteViaBuffer()
		{
			Buffer.BlockCopy(BitConverter.GetBytes(_data), 0, _writeBuffer, 0, 8);
		}

		[Benchmark]
		unsafe public void ReadViaPointer()
		{
			fixed (byte* ptr = _writeBuffer)
			{
				var data = *(ulong*)(ptr + 0);
			}
		}

		[Benchmark]
		public void ReadViaBitShift()
		{
			BinaryConverter_Legacy.ReadUInt64(_buffer, 0, out var data);
		}

		[Benchmark]
		public void ReadViaBitConvert()
		{
			var data = BitConverter.ToUInt64(_buffer, 0);
		}
	}
}
