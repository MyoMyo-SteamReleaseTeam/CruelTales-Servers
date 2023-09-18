using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	public struct JoinRequestToken : IPacketSerializable
	{
		public SkinSet ClientSkinSet;

		public int SerializeSize => ClientSkinSet.SerializeSize;

		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader)
		{
			SkinSet.IgnoreStatic(reader);
		}

		public void Serialize(IPacketWriter writer)
		{
			ClientSkinSet.Serialize(writer);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!ClientSkinSet.TryDeserialize(reader)) return false;

			return true;
		}
	}
}
