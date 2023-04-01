namespace CT.Network.Serialization
{
	public interface IPacketSerializable
	{
		public int SerializeSize { get; }
		public void Serialize(PacketWriter writer);
		public void Deserialize(PacketReader reader);
	}
}
