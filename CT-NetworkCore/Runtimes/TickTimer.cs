using System.Diagnostics;

namespace CT.Network.Runtimes
{
	public class TickTimer
	{
		public static readonly float TICK_PER_MS = 1 / (long)(Stopwatch.Frequency / 1000D);
		public static readonly float TICK_PER_SEC = Stopwatch.Frequency;

		private readonly Stopwatch _stopwatch = new Stopwatch();
		public long CurrentTick => _stopwatch.ElapsedTicks;
		public long CurrentMs => _stopwatch.ElapsedMilliseconds;
		public long CurrentSec => (long)(_stopwatch.ElapsedTicks * TICK_PER_SEC);

		public TickTimer()
		{
			_stopwatch.Start();
		}

		public float GetDeltaTimeSec_Float(long lastTick)
			=> GetDeltaTimeSec_Float(CurrentTick, lastTick);

		public int GetDeltaTimeSec_Int(long lastTick)
			=> GetDeltaTimeSec(CurrentTick, lastTick);

		public float GetDeltaTimeMs_Float(long lastTick)
			=> GetDeltaTimeMs_Float(CurrentTick, lastTick);

		public int GetDeltaTimeMs(long lastTick)
			=> GetDeltaTimeMs(CurrentTick, lastTick);

		public static float GetDeltaTimeSec_Float(long curTick, long lastTick)
			=> (curTick - lastTick) * TICK_PER_SEC;

		public static int GetDeltaTimeSec(long curTick, long lastTick)
			=> (int)((curTick - lastTick) * TICK_PER_SEC);

		public static float GetDeltaTimeMs_Float(long curTick, long lastTick)
			=> (curTick - lastTick) * TICK_PER_MS;

		public static int GetDeltaTimeMs(long curTick, long lastTick)
			=> (int)((curTick - lastTick) * TICK_PER_MS);
	}
}