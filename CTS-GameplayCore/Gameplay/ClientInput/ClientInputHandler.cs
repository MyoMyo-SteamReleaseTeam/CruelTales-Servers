﻿using System.Diagnostics;
using CT.Network.DataType.Input;
using CT.Network.Runtimes;
using log4net;

namespace CTS.Instance.Gameplay.ClientInput
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

	public class ClientInputHandler : IJobHandler<float>
	{
		// Log
		private readonly ILog _log = LogManager.GetLogger(typeof(ClientInputHandler));

		// DI
		private readonly GameInstance _gameInstance;

		// Job Queue
		private readonly JobQueue<ClientInputJob, float> _jobQueue;

		public ClientInputHandler(GameInstance gameInstance)
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

		public void PushClientInput(ClientInputJob job)
		{
			_jobQueue.Push(job);
		}

		private void onJobExecuted(ClientInputJob job, float delta)
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
