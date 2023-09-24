using System;
using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	[Serializable]
	public struct SkinSet : IPacketSerializable, IEquatable<SkinSet>
	{
		public const int SERIALIZE_SIZE = 4 * 14 + 4 * 3;

		public int Back;
		public int Tail;
		public int Cheek;
		public int Dress;
		public int Eyes;
		public int Eyebrow;
		public int FaceAcc;
		public int Hair;
		public int HairAcc;
		public int HairHelmet;
		public int Headgear;
		public int Lips;
		public int Nose;
		public int Shoes;
		public int Hammer;
		public NetColor SkinColor;
		public NetColor HairColor;
		public NetColor EyesColor;

		public int SerializeSize => SERIALIZE_SIZE;

		public static bool operator ==(SkinSet lhs, SkinSet rhs)
		{
			return
				lhs.Back == rhs.Back &&
				lhs.Tail == rhs.Tail &&
				lhs.Cheek == rhs.Cheek &&
				lhs.Dress == rhs.Dress &&
				lhs.Eyes == rhs.Eyes &&
				lhs.Eyebrow == rhs.Eyebrow &&
				lhs.FaceAcc == rhs.FaceAcc &&
				lhs.Hair == rhs.Hair &&
				lhs.HairAcc == rhs.HairAcc &&
				lhs.HairHelmet == rhs.HairHelmet &&
				lhs.Headgear == rhs.Headgear &&
				lhs.Lips == rhs.Lips &&
				lhs.Nose == rhs.Nose &&
				lhs.Shoes == rhs.Shoes &&
				lhs.Hammer == rhs.Hammer &&
				lhs.SkinColor == rhs.SkinColor &&
				lhs.HairColor == rhs.HairColor &&
				lhs.EyesColor == rhs.EyesColor;
		}

		public static bool operator !=(SkinSet lhs, SkinSet rhs)
		{
			return !(lhs == rhs);
		}

		public override bool Equals(object? obj)
		{
			return obj is SkinSet && Equals((SkinSet)obj);
		}

		public bool Equals(SkinSet other)
		{
			return this == other;
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(Back);
			writer.Put(Tail);
			writer.Put(Cheek);
			writer.Put(Dress);
			writer.Put(Eyes);
			writer.Put(Eyebrow);
			writer.Put(FaceAcc);
			writer.Put(Hair);
			writer.Put(HairAcc);
			writer.Put(HairHelmet);
			writer.Put(Headgear);
			writer.Put(Lips);
			writer.Put(Nose);
			writer.Put(Shoes);
			writer.Put(Hammer);
			writer.Put(SkinColor);
			writer.Put(HairColor);
			writer.Put(EyesColor);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!reader.TryReadInt32(out Back)) return false;
			if (!reader.TryReadInt32(out Tail)) return false;
			if (!reader.TryReadInt32(out Cheek)) return false;
			if (!reader.TryReadInt32(out Dress)) return false;
			if (!reader.TryReadInt32(out Eyes)) return false;
			if (!reader.TryReadInt32(out Eyebrow)) return false;
			if (!reader.TryReadInt32(out FaceAcc)) return false;
			if (!reader.TryReadInt32(out Hair)) return false;
			if (!reader.TryReadInt32(out HairAcc)) return false;
			if (!reader.TryReadInt32(out HairHelmet)) return false;
			if (!reader.TryReadInt32(out Headgear)) return false;
			if (!reader.TryReadInt32(out Lips)) return false;
			if (!reader.TryReadInt32(out Nose)) return false;
			if (!reader.TryReadInt32(out Shoes)) return false;
			if (!reader.TryReadInt32(out Hammer)) return false;
			if (!SkinColor.TryDeserialize(reader)) return false;
			if (!HairColor.TryDeserialize(reader)) return false;
			if (!EyesColor.TryDeserialize(reader)) return false;
			return true;
		}

		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);

		public static void IgnoreStatic(IPacketReader reader)
		{
			reader.Ignore(SERIALIZE_SIZE);
		}

		public override int GetHashCode()
		{
			return
			(
				Back,
				Tail,
				Cheek,
				Dress,
				Eyes,
				Eyebrow,
				FaceAcc,
				Hair,
				HairAcc,
				HairHelmet,
				Headgear,
				Lips,
				Nose,
				Shoes,
				Hammer,
				SkinColor,
				HairColor,
				EyesColor
			).GetHashCode();
		}
	}
}
