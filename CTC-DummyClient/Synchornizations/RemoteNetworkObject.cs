using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTC.Networks.SyncObjects.TestSyncObjects;

namespace CTC.Networks.Synchornizations
{
	public abstract class RemoteNetworkObject : IRemoteSynchronizable
	{
		public NetworkIdentity Identity { get; protected set; }
		public abstract NetworkObjectType Type { get; }
		public abstract void DeserializeSyncReliable(PacketReader reader);
		public abstract void DeserializeEveryProperty(PacketReader reader);
		public abstract void DeserializeSyncUnreliable(PacketReader reader);
	}
}