﻿using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Networks.Synchronizations;

namespace CT.CorePatcher.Synchronizations
{
	public abstract class NetworkObject : IMasterSynchronizable
	{
		public NetworkIdentity Identity { get; protected set; }
		public abstract NetworkObjectType Type { get; }

		public bool IsDirty => throw new System.NotImplementedException();

		public abstract void SerializeSyncReliable(PacketWriter writer);
		public abstract void SerializeSyncUnreliable(PacketWriter writer);
		public abstract void SerializeEveryProperty(PacketWriter writer);

		public void ClearDirtyReliable()
		{
		}

		public void ClearDirtyUnreliable()
		{
		}
	}

	public class BBCC : NetworkObject
	{
		public override NetworkObjectType Type => NetworkObjectType.TestEntity;

		public override void SerializeEveryProperty(PacketWriter writer)
		{
			throw new System.NotImplementedException();
		}

		public override void SerializeSyncReliable(PacketWriter writer)
		{
			throw new System.NotImplementedException();
		}

		public override void SerializeSyncUnreliable(PacketWriter writer)
		{
			throw new System.NotImplementedException();
		}
	}

	public interface IMasterSynchronizable
	{
		public bool IsDirty { get; }
		public void SerializeSyncReliable(PacketWriter writer);
		public void SerializeSyncUnreliable(PacketWriter writer);
		public void SerializeEveryProperty(PacketWriter writer);
		public void ClearDirtyReliable();
		public void ClearDirtyUnreliable();
	}

	public interface IRemoteSynchronizable
	{
		public void DeserializeSyncReliable(PacketReader reader);
		public void DeserializeSyncUnreliable(PacketReader reader);
		public void DeserializeEveryProperty(PacketReader reader);
	}

	public abstract class RemoteNetworkObject : IRemoteSynchronizable
	{
		public NetworkIdentity Identity { get; protected set; }
		public abstract NetworkObjectType Type { get; }

		public abstract void DeserializeSyncReliable(PacketReader reader);
		public abstract void DeserializeEveryProperty(PacketReader reader);
		public abstract void DeserializeSyncUnreliable(PacketReader reader);
	}
}
