using System.Diagnostics;
using System.Numerics;
using CT.Common.Gameplay.Players;
using CT.Common.Tools.FSM;

namespace CT.Common.Gameplay.PlayerCharacterStates
{
	public class PlayerCharacterStateMachine
		: BaseStateMachine<PlayerCharacterModel>
	{
		public PlayerCharacter_Idle IdleState { get; private set; } = new();
		public PlayerCharacter_Run RunState { get; private set; } = new();
		public PlayerCharacter_Walk WalkState { get; private set; } = new();

		public BasePlayerCharacterState CurrentPlayerState
		{
			get
			{
				var curState = CurrentState as BasePlayerCharacterState;
				Debug.Assert(curState != null);
				return curState;
			}
		}

		public PlayerCharacterStateMachine(PlayerCharacterModel reference) : base(reference)
		{
			IdleState.Initialize(this, Reference);
			RunState.Initialize(this, Reference);
			WalkState.Initialize(this, Reference);

			ChangeState(IdleState);
		}

		public void OnInputEvent(InputInfo inputInfo)
		{
			CurrentPlayerState.OnInputEvent(inputInfo);
		}
	}

	public abstract class BasePlayerCharacterState
		: BaseState<PlayerCharacterModel, PlayerCharacterStateMachine>
	{
		public abstract void OnInputEvent(InputInfo inputInfo);

		public sealed override void OnUpdate(float deltaTime)
		{
			Reference.Update(deltaTime);
			OnStateUpdate(deltaTime);
		}

		public abstract void OnStateUpdate(float deltaTime);
	}

	public class PlayerCharacter_Idle : BasePlayerCharacterState
	{
		public override void OnEnter()
		{
			Reference.UpdateDokzaDirection();
			Reference.UpdateAnimation(DokzaAnimationState.Idle);
			Reference.Player.UpdateRigidStop();
		}

		public override void OnExit()
		{
		}

		public override void OnStateUpdate(float deltaTime)
		{

		}

		public override void OnInputEvent(InputInfo inputInfo)
		{
			if (inputInfo.InputEvent == InputEvent.Movement)
			{
				Reference.MoveDirection = inputInfo.MoveDirection;
				Reference.UpdateDokzaDirection();

				BasePlayerCharacterState state = inputInfo.IsWalk ? 
					StateMachine.WalkState : StateMachine.RunState;

				StateMachine.ChangeState(state);
			}
		}
	}

	public class PlayerCharacter_Run : BasePlayerCharacterState
	{
		public override void OnEnter()
		{
			Reference.UpdateDokzaDirection();
			Reference.UpdateAnimation(DokzaAnimationState.Run);
			Reference.Player.UpdateRigid(Reference.MoveDirection, isWalk: false);
		}

		public override void OnExit()
		{
		}


		public override void OnStateUpdate(float deltaTime)
		{

		}

		public override void OnInputEvent(InputInfo inputInfo)
		{
			if (inputInfo.InputEvent == InputEvent.Movement)
			{
				Reference.MoveDirection = inputInfo.MoveDirection;
				Reference.UpdateDokzaDirection();

				if (!inputInfo.HasMovementInput)
				{
					StateMachine.ChangeState(StateMachine.IdleState);
					return;
				}

				if (inputInfo.IsWalk)
				{
					StateMachine.ChangeState(StateMachine.WalkState);
				}
				else
				{
					ProxyDirection curDirection = Reference.GetDokzaDirection();
					if (Reference.Player.ProxyDirection != curDirection)
					{
						Reference.UpdateDokzaDirection(curDirection);
						Reference.UpdateAnimation(DokzaAnimationState.Run);
					}

					Reference.Player.UpdateRigid(Reference.MoveDirection, isWalk: false);
				}
			}
		}
	}

	public class PlayerCharacter_Walk : BasePlayerCharacterState
	{
		public override void OnEnter()
		{
			Reference.UpdateDokzaDirection();
			Reference.UpdateAnimation(DokzaAnimationState.Walk);
			Reference.Player.UpdateRigid(Reference.MoveDirection, isWalk: true);
		}

		public override void OnExit()
		{
		}

		public override void OnStateUpdate(float deltaTime)
		{

		}

		public override void OnInputEvent(InputInfo inputInfo)
		{
			if (inputInfo.InputEvent == InputEvent.Movement)
			{
				Reference.MoveDirection = inputInfo.MoveDirection;
				Reference.UpdateDokzaDirection();

				if (!inputInfo.HasMovementInput)
				{
					StateMachine.ChangeState(StateMachine.IdleState);
					return;
				}

				if (inputInfo.IsWalk)
				{
					ProxyDirection curDirection = Reference.GetDokzaDirection();
					if (Reference.Player.ProxyDirection != curDirection)
					{
						Reference.UpdateDokzaDirection(curDirection);
						Reference.UpdateAnimation(DokzaAnimationState.Walk);
					}

					Reference.Player.UpdateRigid(Reference.MoveDirection, isWalk: true);
				}
				else
				{
					StateMachine.ChangeState(StateMachine.RunState);
				}
			}
		}
	}
}
