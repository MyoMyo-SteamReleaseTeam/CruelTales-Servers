using CT.Common.Serialization;
using CT.Packets;

namespace CTC.Networks.Packets
{
	public class SC_ReliableSynchroniation : PacketBase
	{
		public override PacketType PacketType => throw new System.NotImplementedException();

		public override int SerializeSize => throw new System.NotImplementedException();

		public override void Deserialize(PacketReader reader)
		{
			throw new System.NotImplementedException();
		}

		public override void Serialize(PacketWriter writer)
		{
			throw new System.NotImplementedException();
		}
	}
}