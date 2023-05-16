using CT.Common.Serialization;
using CT.Packets;

namespace CTS.Instance.PacketCustom
{
	public sealed partial class SC_MiniGameData : PacketBase
	{
		public override PacketType PacketType => throw new System.NotImplementedException();

		public override int SerializeSize => throw new System.NotImplementedException();

		public override void Deserialize(IPacketReader reader)
		{
			throw new System.NotImplementedException();
		}

		public override void Serialize(IPacketWriter writer)
		{
			throw new System.NotImplementedException();
		}
	}
}