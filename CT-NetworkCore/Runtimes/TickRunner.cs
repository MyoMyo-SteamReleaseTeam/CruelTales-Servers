using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;

namespace CT.Network.Runtimes
{
	public class TickRunner
	{
		public bool IsRunning { get; private set; } = false;
		public int FramePerMs { get; private set; }

		protected ILog _log;
		protected readonly TickTimer _tickTimer;

		private List<float> _deltaAverage = new List<float>();
		private readonly int _averageSample = 16;
		private int _averageCounter = 0;
		private bool _shouldStop = false;

		public event Action<float>? OnUpdate;

		public TickRunner(int famePerMs, TickTimer tickTimer, ILog logger)
		{
			FramePerMs = famePerMs;
			_tickTimer = tickTimer;
			_log = logger;

			for (int i = 0; i < _averageSample; i++)
			{
				_deltaAverage.Add(0);
			}
		}

		public void Run()
		{
			if (IsRunning)
			{
				throw new Exception($"This tick is already running!");
			}

			Thread thread = new Thread(start);
			thread.IsBackground = true;
			thread.Start();
		}

		private void start()
		{
			IsRunning = true;
			long lastTick = 0;

			while (true)
			{
				if (_shouldStop)
				{
					break;
				}

				// Set ticks
				long currentTick = _tickTimer.CurrentTick;
				float deltaTimeSec = TickTimer.GetDeltaTimeSec_Float(currentTick, lastTick);
				lastTick = currentTick;

				// Update call
				OnUpdate?.Invoke(deltaTimeSec);

				// Calculate sleep time
				float currentTimespent = _tickTimer.GetDeltaTimeMs_Float(currentTick);
				float sleepTimeDistance = FramePerMs - currentTimespent;

				//#region Debug
				//this._deltaAverage[_averageCounter++ % _averageSample] = deltaTimeSec;

				//if (deltaTimeSec * 1000 >= FramePerMs)
				//{
				//	Console.WriteLine(
				//		$"delta: {deltaTimeSec * 1000:F2}\t" +
				//		$"AveDel: {_deltaAverage.Average() * 1000:F2}\t" +
				//		$"TimeSpand: {currentTimespent:F2}\t" +
				//		$"SleepTime: {sleepTimeDistance:F2}\t");
				//}
				//#endregion

				if (sleepTimeDistance > 0)
				{
					Thread.Sleep((int)sleepTimeDistance);
				}
			}

			_shouldStop = false;
			IsRunning = false;
		}

		public void Suspend()
		{
			if (IsRunning)
			{
				_shouldStop = true;
			}
		}
	}
}