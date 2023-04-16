using System;
using CT.Common.Serialization;
using CT.Packets;

namespace CT.Common.Packets
{
	public class SC_SpawnEntities : PacketBase
	{
		public override PacketType PacketType => throw new NotImplementedException();

		public override int SerializeSize => throw new NotImplementedException();

		public override void Deserialize(PacketReader reader)
		{
			throw new NotImplementedException();
		}

		public override void Serialize(PacketWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
