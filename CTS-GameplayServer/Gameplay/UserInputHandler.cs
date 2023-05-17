using System;
using System.Diagnostics;
using CT.Common.DataType.Input;
using CT.Networks.Runtimes;
using log4net;

namespace CTS.Instance.Gameplay
{
	public interface IJobHandler
	{
		public void Clear();
		public void Flush();
	}

	public interface IJobHandler<FArg> where FArg : struct
	{
		public void Clear();
		public void Flush(FArg arg);
	}

	[Obsolete]
	public class UserInputHandler : IJobHandler<float>
	{
		// Log
		private readonly ILog _log = LogManager.GetLogger(typeof(UserInputHandler));

		// DI
		private readonly GameplayInstance _gameInstance;

		// Job Queue
		private readonly JobQueue<UserInputJob, float> _jobQueue;

		public UserInputHandler(GameplayInstance gameInstance)
		{
			_gameInstance = gameInstance;
			_jobQueue = new(_gameInstance.ServerTimer, onJobExecuted);
		}

		public void Clear()
		{
			_jobQueue.Clear();
		}

		public void Flush(float delta)
		{
			_jobQueue.Flush(delta);
		}

		public void PushUserInput(UserInputJob job)
		{
			_jobQueue.Push(job);
		}

		private void onJobExecuted(UserInputJob job, float delta)
		{
			switch (job.Type)
			{
				case InputType.Movement: onInputMovement(job.InputMovement, delta); break;
				case InputType.Interaction: onInteraction(delta); break;
				case InputType.Action: onAction(delta); break;

				default:
					_log.Error($"Unknown clinet input detected!");
					Debug.Assert(false);
					break;
			}
		}

		private void onInputMovement(InputMovementData movementData, float delta)
		{

		}

		private void onInteraction(float delta)
		{
		}

		private void onAction(float delta)
		{

		}
	}
}
