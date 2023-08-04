using System;
using System.Numerics;
using CT.Common.Serialization;
using CT.Common.Tools.Collections;

namespace CT.Common.DataType.Input
{
	/// <summary>움직임 타입입니다. 2비트입니다.</summary>
	public enum MovementType : byte
	{
		None = 0,

		/// <summary>정지했습니다.</summary>
		Stop = 1,

		/// <summary>걷기입니다.</summary>
		Walk = 2,
		
		/// <summary>뛰기 입니다.</summary>
		Run = 3,
	}

	/// <summary>플레이어의 키 입력 데이터입니다.</summary>
	public struct MovementInputData : IPacketSerializable
	{
		public int SerializeSize => sizeof(byte);

		/// <summary>이동 데이터 비트마스크입니다.</summary>
		public BitmaskByte MovementData;

		/// <summary>이동 타입입니다.</summary>
		public MovementType MovementInputType
		{
			get => (MovementType)(MovementData.Mask & 0b_0000_0011);
			set => MovementData.Mask = (byte)(MovementData.Mask & 0b_1111_1100 | (int)value);
		}

		/// <summary>이동 방향입니다. 8방향입니다.</summary>
		public InputDirection MovementDirection
		{
			get => (InputDirection)(MovementData.Mask & 0b_0001_1100 >> 2);
			set => MovementData.Mask = (byte)(MovementData.Mask & 0b_1110_0011 | (int)value << 2);
		}

		/// <summary>이동 방향 벡터입니다.</summary>
		public Vector2 MoveDirectionVector
		{
			get => MovementDirection.ToDirectionVector();
			set => MovementDirection = value.ToInputDirection();
		}

		[Obsolete("프로퍼티로 설정하세요.")]
		public void SetMovementInput(MovementType inputType,
									 InputDirection direction)
		{
			MovementData.Mask = (byte)(MovementData.Mask & 0b_1110_0000 |
				(int)direction << 2 | (int)inputType);
		}

		[Obsolete("프로퍼티로 설정하세요.")]
		public void SetMovementInput(MovementType inputType, Vector2 moveDirection)
		{
			SetMovementInput(inputType, moveDirection.ToInputDirection());
		}

		public void Ignore(IPacketReader reader)
		{
			IgnoreStatic(reader);
		}

		public static void IgnoreStatic(IPacketReader reader)
		{
			BitmaskByte.IgnoreStatic(reader);
		}

		public void Serialize(IPacketWriter writer)
		{
			MovementData.Serialize(writer);
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			return reader.TryReadBitmaskByte(out MovementData);
		}
	}
}