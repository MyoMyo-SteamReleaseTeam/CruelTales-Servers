using System.Numerics;
using CT.Common.Serialization;

public static class VectorExtensions
{
	public static void Put(this PacketWriter writer, Vector3 value)
	{
		writer.Put(value.X);
		writer.Put(value.Y);
		writer.Put(value.Z);
	}

	public static Vector3 ReadVector3(this PacketReader reader)
	{
		return new Vector3(reader.ReadSingle(),
						   reader.ReadSingle(),
						   reader.ReadSingle());
	}
}
