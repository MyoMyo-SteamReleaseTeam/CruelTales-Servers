using System.Diagnostics;

namespace CT.Network.Runtimes
{
	public class TickTimer
	{
		public readonly double TickPerMilliseconds = Stopwatch.Frequency / 1000D;
		public readonly double TickPerSeconds = Stopwatch.Frequency;

		private readonly Stopwatch _stopwatch = new Stopwatch();
		public long CurrentTick => _stopwatch.ElapsedTicks;

		public TickTimer()
		{
			_stopwatch.Start();
		}

		public float GetSecDeltaTime_Float(long lastTick)
		{
			return (float)((CurrentTick - lastTick) / TickPerSeconds);
		}

		public float GetSecDeltaTime_Float(long curTick, long lastTick)
		{
			return (float)((curTick - lastTick) / TickPerSeconds);
		}

		public float GetMsDeltaTime_Float(long lastTick)
		{
			return (float)((CurrentTick - lastTick) / TickPerMilliseconds);
		}

		public float GetMsDeltaTime_Float(long curTick, long lastTick)
		{
			return (float)((curTick - lastTick) / TickPerMilliseconds);
		}

		public int GetMsDeltaTime(long curTick, long lastTick)
		{
			return (int)((curTick - lastTick) / TickPerMilliseconds);
		}

		public int GetMsDeltaTime(long lastTick)
		{
			return (int)((CurrentTick - lastTick) / TickPerMilliseconds);
		}
	}
}