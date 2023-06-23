using System;

namespace CT.Common.Tools
{
	public class EnumNotifier<T> where T : Enum
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

		public EnumNotifier(T value = default, bool isDirty = false)
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

	public class ValueNotifier<T> where T : struct, IEquatable<T>
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

		public ValueNotifier(T value = default, bool isDirty = false)
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

	public class ManualSubjectData<T> where T : struct, IEquatable<T>
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
