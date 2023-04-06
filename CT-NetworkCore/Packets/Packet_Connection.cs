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
	
	public sealed class Client_Req_TryJoinMatching : PacketBase
	{
		public override PacketType PacketType => PacketType.Client_Req_TryJoinMatching;
	
		public ClientId Id = new();
		public ClientToken Token = new();
		public RoomGuid MatchTo = new();
	
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
	
	public sealed class Server_Ack_TryJoinMatching : PacketBase
	{
		public override PacketType PacketType => PacketType.Server_Ack_TryJoinMatching;
	
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