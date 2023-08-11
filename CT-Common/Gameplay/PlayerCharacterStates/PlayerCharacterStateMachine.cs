using System.Diagnostics;
using CT.Common.DataType.Input;
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

		public void OnInputEvent(InputData inputData)
		{
			CurrentPlayerState.OnInputEvent(inputData);
		}
	}

	public abstract class BasePlayerCharacterState
		: BaseState<PlayerCharacterModel, PlayerCharacterStateMachine>
	{
		public abstract void OnInputEvent(InputData inputData);

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
			Reference.Player.StopMove();
		}

		public override void OnExit()
		{
		}

		public override void OnStateUpdate(float deltaTime)
		{

		}

		public override void OnInputEvent(InputData inputData)
		{
			InputType inputType = inputData.Type;

			if (inputType == InputType.Movement)
			{
				MovementInputData moveInput = inputData.MovementInput;

				Reference.MoveDirection = moveInput.MoveDirectionVector;
				Reference.UpdateDokzaDirection();

				BasePlayerCharacterState state =
					moveInput.MoveInputType == MovementType.Walk ? 
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
			Reference.Player.Move(Reference.MoveDirection, isWalk: false);
		}

		public override void OnExit()
		{
		}


		public override void OnStateUpdate(float deltaTime)
		{

		}

		public override void OnInputEvent(InputData inputData)
		{
			InputType inputType = inputData.Type;

			if (inputType == InputType.Movement)
			{
				MovementInputData moveInput = inputData.MovementInput;

				Reference.MoveDirection = moveInput.MoveDirectionVector;
				Reference.UpdateDokzaDirection();

				MovementType moveType = moveInput.MoveInputType;

				if (moveType == MovementType.Stop)
				{
					StateMachine.ChangeState(StateMachine.IdleState);
				}
				else if (moveType == MovementType.Walk)
				{
					StateMachine.ChangeState(StateMachine.WalkState);
				}
				else if (moveType == MovementType.Run)
				{
					ProxyDirection curDirection = Reference.GetProxyDirectionByDokza();
					if (Reference.Player.ProxyDirection != curDirection)
					{
						Reference.UpdateDokzaDirection(curDirection);
						Reference.UpdateAnimation(DokzaAnimationState.Run);
					}

					Reference.Player.Move(Reference.MoveDirection, isWalk: false);
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
			Reference.Player.Move(Reference.MoveDirection, isWalk: true);
		}

		public override void OnExit()
		{
		}

		public override void OnStateUpdate(float deltaTime)
		{

		}

		public override void OnInputEvent(InputData inputData)
		{
			InputType inputType = inputData.Type;

			if (inputType == InputType.Movement)
			{
				MovementInputData moveInput = inputData.MovementInput;

				Reference.MoveDirection = moveInput.MoveDirectionVector;
				Reference.UpdateDokzaDirection();

				MovementType moveType = moveInput.MoveInputType;

				if (moveType == MovementType.Stop)
				{
					StateMachine.ChangeState(StateMachine.IdleState);
				}
				else if (moveType == MovementType.Walk)
				{
					ProxyDirection curDirection = Reference.GetProxyDirectionByDokza();
					if (Reference.Player.ProxyDirection != curDirection)
					{
						Reference.UpdateDokzaDirection(curDirection);
						Reference.UpdateAnimation(DokzaAnimationState.Walk);
					}

					Reference.Player.Move(Reference.MoveDirection, isWalk: true);
				}
				else if (moveType == MovementType.Run)
				{
					StateMachine.ChangeState(StateMachine.RunState);
				}
			}
		}
	}
}
