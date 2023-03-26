namespace CT.Network.Serialization
{
	public interface IPacketSerializable
	{
		public void Serialize(Stream stream);
		public void Deserialize(Stream stream);
	}
}
