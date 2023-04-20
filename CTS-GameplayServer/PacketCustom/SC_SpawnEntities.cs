using System.Collections.Generic;
using CT.Common.Serialization;
using CT.Packets;
using CTS.Instance.Gameplay.Entities;

namespace CTS.Instance.PacketCustom
{
	public sealed partial class SC_SpawnEntities : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_SpawnEntities;

		private List<BaseEntity> SpawnEntites = new();

		public override int SerializeSize => 1;

		public void SetEntities(ICollection<BaseEntity> entities)
		{
			SpawnEntites.AddRange(entities);
		}

		public override void Serialize(PacketWriter writer)
		{
			byte count = (byte)SpawnEntites.Count;
			writer.Put(count);
			for (int i = 0; i < count; i++)
			{
				SpawnEntites[i].SerializeSpawnDataTo(writer);
			}
		}

		public override void Deserialize(PacketReader reader) { }
	}
}