using CT.Common.Serialization;

namespace CT.Common.DataType.Input
{
	/// <summary></summary>
	public enum InteractionType : byte
	{
		None = 0,

		/// <summary>
		/// 건드렸습니다.<br/>
		/// 단 한 번 건드리면 발동하는 경우 사용합니다.<br/>
		/// </summary>
		Trigger = 1,

		/// <summary>
		/// 눌렀습니다.<br/>
		/// 누르는 동안 진행되는 경우 사용합니다.<br/>
		/// </summary>
		Press = 2,

		/// <summary>
		/// 해제했습니다.
		/// 진행중인 상호작용을 해제하는 경우 사용합니다.
		/// </summary>
		Release = 3,

		/// <summary>
		/// 집어들었습니다.<br/>
		/// 아이템을 집었을 때 사용합니다.
		/// </summary>
		PickUp = 4,

		/// <summary>
		/// 떨어뜨렸습니다.<br/>
		/// 아이템을 떨어뜨렸을 때 사용합니다.
		/// </summary>
		Drop = 4,
	}

	/// <summary>상호작용 입력 데이터입니다.</summary>
	public struct InteractionInputData : IPacketSerializable
	{
		/// <summary>상호작용할 대상 입니다</summary>
		public NetworkIdentity TargetID;

		/// <summary>상호작용 타입입니다.</summary>
		public InteractionType InteractionType;

		public int SerializeSize => TargetID.SerializeSize + sizeof(InteractionType);

		public void Ignore(IPacketReader reader)
		{
			IgnoreStatic(reader);
		}

		public static void IgnoreStatic(IPacketReader reader)
		{
			NetworkIdentity.IgnoreStatic(reader);
			reader.Ignore(sizeof(InteractionType));
		}

		public void Serialize(IPacketWriter writer)
		{
			TargetID.Serialize(writer);
			writer.Put((byte)InteractionType);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!TargetID.TryDeserialize(reader)) return false;
			if (!reader.TryReadByte(out byte interactionType)) return false;

			InteractionType = (InteractionType)interactionType;

			return true;
		}
	}
}