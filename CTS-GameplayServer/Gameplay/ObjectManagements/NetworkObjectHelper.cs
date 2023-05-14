using System;
using System.Collections.Generic;
using CT.Common.Tools.Collections;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Gameplay.ObjectManagements
{
	public static class NetworkObjectHelper
	{
		private static Dictionary<Type, int> _initialCapacityByType = new()
		{
			{ typeof(int), 0 },
		};

		private static BidirectionalMap<Type, NetworkObjectType> _enumByType = new()
		{

		};
	}
}
