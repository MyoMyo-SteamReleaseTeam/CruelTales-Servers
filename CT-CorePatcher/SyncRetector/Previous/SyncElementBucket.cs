using System;
using System.Collections.Generic;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector.Previous
{
	public class SyncElementBucket
	{
		public List<SyncPropertyToken> ReliableProperties { get; private set; } = new();
		public List<SyncFunctionToken> ReliableFunctions { get; private set; } = new();
		public List<SyncPropertyToken> UnreliableProperties { get; private set; } = new();
		public List<SyncFunctionToken> UnreliableFunctions { get; private set; } = new();
		public List<SyncPropertyToken> AllProperties { get; private set; } = new();
		public List<SyncFunctionToken> AllFunctions { get; private set; } = new();

		public void AddPropertyToken(SyncPropertyToken token, SyncType syncType)
		{
			if (syncType == SyncType.RelibaleOrUnreliable)
			{
				throw new ArgumentException("Property는 Reliable 혹은 Unrealiable 둘 중 하나의 동기화 속성만 가능합니다.");
			}

			AllProperties.Add(token);

			if (syncType == SyncType.Reliable)
			{
				ReliableProperties.Add(token);
			}
			else if (syncType == SyncType.Unreliable)
			{
				UnreliableProperties.Add(token);
			}
			else if (syncType == SyncType.RelibaleOrUnreliable)
			{
				ReliableProperties.Add(token);
				UnreliableProperties.Add(token);
			}
		}

		public void AddFunctionToken(SyncFunctionToken token, SyncType syncType)
		{
			if (syncType == SyncType.RelibaleOrUnreliable)
			{
				throw new ArgumentException("Function은 Reliable 혹은 Unrealiable 둘 중 하나의 동기화 속성만 가능합니다.");
			}

			AllFunctions.Add(token);

			if (syncType == SyncType.Reliable)
			{
				ReliableFunctions.Add(token);
			}
			else if (syncType == SyncType.Unreliable)
			{
				UnreliableFunctions.Add(token);
			}
		}
	}
}