using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	public struct ServerRuntimeOption : IPacketSerializable, IEquatable<ServerRuntimeOption>
	{
		public float PhysicsStepTime;

		public int SerializeSize => sizeof(float);

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(PhysicsStepTime);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!reader.TryReadSingle(out PhysicsStepTime)) return false;
			return true;
		}

		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);

		public static void IgnoreStatic(IPacketReader reader)
		{
			reader.Ignore(sizeof(float));
		}

		public static bool operator ==(ServerRuntimeOption left, ServerRuntimeOption right)
		{
			return left.PhysicsStepTime == right.PhysicsStepTime;
		}

		public static bool operator !=(ServerRuntimeOption left, ServerRuntimeOption right)
		{
			return !(left == right);
		}

		public bool Equals(ServerRuntimeOption other)
		{
			return this == other;
		}

		public override bool Equals(object? obj)
		{
			return obj is ServerRuntimeOption && Equals((ServerRuntimeOption)obj);
		}

		public override int GetHashCode()
		{
			return PhysicsStepTime.GetHashCode();
		}
	}
}
