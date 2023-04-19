﻿using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct NetEntityId : IPacketSerializable
	{
		public byte ID;

		public int SerializeSize => sizeof(byte);

		public static implicit operator byte(NetEntityId value) => value.ID;
		public static explicit operator NetEntityId(byte value) => new NetEntityId(value);

		public NetEntityId(int value)
		{
			ID = (byte)value;
		}

		public NetEntityId(byte value)
		{
			ID = value;
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Put(ID);
		}

		public void Deserialize(PacketReader reader)
		{
			ID = reader.ReadByte();
		}
	}
}