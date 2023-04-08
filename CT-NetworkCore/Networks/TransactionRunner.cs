using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CT.Network.Core
{
	public class TransactionRunner
	{
		public int Timeout { get; private set; } = 5000;
		public int TryInterval { get; private set; } = 500;
		public bool IsRunning { get; private set; } = false;
		public int RepeatTime => Timeout / TryInterval;

		private Action? mCurrentTransaction = null;
		
		public event Action? OnTransactionFailed;
		public event Action? OnTransactionSuccess;

		private Stopwatch mTimer = new Stopwatch();

		public TransactionRunner(int timeout, int tryInterval)
		{
			Timeout = timeout;
			TryInterval = tryInterval;
		}

		public void Start(Action transaction)
		{
			mCurrentTransaction = transaction;

			if (IsRunning || mCurrentTransaction == null)
			{
				Stop(false);
				return;
			}

			IsRunning = true;
			mTimer.Start();
			long elapsed = 0;

			Task.Run(() =>
			{
				while (elapsed < Timeout)
				{
					if (!IsRunning)
					{
						return;
					}

					mCurrentTransaction();
					Task.Delay(TryInterval).Wait();
					elapsed = mTimer.ElapsedMilliseconds;
				}

				if (!IsRunning)
				{
					return;
				}

				Stop(false);
			});
		}

		public void Stop(bool isSucess)
		{
			if (!IsRunning)
			{
				return;
			}

			IsRunning = false;
			mTimer.Reset();

			if (isSucess)
			{
				OnTransactionSuccess?.Invoke();
			}
			else
			{
				OnTransactionFailed?.Invoke();
			}
		}
	}
}