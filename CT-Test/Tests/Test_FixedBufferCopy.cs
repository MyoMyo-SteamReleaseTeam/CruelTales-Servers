using System;
using CT.Common.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tests
{
	public enum TestEnum
	{
		Zero,
		One,
		Two,
		Three,
		Four,
	}

	public unsafe struct TestBuffer
	{
		public TestEnum TestEnum;
		public fixed byte Buffer[Test_FixedBufferCopy.BUFFER_SIZE];
	}

	[TestClass]
	public class Test_FixedBufferCopy
	{
		public const int BUFFER_SIZE = 4;

		public TestBuffer byteBuffer = new();
		public byte[] byteArray = new byte[BUFFER_SIZE];

		[TestMethod]
		public unsafe void FixedBufferCopyTest()
		{
			byteBuffer.Buffer[0] = 10;
			byteBuffer.Buffer[1] = 20;
			byteBuffer.Buffer[2] = 30;
			byteBuffer.Buffer[3] = 40;

			var buffer = byteBuffer;
			Span<byte> refBuffer = new Span<byte>(buffer.Buffer, 4);
			refBuffer.CopyTo(byteArray);

			byteBuffer.Buffer[0] = 50;
			byteBuffer.Buffer[1] = 60;
			byteBuffer.Buffer[2] = 70;
			byteBuffer.Buffer[3] = 80;

			new Span<byte>(buffer.Buffer, 4).CopyTo(byteArray);

			//IPacketReader reader = new IPacketReader();

			fixed (TestBuffer* bp = &byteBuffer)
			{
				var m = bp->Buffer;
				Span<byte> b = new Span<byte>(m, 4);
			}
		}
	}
}
