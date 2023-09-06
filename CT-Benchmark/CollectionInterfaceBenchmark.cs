using BenchmarkDotNet.Attributes;

namespace CT.Benchmark
{
	public class CollectionClass
	{
		public int Count { get; private set; }
		public List<int> List;
		public IReadOnlyList<int> ListToIReadonlyList => List;
		public IList<int> IListInstance;

		public CollectionClass(int count)
		{
			Count = count;
			List = new List<int>(Count);
			IListInstance = new List<int>(Count);
			for (int i = 0; i < Count; i++)
			{
				List.Add(i);
				IListInstance.Add(i);
			}
		}
	}

	[MemoryDiagnoser]
	public class CollectionInterfaceBenchmark
	{
		private CollectionClass TestClass;
		private int _testCount = 100;

		public CollectionInterfaceBenchmark()
		{
			TestClass = new CollectionClass(100);
		}

		[Benchmark]
		public void Test_List()
		{
			int a = 0;
			for (int t = 0; t < _testCount; t++)
			{
				foreach (int i in TestClass.List)
				{
					a += i;
				}
			}
		}

		[Benchmark]
		public void Test_ListToIReadonlyList()
		{
			int a = 0;
			for (int t = 0; t < _testCount; t++)
			{
				foreach (int i in TestClass.ListToIReadonlyList)
				{
					a += i;
				}
			}
		}

		[Benchmark]
		public void Test_ListToIReadonlyListByIndex()
		{
			int a = 0;
			for (int t = 0; t < _testCount; t++)
			{
				for (int i = 0; i < TestClass.ListToIReadonlyList.Count; i++)
				{
					a += TestClass.ListToIReadonlyList[i];
				}
			}
		}

		[Benchmark]
		public void Test_IListInstance()
		{
			int a = 0;
			for (int t = 0; t < _testCount; t++)
			{
				foreach (int i in TestClass.IListInstance)
				{
					a += i;
				}
			}
		}
	}
}