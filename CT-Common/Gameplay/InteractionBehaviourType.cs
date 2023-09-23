namespace CT.Common.Gameplay
{
	public enum InteractorType : byte
	{
		None,
		Teleporter,
		Mission,
	}

	public enum InteractionBehaviourType : byte
	{
		None,

		/// <summary>범위에 닿자마자 동작합니다.</summary>
		Touch,

		/// <summary>범위에서 한 번 누르면 동작합니다.</summary>
		Tigger,

		/// <summary>범위에서 누르고 있어야 동작합니다.</summary>
		Prograss,

		/// <summary>범위에서 한 번 누르면 동작하고, 다시 한 번 누르면 해제됩니다.</summary>
		Toggle,
	}

	public enum InteractResultType : byte
	{
		/// <summary>아무런 상호작용이 없습니다.</summary>
		None,

		/// <summary>성공했습니다.</summary>
		Success = 1,

		/// <summary>상호작용 시작 대기중입니다.</summary>
		Success_Waitting,

		/// <summary>상호작용이 시작되었습니다.</summary>
		Success_Start,

		/// <summary>상호작용이 끝났습니다.</summary>
		Success_Finished,

		/// <summary>실패했습니다.</summary>
		Failed,

		/// <summary>잘못된 요청입니다.</summary>
		Failed_WrongRequest,

		/// <summary>취소되었습니다.</summary>
		Failed_Canceled,

		/// <summary>비활성화 되어있습니다.</summary>
		Failed_Disabled,

		/// <summary>누군가 상호작용중입니다.</summary>
		Failed_SomeoneIsInteracting,

		/// <summary>움직였습니다.</summary>
		Failed_YouMoved,

		/// <summary>아직 쿨타임이 있습니다.</summary>
		Failed_Cooltime,

		/// <summary>상호작용 할 수 없는 위치에 있습니다.</summary>
		Failed_WrongPosition,
	}

	public static class InteractResultTypeExtension
	{
		public static bool IsSuccess(this InteractResultType value)
		{
			return value != InteractResultType.None && 
				   value < InteractResultType.Failed;
		}
	}
}
