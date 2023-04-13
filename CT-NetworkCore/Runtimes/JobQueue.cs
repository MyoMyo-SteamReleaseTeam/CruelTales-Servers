using System;
using System.Collections.Generic;

namespace CT.Network.Runtimes
{
	public class JobQueue<T> where T : struct
	{
		// Stopwatch
		private readonly TickTimer _tickTimer;

		// Lock
		private readonly object _lock = new object();

		// Event
		private readonly Action<T> _onJobExecute;

		// Normal Job
		private Queue<T> _jobQueueBuffer = new();
		private Queue<T> _jobQueueExecute = new();

		// Priority Job
		private PriorityQueue<T, int> _jobPriorityQueueBuffer = new();
		private PriorityQueue<T, int> _jobPriorityQueueExecute = new();

		public JobQueue(TickTimer tickTimer, Action<T> onJobExecute)
		{
			_tickTimer = tickTimer;
			_onJobExecute = onJobExecute;
		}

		public void Push(T job)
		{
			lock (_lock)
			{
				_jobQueueBuffer.Enqueue(job);
			}
		}

		public void Push(T job, int executionTimeMs)
		{
			lock (_lock)
			{
				_jobPriorityQueueBuffer.Enqueue(job, executionTimeMs);
			}
		}

		public void Flush()
		{
			lock (_lock)
			{
				var temp = _jobQueueExecute;
				_jobQueueExecute = _jobQueueBuffer;
				_jobQueueBuffer = temp;

				var ptemp = _jobPriorityQueueExecute;
				_jobPriorityQueueExecute = _jobPriorityQueueBuffer;
				_jobPriorityQueueBuffer = ptemp;
			}

			while (_jobQueueExecute.TryDequeue(out var job))
			{
				_onJobExecute.Invoke(job);
			}
			
			while (true)
			{
				if (_jobPriorityQueueExecute.TryPeek(out _, out int ms))
				{
					if (_tickTimer.CurrentMs < ms)
						break;

					var job = _jobPriorityQueueExecute.Dequeue();
					_onJobExecute.Invoke(job);
				}
			}
		}
	}
}
