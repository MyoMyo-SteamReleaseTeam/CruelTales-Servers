using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tool.Chore
{
	[TestClass]
	public class Test_Chore
	{
		[TestMethod]
		public void Test_ReletivePath()
		{
			string rootPath = @"C:\Runners\KaNet\KaNet\KaNetUtil\Utils";
			string sourcePath1 = @"C:\Runners\KaNet\KaNet\KaNetUtil\Utils\ABC\ddee.cs";
			string sourcePath2 = @"C:\Runners\KaNet\KaNet\KaNetUtil\Utils\eee1234\bbsefs";
			string sourcePath3 = @"C:\Runners\KaNet\KaNet\KaNetUtil\Ivan\eee1234\bbsefs";

			string reletivePath1 = Path.GetRelativePath(rootPath, sourcePath1);
			string reletivePath2 = Path.GetRelativePath(rootPath, sourcePath2);
			string reletivePath3 = Path.GetRelativePath(rootPath, sourcePath3);

			//string directoryName = Path.GetDirectoryName(sourcePath1);
			//string directoryName2 = Path.GetDirectoryName(sourcePath2);
		}

		public enum TestEnumType
		{
			None = 0,
			Type_1 = 10,
			Type_2 = 20,
			Type_3 = 30,
			Type_4 = 40,
			Type_5 = 50,
			Type_6 = 60,
			Type_7 = 70,
			Type_8 = 80,
			Type_9 = 90,
			Type_10 = 100,
		}

		[TestMethod]
		public void Test_EnumParser()
		{
			TestEnumType myType = TestEnumType.Type_6;

			string enumToString = myType.ToString();

			Assert.IsTrue(Enum.TryParse(enumToString, out TestEnumType parseType));

			Assert.AreEqual(myType, parseType);
		}
	}
}
