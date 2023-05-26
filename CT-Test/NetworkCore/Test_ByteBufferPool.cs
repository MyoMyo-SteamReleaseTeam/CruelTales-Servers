using CT.Common.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.NetworkCore
{
	[TestClass]
	public class Test_ByteBufferPool
	{
		[TestMethod]
		public void ByteBufferPoolTest()
		{
			ByteBufferPool pool = new ByteBufferPool(16, 2, ignoreLOH: true);
			ByteBuffer fakeBuffer = new ByteBuffer(15);
			ByteBuffer realBuffer = new ByteBuffer(16);
			var b1 = pool.Get();
			Assert.AreEqual(1, pool.BarrowedCount);
			var b2 = pool.Get();
			Assert.AreEqual(2, pool.BarrowedCount);
			b1.Put((byte)111);
			b2.Put((byte)255);

			pool.Return(fakeBuffer);
			Assert.AreEqual(2, pool.BarrowedCount);

			pool.Return(realBuffer);
			Assert.AreEqual(1, pool.BarrowedCount);

			pool.Return(b1);
			Assert.AreEqual(0, pool.BarrowedCount);

			pool.Return(b2);
			Assert.AreEqual(-1, pool.BarrowedCount);
		}
	}
}