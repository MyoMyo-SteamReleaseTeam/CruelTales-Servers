using System.Linq;
using CT.Tool.GetOpt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tool
{
	[TestClass]
	public class Test_OptionParser
	{
		class ParameterTest
		{
			public int Param_f = 0;
			public int Param_m = 0;
			public string[]? Param_o;
			public int Param_c = 0;
			public string Param_level2 = "";
		}

		[TestMethod]
		public void OptionParser_Single()
		{
			ParameterTest pt = new ParameterTest();
			string argument = "-f-m -o path1 path2.abc path3/cc -c--level2 level2Param";
			var options = new OptionParser();
			options.RegisterEvent("f", 1, (e) => pt.Param_f = 10);
			options.RegisterEvent("m", 1, (e) => pt.Param_m = 20);
			options.RegisterEvent("c", 1, (e) => pt.Param_c = 30);
			options.RegisterEvent("o", 1, (e) => pt.Param_o = e.ToArray());
			options.RegisterEvent("level2", 2, (e) => pt.Param_level2 = e[0]);

			options.OnArguments(argument);


			Assert.AreEqual(10, pt.Param_f);
			Assert.AreEqual(20, pt.Param_m);
			Assert.AreEqual(30, pt.Param_c);

			Assert.IsTrue(pt.Param_o != null);
			Assert.AreEqual("path1", pt.Param_o[0]);
			Assert.AreEqual("path2.abc", pt.Param_o[1]);
			Assert.AreEqual("path3/cc", pt.Param_o[2]);

			Assert.AreEqual("level2Param", pt.Param_level2);
		}

		[TestMethod]
		public void OptionParser_Multiple()
		{
			ParameterTest pt = new ParameterTest();
			string[] args = new string[]
			{
				"-f-m",
				"-o",
				"path1",
				"path2.abc",
				"path3/cc",
				"-c--level2",
				"level2Param"
			};
			var options = new OptionParser();
			options.RegisterEvent("f", 1, (e) => pt.Param_f = 10);
			options.RegisterEvent("m", 1, (e) => pt.Param_m = 20);
			options.RegisterEvent("c", 1, (e) => pt.Param_c = 30);
			options.RegisterEvent("o", 1, (e) => pt.Param_o = e.ToArray());
			options.RegisterEvent("level2", 2, (e) => pt.Param_level2 = e[0]);

			options.OnArguments(args);

			Assert.AreEqual(10, pt.Param_f);
			Assert.AreEqual(20, pt.Param_m);
			Assert.AreEqual(30, pt.Param_c);

			Assert.IsTrue(pt.Param_o != null);
			Assert.AreEqual("path1", pt.Param_o[0]);
			Assert.AreEqual("path2.abc", pt.Param_o[1]);
			Assert.AreEqual("path3/cc", pt.Param_o[2]);

			Assert.AreEqual("level2Param", pt.Param_level2);
		}

		[TestMethod]
		public void OptionParser_Miscellaneous()
		{
			Assert.AreEqual("--f", OptionParser.GetSignature("f", 2));
			Assert.AreEqual("-m", OptionParser.GetSignature("m", 1));
			Assert.AreEqual("---zzz", OptionParser.GetSignature("zzz", 3));
		}
	}
}