using System.Collections.Generic;

namespace CTS.Instance.Coroutines
{
	public class CoroutineRunner
	{
		private PriorityQueue<CoroutineActionVoid, float> _coroutineVoidQueue;
		private PriorityQueue<CoroutineActionArg, float> _coroutineArgQueue;
		private PriorityQueue<CoroutineActionArgs2, float> _coroutineArgs2Queue;
		private PriorityQueue<CoroutineActionArgs3, float> _coroutineArgs3Queue;
		private float _currentTime;

		public CoroutineRunner(int capacity)
		{
			_coroutineVoidQueue = new(capacity);
			_coroutineArgQueue = new(capacity);
			_coroutineArgs2Queue = new(capacity);
			_coroutineArgs3Queue = new(capacity);
		}

		public void Start(CoroutineActionVoid action)
		{
			_coroutineVoidQueue.Enqueue(action, action.Delay + _currentTime);
		}

		public void Start(CoroutineActionArg action)
		{
			_coroutineArgQueue.Enqueue(action, action.Delay + _currentTime);
		}

		public void Start(CoroutineActionArgs2 action)
		{
			_coroutineArgs2Queue.Enqueue(action, action.Delay + _currentTime);
		}

		public void Start(CoroutineActionArgs3 action)
		{
			_coroutineArgs3Queue.Enqueue(action, action.Delay + _currentTime);
		}

		public void CancelCoroutine(CoroutineIdentity coroutineIdentity)
		{

		}

		public void Reset()
		{
			_coroutineVoidQueue.Clear();
			_coroutineArgQueue.Clear();
			_coroutineArgs2Queue.Clear();
			_coroutineArgs3Queue.Clear();
			_currentTime = 0;
		}

		public void Flush(float deltaTime)
		{
			_currentTime += deltaTime;

			while (_coroutineVoidQueue.TryPeek(out CoroutineActionVoid curAction,
											   out float delayTime))
			{
				if (delayTime > _currentTime)
					break;

				curAction.Execute();
				_coroutineVoidQueue.Dequeue();
			}

			while (_coroutineArgQueue.TryPeek(out CoroutineActionArg curAction,
											  out float delayTime))
			{
				if (delayTime > _currentTime)
					break;

				curAction.Execute();
				_coroutineArgQueue.Dequeue();
			}

			while (_coroutineArgs2Queue.TryPeek(out CoroutineActionArgs2 curAction,
												out float delayTime))
			{
				if (delayTime > _currentTime)
					break;

				curAction.Execute();
				_coroutineArgs2Queue.Dequeue();
			}

			while (_coroutineArgs3Queue.TryPeek(out CoroutineActionArgs3 curAction,
												out float delayTime))
			{
				if (delayTime > _currentTime)
					break;

				curAction.Execute();
				_coroutineArgs3Queue.Dequeue();
			}
		}
	}
}
