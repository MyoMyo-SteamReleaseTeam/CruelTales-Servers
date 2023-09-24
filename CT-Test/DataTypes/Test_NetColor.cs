using System.Drawing;
using CT.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.DataTypes
{
	[TestClass]
	public class Test_NetColor
	{
		[TestMethod]
		public void NetColorTest()
		{
			NetColor greenByte = new NetColor(0, 255, 0);
			NetColor greenFloat = new NetColor(0, 1.0f, 0);
			NetColor greenUInt = new NetColor(0x00FF00FF);
			NetColor greenStr6 = new NetColor("00FF00");
			NetColor greenStr8 = new NetColor("00FF00FF");

			Assert.AreEqual(NetColor.Green, greenByte);
			Assert.AreEqual(NetColor.Green, greenFloat);
			Assert.AreEqual(NetColor.Green, greenUInt);
			Assert.AreEqual(NetColor.Green, greenStr6);
			Assert.AreEqual(NetColor.Green, greenStr8);

			NetColor green = NetColor.Green;
			Assert.AreEqual(0, green.R);
			Assert.AreEqual(255, green.G);
			Assert.AreEqual(0, green.B);
			Assert.AreEqual(255, green.A);

			NetColor randomColor = new NetColor(15, 33, 55, 71);
			Assert.AreEqual(15, randomColor.R);
			Assert.AreEqual(33, randomColor.G);
			Assert.AreEqual(55, randomColor.B);
			Assert.AreEqual(71, randomColor.A);

			NetColor fromUInt = new NetColor(0x123456FF);
			NetColor fromStr = NetColor.ParseFromHex("123456FF");

			Assert.AreEqual(fromUInt, fromStr);
			Assert.AreEqual(NetColor.Void, NetColor.ParseFromHex("ABCDEFGH"));

			// Unity에서도 float의 값은 byte의 정밀도로 양자화됨
			//Color32 floatColor = new Color32(0.25f, 0.50f, 0.75f, 1.0f);
			//Assert.AreEqual(0.25f, floatColor.FR);
			//Assert.AreEqual(0.50f, floatColor.FG);
			//Assert.AreEqual(0.75f, floatColor.FB);
			//Assert.AreEqual(1.0f, floatColor.FA);
		}
	}
}
