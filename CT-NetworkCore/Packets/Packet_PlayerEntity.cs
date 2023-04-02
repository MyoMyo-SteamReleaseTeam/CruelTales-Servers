using CT.Network.DataType;
using CT.Network.Serialization;
using CT.Network.Serialization.Type;

namespace CT.Packets
{
	public sealed partial class EntityPlayerState : IPacketSerializable
	{
		public NetEntityID NetID = new();
		public NetTransform Transform = new();
	
		public int SerializeSize => NetID.SerializeSize + Transform.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			NetID.Serialize(writer);
			Transform.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			NetID.Deserialize(reader);
			Transform.Deserialize(reader);
		}
	}
}