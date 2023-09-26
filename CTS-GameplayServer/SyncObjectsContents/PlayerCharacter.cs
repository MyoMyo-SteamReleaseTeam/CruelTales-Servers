using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.DataType.Input;
using CT.Common.Gameplay;
using CT.Common.Gameplay.PlayerCharacterStates;
using CT.Common.Gameplay.Players;
using CTS.Instance.Coroutines;
using CTS.Instance.Data;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using KaNet.Physics;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class PlayerCharacter : MasterNetworkObject, IPlayerBehaviour
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(PlayerCharacter));

		public override VisibilityType Visibility => VisibilityType.ViewAndOwner;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		public float Speed = 14;

		private CoroutineRunnerVoid _skinInitRunner;

		[AllowNull] public NetworkPlayer NetworkPlayer { get; private set; }
		public const float ActionRadius = 1;

		// States
		[AllowNull] protected PlayerCharacterModel _playerModel;
		[AllowNull] public PlayerCharacterStateMachine StateMachine { get; private set; }

		#region Getter Setter

		public DokzaAnimationState AnimationState
		{
			get => _animationState;
			set => _animationState = value;
		}

		public ProxyDirection ProxyDirection
		{
			get => _proxyDirection;
			set => _proxyDirection = value;
		}

		public Vector2 MoveDirection { get; set; }

		public Vector2 ActionDirection { get; set; }

		public float AnimationTime
		{
			get => _animationTime;
			set => _animationTime = value;
		}

		#endregion

		public override void Constructor()
		{
			_playerModel = new(this);
			StateMachine = new(_playerModel);
			_physicsRigidBody.SetLayerMask(PhysicsLayerMask.Player);

			_skinInitRunner = new CoroutineRunnerVoid(this, sendRequestSkinInit);
		}

		public override void OnCreated()
		{
			_animationState = DokzaAnimationState.Idle;
			_proxyDirection = ProxyDirection.RightDown;
			StateMachine.ChangeState(StateMachine.IdleState);

			MoveDirection = Vector2.Zero;
			ActionDirection = Vector2.Zero;
			AnimationTime = 0;

			if (RoomSessionManager.PlayerStateTable.TryGetValue(UserId, out var state))
			{
				state.CurrentSkin = state.SelectedSkin;
			}
		}

		public override void OnDestroyed()
		{
			DropItem();
			base.OnDestroyed();
		}

		public void Initialize(NetworkPlayer player)
		{
			Owner = player.UserId;
			UserId = player.UserId;
			NetworkPlayer = player;
		}

		public override void OnUpdate(float deltaTime)
		{
			StateMachine.UpdateState(deltaTime);
			//Server_TestPositionTickByTick(Position);
		}

		public void Move(Vector2 moveDirection, bool isWalk)
		{
			if (KaPhysics.NearlyEqual(moveDirection.Length(), 0))
			{
				RigidBody.ChangeVelocity(moveDirection);
				return;
			}

			float speed = isWalk ? Speed * 0.5f : Speed;
			Vector2 velocity = Vector2.Normalize(moveDirection) * speed;
			RigidBody.ChangeVelocity(velocity);
		}

		public void Impluse(Vector2 direction, float power, float forceFriction)
		{
			RigidBody.Impulse(direction, power, forceFriction);
		}

		public void Stop()
		{
			RigidBody.ChangeVelocity(Vector2.Zero);
		}

		public void ResetImpluse()
		{
			RigidBody.ResetImpluse();
		}

		public void OnDuringAction()
		{
			if (PhysicsWorld.Raycast(RigidBody.Position,
									 ActionRadius, out var hits,
									 PhysicsLayerMask.Player))
			{
				foreach (var id in hits)
				{
					if (Identity.Id == id )
						continue;

					if (!WorldManager.TryGetNetworkObject(new(id), out var netObj))
						continue;

					OnActionCollide(netObj, out bool isInterrupt);

					if (isInterrupt)
						break;
				}
			}
		}

		/// <summary>망치 충돌 발생시 이벤트입니다.</summary>
		/// <param name="netObj">충돌한 객체입니다.</param>
		/// <param name="isBreak">충돌된 객체가 여러개인 경우 순회를 종료합니다.</param>
		public virtual void OnActionCollide(MasterNetworkObject netObj, out bool isBreak)
		{
			if (netObj is not PlayerCharacter other)
			{
				isBreak = false;
				return;
			}

			var curState = other.StateMachine.CurrentState;
			Vector2 direction = Vector2.Normalize(other.Position - Position);

			if (curState == other.StateMachine.PushState)
			{
				other.OnReactionBy(direction);
				this.OnReactionBy(-direction);
			}
			else if (curState != other.StateMachine.PushedState)
			{
				other.OnReactionBy(direction);
			}

			isBreak = true;
		}

		public void OnReactionBy(Vector2 direction)
		{
			ActionDirection = direction;
			StateMachine.ChangeState(StateMachine.PushedState);
		}
		
		public virtual void LoadDefaultPlayerSkin()
		{
			BroadcastOrderTest((int)UserId.Id, 0);
		}
		
		public void OnPushed(Vector2 pushedDirection)
		{
			NetworkPlayer.CameraController?.Server_Shake();
			DropItem(-pushedDirection);
		}

		public void DropItem(Vector2 direction = default)
		{
			var sceneController = GameplayController.SceneController;
			if (sceneController == null || FieldItem == FieldItemType.None)
			{
				return;
			}

			var item = sceneController.SpawnFieldItemBy(FieldItem, Position);
			if (direction == Vector2.Zero)
			{
				if (KaPhysics.NearlyEqual(RigidBody.LinearVelocity, Vector2.Zero))
				{
					direction = RandomHelper.RandomDirection();
				}
				else
				{
					direction = Vector2.Normalize(-RigidBody.LinearVelocity);
				}
			}

			item.RigidBody.Impulse(direction * 6.0f, 2.0f);
			FieldItem = FieldItemType.None;
		}

		#region Sync

		public void OnAnimationChanged(DokzaAnimationState state)
		{
			this.Server_OnAnimationChanged(state);
		}

		public void OnAnimationChanged(DokzaAnimationState state, ProxyDirection direction)
		{
			this.Server_OnAnimationChanged(state, direction);
		}

		public void OnProxyDirectionChanged(ProxyDirection direction)
		{
			this.Server_OnProxyDirectionChanged(direction);
		}

		public virtual partial void Client_RequestInput(NetworkPlayer player, InputData inputData)
		{
			if (!isValidRequset(player)) return;
			StateMachine.OnInputEvent(inputData);
		}

		public virtual partial void Client_TryDropItem(NetworkPlayer player)
		{
			if (!isValidRequset(player)) return;
			DropItem();
		}

		public void OrderTest(NetworkPlayer player, int fromServer)
		{
			this.Server_OrderTest(player, fromServer);
		}
		
		public void DelayedSkinInitTest()
		{
			_skinInitRunner.StartCoroutine(0.5f);
		}

		private void sendRequestSkinInit()
		{
			BroadcastOrderTest((int)_userId.Id, 1);
			Console.WriteLine($"Test to {(int)_userId.Id}");
		}
		
		public void BroadcastOrderTest(int userId, int fromServer)
		{
			this.Server_BroadcastOrderTest(userId, fromServer);
		}

		private bool _isClientSendingSkinData = false;
		// 명령어 정리
		// 0: NULL(STOP)
		// 1: Request skin data to server
		// 2: Start send skin data from server
		// 3: Request skin change
		public virtual partial void Client_RequestTest(NetworkPlayer player, int fromClient)
		{
			if (!RoomSessionManager.PlayerStateTable.TryGetValue(UserId, out var state))
				return;
			
			SkinSet oldSkinSet = state.CurrentSkin;
			
			switch (fromClient)
			{
				case 0:
					oldSkinSet.Cheek = 2_000_001;
					state.CurrentSkin = oldSkinSet;
					break;
				
				case 1:
					oldSkinSet.Cheek = 2_000_002;
					break;
			}
			
			state.CurrentSkin = oldSkinSet;
			
			//switch (fromClient)
			//{
			//	case -2:
			//		ChangePlayerTypeTo<WolfCharacter>();
			//		break;
				
			//	case -1:
			//		ChangePlayerTypeTo<PlayerCharacter>();
			//		break;
				
			//	case 0:
			//		if (!_isClientSendingSkinData)
			//			break;

			//		_isClientSendingSkinData = false;
			//		BroadcastOrderTest((int)_userId.Id, 2);
			//		foreach (var VARIABLE in CurrentSkinSet)
			//		{
			//			if (VARIABLE == 0)
			//				break;
						
			//			BroadcastOrderTest((int)_userId.Id, VARIABLE);
			//		}
			//		BroadcastOrderTest((int)_userId.Id, 0);
			//		break;
				
			//	case 1:
			//		Console.WriteLine($"Skin data request from {player.UserId.Id}");
			//		BroadcastOrderTest((int)_userId.Id, 2);

			//		if (!_isSkinInit)
			//		{
			//			for (int i = 0; i < DEFAULT_SKINSET.Length; i++)
			//			{
			//				CurrentSkinSet[i] = DEFAULT_SKINSET[i];
			//			}

			//			_isSkinInit = true;
			//		}
					
			//		foreach (var VARIABLE in CurrentSkinSet)
			//		{
			//			if (VARIABLE == 0)
			//				break;
						
			//			BroadcastOrderTest((int)_userId.Id, VARIABLE);
			//		}
					
			//		BroadcastOrderTest((int)_userId.Id, 0);
			//		break;
				
			//	case 3:
			//		for (int i = 0; i < CurrentSkinSet.Length; i++)
			//		{
			//			CurrentSkinSet[i] = 0;
			//		}
			//		_isClientSendingSkinData = true;
			//		break;
				
			//	case > 1000000:
			//		if (!_isClientSendingSkinData)
			//			break;
					
			//		for (int i = 0; i < CurrentSkinSet.Length; i++)
			//		{
			//			if (CurrentSkinSet[i] == 0)
			//			{
			//				CurrentSkinSet[i] = fromClient;
			//				break;
			//			}
			//		}
			//		break;
			//}
		}
		
		#endregion

		private bool isValidRequset(NetworkPlayer player)
		{
			if (_userId != player.UserId)
			{
				player.Session?.Disconnect(DisconnectReasonType.ServerError_UnauthorizedBehaviour);
				return false;
			}

			return true;
		}
	}
}
