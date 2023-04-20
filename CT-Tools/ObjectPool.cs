using System.Collections.Generic;

namespace CT.Tools
{
	public class ManageableObjectPool<T> where T : IManageable, new()
	{
		public int Count => _objectStack.Count;
		private Stack<T> _objectStack;

		public ManageableObjectPool(int capacity = 0)
		{
			_objectStack = new Stack<T>(capacity);

			for (int i = 0; i < capacity; i++)
			{
				_objectStack.Push(new T());
			}
		}

		public T Get()
		{
			T obj = _objectStack.Count == 0 ? new T() : _objectStack.Pop();
			obj.OnInitialize();
			return obj;
		}

		public void Return(T obj)
		{
			if (obj == null)
			{
				return;
			}

			obj.OnFinalize();
			_objectStack.Push(obj);
		}
	}

	public class ObjectPool<T> where T : new()
	{
		public int Count => _objectStack.Count;
		private Stack<T> _objectStack;

		public ObjectPool(int capacity = 0)
		{
			_objectStack = new Stack<T>(capacity);

			for (int i = 0; i < capacity; i++)
			{
				_objectStack.Push(new T());
			}
		}

		public T Get()
		{
			T obj = _objectStack.Count == 0 ? new T() : _objectStack.Pop();
			return obj;
		}

		public void Return(T obj)
		{
			if (obj == null)
			{
				return;
			}

			_objectStack.Push(obj);
		}
	}
}
