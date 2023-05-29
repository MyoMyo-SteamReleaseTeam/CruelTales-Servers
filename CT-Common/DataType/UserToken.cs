using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct UserToken : IPacketSerializable
	{
		public ulong Token;
		public int SerializeSize => sizeof(ulong);

		public UserToken(ulong token) => Token = token;
		public void Serialize(IPacketWriter writer) => writer.Put(Token);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadUInt64(out Token);
		public bool IsValid() => Token > 0;
		public static bool operator ==(UserToken lhs, UserToken rhs) => lhs.Token == rhs.Token;
		public static bool operator !=(UserToken lhs, UserToken rhs) => lhs.Token != rhs.Token;
		public override int GetHashCode() => Token.GetHashCode();
		public override bool Equals(object? obj)
		{
			if (obj is not UserToken value)
				return false;
			return value == this;
		}
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(ulong));
		public override string ToString() => Token.ToString();
	}
}
