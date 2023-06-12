using System.Buffers;
using System.Text;
using BenchmarkDotNet.Attributes;
using CT.Common.Serialization;

namespace CT.Benchmark
{
	[MemoryDiagnoser]
	public class StringBufferBenchmark
	{
		private StringBuilder _sb = new StringBuilder(1024);
		private string _tempStr = string.Empty;
		private int _testCount = 50;

		//private string _testStr1 = "This is test";
		//private string _testStr2 = " Test for ";

		private string _testStr1 = "한글테스트";
		private string _testStr2 = "韓契他社打";

		[Benchmark]
		public void OnlyAppend()
		{
			for (int i = 0; i < _testCount; i++)
			{
				_sb.Clear();
				_sb.Append(_testStr1);
				_sb.Append(_testCount);
				_sb.Append(_testStr2);
				_sb.Append(_testCount * 2);
			}
		}

		[Benchmark]
		public void AppendFormat()
		{
			for (int i = 0; i < _testCount; i++)
			{
				_sb.Clear();
				_sb.Append(_testStr1);
				_sb.AppendFormat("{0}{1}{2}", _testCount, _testStr2, _testCount * 2);
			}
		}

		private ArraySegment<byte> _byteBuffer = new(new byte[1024]);

		[Benchmark]
		public void SBToByteArray()
		{
			for (int i = 0; i < _testCount; i++)
			{
				_sb.Clear();
				_sb.Append(_testStr1);
				_sb.Append(_testCount);
				_sb.Append(_testStr2);
				_sb.Append(_testCount * 2);
				BinaryConverter.WriteString(_byteBuffer, 0, _sb.ToString());
			}
		}

		[Benchmark]
		public void SBToByteArrayByIter()
		{
			for (int i = 0; i < _testCount; i++)
			{
				_sb.Clear();
				_sb.Append(_testStr1);
				_sb.Append(_testCount);
				_sb.Append(_testStr2);
				_sb.Append(_testCount * 2);

				int index = 0;
				for (int c  = 0; c < _sb.Length; c++)
				{
					var character = (ushort)_sb[c];
					BinaryConverter.WriteUInt16(_byteBuffer, index, character);
					index += 2;
				}

				StringBuilder sb = new(1024);
				int readIdx = 0;
				for (int n = 0; n < _sb.Length; n++)
				{
					var read = (char)BinaryConverter.ReadUInt16(_byteBuffer, readIdx);
					readIdx += 2;
					sb.Append(read);
				}

				string temp = sb.ToString();
				var utf8bytes = Encoding.UTF8.GetBytes(temp);
			}
		}

		private ArraySegment<byte> _encodeBuffer = new(new byte[1024]);

		public static readonly UTF8Encoding Utf8Encoding = new UTF8Encoding();
		public void TestEncoding()
		{
			//_sb.Clear();
			//_sb.Append(_testStr1);
			//_sb.Append(_testCount);
			//_sb.Append(_testStr2);
			//_sb.Append(_testCount * 2);
			//int length = _sb.Length;

			//Span<char> tempBuffer = stackalloc char[length];
			//_sb.CopyTo(0, tempBuffer, length);

			//Utf8Encoding.GetBytes(tempBuffer, 0, 0, _encodeBuffer.Array, 0);
			//Utf8Encoding.GetBytes(tempBuffer, length, _encodeBuffer.Array, 0);

			//var encoder = Encoding.UTF8.GetEncoder();
			//encoder.Convert(_sb, )
		}

	}
}