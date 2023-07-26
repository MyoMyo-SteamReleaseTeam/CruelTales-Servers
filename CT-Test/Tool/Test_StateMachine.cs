using System;
using CT.Common.Tools.FSM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tool
{
	[TestClass]
	public class Tester_StateMachine
	{
		public class Test_FSMReference
		{
			public int HP = 100;
			public void OnDamage(int damage)
			{
				HP -= damage;
			}
		}

		public class Test_FSMStateController : BaseStateMachine<Test_FSMReference>
		{
			public Test_FSMIdleState IdleState { get; private set; } = new();
			public Test_FSMDeathState DeathState { get; private set; } = new();
			public Test_FSMAttackState AttackState { get; private set; } = new();

			public Test_FSMStateController(Test_FSMReference reference) : base(reference)
			{
				IdleState.Initialize(this, reference);
				DeathState.Initialize(this, reference);
				AttackState.Initialize(this, reference);
			}
		}

		public class Test_FSMDeathState : BaseState<Test_FSMReference, Test_FSMStateController>
		{
			public override void OnEnter()
			{
				Console.WriteLine($"{nameof(Test_FSMDeathState)} State entered. Current state : {GetType().Name}");
			}

			public override void OnExit()
			{
				Console.WriteLine($"{nameof(Test_FSMDeathState)} State exit. Current state : {GetType().Name}");
			}

			public override void OnUpdate(float deltaTime)
			{
				Console.WriteLine($"{nameof(Test_FSMDeathState)} State update. Current state : {GetType().Name}");
			}
		}

		public class Test_FSMAttackState : BaseState<Test_FSMReference, Test_FSMStateController>
		{
			public override void OnEnter()
			{
				Console.WriteLine($"{nameof(Test_FSMAttackState)} State entered. Current state : {GetType().Name}");
			}

			public override void OnExit()
			{
				Console.WriteLine($"{nameof(Test_FSMAttackState)} State exit. Current state : {GetType().Name}");
			}

			public override void OnUpdate(float deltaTime)
			{
				Console.WriteLine($"{nameof(Test_FSMAttackState)} State update. Current state : {GetType().Name}");

				this.Reference.OnDamage(60);
				if (this.Reference.HP < 0)
				{
					this.StateMachine.ChangeState(StateMachine.DeathState);
				}
			}
		}

		public class Test_FSMIdleState : BaseState<Test_FSMReference, Test_FSMStateController>
		{
			public override void OnEnter()
			{
				Console.WriteLine($"{nameof(Test_FSMIdleState)} State entered. Current state : {GetType().Name}");
			}

			public override void OnExit()
			{
				Console.WriteLine($"{nameof(Test_FSMIdleState)} State exit. Current state : {GetType().Name}");
			}

			public override void OnUpdate(float deltaTime)
			{
				Console.WriteLine($"{nameof(Test_FSMIdleState)} State update. Current state : {GetType().Name}");

				this.Reference.OnDamage(60);
				if (this.Reference.HP < 0)
				{
					this.StateMachine.ChangeState(StateMachine.AttackState);
				}
			}
		}

		[TestMethod]
		public void Test_StateMachine()
		{
			Test_FSMReference reference = new();

			Test_FSMStateController controller = new(reference);

			controller.ChangeState(controller.IdleState);

			for (int i = 0; i < 5; i++)
			{
				controller.UpdateState(0);
			}
		}
	}
}