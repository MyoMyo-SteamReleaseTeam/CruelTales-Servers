using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using CT.Common.Serialization;

namespace CT.Common
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct Color32 : IPacketSerializable, IEquatable<Color32>
	{
		internal const int RedShift = 24;
		internal const int GreenShift = 16;
		internal const int BlueShift = 8;
		internal const int AlphaShift = 0;

		public byte R => (byte)(RGBA >> RedShift);
		public byte G => (byte)(RGBA >> GreenShift);
		public byte B => (byte)(RGBA >> BlueShift);
		public byte A => (byte)(RGBA >> AlphaShift);

		public float FR => KaMath.ByteToNormalizeFloat(R);
		public float FG => KaMath.ByteToNormalizeFloat(G);
		public float FB => KaMath.ByteToNormalizeFloat(B);
		public float FA => KaMath.ByteToNormalizeFloat(A);

		[FieldOffset(0)]
		public uint RGBA;

		public static readonly Color32 White	= new Color32(255, 255, 255, 255);
		public static readonly Color32 Black	= new Color32(0, 0, 0, 255);
		public static readonly Color32 Red		= new Color32(255, 0, 0, 255);
		public static readonly Color32 Green	= new Color32(0, 255, 0, 255);
		public static readonly Color32 Blue		= new Color32(0, 0, 255, 255);
		public static readonly Color32 Yellow	= new Color32(255, 255, 0, 255);
		public static readonly Color32 Purple	= new Color32(255, 0, 255, 255);
		public static readonly Color32 Cyan		= new Color32(0, 255, 255, 255);
		public static readonly Color32 Void		= new Color32(0, 0, 0, 0);

		public const int SERIALIZE_SIZE = sizeof(uint);
		public int SerializeSize => SERIALIZE_SIZE;

		/// <summary>RGBA 순서로 색상을 바인딩합니다.</summary>
		public Color32(uint value)
		{
			RGBA = value;
		}

		public Color32(float r, float g, float b, float a = 1.0f)
		{
			byte rv = KaMath.NormalizeFloatToByte(r);
			byte gv = KaMath.NormalizeFloatToByte(g);
			byte bv = KaMath.NormalizeFloatToByte(b);
			byte av = KaMath.NormalizeFloatToByte(a);
			this = new Color32(rv, gv, bv, av);
		}

		public Color32(byte r, byte g, byte b, byte a = 255)
		{
			RGBA = 0;
			RGBA |= (uint)(r << RedShift);
			RGBA |= (uint)(g << GreenShift);
			RGBA |= (uint)(b << BlueShift);
			RGBA |= (uint)(a << AlphaShift);
		}

		/// <summary>
		/// 16진수 코드로 색상을 생성합니다.
		/// FFFFFF RGB 형태로 6글자이거나 FFFFFFFF RGBA 형태로 8글자만 가능합니다.
		/// 해석할 수 없는 문자인 경우 RGBA가 각각 0인 색상을 생성합니다.
		/// </summary>
		/// <param name="hexCode">6, 8글자의 16진수 코드입니다.</param>
		public Color32(string hexCode)
		{
			this = ParseFromHex(hexCode);
		}

		/// <summary>
		/// 6자리 16진수 RGB 색상 코드를 색상 코드로 변환합니다.
		/// </summary>
		public static Color32 ParseFromHex(string colorHexCode)
		{
			int codeLength = colorHexCode.Length;

			if (codeLength == 6)
			{
				if (!IsColorHexCode(colorHexCode, 6))
				{
					return Void;
				}

				byte r = byte.Parse($"{colorHexCode[0..2]}", NumberStyles.HexNumber);
				byte g = byte.Parse($"{colorHexCode[2..4]}", NumberStyles.HexNumber);
				byte b = byte.Parse($"{colorHexCode[4..6]}", NumberStyles.HexNumber);
				return new Color32(r, g, b);
			}
			else if (codeLength == 8)
			{
				if (!IsColorHexCode(colorHexCode, 8))
				{
					return Void;
				}

				byte r = byte.Parse($"{colorHexCode[0..2]}", NumberStyles.HexNumber);
				byte g = byte.Parse($"{colorHexCode[2..4]}", NumberStyles.HexNumber);
				byte b = byte.Parse($"{colorHexCode[4..6]}", NumberStyles.HexNumber);
				byte a = byte.Parse($"{colorHexCode[6..8]}", NumberStyles.HexNumber);
				return new Color32(r, g, b, a);
			}

			return Void;
		}

		/// <summary>
		/// 해당 색상코드가 유효한지 여부를 반환합니다.
		/// </summary>
		public static bool IsColorHexCode(string colorHexCode, int length)
		{
			colorHexCode = colorHexCode.ToLower();

			if (colorHexCode.Length != length)
			{
				return false;
			}

			for (int i = 0; i < colorHexCode.Length; i++)
			{
				char c = colorHexCode[i];
				if (!c.IsHexChar())
					return false;
			}

			return true;
		}

#if NET
		/// <summary>.Net의 Color로 변환합니다.</summary>
		public System.Drawing.Color ToNetColor()
		{
			return System.Drawing.Color.FromArgb(R, G, B);
		}
#endif

#if UNITY_2021
		/// <summary>Unity의 Color로 변환합니다.</summary>
		public UnityEngine.Color ToUnityColor()
		{
			return new UnityEngine.Color(FR, FG, FB, FA);
		}
#endif

		public static bool operator ==(Color32 lhs, Color32 rhs)
		{
			return lhs.RGBA == rhs.RGBA;
		}

		public static bool operator !=(Color32 lhs, Color32 rhs)
		{
			return lhs.RGBA != rhs.RGBA;
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return (obj is not Color32 value) ? false : value == this;
		}

		public bool Equals(Color32 other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			return RGBA.GetHashCode();
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(RGBA);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			return reader.TryReadUInt32(out RGBA);
		}

		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);

		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);

		public override string ToString()
		{
			return RGBA.ToString("X8");
		}
	}
}
