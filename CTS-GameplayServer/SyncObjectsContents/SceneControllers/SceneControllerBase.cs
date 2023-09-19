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

		protected GameSceneMapData _mapData;

		// Player Management
		public BidirectionalMap<NetworkPlayer, PlayerCharacter> PlayerCharacterByPlayer { get; private set; }
		protected int _spawnIndex;

		public override void Constructor()
		{
			PlayerCharacterByPlayer = new(GameplayManager.Option.SystemMaxUser);
		}

		public virtual void Initialize(GameSceneIdentity identity)
		{
			GameSceneIdentity = identity;
			PlayerCharacterByPlayer.Clear();
			_mapData = GameSceneMapDataDB.GetGameSceneMapData(identity);
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

		public void SpawnPlayerBy<T>(NetworkPlayer player) where T : PlayerCharacter, new()
		{
			var spawnPositions = _mapData.SpawnPositions;
			int spawnPosCount = spawnPositions.Count;
			Vector2 spawnPos = spawnPositions[_spawnIndex];
			_spawnIndex = (_spawnIndex + 1) % spawnPosCount;
			CreatePlayerBy<T>(player, spawnPos);
		}

		public void CreatePlayerBy<T>(NetworkPlayer player, Vector2 position) where T : PlayerCharacter, new()
		{
			if (PlayerCharacterByPlayer.TryGetValue(player, out var existCharacter))
			{
				_log.Error($"{player} already has player character. {existCharacter.GetType().Name}");
				return;
			}

			var playerCharacter = WorldManager.CreateObject<T>(position);
			playerCharacter.BindNetworkPlayer(player);
			PlayerCharacterByPlayer.Add(player, playerCharacter);
		}

		public void DestroyPlayer(PlayerCharacter playerCharacter)
		{
			DestroyPlayer(playerCharacter.NetworkPlayer);
		}

		public void DestroyPlayer(NetworkPlayer player)
		{
			if (!PlayerCharacterByPlayer.TryGetValue(player, out var playerCharacter))
			{
				_log.Error($"There is no matched player character. NetworkPlayer : {player}");
				return;
			}

			PlayerCharacterByPlayer.TryRemove(player);
			playerCharacter.Destroy();
			player.ReleaseViewTarget();
		}
	}
}