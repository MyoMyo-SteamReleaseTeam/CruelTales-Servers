using System.Text;
using BenchmarkDotNet.Attributes;
using CT.Common.Serialization;
using CT.Network.Legacy;

namespace CT.Benchmark
{
	[MemoryDiagnoser]
	public class StringSerializationBenchmark
	{
		public readonly string LongData = "우는 뛰노는 어디 이것이야말로 것은 그러므로 운다. 작고 있는 이상은 이것을 이는 것이다. 스며들어 것은 없는 대중을 수 얼음에 넣는 것이다. 위하여, 목숨을 싶이 이것이다. 못할 있는 시들어 부패뿐이다. 충분히 장식하는 그들에게 있는 길을 맺어, 철환하였는가? 바이며, 이상을 능히 인류의 없으면, 새가 오아이스도 충분히 되려니와, 것이다. 군영과 예가 꽃 미묘한 찬미를 아니다. 눈이 그들의 붙잡아 가치를 영락과 곳으로 희망의 보라. 투명하되 청춘 가진 군영과 우리의 노년에게서 용감하고 사는가 이것이다. 구하기 보내는 미인을 할지라도 황금시대다.";

		public readonly string ShortData = "우는 뛰노는";

		private byte[] _buffer;

		public StringSerializationBenchmark()
		{
			_buffer = new byte[1024];
		}

		[Benchmark]
		public void LengthUnchecked_Short() => LengthUnchecked(ShortData);

		[Benchmark]
		public void LengthCheck_Short() => LengthCheck(ShortData);

		[Benchmark]
		public void BytesCopy_Short() => BytesCopy(ShortData);

		[Benchmark]
		public void LengthUnchecked_Long() => LengthUnchecked(LongData);

		[Benchmark]
		public void LengthCheck_Long() => LengthCheck(LongData);

		[Benchmark]
		public void BytesCopy_Long() => BytesCopy(LongData);

		public void LengthUnchecked(string data)
		{
			var byteSize = Encoding.UTF8.GetBytes(data, 0, data.Length, _buffer, 2);
			BinaryConverter.WriteUInt16(_buffer, 0, (ushort)byteSize);
		}

		public void LengthCheck(string data)
		{
			var byteSize = Encoding.UTF8.GetByteCount(data);
			BinaryConverter.WriteUInt16(_buffer, 0, (ushort)byteSize);
			Encoding.UTF8.GetBytes(data, 0, data.Length, _buffer, 2);
		}

		public void BytesCopy(string data)
		{
			var rawData = Encoding.UTF8.GetBytes(data);
			BinaryConverter.WriteUInt16(_buffer, 0, (ushort)rawData.Length);
			BinaryConverter.WriteBytes(_buffer, 2, rawData);
		}
	}
}
