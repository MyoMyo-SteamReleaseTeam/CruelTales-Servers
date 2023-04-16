using CT.Common.DataType;
using CT.Common.Serialization;

namespace CTS.Instance.Gameplay.Entities
{
	public abstract class BaseEntity
	{
		public abstract NetEntityType EntityType { get; }
		public NetEntityId EntityId { get; protected set; }
		public NetTransform Transform { get; protected set; } = new();

		public virtual void Initialize(NetEntityId id, NetVec2 position)
		{
			EntityId = id;
			Transform.Position = position;
		}
	}

	public class Entity_Player : BaseEntity
	{
		public override NetEntityType EntityType => NetEntityType.Player;
		public ClientId OwnerId { get; protected set; }



		public void BindClient(ClientId clientId)
		{
			OwnerId = clientId;
		}
	}
}
