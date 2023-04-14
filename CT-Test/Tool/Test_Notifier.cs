using CT.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tool
{
	[TestClass]
	public class Test_Notifier
	{
		public readonly struct TEST_NotiStruct
		{
			public readonly int Value1;
			public readonly int Value2;

			public TEST_NotiStruct(int value1, int value2)
			{
				Value1 = value1;
				Value2 = value2;
			}
		}

		[TestMethod]
		public void Notifier()
		{
			Notifier<int> intNoti = new Notifier<int>(100);

			intNoti.Value = 120;
			Assert.IsTrue(intNoti.IsDirty);

			intNoti.SetPristine();
			Assert.IsFalse(intNoti.IsDirty);

			intNoti.Value = 120;
			Assert.IsFalse(intNoti.IsDirty);

			intNoti.Value = 130;
			Assert.IsTrue(intNoti.IsDirty);

			Notifier<TEST_NotiStruct> structNoti = new Notifier<TEST_NotiStruct>(new TEST_NotiStruct(10, 20));

			Assert.IsFalse(structNoti.IsDirty);

			structNoti.Value = new TEST_NotiStruct(50, 20);
			Assert.IsTrue(structNoti.IsDirty);

			structNoti.SetPristine();

			structNoti.Value = new TEST_NotiStruct(50, 20);
			Assert.IsFalse(structNoti.IsDirty);
		}
	}
}
