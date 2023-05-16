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

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(IpEndpoint);
			writer.Put(Port);
		}

		public void Deserialize(IPacketReader reader)
		{
			IpEndpoint = reader.ReadNetStringShort();
			Port = reader.ReadUInt16();
		}

		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);

		public static void IgnoreStatic(IPacketReader reader)
		{
			NetStringShort.IgnoreStatic(reader);
			reader.Ignore(sizeof(ushort));
		}
	}
}
