﻿/*
 * Generated File : Packet_UserData.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using CT.Common.DataType;
using CT.Common.Serialization;

namespace CT.Packets
{
	public partial struct UserDataInfo : IPacketSerializable
	{
		public UserId UserId;
		public NetStringShort Username;
		public DokzaCostume UserCostume;
	
		public int SerializeSize => UserId.SerializeSize + Username.SerializeSize + UserCostume.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			UserId.Serialize(writer);
			Username.Serialize(writer);
			UserCostume.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			UserId.Deserialize(reader);
			Username.Deserialize(reader);
			UserCostume.Deserialize(reader);
		}
	}
	
	public partial struct DokzaCostume : IPacketSerializable
	{
		public int Head;
		public int Body;
	
		public int SerializeSize =>  + 8;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(Head);
			writer.Put(Body);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Head = reader.ReadInt32();
			Body = reader.ReadInt32();
		}
	}
}