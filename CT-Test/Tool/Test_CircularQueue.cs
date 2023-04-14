using CT.Tools.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tool
{
	[TestClass]
	public class Test_CircularQueue
	{
		[TestMethod]
		public void CircularQueue_Test()
		{
			CircularQueue<int> circularQueue = new CircularQueue<int>(100);

			int repeatCount = 100;

			Assert.IsFalse(circularQueue.TryDequeue(out _));

			for (int i = 0; i < repeatCount; i++)
			{
				circularQueue.TryEnqueue(i);
			}

			Assert.IsFalse(circularQueue.TryEnqueue(10));

			for (int i = 0; i < repeatCount; i++)
			{
				Assert.IsTrue(circularQueue.TryDequeue(out var dequeueValue));
				Assert.AreEqual(i, dequeueValue);
			}
		}
	}
}
