using System.Drawing;
using CT.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.DataTypes
{
	[TestClass]
	public class Test_Color32
	{
		[TestMethod]
		public void Color32Test()
		{
			Color32 greenByte = new Color32(0, 255, 0);
			Color32 greenFloat = new Color32(0, 1.0f, 0);
			Color32 greenUInt = new Color32(0x00FF00FF);
			Color32 greenStr6 = new Color32("00FF00");
			Color32 greenStr8 = new Color32("00FF00FF");

			Assert.AreEqual(Color32.Green, greenByte);
			Assert.AreEqual(Color32.Green, greenFloat);
			Assert.AreEqual(Color32.Green, greenUInt);
			Assert.AreEqual(Color32.Green, greenStr6);
			Assert.AreEqual(Color32.Green, greenStr8);

			Color32 green = Color32.Green;
			Assert.AreEqual(0, green.R);
			Assert.AreEqual(255, green.G);
			Assert.AreEqual(0, green.B);
			Assert.AreEqual(255, green.A);

			Color32 randomColor = new Color32(15, 33, 55, 71);
			Assert.AreEqual(15, randomColor.R);
			Assert.AreEqual(33, randomColor.G);
			Assert.AreEqual(55, randomColor.B);
			Assert.AreEqual(71, randomColor.A);

			Color32 fromUInt = new Color32(0x123456FF);
			Color32 fromStr = Color32.ParseFromHex("123456FF");

			Assert.AreEqual(fromUInt, fromStr);
			Assert.AreEqual(Color32.Void, Color32.ParseFromHex("ABCDEFGH"));

			// Unity에서도 float의 값은 byte의 정밀도로 양자화됨
			//Color32 floatColor = new Color32(0.25f, 0.50f, 0.75f, 1.0f);
			//Assert.AreEqual(0.25f, floatColor.FR);
			//Assert.AreEqual(0.50f, floatColor.FG);
			//Assert.AreEqual(0.75f, floatColor.FB);
			//Assert.AreEqual(1.0f, floatColor.FA);
		}
	}
}
