using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace CT.Benchmark
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ArgUnion
	{
		[FieldOffset(0)] public int IntArgu;
		[FieldOffset(0)] public float FloatArgu;
	}

	public struct CoroutineAction
	{
		public CoroutineTestClass TestClass;
		public Action Action;
		public int ExecutionTickTime;
		
		public CoroutineAction(CoroutineTestClass testClass, Action action, int tickAfter)
		{
			TestClass = testClass;
			Action = action;
			ExecutionTickTime = Environment.TickCount + tickAfter;
		}

		public void Execute()
		{
			Action.Invoke();
		}
	}

	public struct CoroutineActionArg
	{
		public CoroutineTestClass TestClass;
		public Action<ArgUnion> Action;
		public int ExecutionTickTime;
		public ArgUnion Argument;

		public CoroutineActionArg(CoroutineTestClass testClass, Action<ArgUnion> action, int tickAfter, ArgUnion argument)
		{
			TestClass = testClass;
			Action = action;
			ExecutionTickTime = Environment.TickCount + tickAfter;
			Argument = argument;
		}

		public void Execute()
		{
			Action.Invoke(Argument);
		}
	}

	public class CoroutineRunner
	{
		private Queue<CoroutineAction> _coroutineQueue = new(64);
		private Queue<CoroutineActionArg> _coroutineArgQueue = new(64);

		public void AddCoroutineAction(CoroutineAction action)
		{
			_coroutineQueue.Enqueue(action);
		}

		public void AddCoroutineAction(CoroutineActionArg action)
		{
			_coroutineArgQueue.Enqueue(action);
		}

		public void Flush()
		{
			while (_coroutineQueue.TryDequeue(out var action))
			{
				action.Execute();
			}

			while (_coroutineArgQueue.TryDequeue(out var action))
			{
				action.Execute();
			}
		}
	}

	public class CoroutineTestClass
	{
		public Action SomeActionCache;
		public Action<ArgUnion> SomeActionArgCache;
		public int ActionCount = 0;
		public float FloatCount = 0;

		public CoroutineTestClass()
		{
			SomeActionCache = SomeAction;
			SomeActionArgCache = SomeActionArg;
		}

		public void SomeAction()
		{
			ActionCount++;
		}

		public void SomeActionArg(ArgUnion floatArg)
		{
			FloatCount += floatArg.FloatArgu;
		}
	}

	[MemoryDiagnoser]
	public class CoroutineActionBenchmark
	{
		private CoroutineRunner _runner = new();
		private CoroutineTestClass _testClass = new();
		private const int TEST_COUNT = 100;

		public CoroutineActionBenchmark()
		{
			
		}

		[Benchmark]
		public void ActionWithCache()
		{
			for (int i = 0; i < TEST_COUNT; i++)
			{
				CoroutineAction action = new(_testClass, _testClass.SomeActionCache, 0);
				_runner.AddCoroutineAction(action);
			}

			_runner.Flush();
			_testClass.ActionCount = 0;
		}

		[Benchmark]
		public void ActionNoCache()
		{
			for (int i = 0; i < TEST_COUNT; i++)
			{
				CoroutineAction action = new(_testClass, _testClass.SomeAction, 0);
				_runner.AddCoroutineAction(action);
			}

			_runner.Flush();
			_testClass.ActionCount = 0;
		}

		[Benchmark]
		public void ActionArgWithCache()
		{
			ArgUnion argUnion = new ArgUnion() { FloatArgu = 10.0f };

			for (int i = 0; i < TEST_COUNT; i++)
			{
				CoroutineActionArg action = new(_testClass, _testClass.SomeActionArgCache, 0, argUnion);
				_runner.AddCoroutineAction(action);
			}

			_runner.Flush();
			_testClass.FloatCount = 0;
		}

		[Benchmark]
		public void ActionArgNoCache()
		{
			ArgUnion argUnion = new ArgUnion() { FloatArgu = 10.0f };

			for (int i = 0; i < TEST_COUNT; i++)
			{
				CoroutineActionArg action = new(_testClass, _testClass.SomeActionArg, 0, argUnion);
				_runner.AddCoroutineAction(action);
			}

			_runner.Flush();
			_testClass.FloatCount = 0;
		}
	}
}
