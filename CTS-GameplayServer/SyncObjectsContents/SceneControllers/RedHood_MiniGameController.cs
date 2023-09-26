using System;
using System.Diagnostics.CodeAnalysis;
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

		[AllowNull] private PlayerCharacterTable _playerCharacterTable;

		private Action<Arg, Arg, Arg, Arg> _onChangeRole;

		public override void Constructor()
		{
			base.Constructor();

			_onChangeRole = onChangeRole;
			_playerCharacterTable = new(GameplayManager.Option.SystemMaxUser);
		}

		public override void OnCreated()
		{
			base.OnCreated();
			_playerCharacterTable.Clear();

			int playerCount = GameplayController.PlayerSet.Count;
			int wolfCount = (playerCount > 2) ? (int)(playerCount / WOLF_RATIO) : 1;
			if (wolfCount > playerCount)
			{
				_log.Fatal($"Initial wolf count error! PlayerCount : {playerCount} / WolfCount : {wolfCount}");
				wolfCount = playerCount;
			}

			Span<int> wolfIndices = stackalloc int[wolfCount];

			int i = 0;
			while (i < wolfCount)
			{
				wolfIndices[i] = RandomHelper.NextInt(playerCount);

				bool hasSameIndex = false;
				for (int c = 0; c < i; c++)
				{
					if (wolfIndices[c] == wolfIndices[i])
					{
						hasSameIndex = true;
						break;
					}
				}

				if (!hasSameIndex)
				{
					i++;
				}
			}

			int spawnCount = 0;
			foreach (NetworkPlayer player in GameplayController.PlayerSet)
			{
				bool isWolf = false;

				for (int w = 0; w < wolfCount; w++)
				{
					if (wolfIndices[w] == spawnCount)
					{
						isWolf = true;
						break;
					}
				}

				if (isWolf)
				{
					SpawnPlayerBy<WolfCharacter>(player);
					_playerCharacterTable.AddPlayerByType(player, NetworkObjectType.WolfCharacter);
				}
				else
				{
					SpawnPlayerBy<RedHoodCharacter>(player);
					_playerCharacterTable.AddPlayerByType(player, NetworkObjectType.RedHoodCharacter);
				}

				spawnCount++;
			}
		}

		public override void OnPlayerEnter(NetworkPlayer player)
		{
			SpawnPlayerBy<RedHoodCharacter>(player);
			_playerCharacterTable.AddPlayerByType(player, NetworkObjectType.RedHoodCharacter);
		}

		public override void OnPlayerLeave(NetworkPlayer player)
		{
			DestroyPlayer(player);
			_playerCharacterTable.DeletePlayer(player);
		}
		
		protected override void onGameEnd()
		{
			base.onGameEnd();
			_playerCharacterTable.GetPlayerSetBy(NetworkObjectType.WolfCharacter);
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
					_playerCharacterTable.AddPlayerByType(targetPlayer, NetworkObjectType.WolfCharacter);
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
					_playerCharacterTable.AddPlayerByType(wolfPlayer, NetworkObjectType.WolfCharacter);
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
				_playerCharacterTable.AddPlayerByType(targetPlayer, NetworkObjectType.WolfCharacter);

				TryCreatePlayerBy<RedHoodCharacter>(wolfPlayer, targetPos, out _);
				_playerCharacterTable.AddPlayerByType(wolfPlayer, NetworkObjectType.RedHoodCharacter);
			}
		}
	}
}