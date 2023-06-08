#if NET
using System.Numerics;
#elif UNITY_2021
using UnityEngine;
#endif
using CT.Common.Serialization;

public static class Vector2Extension
{
	public static void Serialize(this Vector2 value, IPacketWriter writer)
	{
		writer.Put(value);
	}

	public static void Put(this IPacketWriter writer, Vector2 value)
	{
#if NET
		writer.Put(value.X);
		writer.Put(value.Y);
#elif UNITY_2021
		writer.Put(value.x);
		writer.Put(value.y);
#endif
	}

	public static Vector2 ReadVector2(this IPacketReader reader)
	{
		return new Vector2(reader.ReadSingle(),
						   reader.ReadSingle());
	}

	public static void IgnoreStatic(IPacketReader reader)
	{
		reader.Ignore(8);
	}
}

public static class Vector3Extension
{
	public static void Serialize(this Vector3 value, IPacketWriter writer)
	{
		writer.Put(value);
	}

	public static void Put(this IPacketWriter writer, Vector3 value)
	{
#if NET
		writer.Put(value.X);
		writer.Put(value.Y);
		writer.Put(value.Z);
#elif UNITY_2021
		writer.Put(value.x);
		writer.Put(value.y);
		writer.Put(value.z);
#endif
	}

	public static Vector3 ReadVector3(this IPacketReader reader)
	{
		return new Vector3(reader.ReadSingle(),
						   reader.ReadSingle(),
						   reader.ReadSingle());
	}

	public static void IgnoreStatic(IPacketReader reader)
	{
		reader.Ignore(12);
	}
}
