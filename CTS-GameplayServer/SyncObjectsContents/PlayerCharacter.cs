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

		public override VisibilityType Visibility => VisibilityType.View;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		public float Speed = 5.0f;

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

			if (RoomSessionManager.PlayerStateTable.TryGetValue(UserId, out var state))
			{
				state.CurrentSkin = state.SelectedSkin;
			}
		}

		public void BindNetworkPlayer(NetworkPlayer player)
		{
			Owner = player.UserId;
			UserId = player.UserId;
			NetworkPlayer = player;
			NetworkPlayer.BindViewTarget(RigidBody);
		}

		public override void OnUpdate(float deltaTime)
		{
			StateMachine.UpdateState(deltaTime);
			Server_TestPositionTickByTick(Position);
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

		public virtual void OnDuringAction()
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

					if (netObj is not PlayerCharacter other)
						continue;

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

					break;
				}
			}
		}

		public void OnReactionBy(Vector2 direction)
		{
			ActionDirection = direction;
			StateMachine.ChangeState(StateMachine.PushedState);
		}
		
		/// <summary>
		/// 플레이어의 타입을 T로 변경 시도합니다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		[Obsolete]
		public virtual PlayerCharacter ChangePlayerTypeTo<T>() where T : PlayerCharacter, new()
		{
			var _minigameController = GameplayManager.GameplayController.SceneController;
			if (NetworkPlayer == null || 
				GameplayManager.GameplayController == null ||
				_minigameController == null)
			{
				Console.WriteLine(@"NetWorkPlayer or GameplayController is Null");
				return null;
			}

			// 리스트에서 제거
			_minigameController.PlayerCharacterByPlayer.TryRemove(this);
			NetworkPlayer.ReleaseViewTarget();

			NetworkIdentity previousId = this.Identity;

			// 우선 생성 후 바인딩
			NetworkPlayer currentPlayer = NetworkPlayer;
			this.NetworkPlayer = null;
			var createdAvatar = WorldManager.CreateObject<T>(Position);
			createdAvatar.BindNetworkPlayer(currentPlayer);

			NetworkIdentity nextId = createdAvatar.Identity;
			Console.WriteLine($"PlayerCharacter switch {previousId} to {nextId}");

			// 원본 캐릭터 제거
			Destroy();

			// 현재 캐릭터 리스트에 추가
			_minigameController.PlayerCharacterByPlayer.Add(createdAvatar.NetworkPlayer, createdAvatar);
			return createdAvatar;
		}

		public virtual void LoadDefaultPlayerSkin()
		{
			BroadcastOrderTest((int)UserId.Id, 0);
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
			if (_userId != player.UserId)
			{
				player.Session?.Disconnect(DisconnectReasonType.ServerError_UnauthorizedBehaviour);
				return;
			}

			StateMachine.OnInputEvent(inputData);
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
	}
}
