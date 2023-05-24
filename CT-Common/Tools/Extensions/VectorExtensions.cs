#if NET
using System.Numerics;
#elif UNITY_2021
using UnityEngine;
#endif
using CT.Common.Serialization;

public static class VectorExtensions
{
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
}
