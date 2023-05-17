using System;
using System.Collections.Generic;

namespace CT.Common.Tools
{
	public class ManageableObjectPool<T> where T : IManageable, new()
	{
		public int Count => _objectStack.Count;
		private Stack<T> _objectStack;
		private Func<T> _factoryFunc;

		public ManageableObjectPool(Func<T> factoryFunc, int capacity)
		{
			_objectStack = new Stack<T>(capacity);
			_factoryFunc = factoryFunc;
			for (int i = 0; i < capacity; i++)
			{
				_objectStack.Push(_factoryFunc());
			}
		}

		public T Get()
		{
			T obj = _objectStack.Count == 0 ? _factoryFunc() : _objectStack.Pop();
			obj.OnInitialize();
			return obj;
		}

		public void Return(T obj)
		{
			if (obj == null)
				return;

			obj.OnFinalize();
			_objectStack.Push(obj);
		}
	}

	public class ObjectPool<T>
	{
		public int Count => _objectStack.Count;
		private Stack<T> _objectStack;
		private Func<T> _factoryFunc;

		public ObjectPool(Func<T> factoryFunc, int capacity)
		{
			_objectStack = new Stack<T>(capacity);
			_factoryFunc = factoryFunc;

			for (int i = 0; i < capacity; i++)
			{
				_objectStack.Push(_factoryFunc());
			}
		}

		public T Get()
		{
			T obj = _objectStack.Count == 0 ? _factoryFunc() : _objectStack.Pop();
			return obj;
		}

		public void Return(T obj)
		{
			if (obj == null)
				return;

			_objectStack.Push(obj);
		}
	}
}
