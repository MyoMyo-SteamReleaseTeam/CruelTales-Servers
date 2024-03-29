﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Infos;
using CT.Networks;
using CTS.Instance.Coroutines;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using KaNet.Physics;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class Interactor : MasterNetworkObject
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(Interactor));

		public override VisibilityType Visibility => VisibilityType.View;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		[AllowNull] private HashSet<UserId> _cooltimePlayers;
		[AllowNull] private Action<Arg> _onCooltimeEnd;

		public bool HasCooltime => Cooltime > 0;

		public override void Constructor()
		{
			_onCooltimeEnd = onCooltimeEnd;
			_cooltimePlayers = new HashSet<UserId>(GlobalNetwork.SYSTEM_MAX_USER);
		}

		public virtual void Initialize(in InteractorInfo info)
		{
			BehaviourType = info.BehaviourType;
			Size = info.Size;
			ProgressTime = info.ProgressTime;
			Cooltime = info.Cooltime;
		}

		public virtual void OnInteracted(NetworkPlayer player, PlayerCharacter playerCharacter) {}

		public override void OnUpdate(float deltaTime)
		{
			if (BehaviourType == InteractionBehaviourType.Touch)
			{
				if (Interactable == false)
					return;

				if (!tryGetHits(out var hits))
					return;

				foreach (var id in hits)
				{
					if (!WorldManager.TryGetNetworkObject(new NetworkIdentity(id), out var netObj))
						continue;

					if (netObj is not PlayerCharacter player)
						continue;

					OnInteracted(player.NetworkPlayer, player);
				}
			}
		}

		public virtual partial void Client_TryInteract(NetworkPlayer player)
		{
			if (Interactable == false)
				return;

			if (!checkValidationRequest(player))
			{
				_log.Warn($"Authentication failed! Wrong interaction request! Player: {player}");
				Anticheat.CheatDetected(player);
				Server_InteractResult(InteractResultType.None);
				return;
			}

			// Check cooltime
			if (HasCooltime)
			{
				if (HasPlayerCooltime(player))
				{
					//Server_InteractResult(InteractResultType.Failed_Cooltime);
					return;
				}

				onCooltimeStart(player);
				StartCoroutine(_onCooltimeEnd, new(player.UserId), Cooltime);
			}

			// Check collide
			if (!isPlayerCollided(player, out var playerCharacter))
			{
				Server_InteractResult(InteractResultType.Failed_WrongPosition);
				return;
			}

			switch (BehaviourType)
			{
				case InteractionBehaviourType.Touch:
					// You cannot interact through touch behavior.
					Server_InteractResult(InteractResultType.Failed_WrongRequest);
					return;

				case InteractionBehaviourType.Tigger:
					OnInteracted(player, playerCharacter);
					Server_InteractResult(InteractResultType.Success);
					break;

				case InteractionBehaviourType.Progress:
					break;

				case InteractionBehaviourType.Toggle:
					break;

				case InteractionBehaviourType.None:
				default:
					Server_InteractResult(InteractResultType.None);
					return;
			}

			// Run cooltime
			if (HasCooltime)
			{
				onCooltimeStart(player);
				StartCoroutine(_onCooltimeEnd, new(player.UserId), Cooltime);
			}
		}

		public virtual partial void Client_TryCancel(NetworkPlayer player)
		{
			Server_InteractResult(InteractResultType.Failed_Canceled);
		}

		private bool isPlayerCollided(NetworkPlayer player,
									  [MaybeNullWhen(false)]
									  out PlayerCharacter playerCharacter)
		{
			var currentScene = GameplayController.SceneController;

			if (currentScene == null ||
				!currentScene.TryGetPlayerCharacter(player, out var character))
			{
				playerCharacter = null;
				return false;
			}

			playerCharacter = character;

			if (!tryGetHits(out var hits))
				return false;

			bool isCollide = false;

			foreach (var id in hits)
			{
				if (id == character.Identity.Id)
				{
					isCollide = true;
					break;
				}
			}

			return isCollide;
		}

		private bool tryGetHits([MaybeNullWhen(false)] out List<int> hits)
		{
			InteractorColliderShapeType shapeType = Size.ShapeType;
			if (shapeType == InteractorColliderShapeType.Box)
			{
				if (!PhysicsWorld.Raycast(Position, Size.Width, Size.Height,
										  out hits, PhysicsLayerMask.Player))
				{
					return false;
				}
			}
			else if (shapeType == InteractorColliderShapeType.Circle)
			{
				if (!PhysicsWorld.Raycast(Position, Size.Radius,
										  out hits, PhysicsLayerMask.Player))
				{
					return false;
				}
			}
			else
			{
				hits = null;
				return false;
			}

			return true;
		}

		/// <summary>
		/// 클라이언트의 요청이 올바른지 확인합니다.
		/// 
		/// </summary>
		/// <returns></returns>
		private bool checkValidationRequest(NetworkPlayer player)
		{
			switch (VisibilityAuthority)
			{
				case VisibilityAuthority.None:
					return false;

				case VisibilityAuthority.All:
					return true;

				case VisibilityAuthority.Owner:
					return Owner == player.UserId;

				case VisibilityAuthority.Faction:
					return Faction.IsSameFaction(player.Faction);

				case VisibilityAuthority.Users:
					return _visibleUserSet.Contains(player.UserId);

				default:
					Debug.Assert(false);
					return false;
			}
		}

		public bool HasPlayerCooltime(NetworkPlayer player)
		{
			return _cooltimePlayers.Contains(player.UserId);
		}

		private void onCooltimeStart(NetworkPlayer player)
		{
			_cooltimePlayers.Add(player.UserId);
		}

		private void onCooltimeEnd(Arg userIdArg)
		{
			UserId userId = userIdArg.UInt32;
			_cooltimePlayers.Remove(userId);
		}

	}
}