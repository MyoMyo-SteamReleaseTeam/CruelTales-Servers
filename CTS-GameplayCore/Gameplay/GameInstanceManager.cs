using CTS.Instance.Gameplay;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using CTS.Instance;
using CTS.Instance.Services;
using System.Linq;
using log4net;
using CT.Network.DataType;
using CT.Network.Serialization;

namespace TestThreadManagement
{
	public class GameInstanceManager
	{
		private static ILog _log = LogManager.GetLogger(typeof(GameInstanceManager));

		private ServerServices _serverService;
		private TickTimer _serverTimer;

		private float _framePerMs;
		private readonly Dictionary<int, GameInstance> _gameInstanceById = new();
		private readonly int _gameCount;
		private float _sleepDiffMs = 0;
		private int _maxSleepDiffMs = 0;
		private float _sleepDiffLerp = 0.2f;

		private List<float> _deltaAverage = new List<float>();
		private readonly int _averageSample = 16;
		private int _averageCounter = 0;

		public GameInstanceManager(ServerServices serverService, ServerOption serverOption)
		{
			_serverService = serverService;
			_serverTimer = serverService.ServerTimer;

			_framePerMs = serverOption.FramePerMs;
			_maxSleepDiffMs = (int)(_framePerMs * 0.2f);
			if (_maxSleepDiffMs < 7)
			{
				_maxSleepDiffMs = 7;
			}

			_gameCount = serverOption.GameCount;
			for (int i = 0; i < _gameCount; i++)
			{
				var instance = new GameInstance(serverOption);
				_gameInstanceById.Add(instance.Id, instance);
			}

			for (int i = 0; i < _averageSample; i++)
			{
				_deltaAverage.Add(0);
			}
		}

		public void Start()
		{
			long lastTick = 0;
			float logTick = 0;

			while (true)
			{
				long currentTick = _serverTimer.CurrentTick;
				float deltaTime = _serverTimer.GetSecDeltaTime_Float(currentTick, lastTick);
				lastTick = currentTick;

				#region Update
				foreach (var  game in _gameInstanceById.Values)
				{
					game.ReadPackets();
					game.Update(deltaTime);
					game.WritePackets();
				}
				//var result = Parallel.ForEach(_games, (game) =>
				//{
				//	game.ReadPackets();
				//	game.Update(deltaTime);
				//	game.WritePackets();
				//});
				#endregion

				#region For Debug
				logTick += deltaTime;
				if (logTick > 5)
				{
					Console.WriteLine(deltaTime);
					logTick = 0;
					PrintCollecitonCount();
				}
				#endregion

				#region Sleep
				int deltaMs = (int)(deltaTime * 1000);

				this._deltaAverage[_averageCounter++ % _averageSample] = deltaMs;
				float average = _deltaAverage.Average();

				float timespent = _serverTimer.GetMsDeltaTime_Float(currentTick);
				float msLeft = _framePerMs - timespent;

				//if (deltaMs >= _framePerMs)
				{
					_log.Info(
						$"delta: {deltaMs:F2}\tAveDel: {average:F2}\tts: {timespent:F2}\t" +
						$"left: {msLeft:F2}\tDiff : {_sleepDiffMs:F2}\tsleep: {(int)(msLeft - _sleepDiffMs):F2}");
				}

				if (msLeft - _sleepDiffMs > 0)
				{
					Thread.Sleep((int)(msLeft - _sleepDiffMs));
				}

				float diff = _serverTimer.GetMsDeltaTime_Float(currentTick);
				if (diff - _framePerMs >= 0)
				{
					if (_sleepDiffMs < _maxSleepDiffMs)
					{
						_sleepDiffMs += (_maxSleepDiffMs - _sleepDiffMs) * _sleepDiffLerp;
						//_sleepDiffMs += 3;
					}
				}
				else
				{
					if (_sleepDiffMs > -_maxSleepDiffMs)
					{
						_sleepDiffMs -= (_maxSleepDiffMs - _sleepDiffMs) * _sleepDiffLerp;
						//_sleepDiffMs -= 3;
					}
				}
				#endregion
			}
		}

		public void PrintCollecitonCount()
		{
			_log.Info($"[Gen 0 : {GC.CollectionCount(0).ToString()}][Gen 1 : {GC.CollectionCount(1).ToString()}][Gen 2 : {GC.CollectionCount(2)}]");
		}

		public void OnReceivePacket(int instanceNumber, ClientToken clientToken, PacketReader reader)
		{
			//if (_gameInstanceById.TryGetValue(instanceNumber, out var instance))
			//{
			//	instance.
			//}
		}
	}
}
