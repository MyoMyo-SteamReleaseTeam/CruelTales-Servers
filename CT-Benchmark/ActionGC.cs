using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Benchmark
{
	internal class ActionGC
	{
		public static void Test()
		{
			SomeClass someClass = new SomeClass(1);

			Action voidAction;
			int[] gcCount = new int[3];
			StringBuilder gcLog = new StringBuilder(1024);

			while (true)
			{
				voidAction = new Action(SomeClass.StaticSomeFunc);

				bool hasGC = false;
				for (int i = 0; i < gcCount.Length; i++)
				{
					int preCount = gcCount[i];
					int curCount = GC.CollectionCount(i);
					if (preCount != curCount)
					{
						gcCount[i] = curCount;
						hasGC |= true;
					}
				}

				if (hasGC)
				{
					gcLog.Clear();

					for (int i = 0; i < gcCount.Length; i++)
					{
						gcLog.Append("Gen");
						gcLog.Append(i);
						gcLog.Append(":");
						gcLog.Append(gcCount[i]);
						gcLog.Append("\t");
					}

					Console.WriteLine(gcLog.ToString());
				}
			}
		}
	}

	public class SomeClass
	{
		private int _value;

		public SomeClass(int value)
		{
			_value = value;
		}

		public void SomeFunc()
		{

		}

		public static void StaticSomeFunc()
		{

		}
	}
}
