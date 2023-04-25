using CT.Common.Serialization;
using CTC.Networks.SyncObjects.TestSyncObjects;

namespace CTC.Networks.Synchronizations
{
	public static class NetworkObjectTypeExtension
	{
		public static void Put(this PacketWriter writer, NetworkObjectType value)
		{
			writer.Put((byte)value);
		}

		public static NetworkObjectType ReadNetworkObjectType(this PacketReader reader)
		{
			return (NetworkObjectType)reader.ReadByte();
		}
	}
}
