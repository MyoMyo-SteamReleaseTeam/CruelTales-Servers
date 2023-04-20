using CT.Common.DataType;
using CT.Common.Serialization;

namespace CTS.Instance.Gameplay.Entities
{
	public partial class Entity_Player : BaseEntity
	{
		public override NetEntityType EntityType => NetEntityType.Player;
		public ClientId OwnerId { get; protected set; }

		public void BindClient(ClientId clientId)
		{
			OwnerId = clientId;
		}

		public override void DeserializeSpawnDataFrom(PacketReader reader)
		{
			base.DeserializeSpawnDataFrom(reader);
			OwnerId.Deserialize(reader);
		}

		public override void SerializeSpawnDataTo(PacketWriter writer)
		{
			base.SerializeSpawnDataTo(writer);
			writer.Put(OwnerId);
		}
	}
}
