using CT.Common.Serialization;

namespace CT.Common.DataType
{
	public struct NetVec2 : IPacketSerializable
	{
		public float X;
		public float Y;
	
		public int SerializeSize =>  + 8;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(X);
			writer.Put(Y);
		}
	
		public void Deserialize(PacketReader reader)
		{
			X = reader.ReadSingle();
			Y = reader.ReadSingle();
		}
	}
	
	public struct NetVecZ : IPacketSerializable
	{
		public float Z;
	
		public int SerializeSize =>  + 4;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(Z);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Z = reader.ReadSingle();
		}
	}
	
	public sealed class NetTransform : IPacketSerializable
	{
		public NetVec2 Position = new();
		//public NetVecZ PositionZ = new();
		public NetVec2 Velocity = new();
	
		public int SerializeSize =>
			Position.SerializeSize + 
			//PositionZ.SerializeSize + 
			Velocity.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			Position.Serialize(writer);
			//PositionZ.Serialize(writer);
			Velocity.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Position.Deserialize(reader);
			//PositionZ.Deserialize(reader);
			Velocity.Deserialize(reader);
		}
	}
}