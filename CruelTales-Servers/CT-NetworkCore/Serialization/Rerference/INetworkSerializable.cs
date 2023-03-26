namespace CT.Network.Serialization.Rerference
{
	public interface INetworkSerializable
	{
		public int DataSize { get; }
		public void Deserialize(NetPacketReader reader);
		public void Serialize(NetPacketWriter writer);
	}
}
