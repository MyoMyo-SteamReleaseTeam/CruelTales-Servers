using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT.Common.Synchronizations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.NetworkCore
{
	public class KeyPlayer
	{
		public int Id;

		public KeyPlayer(int id)
		{
			Id = id;
		}
	}

	[TestClass]
	public class Test_TargetCallstack
	{
		[TestMethod]
		public void CallstackTest()
		{
			KeyPlayer p1 = new KeyPlayer(1);
			KeyPlayer p2 = new KeyPlayer(2);

			TargetCallstack<KeyPlayer, long> callstack = new(7);
			Assert.IsFalse(callstack.IsDirty);
			callstack.Add(p1, 11);
			Assert.IsTrue(callstack.IsDirty);
			Assert.AreEqual(1, callstack.GetCallCount(p1));
			Assert.IsFalse(callstack.GetCallCount(p2) > 0);
			callstack.Add(p1, 12);
			callstack.Add(p2, 21);
			Assert.IsTrue(callstack.GetCallCount(p2) > 0);
			callstack.Add(p2, 22);
			callstack.Add(p2, 23);

			var p1List = callstack.GetCallList(p1);
			Assert.AreEqual(11, p1List[0]);
			Assert.AreEqual(12, p1List[1]);

			var p2List = callstack.GetCallList(p2);
			Assert.AreEqual(21, p2List[0]);
			Assert.AreEqual(22, p2List[1]);
			Assert.AreEqual(23, p2List[2]);

			callstack.Clear();
			Assert.IsFalse(callstack.IsDirty);
			Assert.IsFalse(callstack.GetCallCount(p1) > 0);
			Assert.IsFalse(callstack.GetCallCount(p2) > 0);
		}

		[TestMethod]
		public void CallstackVoidTest()
		{
			KeyPlayer p1 = new KeyPlayer(1);
			KeyPlayer p2 = new KeyPlayer(2);

			TargetVoidCallstack<KeyPlayer> callstack = new(7);
			Assert.IsFalse(callstack.IsDirty);
			callstack.Add(p1);
			Assert.IsTrue(callstack.IsDirty);
			Assert.AreEqual(1, callstack.GetCallCount(p1));
			Assert.AreEqual(0, callstack.GetCallCount(p2));
			callstack.Add(p1);
			callstack.Add(p2);
			Assert.AreEqual(2, callstack.GetCallCount(p1));
			Assert.AreEqual(1, callstack.GetCallCount(p2));
			callstack.Add(p2);
			callstack.Add(p2);
			Assert.AreEqual(2, callstack.GetCallCount(p1));
			Assert.AreEqual(3, callstack.GetCallCount(p2));

			callstack.Clear();
			Assert.IsFalse(callstack.IsDirty);
			Assert.IsFalse(callstack.GetCallCount(p1) > 0);
			Assert.IsFalse(callstack.GetCallCount(p2) > 0);
		}
	}
}
