using System;
using CT.Packets;

namespace CT.Common.Serialization
{
	public abstract class PacketBase : IPacketSerializable
	{
		private static NotImplementedException _exception = new();

		public abstract PacketType PacketType { get; }
		public abstract int SerializeSize { get; }
		public abstract void Deserialize(PacketReader reader);
		public abstract void Serialize(PacketWriter writer);
		public static void IgnoreStatic(PacketReader reader) => throw _exception;
		public void Ignore(PacketReader reader) => throw _exception;
	}
}
