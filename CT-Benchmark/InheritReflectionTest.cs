namespace CT.Benchmark
{
	public class ParentClass
	{
		public int BaseA;
		public string? BaseB;

		public void BaseFuncA() {}
	}

	public class ChildClass : ParentClass
	{
		public int ChildC;

		public void ChildFuncB() {}
	}

	public class InheritReflectionTest
	{
		public static void Test()
		{
			Type parent = typeof(ParentClass);
			Type child = typeof(ChildClass);
			Console.WriteLine(child.BaseType);
			Console.WriteLine(child.BaseType == parent);

			var childFieldInfos = child.GetFields();
			var parentFieldInfos = parent.GetFields();
			
			foreach (var cf in childFieldInfos)
			{
				foreach (var pf in parentFieldInfos)
				{
					if (cf.Name == pf.Name)
					{
						Console.WriteLine(cf.Name);
					}

					if (cf == pf)
					{
						Console.WriteLine(cf);
					}
				}
			}
		}
	}
}