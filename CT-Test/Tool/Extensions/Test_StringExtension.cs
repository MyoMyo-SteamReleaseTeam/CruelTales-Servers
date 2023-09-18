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

				Assert.IsTrue(testString_1.IsOnlyAlphabetOrNumber());
				Assert.IsFalse(testString_2.IsOnlyAlphabetOrNumber());
				Assert.IsFalse(testString_3.IsOnlyAlphabetOrNumber());
				Assert.IsFalse(testString_4.IsOnlyAlphabetOrNumber());
			}
		}

		[TestMethod]
		public void CaseChangeTest()
		{
			string pascalCase_0 = "PascalCase";
			string pascalCase_1 = "PascalCaseLastUpperT";

			string snakeCase_0 = "snake_case_0";
			string snakeCase_1 = "snake_case_2_str";

			Assert.AreEqual("pascal_case", pascalCase_0.ToSnakeCase());
			Assert.AreEqual("pascal_case_last_upperT", pascalCase_1.ToSnakeCase());

			Assert.AreEqual("SnakeCase0", snakeCase_0.ToPascalCase());
			Assert.AreEqual("SnakeCase2Str", snakeCase_1.ToPascalCase());
		}
	}
}
