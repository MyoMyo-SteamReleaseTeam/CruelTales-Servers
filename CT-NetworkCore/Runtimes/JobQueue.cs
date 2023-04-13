using System;
using System.Collections.Generic;

namespace CT.Network.Runtimes
{
#if NET7_0_OR_GREATER
	/// <summary>스레드로 부터 안전한 Job Queue 입니다. 우선 순위 Job Queue를 포함합니다.</summary>
	/// <typeparam name="Job">Job의 타입입니다.</typeparam>
	/// <typeparam name="FArg">Flush 후 Job과 함께 넘겨지는 인자의 타입입니다.</typeparam>
	public class JobQueue<Job, FArg>
		where Job : struct
		where FArg : struct
	{
		// Stopwatch
		private readonly TickTimer _tickTimer;

		// Lock
		private readonly object _lock = new object();

		// Event
		private readonly Action<Job, FArg> _onJobExecute;

		// Normal Job
		private Queue<Job> _jobQueueBuffer = new();
		private Queue<Job> _jobQueueExecute = new();

		// Priority Job
		private PriorityQueue<Job, int> _jobPriorityQueueBuffer = new();
		private PriorityQueue<Job, int> _jobPriorityQueueExecute = new();

		public JobQueue(TickTimer tickTimer, Action<Job, FArg> onJobExecute)
		{
			_tickTimer = tickTimer;
			_onJobExecute = onJobExecute;
		}

		public void Push(Job job)
		{
			lock (_lock)
			{
				_jobQueueBuffer.Enqueue(job);
			}
		}

		public void Push(Job job, int executionTimeMs)
		{
			lock (_lock)
			{
				_jobPriorityQueueBuffer.Enqueue(job, executionTimeMs);
			}
		}

		public void Flush(FArg argument)
		{
			lock (_lock)
			{
				if (_jobQueueBuffer.Count == 0 && _jobPriorityQueueBuffer.Count == 0)
					return;

				var temp = _jobQueueExecute;
				_jobQueueExecute = _jobQueueBuffer;
				_jobQueueBuffer = temp;

				var ptemp = _jobPriorityQueueExecute;
				_jobPriorityQueueExecute = _jobPriorityQueueBuffer;
				_jobPriorityQueueBuffer = ptemp;
			}

			while (_jobQueueExecute.TryDequeue(out var job))
			{
				_onJobExecute.Invoke(job, argument);
			}

			while (true)
			{
				if (_jobPriorityQueueExecute.TryPeek(out _, out int ms))
				{
					if (_tickTimer.CurrentMs < ms)
						break;

					var job = _jobPriorityQueueExecute.Dequeue();
					_onJobExecute.Invoke(job, argument);
				}
			}
		}

		public void Clear()
		{
			lock (_lock)
			{
				_jobQueueBuffer.Clear();
				_jobQueueExecute.Clear();
				_jobPriorityQueueBuffer.Clear();
				_jobPriorityQueueExecute.Clear();
			}
		}
	}
#endif

	/// <summary>스레드로 부터 안전한 Job Queue 입니다.</summary>
	/// <typeparam name="Job">Job의 타입입니다.</typeparam>
	public class JobQueue<Job> where Job : struct
	{
		// Stopwatch
		private readonly TickTimer _tickTimer;

		// Lock
		private readonly object _lock = new object();

		// Event
		private readonly Action<Job> _onJobExecute;

		// Normal Job
		private Queue<Job> _jobQueueBuffer = new();
		private Queue<Job> _jobQueueExecute = new();

		public JobQueue(TickTimer tickTimer, Action<Job> onJobExecute)
		{
			_tickTimer = tickTimer;
			_onJobExecute = onJobExecute;
		}

		public void Push(Job job)
		{
			lock (_lock)
			{
				_jobQueueBuffer.Enqueue(job);
			}
		}

		public void Flush()
		{
			lock (_lock)
			{
				if (_jobQueueBuffer.Count == 0)
					return;

				var temp = _jobQueueExecute;
				_jobQueueExecute = _jobQueueBuffer;
				_jobQueueBuffer = temp;
			}

			while (_jobQueueExecute.TryDequeue(out var job))
			{
				_onJobExecute.Invoke(job);
			}
		}

		public void Clear()
		{
			lock (_lock)
			{
				_jobQueueBuffer.Clear();
				_jobQueueExecute.Clear();
			}
		}
	}
}
