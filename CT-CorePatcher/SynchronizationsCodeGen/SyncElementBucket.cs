using System.Collections.Generic;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class SyncElementBucket
	{
		public List<SyncPropertyToken> ReliableProperties { get; private set; } = new();
		public List<SyncFunctionToken> ReliableFunctions { get; private set; } = new();
		public List<SyncPropertyToken> UnreliableProperties { get; private set; } = new();
		public List<SyncFunctionToken> UnreliableFunctions { get; private set; } = new();
		public List<SyncPropertyToken> AllProperties { get; private set; } = new();
		public List<SyncFunctionToken> AllFunctions { get; private set; } = new();

		public void AddPropertyToken(SyncPropertyToken token)
		{
			AllProperties.Add(token);

			if (token.SyncType == SyncType.Reliable)
			{
				ReliableProperties.Add(token);
			}
			else if (token.SyncType == SyncType.Unreliable)
			{
				UnreliableProperties.Add(token);
			}
			else if (token.SyncType == SyncType.RelibaleOrUnreliable)
			{
				ReliableProperties.Add(token);
				UnreliableProperties.Add(token);
			}
		}

		public void AddFunctionToken(SyncFunctionToken token)
		{
			AllFunctions.Add(token);

			if (token.SyncType == SyncType.Reliable)
			{
				ReliableFunctions.Add(token);
			}
			else if (token.SyncType == SyncType.Unreliable)
			{
				UnreliableFunctions.Add(token);
			}
		}
	}
}