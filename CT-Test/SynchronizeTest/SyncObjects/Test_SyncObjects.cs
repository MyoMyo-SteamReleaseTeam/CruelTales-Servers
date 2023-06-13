using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT.Common.DataType;
using CTS.Instance.Gameplay;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.SynchronizeTest.SyncObjects
{
	[TestClass]
	public class Test_SyncObjects
	{
		[TestMethod]
		public void TestValue8()
		{
			NetworkPlayer p1 = new(new UserId(1));
			NetworkPlayer p2 = new(new UserId(2));

			CTS.Instance.SyncObjects.ZTest_Value8 master = new ();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value8 remote = new();

			//master.
		}
	}
}
