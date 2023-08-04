using CT.Common.Serialization;

namespace CT.Common.DataType.Input
{
	/// <summary>입력 타입입니다.</summary>
	public enum InputType : byte
	{
		None = 0,

		/// <summary>이동 입력입니다.</summary>
		Movement = 1,

		/// <summary>액션 입력입니다.</summary>
		Action = 2,

		/// <summary>상호작용 입력입니다.</summary>
		Interaction = 3,
	}

	/// <summary>플레이어의 키 입력 데이터입니다.</summary>
	public struct InputData : IPacketSerializable
	{
		/// <summary>입력 타입입니다.</summary>
		public InputType Type;

		public int SerializeSize
		{
			get
			{
				return Type switch
				{
					InputType.None => 0,
					InputType.Movement => MovementInput.SerializeSize,
					InputType.Action => ActionDirection.SerializeSize,
					InputType.Interaction => InteractionInput.SerializeSize,
					_ => 0,
				};
			}
		}

		/// <summary>이동 입력 데이터입니다.</summary>
		private MovementInputData _movementInput;
		public MovementInputData MovementInput
		{
			get => _movementInput;
			set
			{
				Type = InputType.Movement;
				_movementInput = value;
			}
		}

		/// <summary>액션을 수행한 방향 벡터입니다.</summary>
		private ByteDirection _actionDirection;
		public ByteDirection ActionDirection
		{
			get => _actionDirection;
			set
			{
				Type = InputType.Action;
				_actionDirection = value;
			}
		}

		/// <summary>상호작용 입력 데이터입니다.</summary>
		private InteractionInputData _interactionInput;
		public InteractionInputData InteractionInput
		{
			get => _interactionInput;
			set
			{
				Type = InputType.Action;
				_interactionInput = value;
			}
		}

		public void Ignore(IPacketReader reader)
		{
			switch (Type)
			{
				case InputType.Movement:
					MovementInputData.IgnoreStatic(reader);
					break;

				case InputType.Action:
					ByteDirection.IgnoreStatic(reader);
					break;

				case InputType.Interaction:
					InteractionInputData.IgnoreStatic(reader);
					break;
			}
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put((byte)Type);

			switch (Type)
			{
				case InputType.Movement:
					_movementInput.Serialize(writer);
					break;

				case InputType.Action:
					_actionDirection.Serialize(writer);
					break;

				case InputType.Interaction:
					_interactionInput.Serialize(writer);
					break;
			}
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!reader.TryReadByte(out byte typeValue)) return false;
			Type = (InputType)typeValue;

			switch (Type)
			{
				case InputType.Movement:
					if (!_movementInput.TryDeserialize(reader)) return false;
					break;

				case InputType.Action:
					if (!_actionDirection.TryDeserialize(reader)) return false;
					break;

				case InputType.Interaction:
					if (!_interactionInput.TryDeserialize(reader)) return false;
					break;
			}

			return true;
		}
	}
}