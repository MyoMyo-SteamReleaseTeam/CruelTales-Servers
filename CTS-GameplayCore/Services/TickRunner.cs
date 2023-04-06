using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;

namespace CTS.Instance.Services
{
	public abstract class TickRunner
	{
		public bool IsRunning { get; private set; } = false;
		public int FramePerMs { get; private set; }

		protected ILog _log;
		protected readonly TickTimer _tickTimer;

		private List<float> _deltaAverage = new List<float>();
		private readonly int _averageSample = 16;
		private int _averageCounter = 0;
		private bool _shouldStop = false;

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

		protected abstract void OnUpdate(float deltaTime);

		public void Run()
		{
			IsRunning = true;
			long lastTick = 0;

			while (true)
			{
				if (_shouldStop)
				{
					IsRunning = false;
					break;
				}

				// Set ticks
				long currentTick = _tickTimer.CurrentTick;
				float deltaTimeSec = _tickTimer.GetSecDeltaTime_Float(currentTick, lastTick);
				lastTick = currentTick;

				// On update
				OnUpdate(deltaTimeSec);

				// Calculate sleep time
				float currentTimespent = _tickTimer.GetMsDeltaTime_Float(currentTick);
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
		}

		public void Suspend()
		{
			_shouldStop = true;
		}
	}
}