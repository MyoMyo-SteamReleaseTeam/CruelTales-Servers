﻿using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct NetVec2 : IPacketSerializable, IEquatable<NetVec2>
	{
		public float X;
		public float Y;
	
		public int SerializeSize =>  + 8;

		public NetVec2(float x, float y)
		{
			X = x;
			Y = y;
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(X);
			writer.Put(Y);
		}
	
		public bool TryDeserialize(IPacketReader reader)
		{
			return reader.TryReadSingle(out X) && reader.TryReadSingle(out Y);
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
		public bool Equals(NetVec2 other) => this == other;
		public override string ToString() => $"({X}, {Y})";
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(8);
	}

	[Serializable]
	public struct NetVecZ : IPacketSerializable, IEquatable<NetVecZ>
	{
		public float Z;
		public int SerializeSize => sizeof(float);
	
		public void Serialize(IPacketWriter writer) => writer.Put(Z);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadSingle(out Z);
		public static bool operator ==(NetVecZ lhs, NetVecZ rhs) => lhs.Z == rhs.Z && lhs.Z == rhs.Z;
		public static bool operator !=(NetVecZ lhs, NetVecZ rhs) => lhs.Z != rhs.Z || lhs.Z != rhs.Z;
		public bool Equals(NetVecZ other) => this == other;
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(float));
		public override bool Equals(object? obj) => obj is NetVecZ && Equals((NetVecZ)obj);
		public override int GetHashCode() => Z.GetHashCode();
	}

	[Serializable]
	public struct NetTransform : IPacketSerializable, IEquatable<NetTransform>
	{
		public NetVec2 Position;
		//public NetVecZ PositionZ = new();
		public NetVec2 Velocity;

#if NET
		public NetTransform()
		{
			Position = new NetVec2();
			Velocity = new NetVec2();
		}
#endif

		public int SerializeSize =>
			Position.SerializeSize + 
			//PositionZ.SerializeSize + 
			Velocity.SerializeSize;
	
		public void Serialize(IPacketWriter writer)
		{
			Position.Serialize(writer);
			//PositionZ.Serialize(writer);
			Velocity.Serialize(writer);
		}
	
		public bool TryDeserialize(IPacketReader reader)
		{
			return Position.TryDeserialize(reader) && Velocity.TryDeserialize(reader);
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
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(12);

		public bool Equals(NetTransform other)
		{
			throw new NotImplementedException();
		}
	}
}