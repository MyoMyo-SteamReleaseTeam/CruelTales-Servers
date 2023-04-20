/*
 * Generated File : Packet_Gameplay.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Serialization.Type;

namespace CT.Packets
{
	public sealed partial class SC_OnClientEnter : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_OnClientEnter;
	
		public ClientId ClientId = new();
		public NetStringShort Username = new();
		public int Costume;
	
		public override int SerializeSize => ClientId.SerializeSize + Username.SerializeSize + 6;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			ClientId.Serialize(writer);
			Username.Serialize(writer);
			writer.Put(Costume);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			ClientId.Deserialize(reader);
			Username.Deserialize(reader);
			Costume = reader.ReadInt32();
		}
	}
	
	public sealed partial class SC_OnClientLeave : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_OnClientLeave;
	
		public ClientId ClientId = new();
	
		public override int SerializeSize => ClientId.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			ClientId.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			ClientId.Deserialize(reader);
		}
	}
	
	
	
	
}