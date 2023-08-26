using System;
using CT.Common.DataType;

namespace CTS.Instance.Coroutines
{
	public struct CoroutineIdentity : IEquatable<CoroutineIdentity>
	{
		public NetworkIdentity NetworkIdentity;
		public int CoroutineCallIndex;

		public CoroutineIdentity(NetworkIdentity networkIdentity, int callIndex)
		{
			NetworkIdentity = networkIdentity;
			CoroutineCallIndex = callIndex;
		}

		public static bool operator ==(CoroutineIdentity lhs, CoroutineIdentity rhs)
		{
			return lhs.NetworkIdentity == rhs.NetworkIdentity &&
				   lhs.CoroutineCallIndex == rhs.CoroutineCallIndex;
		}
		public static bool operator !=(CoroutineIdentity lhs, CoroutineIdentity rhs)
		{
			return lhs.NetworkIdentity != rhs.NetworkIdentity ||
				   lhs.CoroutineCallIndex != rhs.CoroutineCallIndex;
		}

		public bool Equals(CoroutineIdentity other)
		{
			return this == other;
		}

		public override bool Equals(object? obj)
		{
			return obj is CoroutineIdentity && Equals((CoroutineIdentity)obj);
		}

		public override int GetHashCode()
		{
			return NetworkIdentity.GetHashCode();
		}
	}

}
