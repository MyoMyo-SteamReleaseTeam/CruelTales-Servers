using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Gameplay;
using CT.Common.Gameplay.RedHood;
using CTS.Instance.Coroutines;
using CTS.Instance.Gameplay;
using CTS.Instance.Gameplay.Events;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class RedHood_MiniGameController 
		: MiniGameControllerBase, IWolfEventHandler, IMissionTableHandler
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(RedHood_MiniGameController));

		public const float WOLF_RATIO = 2.5f;

		private Action<Arg, Arg, Arg, Arg> _onChangeRole;
		private Action<NetworkPlayer, PlayerCharacter> _onCharacterCreated;

		private Dictionary<NetworkPlayer, RedHoodMissionInfo> _playerMissionInfoByPlayer = new();
		private Dictionary<RedHoodMission, RedHoodMissionInteractor> _missionInteractorByMission = new();

		public override void Constructor()
		{
			base.Constructor();
			_onChangeRole = onChangeRole;
			_onCharacterCreated = onCharacterCreated;
		}

		public override void OnCreated()
		{
			base.OnCreated();

			// Initialize
			_playerMissionInfoByPlayer.Clear();
			_missionInteractorByMission.Clear();

			// Create mission interactors
			foreach (var info in MapData.InteractorTable[InteractorType.Mission])
			{
				if (info.RedHoodMission == RedHoodMission.None)
					continue;

				var interactor = WorldManager.CreateObject<RedHoodMissionInteractor>(info.Position);
				interactor.Initialize(info);
				interactor.Mission = info.RedHoodMission;
				_missionInteractorByMission.Add(interactor.Mission, interactor);
			}

			// Get player sets
			GameplayController.GetPlayers(out var alivePlayers, out var eliminatedPlayers);
			alivePlayers.Shuffle();

			// Create mission info by players
			foreach (var player in alivePlayers)
			{
				var missionInfo = WorldManager.CreateObject<RedHoodMissionInfo>();
				missionInfo.SetOwner(player);
				_playerMissionInfoByPlayer.Add(player, missionInfo);
			}

			// Calculate wolf count
			var players = alivePlayers;
			int playerCount = players.Count;
			int wolfCount = (playerCount > 2) ? (int)(playerCount / WOLF_RATIO) : 1;
			if (wolfCount > playerCount)
			{
				_log.Fatal($"Initial wolf count error! PlayerCount : {playerCount} / WolfCount : {wolfCount}");
				wolfCount = playerCount;
			}

			// Select wolf and spawn players as roles
			for (int i = 0; i < wolfCount; i++)
			{
				NetworkPlayer player = players[i];
				SpawnPlayerBy<WolfCharacter>(player, _onCharacterCreated);
			}

			for (int i = wolfCount; i < playerCount; i++)
			{
				NetworkPlayer player = players[i];
				SpawnPlayerBy<RedHoodCharacter>(player, _onCharacterCreated);
			}

			// TODO : Create spectator
		}

		protected override void onGameEnd()
		{
			base.onGameEnd();
			var eliminatedPlayers = GetPlayerSetBy(NetworkObjectType.WolfCharacter);
			foreach (var ep in eliminatedPlayers)
			{
				OnPlayerEliminated(ep);
			}
		}

		public override void OnPlayerLeave(NetworkPlayer player)
		{
			base.OnPlayerLeave(player);
		}

		public void OnWolfCatch(WolfCharacter wolf, RedHoodCharacter target)
		{
			Arg wolfUserId = new(wolf.NetworkPlayer.UserId.Id);
			Arg targetUserId = new(target.NetworkPlayer.UserId.Id);

			Vector2 wolfPos = wolf.Position;
			Vector2 targetPos = target.Position;
			Vector2 center = (wolfPos + targetPos) * 0.5f;
			Arg centerArg = new(center);
			Arg isRight = new(center.X < wolfPos.X);

			StartCoroutine(_onChangeRole, centerArg, isRight, wolfUserId, targetUserId, 2.0f);
			GameplayController.EffectController.Play(EffectType.WolfnPlayerFight, center, 2.1f);

			wolf.NetworkPlayer.CameraController?.Server_LookAt(center, 0.1f);
			target.NetworkPlayer.CameraController?.Server_LookAt(center, 0.1f);

			DestroyPlayer(wolf);
			DestroyPlayer(target);
		}

		private void onChangeRole(Arg centerPosArg, Arg wasWolfRightArg,
								  Arg wolfUserIdArg, Arg targetUserIdArg)
		{
			Vector2 centerPos = centerPosArg.Vector2;
			bool wasWolfRight = wasWolfRightArg.Bool;
			UserId wolfUserId = wolfUserIdArg.UInt64;
			UserId targetUserId = targetUserIdArg.UInt64;

			GameplayManager.TryGetNetworkPlayer(wolfUserId, out var wolfPlayer);
			GameplayManager.TryGetNetworkPlayer(targetUserId, out var targetPlayer);

			const float offsetX = 1f;
			const float offsetY = 1f;

			float wolfX;
			float targetX;

			// 원래 존재했던 방향에서 스폰

			if (wasWolfRight)
			{
				wolfX = -offsetX;
				targetX = -offsetY;
			}
			else
			{
				wolfX = offsetX;
				targetX = offsetY;
			}

			Vector2 wolfPos = centerPos + new Vector2(wolfX, -offsetY);
			Vector2 targetPos = centerPos + new Vector2(targetX, offsetY);

			/*
			 * 늑대 플레이어가 잡기 도중에 게임에서 나갔을 때
			 * 
			 * 대상 플레이어 : 늑대
			 */
			if (wolfPlayer == null)
			{
				if (targetPlayer != null)
				{
					TryCreatePlayerBy<WolfCharacter>(targetPlayer, wolfPos, out _);
				}
			}
			/*
			 * 대상 플레이어가 잡기 도중에 게임에서 나갔을 때
			 * 
			 * 늑대 플레이어 : 늑대
			 */
			else if (targetPlayer == null)
			{
				if (wolfPlayer != null)
				{
					TryCreatePlayerBy<WolfCharacter>(wolfPlayer, wolfPos, out _);
				}
			}
			/*
			 * 늑대 플레이어와 대상 플레이어가 모두 존재할 때
			 * 
			 * 늑대 플레이어 : 빨간모자
			 * 대상 플레이어 : 늑대
			 */
			else
			{
				TryCreatePlayerBy<WolfCharacter>(targetPlayer, wolfPos, out _);
				TryCreatePlayerBy<RedHoodCharacter>(wolfPlayer, targetPos, out _);
			}
		}

		private void onCharacterCreated(NetworkPlayer player, PlayerCharacter character)
		{
			foreach (var interactor in _missionInteractorByMission.Values)
			{
				interactor.OnCharacterCreated(player, character);
			}
		}

		public void OnInteracted(NetworkPlayer player, PlayerCharacter Character, Interactor interactor)
		{
			if (!_playerMissionInfoByPlayer.TryGetValue(player, out var missionInfo))
			{
				Anticheat.CheatDetected(player, AntiCheatType.PlayersDataIsNotExist);
				return;
			}

			if (interactor is not RedHoodMissionInteractor missionInteractor)
			{
				_log.Fatal($"This is not RedHoodMissionInteractor!");
				Debug.Assert(false);
				return;
			}

			RedHoodMission mission = missionInteractor.Mission;
			NetByte missionKey = mission.GetKey();
			if (!missionInfo.MissionTable.TryGetValue(missionKey, out var isClear))
			{
				Anticheat.CheatDetected(player, AntiCheatType.PlayersDataIsNotExist);
				return;
			}

			if (isClear)
			{
				_log.Error($"{player} already clear mission {missionKey}!");
				return;
			}

			missionInfo.MissionTable[missionKey] = true;
		}

		public bool TryGetMissionTable(NetworkPlayer player,
									   [MaybeNullWhen(false)]
									   out SyncDictionary<NetByte, NetBool> missionTable)
		{
			if (!_playerMissionInfoByPlayer.TryGetValue(player, out var missionInfo))
			{
				missionTable = null;
				return false;
			}

			missionTable = missionInfo.MissionTable;
			return true;
		}
	}
}