using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Infos;
using CT.Common.Tools.Collections;
using CT.Networks;
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

		public GameSceneMapData MapData { get; private set; }

		// Player Management
		protected BidirectionalMap<NetworkPlayer, PlayerCharacter> _playerCharacterByPlayer;
		protected int _spawnIndex;

		[AllowNull] private Dictionary<int, AreaInfo> _areaInfoBySection;

		public override void Constructor()
		{
			_areaInfoBySection = new Dictionary<int, AreaInfo>(30);
			_playerCharacterByPlayer = new(GameplayManager.Option.SystemMaxUser);
		}

		public virtual void Initialize(GameSceneIdentity identity)
		{
			GameSceneIdentity = identity;
			_playerCharacterByPlayer.Clear();
			_spawnIndex = 0;

			// Setup Map Data
			MapData = GameSceneMapDataDB.GetGameSceneMapData(identity);
			_areaInfoBySection.Clear();
			foreach (var areaInfo in MapData.AreaInfos)
			{
				if (areaInfo.Index >= GlobalNetwork.LAST_SECTION_AREA_INDEX)
					continue;
				_areaInfoBySection.Add(areaInfo.Index, areaInfo);
			}
		}

		public override void OnCreated()
		{
			WorldManager.SetGameMapData(MapData);
			_spawnIndex = 0;

			// Create teleporters
			if (MapData.InteractorTable.TryGetValue(InteractorType.Teleporter,
													 out var teleporterInfoList))
			{
				foreach (var info in teleporterInfoList)
				{
					var teleporter = WorldManager.CreateObject<Teleporter>(info.Position);
					teleporter.Initialize(info);
				}
			}
		}

		public virtual partial void Client_OnSceneLoaded(NetworkPlayer player)
		{
			player.IsMapLoaded = true;
			player.CanSeeViewObject = true;
		}

		public virtual void OnPlayerEnter(NetworkPlayer player) { }
		public virtual void OnPlayerLeave(NetworkPlayer player)
		{
			DestroyPlayer(player);
		}

		public bool TryGetPlayerCharacter(NetworkPlayer player,
										  [MaybeNullWhen(false)]
										  out PlayerCharacter playerCharacter)
		{
			return _playerCharacterByPlayer.TryGetValue(player, out playerCharacter);
		}

		public T SpawnPlayerBy<T>(NetworkPlayer player) where T : PlayerCharacter, new()
		{
			int spawnPosCount = MapData.SpawnPositions.Count;
			_spawnIndex = (_spawnIndex + 1) % spawnPosCount;
			return SpawnPlayerBy<T>(player, _spawnIndex);
		}

		public T SpawnPlayerBy<T>(NetworkPlayer player, int spwanIndex) where T : PlayerCharacter, new()
		{
			var spawnPositions = MapData.SpawnPositions;
			Vector2 spawnPos = spawnPositions[spwanIndex];
			TryCreatePlayerBy<T>(player, spawnPos, out var character);

			if (GameplayController.CameraControllerByPlayer.TryGetValue(player, out var cameraController))
			{
				cameraController.MoveToTarget();
			}
			else
			{
				_log.Fatal($"There is no camera for {player} to move to target!");
			}

			Debug.Assert(character != null);
			return character;
		}

		public FieldItem SpawnFieldItemBy(FieldItemType itemType, Vector2 position)
		{
			var item = WorldManager.CreateObject<FieldItem>(position);
			var interactor = InteractorConst.FieldItemInteractorInfo;
			item.Initialize(interactor);
			item.InitializeAs(itemType);
			return item;
		}

		public bool TryCreatePlayerBy<T>(NetworkPlayer player,
										 Vector2 position,
										 [MaybeNullWhen(false)]
										 out T playerCharacter) where T : PlayerCharacter, new()
		{
			if (_playerCharacterByPlayer.TryGetValue(player, out var existCharacter))
			{
				_log.Error($"{player} already has player character. {existCharacter.GetType().Name}");
				playerCharacter = null;
				return false;
			}

			// Create object
			playerCharacter = WorldManager.CreateObject<T>(position);
			playerCharacter.Initialize(player);

			// Binding
			player.BindCharacter(playerCharacter);
			_playerCharacterByPlayer.Add(player, playerCharacter);

			// Set Section
			foreach (var value in _areaInfoBySection.Values)
			{
				if (value.IsInnerPosition(position))
				{
					playerCharacter.Section = (byte)value.Index;
					break;
				}
			}

			// Bind camera
			if (!GameplayController.CameraControllerByPlayer.TryGetValue(player, out var playerCamera))
			{
				_log.Fatal($"There is no camera for {player}!");
			}
			else
			{
				playerCamera.BindPlayerCharacter(playerCharacter);
			}

			return true;
		}

		public void DestroyPlayer(PlayerCharacter playerCharacter)
		{
			DestroyPlayer(playerCharacter.NetworkPlayer);
		}

		public void DestroyPlayer(NetworkPlayer player)
		{
			if (!_playerCharacterByPlayer.TryGetValue(player, out var playerCharacter))
			{
				_log.Error($"There is no matched player character. NetworkPlayer : {player}");
				return;
			}

			_playerCharacterByPlayer.TryRemove(player);

			// Destroy object
			playerCharacter.Destroy();
			
			// Releasing
			player.ReleaseCharacter(playerCharacter);

			// Release camera
			if (!GameplayController.CameraControllerByPlayer.TryGetValue(player, out var playerCamera))
			{
				_log.Fatal($"There is no camera for {player}!");
			}
			else
			{
				playerCamera.ReleasePlayerCharacter(playerCharacter);
			}
		}

		public void Release()
		{
			foreach (NetworkPlayer player in _playerCharacterByPlayer.ForwardKeys)
			{
				DestroyPlayer(player);
			}
		}
	}
}