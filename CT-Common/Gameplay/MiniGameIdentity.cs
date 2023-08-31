using System;
using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	public struct MiniGameIdentity : IPacketSerializable, IEquatable<MiniGameIdentity>
	{
		public GameModeType ModeType;
		public GameMapType MapType;
		public GameMapTheme Theme;

		public int SerializeSize => sizeof(ushort) * 3;

		public static bool operator ==(MiniGameIdentity lhs, MiniGameIdentity rhs)
		{
			return lhs.ModeType == rhs.ModeType && lhs.MapType == rhs.MapType && lhs.Theme == rhs.Theme;
		}

		public static bool operator !=(MiniGameIdentity lhs, MiniGameIdentity rhs)
		{
			return !(lhs == rhs);
		}

		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public void IgnoreStatic(IPacketReader reader)
		{
			reader.Ignore(6);
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(ModeType);
			writer.Put(MapType);
			writer.Put(Theme);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!reader.TryReadGameModeType(out ModeType)) return false;
			if (!reader.TryReadGameMapType(out MapType)) return false;
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
			return (ModeType, MapType, Theme).GetHashCode();
		}
	}
}
