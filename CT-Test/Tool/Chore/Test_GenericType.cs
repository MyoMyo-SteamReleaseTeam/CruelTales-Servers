using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tool.Chore
{
	public class TestGeneric<T>
	{
		public T Value;

		public TestGeneric(T value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return $"Class TestGeneric<{typeof(T).Name}> Value : {Value}";
		}
	}

	public class TestGeneric
	{
		public int Value;

		public TestGeneric()
		{
			Value = 100;
		}

		public override string ToString()
		{
			return $"Class TestGeneric<{typeof(int).Name}> Value : {Value}";
		}
	}

	public class TestServiceLocator
	{
		private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

		public static void Register<T>(object serviceInstance)
		{
			Services[typeof(T)] = serviceInstance;
		}

		public static T Resolve<T>()
		{
			return (T)Services[typeof(T)];
		}

		public static void Reset()
		{
			Services.Clear();
		}
	}

	public interface ITestService { }

	public class TestParent
	{
		private string mName;

		public TestParent(string name)
		{
			mName = name;
		}

		public override string ToString()
		{
			return mName;
		}
	}

	public class TestChild : TestParent, ITestService
	{
		public TestChild(string name) : base(name) { }
	}

	public class TestSomethingElse : ITestService { }

	[TestClass]
	public class Tester_GenericClass
	{
		[TestMethod]
		public void Test_GenericClass()
		{
			TestGeneric gInt = new TestGeneric();
			TestGeneric<float> gFloat = new TestGeneric<float>(99.0f);

			Console.WriteLine(gInt);
			Console.WriteLine(gFloat);
		}
	}
}
