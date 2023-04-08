using System;

namespace CT.Network.Runtimes
{
	public abstract class JobContextBase
	{
		public abstract void Excute();
	}

	public class JobContext : JobContextBase
	{
		private Action _jobAction;

		public JobContext(Action action)
		{
			_jobAction = action;
		}

		public override void Excute()
		{
			_jobAction();
		}
	}

	public class JobContext<T> : JobContextBase
	{
		private Action<T> _jobAction;
		private T _value;

		public JobContext(Action<T> action, T value)
		{
			_jobAction = action;
			_value = value;
		}

		public override void Excute()
		{
			_jobAction(_value);
		}
	}

	public class JobContext<T1, T2> : JobContextBase
	{
		private Action<T1, T2> _jobAction;
		private T1 _value1;
		private T2 _value2;

		public JobContext(Action<T1, T2> action, T1 value1, T2 value2)
		{
			_jobAction = action;
			_value1 = value1;
			_value2 = value2;
		}

		public override void Excute()
		{
			_jobAction(_value1, _value2);
		}
	}

	public class JobContext<T1, T2, T3> : JobContextBase
	{
		private Action<T1, T2, T3> _jobAction;
		private T1 _value1;
		private T2 _value2;
		private T3 _value3;

		public JobContext(Action<T1, T2, T3> action, T1 value1, T2 value2, T3 value3)
		{
			_jobAction = action;
			_value1 = value1;
			_value2 = value2;
			_value3 = value3;
		}

		public override void Excute()
		{
			_jobAction(_value1, _value2, _value3);
		}
	}
}
