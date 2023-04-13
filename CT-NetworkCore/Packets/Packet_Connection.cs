/*
 * Generated File : Packet_Connection.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using CT.Network.DataType;
using CT.Network.Serialization;
using CT.Network.Serialization.Type;

namespace CT.Packets
{
	public sealed partial class UserProfile : IPacketSerializable
	{
		public NetStringShort Username;
		public NetStringShort Clothes;
	
		public int SerializeSize => Username.SerializeSize + Clothes.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			writer.Put(Username);
			writer.Put(Clothes);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Username = reader.ReadNetStringShort();
			Clothes = reader.ReadNetStringShort();
		}
	}
	
	public sealed class CS_Req_TryJoinGameInstance : PacketBase
	{
		public override PacketType PacketType => PacketType.CS_Req_TryJoinGameInstance;
	
		public ClientId Id = new();
		public ClientToken Token = new();
		public GameInstanceGuid MatchTo = new();
	
		public override int SerializeSize => Id.SerializeSize + Token.SerializeSize + MatchTo.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			Id.Serialize(writer);
			Token.Serialize(writer);
			MatchTo.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			Id.Deserialize(reader);
			Token.Deserialize(reader);
			MatchTo.Deserialize(reader);
		}
	}
	
	public sealed class SC_Ack_TryJoinGameInstance : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_Ack_TryJoinGameInstance;
	
		public AckJoinMatch AckResult;
	
		public override int SerializeSize =>  + 3;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			writer.Put(AckResult);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			AckResult = reader.ReadAckJoinMatch();
		}
	}
}