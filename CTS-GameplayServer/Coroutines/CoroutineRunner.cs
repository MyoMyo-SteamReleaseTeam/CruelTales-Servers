using System;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.Coroutines
{
	public class CoroutineRunnerBase
	{
		protected MasterNetworkObject _networkObject;
		protected CoroutineIdentity? _identity;

		public CoroutineRunnerBase(MasterNetworkObject netObject)
		{
			_networkObject = netObject;
		}

		public void StopCoroutine()
		{
			if (_identity == null)
				return;
			_networkObject.CancelCoroutine(_identity.Value);
			_identity = null;
		}
	}

	public class CoroutineRunner : CoroutineRunnerBase
	{
		private Action _cachedAction;

		public CoroutineRunner(MasterNetworkObject netObject, Action cachedAction)
			: base(netObject)
		{
			_cachedAction = cachedAction;
		}

		public void StartCoroutine(float delay)
		{
			StopCoroutine();
			_identity = _networkObject.StartCoroutine(_cachedAction, delay);
		}
	}

	public class CoroutineRunnerArg : CoroutineRunnerBase
	{
		private Action<Arg> _cachedAction;

		public CoroutineRunnerArg(MasterNetworkObject netObject, Action<Arg> cachedAction)
			: base(netObject)
		{
			_cachedAction = cachedAction;
		}

		public void StartCoroutine(Arg argument, float delay)
		{
			StopCoroutine();
			_identity = _networkObject.StartCoroutine(_cachedAction, argument, delay);
		}
	}

	public class CoroutineRunnerArgs2 : CoroutineRunnerBase
	{
		private Action<Arg, Arg> _cachedAction;

		public CoroutineRunnerArgs2(MasterNetworkObject netObject, Action<Arg, Arg> cachedAction)
			: base(netObject)
		{
			_cachedAction = cachedAction;
		}

		public void StartCoroutine(Arg argument0, Arg argument1, float delay)
		{
			StopCoroutine();
			_identity = _networkObject.StartCoroutine(_cachedAction, argument0, argument1, delay);
		}
	}

	public class CoroutineRunnerArgs3 : CoroutineRunnerBase
	{
		private Action<Arg, Arg, Arg> _cachedAction;

		public CoroutineRunnerArgs3(MasterNetworkObject netObject, Action<Arg, Arg, Arg> cachedAction)
			: base(netObject)
		{
			_networkObject = netObject;
			_cachedAction = cachedAction;
		}

		public void StartCoroutine(Arg argument0, Arg argument1, Arg argument2, float delay)
		{
			StopCoroutine();
			_identity = _networkObject.StartCoroutine(_cachedAction, argument0, argument1, argument2, delay);
		}
	}
}
