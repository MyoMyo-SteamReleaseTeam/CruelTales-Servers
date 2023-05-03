using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct NetVec2 : IPacketSerializable
	{
		public float X;
		public float Y;
	
		public int SerializeSize =>  + 8;

		public NetVec2(float x, float y)
		{
			X = x;
			Y = y;
		}

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

		public static bool operator ==(NetVec2 lhs, NetVec2 rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
		public static bool operator !=(NetVec2 lhs, NetVec2 rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y;
		public override int GetHashCode() => ValueTuple.Create(X, Y).GetHashCode();
		public override bool Equals(object? obj)
		{
			if (obj is not NetVec2 lhs)
				return false;
			return this == lhs;
		}
		public override string ToString() => $"({X}, {Y})";
		public static void Ignore(PacketReader reader) => reader.Ignore(8);
	}

	[Serializable]
	public struct NetVecZ : IPacketSerializable
	{
		public float Z;
	
		public int SerializeSize => sizeof(float);
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(Z);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Z = reader.ReadSingle();
		}

		public void Ignore(PacketReader reader) => reader.Ignore(sizeof(float));
	}

	[Serializable]
	public struct NetTransform : IPacketSerializable
	{
		public NetVec2 Position;
		//public NetVecZ PositionZ = new();
		public NetVec2 Velocity;

#if NET
		public NetTransform() {}
#endif

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

		public static bool operator ==(NetTransform lhs, NetTransform rhs) 
			=> lhs.Position == rhs.Position && lhs.Velocity == rhs.Velocity;
		public static bool operator !=(NetTransform lhs, NetTransform rhs)
			=> lhs.Position != rhs.Position || lhs.Velocity != rhs.Velocity;
		public override int GetHashCode()
			=> ValueTuple.Create(Position, Velocity).GetHashCode();
		public override bool Equals(object? obj)
		{
			if (obj is not NetTransform lhs)
				return false;
			return this == lhs;
		}
		public override string ToString() => $"([{nameof(Position)}:{Position}][{nameof(Velocity)}:{Velocity}])";
		public static void Ignore(PacketReader reader) => reader.Ignore(12);
	}
}