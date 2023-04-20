using CT.Common.DataType;
using CT.Common.Serialization;

namespace CTS.Instance.Gameplay.Entities
{
	public abstract partial class BaseEntity
	{
		public abstract NetEntityType EntityType { get; }
		public NetEntityId EntityId { get; protected set; }
		public NetTransform Transform { get; protected set; } = new();

		public virtual void SerializeSpawnDataTo(PacketWriter writer)
		{
			writer.Put(EntityType);
			writer.Put(EntityId);
			writer.Put(Transform);
		}

		public virtual void DeserializeSpawnDataFrom(PacketReader reader)
		{
			EntityId.Deserialize(reader);
			Transform.Deserialize(reader);
		}
	}
}
