using System.Collections.Generic;
using CT.Common.Serialization;
using CT.Packets;
using CTS.Instance.Gameplay.Entities;

namespace CTC.Networks.PacketCustom
{
	public sealed partial class SC_SpawnEntities : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_SpawnEntities;

		public List<BaseEntity> SpawnEntites = new();

		public override int SerializeSize => 1;

		public override void Serialize(PacketWriter writer) { }

		public override void Deserialize(PacketReader reader)
		{
			var count = reader.ReadByte();
			for (int i = 0; i < count; ++i)
			{

			}
		}
	}
}