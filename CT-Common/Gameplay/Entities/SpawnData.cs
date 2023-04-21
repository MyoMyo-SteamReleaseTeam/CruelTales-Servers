using CT.Common.DataType;
using CT.Common.Serialization;

namespace CT.Common.Gameplay.Entities
{
	public struct SpawnData
	{
		public NetEntityId EntityId;
		public NetTransform Transform;
		public IPacketSerializable Property;
	}
}
