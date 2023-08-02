using System.Numerics;
using CT.Common.Serialization;
using CT.Common.Tools.Collections;

namespace CT.Common.Gameplay.Players
{
	/// <summary>플레이어의 키 입력 데이터입니다.</summary>
	public struct KeyInputData : IPacketSerializable
	{
		public const int INPUT_TYPE_OFFSET = 2;

		public int SerializeSize => sizeof(byte);

		public BitmaskByte Data;

		public bool HasMovementInput => Data[5];

		public MovementInputType MovementInputType
		{
			get => (MovementInputType)(Data.Mask & 0b_0000_0011);
			set => Data.Mask = (byte)((Data.Mask & 0b_1111_1100) | (int)value);
		}

		public InputDirection MovementDirection
		{
			get => (InputDirection)(Data.Mask & 0b_0001_1100 >> INPUT_TYPE_OFFSET);
			set => Data.Mask = (byte)((Data.Mask & 0b_1110_0011) | (int)value << INPUT_TYPE_OFFSET);
		}

		public Vector2 MoveDirectionVector
		{
			get => MovementDirection.GetDirectionVector();
			set => MovementDirection = value.GetInputDirection();
		}

		public bool HasInteraction
		{
			get => Data[5];
			set => Data[5] = value;
		}

		public bool HasAction
		{
			get => Data[6];
			set => Data[6] = value;
		}

		public void SetMovementInput(MovementInputType inputType,
									 InputDirection direction)
		{
			Data.Mask = (byte)((Data.Mask & 0b_1110_0000) |
				((int)direction << INPUT_TYPE_OFFSET) | ((int)inputType));
		}

		public void SetMovementInput(MovementInputType inputType, Vector2 moveDirection)
		{
			SetMovementInput(inputType, moveDirection.GetInputDirection());
		}

		public void Ignore(IPacketReader reader)
		{
			Data.Ignore(reader);
		}

		public void Serialize(IPacketWriter writer)
		{
			Data.Serialize(writer);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			return Data.TryDeserialize(reader);
		}
	}
}