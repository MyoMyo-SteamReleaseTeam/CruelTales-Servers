using CT.Common.Serialization;
using CTC.Networks.SyncObjects.TestSyncObjects;

namespace CTC.Networks.Synchronizations
{
	public static class NetworkObjectTypeExtension
	{
		public static void Put(this IPacketWriter writer, NetworkObjectType value)
		{
			writer.Put((byte)value);
		}

		public static NetworkObjectType ReadNetworkObjectType(this IPacketReader reader)
		{
			return (NetworkObjectType)reader.ReadByte();
		}
	}
}
