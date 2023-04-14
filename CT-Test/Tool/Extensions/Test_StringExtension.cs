using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tool.Extensions
{
	[TestClass]
	public class Test_StringExtension
	{
		[TestMethod]
		public void StringExtension()
		{
			// IsOnlyAlphabetAndNumber
			{
				string testString_1 = "abcz13242AbxcvZ2602";
				string testString_2 = "家羅";
				string testString_3 = "한글1235zhjalg^@";
				string testString_4 = "#@&&&(@^@";

				Assert.IsTrue(testString_1.IsOnlyAlphabetAndNumber());
				Assert.IsFalse(testString_2.IsOnlyAlphabetAndNumber());
				Assert.IsFalse(testString_3.IsOnlyAlphabetAndNumber());
				Assert.IsFalse(testString_4.IsOnlyAlphabetAndNumber());
			}
		}
	}
}
