using System;
using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	public struct GameSceneIdentity : IPacketSerializable, IEquatable<GameSceneIdentity>
	{
		public GameModeType Mode;
		public GameMapType Map;
		public GameMapThemeType Theme;

		public const int SERIALIZE_SIZE = sizeof(GameModeType) +
										  sizeof(GameMapType) +
										  sizeof(GameMapThemeType);

		public int SerializeSize => SERIALIZE_SIZE;

		public static bool operator ==(GameSceneIdentity lhs, GameSceneIdentity rhs)
		{
			return lhs.Mode == rhs.Mode && lhs.Map == rhs.Map && lhs.Theme == rhs.Theme;
		}

		public static bool operator !=(GameSceneIdentity lhs, GameSceneIdentity rhs)
		{
			return !(lhs == rhs);
		}

		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader)
		{
			reader.Ignore(SERIALIZE_SIZE);
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
			return obj is GameSceneIdentity && Equals((GameSceneIdentity)obj);
		}

		public bool Equals(GameSceneIdentity other)
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
