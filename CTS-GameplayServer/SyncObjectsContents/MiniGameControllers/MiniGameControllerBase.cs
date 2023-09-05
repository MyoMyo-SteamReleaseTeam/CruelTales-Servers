using System;
using System.Numerics;
using CT.Common.Gameplay;
using CT.Common.Tools.Collections;
using CTS.Instance.Data;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class MiniGameControllerBase : MasterNetworkObject
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MiniGameControllerBase));

		public override VisibilityType Visibility => VisibilityType.Global;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		// Reference
		public GameplayController GameplayController { get; private set; }
		protected MiniGameMapData _miniGameData;

		// Player Management
		public BidirectionalMap<NetworkPlayer, PlayerCharacter> PlayerCharacterByPlayer { get; private set; }
		protected int _spawnIndex;

		public override void Constructor()
		{
			PlayerCharacterByPlayer = new(GameplayManager.Option.SystemMaxUser);
		}

		public virtual void Initialize(GameplayController gameplayController,
									   MiniGameIdentity identity)
		{
			GameplayController = gameplayController;
			MiniGameIdentity = identity;
			_miniGameData = MiniGameMapDataDB.GetMiniGameMapData(identity);
			_spawnIndex = 0;
		}

		public override void OnCreated()
		{
			foreach (NetworkPlayer player in PlayerCharacterByPlayer)
			{
				player.IsMapLoaded = false;
				player.IsReady = false;

				Server_LoadMiniGame(player, MiniGameIdentity);
			}
		}

		public virtual partial void Client_ReadyGame(NetworkPlayer player, bool isReady)
		{
			player.IsReady = isReady;
			if (GameplayController.RoomSessionManager.CheckAllReady())
			{
				OnAllReady();
			}
		}

		public virtual void OnAllReady()
		{
			_log.Info("All ready!");
		}

		public void OnGameStart()
		{
			WorldManager.SetMiniGameMapData(_miniGameData);
			_spawnIndex = 0;

			foreach (NetworkPlayer player in GameplayController.PlayerSet)
			{
				OnPlayerEnter(player);
			}
		}

		public void OnGameEnd()
		{
			WorldManager.ReleaseMiniGameMapData();

			foreach (var pc in PlayerCharacterByPlayer.ForwardValues)
			{
				pc.Destroy();
			}
		}

		public virtual void OnPlayerEnter(NetworkPlayer player)
		{

		}

		public virtual void OnPlayerLeave(NetworkPlayer player)
		{
			if (PlayerCharacterByPlayer.TryGetValue(player, out var pc))
			{
				pc.Destroy();
				PlayerCharacterByPlayer.TryRemove(player);
			}

			checkGameOverCondition();
		}

		protected void createPlayerBy(NetworkPlayer player)
		{
			var spawnPositions = _miniGameData.SpawnPositions;
			int spawnPosCount = spawnPositions.Count;
			Vector2 spawnPos = spawnPositions[_spawnIndex];
			_spawnIndex = (_spawnIndex + 1) % spawnPosCount;

			var playerCharacter = WorldManager.CreateObject<PlayerCharacter>(spawnPos);
			playerCharacter.BindNetworkPlayer(player);
			PlayerCharacterByPlayer.Add(player, playerCharacter);
		}

		protected virtual void checkGameOverCondition()
		{

		}

		public partial void Client_OnMiniGameLoaded(NetworkPlayer player)
		{
			player.IsMapLoaded = true;
			player.CanSeeViewObject = true;
		}
	}
}
