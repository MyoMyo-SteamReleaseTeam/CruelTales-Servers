using System;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.Coroutines
{
	public struct CoroutineActionVoid
	{
		public MasterNetworkObject CallerObject;
		public Action Action;
		public CoroutineIdentity Identity;
		public float Delay;

		public CoroutineActionVoid(MasterNetworkObject callerObject,
								   CoroutineIdentity identity,
								   Action action,
								   float delay)
		{
			CallerObject = callerObject;
			Identity = identity;
			Action = action;
			Delay = delay;
		}

		public void Execute()
		{
			if (!CallerObject.TryPopWaittingCoroutine(Identity))
				return;

			if (!CallerObject.IsAlive)
				return;

			Action.Invoke();
		}
	}

	public struct CoroutineActionArg
	{
		public MasterNetworkObject CallerObject;
		public Action<Arg> Action;
		public CoroutineIdentity Identity;
		public Arg Argument;
		public float Delay;

		public CoroutineActionArg(MasterNetworkObject callerObject,
								  CoroutineIdentity identity,
								  Action<Arg> action,
								  Arg arg,
								  float delay)
		{
			CallerObject = callerObject;
			Identity = identity;
			Action = action;
			Argument = arg;
			Delay = delay;
		}

		public void Execute()
		{
			if (!CallerObject.TryPopWaittingCoroutine(Identity))
				return;

			if (!CallerObject.IsAlive)
				return;

			Action.Invoke(Argument);
		}
	}

	public struct CoroutineActionArgs2
	{
		public MasterNetworkObject CallerObject;
		public Action<Arg, Arg> Action;
		public CoroutineIdentity Identity;
		public Arg Argument0;
		public Arg Argument1;
		public float Delay;

		public CoroutineActionArgs2(MasterNetworkObject callerObject,
									CoroutineIdentity identity,
									Action<Arg, Arg> action,
									Arg arg0, Arg arg1,
									float delay)
		{
			CallerObject = callerObject;
			Identity = identity;
			Action = action;
			Argument0 = arg0;
			Argument1 = arg1;
			Delay = delay;
		}

		public void Execute()
		{
			if (!CallerObject.TryPopWaittingCoroutine(Identity))
				return;

			if (!CallerObject.IsAlive)
				return;

			Action.Invoke(Argument0, Argument1);
		}
	}

	public struct CoroutineActionArgs3
	{
		public MasterNetworkObject CallerObject;
		public Action<Arg, Arg, Arg> Action;
		public CoroutineIdentity Identity;
		public Arg Argument0;
		public Arg Argument1;
		public Arg Argument2;
		public float Delay;

		public CoroutineActionArgs3(MasterNetworkObject callerObject,
								   CoroutineIdentity identity,
								   Action<Arg, Arg, Arg> action,
								   Arg arg0, Arg arg1, Arg arg2,
								   float delay)
		{
			CallerObject = callerObject;
			Identity = identity;
			Action = action;
			Argument0 = arg0;
			Argument1 = arg1;
			Argument2 = arg2;
			Delay = delay;
		}

		public void Execute()
		{
			if (!CallerObject.TryPopWaittingCoroutine(Identity))
				return;

			if (!CallerObject.IsAlive)
				return;

			Action.Invoke(Argument0, Argument1, Argument2);
		}
	}
}