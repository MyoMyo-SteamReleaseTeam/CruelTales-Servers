using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Network.Serialization
{
	public interface IPacketSerializable
	{
		public int SerializeSize { get; }
		public void Serialize(PacketWriter writer);
		public void Deserialize(PacketReader reader);
	}
}
