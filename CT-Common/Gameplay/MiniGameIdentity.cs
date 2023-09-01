using System;
using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	public struct MiniGameIdentity : IPacketSerializable, IEquatable<MiniGameIdentity>
	{
		public GameModeType Mode;
		public GameMapType Map;
		public GameMapThemeType Theme;

		public int SerializeSize => sizeof(ushort) * 3;

		public static bool operator ==(MiniGameIdentity lhs, MiniGameIdentity rhs)
		{
			return lhs.Mode == rhs.Mode && lhs.Map == rhs.Map && lhs.Theme == rhs.Theme;
		}

		public static bool operator !=(MiniGameIdentity lhs, MiniGameIdentity rhs)
		{
			return !(lhs == rhs);
		}

		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader)
		{
			reader.Ignore(6);
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(Mode);
			writer.Put(Map);
			writer.Put(Theme);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!reader.TryReadGameModeType(out Mode)) return false;
			if (!reader.TryReadGameMapType(out Map)) return false;
			if (!reader.TryReadGameMapTheme(out Theme)) return false;
			return true;
		}

		public override bool Equals(object? obj)
		{
			return obj is MiniGameIdentity && Equals((MiniGameIdentity)obj);
		}

		public bool Equals(MiniGameIdentity other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			return (Mode, Map, Theme).GetHashCode();
		}

		public override string ToString()
		{
			return $"(Mode: {Mode}, Map: {Map}, Theme: {Theme}";
		}
	}
}
