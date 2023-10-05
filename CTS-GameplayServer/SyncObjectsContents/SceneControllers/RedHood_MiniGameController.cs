using System;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CTS.Instance.Coroutines;
using CTS.Instance.Gameplay;
using CTS.Instance.Gameplay.Events;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class RedHood_MiniGameController : MiniGameControllerBase, IWolfEventHandler
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(RedHood_MiniGameController));

		public const float WOLF_RATIO = 2.5f;

		private Action<Arg, Arg, Arg, Arg> _onChangeRole;

		public override void Constructor()
		{
			base.Constructor();
			_onChangeRole = onChangeRole;
		}

		public override void OnCreated()
		{
			base.OnCreated();

			// Calculate wolf count
			int playerCount = GameplayController.PlayerSet.Count;
			int wolfCount = (playerCount > 2) ? (int)(playerCount / WOLF_RATIO) : 1;
			if (wolfCount > playerCount)
			{
				_log.Fatal($"Initial wolf count error! PlayerCount : {playerCount} / WolfCount : {wolfCount}");
				wolfCount = playerCount;
			}

			// Select wolf and spawn players as roles
			var players = GameplayController.GetShuffledPlayers();
			for (int i = 0; i < wolfCount; i++)
			{
				NetworkPlayer player = players[i];
				SpawnPlayerBy<WolfCharacter>(player);
			}

			for (int i = wolfCount; i < playerCount; i++)
			{
				NetworkPlayer player = players[i];
				SpawnPlayerBy<RedHoodCharacter>(player);
			}
		}

		protected override void onGameEnd()
		{
			base.onGameEnd();
			var eliminatedPlayers = GetPlayerSetBy(NetworkObjectType.WolfCharacter);
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
	}
}