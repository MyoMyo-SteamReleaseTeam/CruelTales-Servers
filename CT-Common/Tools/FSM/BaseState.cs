namespace CT.Common.Tools.FSM
{
	public abstract class BaseState
	{
		/// <summary>현재 상태에 진입한 후의 경과시간 입니다.</summary>
		public float Elapsed { get; private set; }

		/// <summary>상태에 진입합니다.</summary>
		public void Enter()
		{
			Elapsed = 0;
			OnEnter();
		}

		/// <summary>상태를 갱신합니다.</summary>
		public void Update(float deltaTime)
		{
			Elapsed += deltaTime;
			OnUpdate(deltaTime);
		}

		/// <summary>상태를 종료합니다.</summary>
		public void Exit()
		{
			Elapsed = 0;
			OnExit();
		}

		public abstract void OnEnter();

		public abstract void OnUpdate(float deltaTime);

		public abstract void OnExit();
	}

	/// <summary>상태 클래스입니다. 상태 기계를 내부적으로 참조합니다.</summary>
	/// <typeparam name="R">상태기계의 참조 타입입니다.</typeparam>
	/// <typeparam name="C">상태기계의 타입입니다.</typeparam>
	public abstract class BaseState<R, C> : BaseState
		where R : class
		where C : BaseStateMachine<R>
	{
		public R? Reference { get; private set; }
		public C? StateMachine { get; private set; }

		/// <summary>상태에 진입합니다.</summary>
		/// <param name="reference">상태를 관리하는 컨트롤러입니다.</param>
		public void Initialize(C stateMachine, R reference)
		{
			StateMachine = stateMachine;
			Reference = reference;
		}
	}
}
