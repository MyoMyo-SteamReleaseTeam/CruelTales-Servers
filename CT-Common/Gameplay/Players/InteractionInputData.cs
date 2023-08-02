using CT.Common.DataType;
using CT.Common.Serialization;

namespace CT.Common.Gameplay.Players
{
	public struct InteractionInputData : IPacketSerializable
	{
		public NetworkIdentity TargetID;
		public int InteractionType;

		public int SerializeSize => throw new System.NotImplementedException();

		public void Ignore(IPacketReader reader)
		{
			throw new System.NotImplementedException();
		}

		public void Serialize(IPacketWriter writer)
		{
			throw new System.NotImplementedException();
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			throw new System.NotImplementedException();
		}
	}
}