﻿using CT.Network.Serialization;

namespace CT.Network.Packet
{
	public enum TestType : byte
	{

	}

	public static class TestTypeExtension
	{
		public static void Put(this PacketWriter writer, TestType value)
		{
			writer.Put((byte)value);
		}

		public static TestType ReadTestType(this PacketReader reader)
		{
			return (TestType)reader.ReadByte();
		}
	}

	public enum PacketType : byte
	{
		PACKET_NONE = 0,
		PACKET_MOVE = 1,
		PACKET_SYNC = 2,
	}

	public enum EntityType : byte
	{
		ENTITY_NONE = 0,
		ENTITY_PLAYER = 1,
		ENTITY_MONSTER = 2,
	}

	public static class EntityTypeExtension
	{
		public static void Put(this PacketWriter writer, EntityType value)
		{
			writer.Put((byte)value);
		}

		public static EntityType ReadEntityType(this PacketReader reader)
		{
			return (EntityType)reader.ReadByte();
		}
	}
}
