using System;

namespace CT.Common.Tools
{
	public class Notifier<T> where T : struct
	{
		public event Action? OnChanged;
		public event Action<T>? OnDataChanged;

		protected T _value;
		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (!_value.Equals(value))
				{
					_value = value;
					IsDirty = true;
					OnChanged?.Invoke();
					OnDataChanged?.Invoke(Value);
				}
			}
		}

		public bool IsDirty { get; private set; }

		public Notifier(T value = default, bool isDirty = false)
		{
			_value = value;

			if (isDirty)
			{
				IsDirty = true;
				OnChanged?.Invoke();
				OnDataChanged?.Invoke(_value);
			}
		}

		public void SetPristine()
		{
			IsDirty = false;
		}

		public void SetValueWithoutEvent(T value)
		{
			_value = value;
		}
	}

	public class ManualSubjectData<T> where T : struct
	{
		public event Action? OnChanged;
		public event Action<T>? OnDataChanged;

		protected T _previousValue;
		public T Value;

		public bool IsDirty { get; private set; }

		public ManualSubjectData(T value = default, bool isDirty = false)
		{
			Value = value;
			_previousValue = Value;

			if (isDirty)
			{
				IsDirty = true;
				OnChanged?.Invoke();
				OnDataChanged?.Invoke(Value);
			}
		}

		public void EvaluateDirty()
		{
			if (!_previousValue.Equals(Value))
			{
				IsDirty = true;
				_previousValue = Value;
				OnChanged?.Invoke();
				OnDataChanged?.Invoke(Value);
			}
		}

		public void SetPristine()
		{
			IsDirty = false;
		}
	}
}
