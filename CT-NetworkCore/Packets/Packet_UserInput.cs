﻿/*
 * Generated File : Packet_UserInput.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using CT.Common.DataType;
using CT.Common.Serialization;

namespace CT.Packets
{
	public sealed partial class Input_Movement : IPacketSerializable
	{
		public NetVec2 PivotPosition = new();
		public NetVec2 Direction = new();
		public float Velocity;
	
		public int SerializeSize => PivotPosition.SerializeSize + Direction.SerializeSize + 4;
	
		public void Serialize(PacketWriter writer)
		{
			PivotPosition.Serialize(writer);
			Direction.Serialize(writer);
			writer.Put(Velocity);
		}
	
		public void Deserialize(PacketReader reader)
		{
			PivotPosition.Deserialize(reader);
			Direction.Deserialize(reader);
			Velocity = reader.ReadSingle();
		}
	}
	
	public sealed partial class Input_Action : IPacketSerializable
	{
		public int ActionType;
	
		public int SerializeSize =>  + 4;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(ActionType);
		}
	
		public void Deserialize(PacketReader reader)
		{
			ActionType = reader.ReadInt32();
		}
	}
	
	public sealed partial class CS_Req_UserInput_Movement : PacketBase
	{
		public override PacketType PacketType => PacketType.CS_Req_UserInput_Movement;
	
		public UserToken Token = new();
		public Input_Movement InputMovement = new();
	
		public override int SerializeSize => Token.SerializeSize + InputMovement.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			Token.Serialize(writer);
			InputMovement.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			Token.Deserialize(reader);
			InputMovement.Deserialize(reader);
		}
	}
	
	public sealed partial class CS_Req_UserInput_Action : PacketBase
	{
		public override PacketType PacketType => PacketType.CS_Req_UserInput_Action;
	
		public UserToken Token = new();
		public Input_Action InputAction = new();
	
		public override int SerializeSize => Token.SerializeSize + InputAction.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			Token.Serialize(writer);
			InputAction.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			Token.Deserialize(reader);
			InputAction.Deserialize(reader);
		}
	}
}