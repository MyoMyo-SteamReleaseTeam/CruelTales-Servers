using CT.Common.Gameplay;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.DataTypes
{
	[TestClass]
	public class Test_SectionDirection
	{
		[TestMethod]
		public void SectionDirectionTest()
		{
			SectionDirection test = new() { From = 3, To = 6 };
			ushort combined = test.GetCombinedValue();
			SectionDirection.ParseTo(combined, out byte parseFrom, out byte parseTo);
			Assert.AreEqual(test.From, parseFrom);
			Assert.AreEqual(test.To, parseTo);
		}
	}
}
