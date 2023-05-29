using System;
using CT.Packets;

namespace CT.Common.Serialization
{
	public abstract class PacketBase : IPacketSerializable
	{
		private static NotImplementedException _exception = new();

		public abstract PacketType PacketType { get; }
		public abstract int SerializeSize { get; }
		public abstract bool TryDeserialize(IPacketReader reader);
		public abstract void Serialize(IPacketWriter writer);
		public void Ignore(IPacketReader reader) => throw _exception;
	}
}
