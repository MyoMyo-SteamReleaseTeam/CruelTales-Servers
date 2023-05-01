using CT.Common.Serialization;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Synchronizations
{
	//public enum NetworkObjectType
	//{
	//	None = 0,
	//}

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
