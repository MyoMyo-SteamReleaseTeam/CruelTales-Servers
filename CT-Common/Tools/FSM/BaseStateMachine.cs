namespace CT.Common.Tools.FSM
{
	public class BaseStateMachine
	{
		public BaseState? CurrentState { get; protected set; }

		/// <summary>상태를 교체합니다.</summary>
		/// <param name="state">교체할 상태입니다.</param>
		public void ChangeState(BaseState state)
		{
			if (CurrentState == state)
				return;

			CurrentState?.Exit();
			CurrentState = state;
			CurrentState.Enter();
		}

		/// <summary>상태를 갱신합니다.</summary>
		/// <param name="deltaTime">Update deltaTime입니다.</param>
		public void UpdateState(float deltaTime)
		{
			CurrentState?.OnUpdate(deltaTime);
		}
	}

	/// <summary>상태기계입니다.</summary>
	/// <typeparam name="R">상태기계의 참조 타입입니다.</typeparam>
	public class BaseStateMachine<R> : BaseStateMachine
		where R : class
	{
		public R Reference { get; private set; }

		public BaseStateMachine(R reference)
		{
			Reference = reference;
		}
	}
}
