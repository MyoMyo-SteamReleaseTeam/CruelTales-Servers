using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct UserToken : IPacketSerializable
	{
		public ulong Token;

		public UserToken(ulong token)
		{
			Token = token;
		}

		public int SerializeSize => sizeof(ulong);

		public void Serialize(PacketWriter writer)
		{
			writer.Put(Token);
		}

		public void Deserialize(PacketReader reader)
		{
			Token = reader.ReadUInt64();
		}

		public override string ToString()
		{
			return $"Token:{Token}";
		}

		public bool IsValid()
		{
			return Token > 0;
		}
	}
}
