using System.Numerics;
#if UNITY_2021
using UnityEngine;
#endif
using CT.Common.Serialization;

public static class Vector2SerializeExtension
{
	public static void Serialize(this System.Numerics.Vector2 value, IPacketWriter writer)
	{
		writer.Put(value);
	}

#if UNITY_2021
	public static void Serialize(this UnityEngine.Vector2 value, IPacketWriter writer)
	{
		writer.Put(value);
	}
#endif

	public static void Put(this IPacketWriter writer, System.Numerics.Vector2 value)
	{
		writer.Put(value.X);
		writer.Put(value.Y);
	}

#if UNITY_2021
	public static void Put(this IPacketWriter writer, UnityEngine.Vector2 value)
	{
		writer.Put(value.x);
		writer.Put(value.y);
	}
#endif

	public static System.Numerics.Vector2 ReadVector2(this IPacketReader reader)
	{
		return new System.Numerics.Vector2(reader.ReadSingle(), reader.ReadSingle());
	}

	public static void IgnoreStatic(IPacketReader reader)
	{
		reader.Ignore(8);
	}
}

public static class Vector3SerializeExtension
{
	public static void Serialize(this System.Numerics.Vector3 value, IPacketWriter writer)
	{
		writer.Put(value);
	}

#if UNITY_2021
	public static void Serialize(this UnityEngine.Vector3 value, IPacketWriter writer)
	{
		writer.Put(value);
	}
#endif

	public static void Put(this IPacketWriter writer, System.Numerics.Vector3 value)
	{
		writer.Put(value.X);
		writer.Put(value.Y);
		writer.Put(value.Z);
	}

#if UNITY_2021
	public static void Put(this IPacketWriter writer, UnityEngine.Vector3 value)
	{
		writer.Put(value.x);
		writer.Put(value.y);
		writer.Put(value.z);
	}
#endif

	public static System.Numerics.Vector3 ReadVector3(this IPacketReader reader)
	{
		return new System.Numerics.Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	public static void IgnoreStatic(IPacketReader reader)
	{
		reader.Ignore(12);
	}
}
