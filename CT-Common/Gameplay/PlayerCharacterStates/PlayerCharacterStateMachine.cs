﻿using System;
using System.Buffers;
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
		public PlayerCharacter_Push PushState { get; private set; } = new();
		public PlayerCharacter_Pushed PushedState { get; private set; } = new();

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
			PushState.Initialize(this, Reference);
			PushedState.Initialize(this, Reference);
			
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
			//Reference.UpdateProxyDirectionByMoveDirection();
			Reference.UpdateAnimation(DokzaAnimationState.Idle, Reference.Player.ProxyDirection);
			Reference.Player.Stop();
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

				if (moveInput.MoveInputType == MovementType.Walk)
				{
					Reference.Player.MoveDirection = moveInput.MoveDirectionVector;
					StateMachine.ChangeState(StateMachine.WalkState);
				}
				else if (moveInput.MoveInputType == MovementType.Run)
				{
					Reference.Player.MoveDirection = moveInput.MoveDirectionVector;
					StateMachine.ChangeState(StateMachine.RunState);
				}
			}
			else if (inputType == InputType.Action)
			{
				Reference.Player.MoveDirection = inputData.ActionDirection.GetDirection();
				StateMachine.ChangeState(StateMachine.PushState);
			}
		}
	}

	public class PlayerCharacter_Run : BasePlayerCharacterState
	{
		public override void OnEnter()
		{
			Reference.UpdateProxyDirectionByMoveDirection();
			Reference.UpdateAnimation(DokzaAnimationState.Run, Reference.Player.ProxyDirection);
			Reference.Player.Move(Reference.Player.MoveDirection, isWalk: false);
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
				Reference.Player.MoveDirection = moveInput.MoveDirectionVector;

				MovementType moveType = moveInput.MoveInputType;

				if (moveType == MovementType.Stop)
				{
					Reference.Player.ProxyDirection = Reference.Player.ProxyDirection;
					StateMachine.ChangeState(StateMachine.IdleState);
				}
				else if (moveType == MovementType.Walk)
				{
					StateMachine.ChangeState(StateMachine.WalkState);
				}
				else if (moveType == MovementType.Run)
				{
					if (Reference.UpdateProxyDirectionByMoveDirection())
					{
						Reference.UpdateAnimation(DokzaAnimationState.Run, 
							Reference.Player.ProxyDirection);
					}

					Reference.Player.Move(Reference.Player.MoveDirection, isWalk: false);
				}
			}
			else if (inputType == InputType.Action)
			{
				Reference.Player.MoveDirection = inputData.ActionDirection.GetDirection();
				StateMachine.ChangeState(StateMachine.PushState);
			}
		}
	}

	public class PlayerCharacter_Walk : BasePlayerCharacterState
	{
		public override void OnEnter()
		{
			Reference.UpdateProxyDirectionByMoveDirection();
			Reference.UpdateAnimation(DokzaAnimationState.Walk, Reference.Player.ProxyDirection);
			Reference.Player.Move(Reference.Player.MoveDirection, isWalk: true);
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
				Reference.Player.MoveDirection = moveInput.MoveDirectionVector;

				MovementType moveType = moveInput.MoveInputType;

				if (moveType == MovementType.Stop)
				{
					StateMachine.ChangeState(StateMachine.IdleState);
				}
				else if (moveType == MovementType.Walk)
				{
					if (Reference.UpdateProxyDirectionByMoveDirection())
					{
						Reference.UpdateAnimation(DokzaAnimationState.Walk, 
							Reference.Player.ProxyDirection);
					}

					Reference.Player.Move(Reference.Player.MoveDirection, isWalk: true);
				}
				else if (moveType == MovementType.Run)
				{
					StateMachine.ChangeState(StateMachine.RunState);
				}
			}
			else if (inputType == InputType.Action)
			{
				Reference.Player.MoveDirection = inputData.ActionDirection.GetDirection();
				StateMachine.ChangeState(StateMachine.PushState);
			}
		}
	}
	
	public class PlayerCharacter_Push : BasePlayerCharacterState
	{
		private float _pushPower = 12f;
		private float _pushFriction = 2.0f;
		private const float _pushLimitTime = 1f;
		private float _timer = 0f;
		
		public override void OnEnter()
		{
			_timer = 0f;
			Reference.Player.Stop();
			Reference.UpdateProxyDirectionToFront();
			Reference.UpdateAnimation(DokzaAnimationState.Push, Reference.Player.ProxyDirection);
			Reference.Player.Impluse(Reference.Player.MoveDirection, _pushPower, _pushFriction);
		}

		public override void OnExit()
		{
			Reference.Player.ResetImpluse();
		}

		public override void OnStateUpdate(float deltaTime)
		{
			Reference.OnDuringAction();

			_timer += deltaTime;
			if (_timer >= _pushLimitTime)
			{
				StateMachine.ChangeState(StateMachine.IdleState);
			}
		}

		public override void OnInputEvent(InputData inputData)
		{
			
		}
	}
	
	public class PlayerCharacter_Pushed : BasePlayerCharacterState
	{
		private const float _pushedPower = 9f;
		private const float _pushedFriction = 2.0f;
		private const float _animationLength = 1f;
		private float _timer = 0f;
		
		public override void OnEnter()
		{
			_timer = 0f;
			Reference.Player.Stop();
			Reference.Player.Impluse(Reference.Player.MoveDirection, _pushedPower, _pushedFriction);
			Reference.UpdateProxyDirectionToFront();
			Reference.UpdateAnimation(DokzaAnimationState.Pushed, Reference.Player.ProxyDirection);
		}

		public override void OnExit()
		{
			Reference.Player.ResetImpluse();
		}

		public override void OnStateUpdate(float deltaTime)
		{
			_timer += deltaTime;
			if (_timer >= _animationLength)
			{
				StateMachine.ChangeState(StateMachine.IdleState);
			}
		}

		public override void OnInputEvent(InputData inputData)
		{
			
		}
	}
}
