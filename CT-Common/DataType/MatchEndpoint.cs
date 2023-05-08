using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct MatchEndpoint : IPacketSerializable
	{
		public NetStringShort IpEndpoint;
		public ushort Port;

		public MatchEndpoint(string ipEndpoint, ushort port)
		{
			IpEndpoint = ipEndpoint;
			Port = port;
		}

		public int SerializeSize => IpEndpoint.SerializeSize + sizeof(ushort);

		public void Serialize(PacketWriter writer)
		{
			writer.Put(IpEndpoint);
			writer.Put(Port);
		}

		public void Deserialize(PacketReader reader)
		{
			IpEndpoint = reader.ReadNetStringShort();
			Port = reader.ReadUInt16();
		}

		public void Ignore(PacketReader reader) => IgnoreStatic(reader);

		public static void IgnoreStatic(PacketReader reader)
		{
			NetStringShort.IgnoreStatic(reader);
			reader.Ignore(sizeof(ushort));
		}
	}
}
