using CT.Tools.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tool
{
	[TestClass]
	public class Test_BidirectionalMap
	{
		[TestMethod]
		public void BidirectionalMapBySeparatedOperation()
		{
			BidirectionalMap<string, int> biMap = new BidirectionalMap<string, int>();

			string fKey1 = "Test1";
			string fKey2 = "Test2";
			string fKey3 = "Test3";
			string fKey4 = "Test4";
			string wrongFKey1 = "Test5";
			string wrongFKey2 = "Test6";

			int sKey1 = 10;
			int sKey2 = 20;
			int sKey3 = 30;
			int sKey4 = 40;
			int wrongSKey1 = 50;
			int wrongSKey2 = 60;

			// Test add
			Assert.IsTrue(biMap.TryAddForward(fKey2, sKey2));
			Assert.IsTrue(biMap.TryAddForward(fKey1, sKey1));
			Assert.IsTrue(biMap.TryAddReverse(sKey3, fKey3));
			Assert.IsTrue(biMap.TryAddReverse(sKey4, fKey4));

			Assert.IsTrue(biMap.Count == 4);
			Assert.IsTrue(biMap.TryGetForward(fKey1, out int test_1));
			Assert.AreEqual(sKey1, test_1);
			Assert.IsTrue(biMap.TryGetReverse(sKey3, out var test_3));
			Assert.AreEqual(fKey3, test_3);
			Assert.IsFalse(biMap.TryGetForward(wrongFKey2, out int someValue));
			Assert.IsFalse(biMap.TryGetReverse(wrongSKey1, out var someString));

			// Test exception
			Assert.IsFalse(biMap.TryAddForward(fKey2, wrongSKey2));
			Assert.IsFalse(biMap.TryAddReverse(sKey1, wrongFKey1));

			// Test contains
			Assert.IsTrue(biMap.ContainsForward(fKey1));
			Assert.IsTrue(biMap.ContainsForward(fKey4));
			Assert.IsTrue(biMap.ContainsReverse(sKey2));
			Assert.IsTrue(biMap.ContainsReverse(sKey3));

			// Test remove
			Assert.IsTrue(biMap.TryRemoveForward(fKey2));
			Assert.IsFalse(biMap.TryRemoveReverse(sKey2));
			Assert.IsFalse(biMap.TryRemoveForward(fKey2));
			Assert.IsTrue(biMap.Count == 3);

			Assert.IsTrue(biMap.TryRemoveReverse(sKey3));
			Assert.IsFalse(biMap.TryRemoveForward(fKey3));
			Assert.IsFalse(biMap.TryRemoveReverse(sKey3));
			Assert.IsTrue(biMap.Count == 2);

			// Test clear
			biMap.Clear();
			Assert.IsTrue(biMap.Count == 0);
			Assert.IsFalse(biMap.TryRemoveForward(fKey1));
			Assert.IsFalse(biMap.TryRemoveReverse(sKey4));
		}

		[TestMethod]
		public void BidirectionalMapByGenericOperation()
		{
			BidirectionalMap<string, int> biMap = new BidirectionalMap<string, int>();

			string fKey1 = "Test1";
			string fKey2 = "Test2";
			string fKey3 = "Test3";
			string fKey4 = "Test4";
			string wrongFKey1 = "Test5";
			string wrongFKey2 = "Test6";

			int sKey1 = 10;
			int sKey2 = 20;
			int sKey3 = 30;
			int sKey4 = 40;
			int wrongSKey1 = 50;
			int wrongSKey2 = 60;

			// Test add
			Assert.IsTrue(biMap.TryAdd(fKey2, sKey2));
			Assert.IsTrue(biMap.TryAdd(fKey1, sKey1));
			Assert.IsTrue(biMap.TryAdd(sKey3, fKey3));
			Assert.IsTrue(biMap.TryAdd(sKey4, fKey4));

			Assert.IsTrue(biMap.Count == 4);
			Assert.IsTrue(biMap.TryGetValue(fKey1, out int test_1));
			Assert.AreEqual(sKey1, test_1);
			Assert.IsTrue(biMap.TryGetValue(sKey3, out var test_3));
			Assert.AreEqual(fKey3, test_3);
			Assert.IsFalse(biMap.TryGetValue(wrongFKey2, out int someValue));
			Assert.IsFalse(biMap.TryGetValue(wrongSKey1, out var someString));

			// Test exception
			Assert.IsFalse(biMap.TryAdd(fKey2, wrongSKey2));
			Assert.IsFalse(biMap.TryAdd(sKey1, wrongFKey1));

			// Test contains
			Assert.IsTrue(biMap.Contains(fKey1));
			Assert.IsTrue(biMap.Contains(fKey4));
			Assert.IsTrue(biMap.Contains(sKey2));
			Assert.IsTrue(biMap.Contains(sKey3));

			// Test remove
			Assert.IsTrue(biMap.TryRemove(fKey2));
			Assert.IsFalse(biMap.TryRemove(sKey2));
			Assert.IsFalse(biMap.TryRemove(fKey2));
			Assert.IsTrue(biMap.Count == 3);

			Assert.IsTrue(biMap.TryRemove(sKey3));
			Assert.IsFalse(biMap.TryRemove(fKey3));
			Assert.IsFalse(biMap.TryRemove(sKey3));
			Assert.IsTrue(biMap.Count == 2);

			// Test clear
			biMap.Clear();
			Assert.IsTrue(biMap.Count == 0);
			Assert.IsFalse(biMap.TryRemove(fKey1));
			Assert.IsFalse(biMap.TryRemove(sKey4));
		}

		private class Cat { }
		private class Dog { }

		[TestMethod]
		public void BidirectionalMapByReferenceType()
		{
			BidirectionalMap<Cat, Dog> biMap = new BidirectionalMap<Cat, Dog>();

			Cat fKey1 = new Cat();
			Cat fKey2 = new Cat();
			Cat fKey3 = new Cat();
			Cat fKey4 = new Cat();
			Cat wrongFKey1 = new Cat();
			Cat wrongFKey2 = new Cat();

			Dog sKey1 = new Dog();
			Dog sKey2 = new Dog();
			Dog sKey3 = new Dog();
			Dog sKey4 = new Dog();
			Dog wrongSKey1 = new Dog();
			Dog wrongSKey2 = new Dog();

			// Test add
			Assert.IsTrue(biMap.TryAdd(fKey2, sKey2));
			Assert.IsTrue(biMap.TryAdd(fKey1, sKey1));
			Assert.IsTrue(biMap.TryAdd(sKey3, fKey3));
			Assert.IsTrue(biMap.TryAdd(sKey4, fKey4));

			Assert.IsTrue(biMap.Count == 4);
			Assert.IsTrue(biMap.TryGetValue(fKey1, out var test_1));
			Assert.AreEqual(sKey1, test_1);
			Assert.IsTrue(biMap.TryGetValue(sKey3, out var test_3));
			Assert.AreEqual(fKey3, test_3);
			Assert.IsFalse(biMap.TryGetValue(wrongFKey2, out var someValue));
			Assert.IsFalse(biMap.TryGetValue(wrongSKey1, out var someString));

			// Test exception
			Assert.IsFalse(biMap.TryAdd(fKey2, wrongSKey2));
			Assert.IsFalse(biMap.TryAdd(sKey1, wrongFKey1));

			// Test contains
			Assert.IsTrue(biMap.Contains(fKey1));
			Assert.IsTrue(biMap.Contains(fKey4));
			Assert.IsTrue(biMap.Contains(sKey2));
			Assert.IsTrue(biMap.Contains(sKey3));

			// Test remove
			Assert.IsTrue(biMap.TryRemove(fKey2));
			Assert.IsFalse(biMap.TryRemove(sKey2));
			Assert.IsFalse(biMap.TryRemove(fKey2));
			Assert.IsTrue(biMap.Count == 3);

			Assert.IsTrue(biMap.TryRemove(sKey3));
			Assert.IsFalse(biMap.TryRemove(fKey3));
			Assert.IsFalse(biMap.TryRemove(sKey3));
			Assert.IsTrue(biMap.Count == 2);

			// Test clear
			biMap.Clear();
			Assert.IsTrue(biMap.Count == 0);
			Assert.IsFalse(biMap.TryRemove(fKey1));
			Assert.IsFalse(biMap.TryRemove(sKey4));
		}

		[TestMethod]
		public void Test_BidirectionalMapIfTypeAreSame()
		{
			BidirectionalMap<int, int> biMap = new BidirectionalMap<int, int>();

			int fKey1 = 10;
			int fKey2 = 20;
			int fKey3 = 30;
			int fKey4 = 40;
			int wrongFKey1 = 50;
			int wrongFKey2 = 60;

			int sKey1 = 10;
			int sKey2 = 20;
			int sKey3 = 30;
			int sKey4 = 40;
			int wrongSKey1 = 50;
			int wrongSKey2 = 60;

			// Test add
			Assert.IsTrue(biMap.TryAddForward(fKey2, sKey2));
			Assert.IsTrue(biMap.TryAddForward(fKey1, sKey1));
			Assert.IsTrue(biMap.TryAddReverse(sKey3, fKey3));
			Assert.IsTrue(biMap.TryAddReverse(sKey4, fKey4));

			Assert.IsTrue(biMap.Count == 4);
			Assert.IsTrue(biMap.TryGetForward(fKey1, out int test_1));
			Assert.AreEqual(sKey1, test_1);
			Assert.IsTrue(biMap.TryGetReverse(sKey3, out var test_3));
			Assert.AreEqual(fKey3, test_3);
			Assert.IsFalse(biMap.TryGetForward(wrongFKey2, out int someValue));
			Assert.IsFalse(biMap.TryGetReverse(wrongSKey1, out var someString));

			// Test exception
			Assert.IsFalse(biMap.TryAddForward(fKey2, wrongSKey2));
			Assert.IsFalse(biMap.TryAddReverse(sKey1, wrongFKey1));

			// Test contains
			Assert.IsTrue(biMap.ContainsForward(fKey1));
			Assert.IsTrue(biMap.ContainsForward(fKey4));
			Assert.IsTrue(biMap.ContainsReverse(sKey2));
			Assert.IsTrue(biMap.ContainsReverse(sKey3));

			// Test remove
			Assert.IsTrue(biMap.TryRemoveForward(fKey2));
			Assert.IsFalse(biMap.TryRemoveReverse(sKey2));
			Assert.IsFalse(biMap.TryRemoveForward(fKey2));
			Assert.IsTrue(biMap.Count == 3);

			Assert.IsTrue(biMap.TryRemoveReverse(sKey3));
			Assert.IsFalse(biMap.TryRemoveForward(fKey3));
			Assert.IsFalse(biMap.TryRemoveReverse(sKey3));
			Assert.IsTrue(biMap.Count == 2);

			// Test clear
			biMap.Clear();
			Assert.IsTrue(biMap.Count == 0);
			Assert.IsFalse(biMap.TryRemoveForward(fKey1));
			Assert.IsFalse(biMap.TryRemoveReverse(sKey4));
		}
	}
}
