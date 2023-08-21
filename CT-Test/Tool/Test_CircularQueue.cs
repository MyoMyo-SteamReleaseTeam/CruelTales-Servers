using CT.Common.Tools.Collections;
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

		[TestMethod]
		public void Boundary_Test()
		{
			CircularQueue<int> queue = new(4);

			queue.Enqueue(10);
			queue.Enqueue(20);
			queue.Enqueue(30);
			queue.Enqueue(40);

			Assert.AreEqual(4, queue.Count);
			Assert.IsFalse(queue.TryEnqueue(50));
			Assert.AreEqual(10, queue.Dequeue());
			Assert.AreEqual(3, queue.Count);
			Assert.AreEqual(20, queue.Dequeue());
			Assert.AreEqual(30, queue.Dequeue());
			Assert.AreEqual(40, queue.Dequeue());
			Assert.AreEqual(0, queue.Count);
			Assert.IsFalse(queue.TryDequeue(out _));

			queue.Enqueue(50);
			queue.Enqueue(60);

			Assert.AreEqual(2, queue.Count);
			Assert.IsTrue(queue.TryDequeue(out int value));
			Assert.AreEqual(50, value);
			Assert.AreEqual(60, queue.Dequeue());
			Assert.AreEqual(0, queue.Count);
			Assert.IsFalse(queue.TryDequeue(out _));
		}
	}
}
