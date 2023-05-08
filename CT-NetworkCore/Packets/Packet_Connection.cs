/*
 * Generated File : Packet_Connection.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using CT.Common.DataType;
using CT.Common.Serialization;

namespace CT.Packets
{
	public partial struct UserProfile : IPacketSerializable
	{
		public NetStringShort Username;
		public NetStringShort Clothes;
	
		public int SerializeSize => Username.SerializeSize + Clothes.SerializeSize;
	
		public void Serialize(PacketWriter writer)
		{
			Username.Serialize(writer);
			Clothes.Serialize(writer);
		}
	
		public void Deserialize(PacketReader reader)
		{
			Username.Deserialize(reader);
			Clothes.Deserialize(reader);
		}
	
		public static void IgnoreStatic(PacketReader reader) => throw new System.NotImplementedException();
		public void Ignore(PacketReader reader) => throw new System.NotImplementedException();
	}
	
	public sealed partial class CS_Req_TryEnterGameInstance : PacketBase
	{
		public override PacketType PacketType => PacketType.CS_Req_TryEnterGameInstance;
	
		public GameInstanceGuid MatchTo = new();
		public UserDataInfo UserDataInfo = new();
		public UserToken Token = new();
	
		public override int SerializeSize => MatchTo.SerializeSize + UserDataInfo.SerializeSize + Token.SerializeSize + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			MatchTo.Serialize(writer);
			UserDataInfo.Serialize(writer);
			Token.Serialize(writer);
		}
	
		public override void Deserialize(PacketReader reader)
		{
			MatchTo.Deserialize(reader);
			UserDataInfo.Deserialize(reader);
			Token.Deserialize(reader);
		}
	}
	
	public sealed partial class SC_Ack_TryEnterGameInstance : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_Ack_TryEnterGameInstance;
	
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
	
	public sealed partial class CS_Req_ReadyToSync : PacketBase
	{
		public override PacketType PacketType => PacketType.CS_Req_ReadyToSync;
	
	
	
		public override int SerializeSize =>  + 2;
	
		public override void Serialize(PacketWriter writer)
		{
			writer.Put(PacketType);
			
		}
	
		public override void Deserialize(PacketReader reader)
		{
			
		}
	}
}