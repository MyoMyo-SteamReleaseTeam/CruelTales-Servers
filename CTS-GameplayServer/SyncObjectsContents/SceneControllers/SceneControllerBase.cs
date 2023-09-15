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
	public partial class SceneControllerBase : MasterNetworkObject
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(SceneControllerBase));

		public override VisibilityType Visibility => VisibilityType.Global;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		// Reference
		public GameplayController GameplayController { get; private set; }
		public RoomSessionManager RoomSessionManager => GameplayController.RoomSessionManager;
		protected GameSceneMapData _mapData;

		// Player Management
		public BidirectionalMap<NetworkPlayer, PlayerCharacter> PlayerCharacterByPlayer { get; private set; }
		protected int _spawnIndex;

		public override void Constructor()
		{
			PlayerCharacterByPlayer = new(GameplayManager.Option.SystemMaxUser);
		}

		public virtual void Initialize(GameplayController gameplayController,
									   GameSceneIdentity identity)
		{
			GameplayController = gameplayController;
			GameSceneIdentity = identity;
			_mapData = MiniGameMapDataDB.GetMiniGameMapData(identity);
			_spawnIndex = 0;
		}

		public override void OnCreated()
		{
			WorldManager.SetGameMapData(_mapData);
			_spawnIndex = 0;
		}

		public virtual partial void Client_OnSceneLoaded(NetworkPlayer player)
		{
			player.IsMapLoaded = true;
			player.CanSeeViewObject = true;
		}

		public virtual void OnPlayerEnter(NetworkPlayer player) { }
		public virtual void OnPlayerLeave(NetworkPlayer player) { }

		protected void createPlayerBy(NetworkPlayer player)
		{
			var spawnPositions = _mapData.SpawnPositions;
			int spawnPosCount = spawnPositions.Count;
			Vector2 spawnPos = spawnPositions[_spawnIndex];
			_spawnIndex = (_spawnIndex + 1) % spawnPosCount;

			var playerCharacter = WorldManager.CreateObject<PlayerCharacter>(spawnPos);
			playerCharacter.BindNetworkPlayer(player);
			PlayerCharacterByPlayer.Add(player, playerCharacter);
		}
	}
}