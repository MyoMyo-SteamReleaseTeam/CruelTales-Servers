using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CT.Common.DataType;
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

		[AllowNull] public GameSceneMapData MapData { get; private set; }

		// Player Management
		protected BidirectionalMap<NetworkPlayer, PlayerCharacter> _playerCharacterByPlayer = new(GlobalNetwork.SYSTEM_MAX_USER);
		private PlayerCharacterTable _playerCharacterTable = new(GlobalNetwork.SYSTEM_MAX_USER);
		protected Dictionary<Faction, int> _spawnIndexTable = new()
		{
			{ Faction.None, 0 }, { Faction.Red, 0 },
			{ Faction.Blue, 0 }, { Faction.Green, 0 },
		};

		// Maps
		protected Dictionary<int, AreaInfo> _areaInfoBySection = new(64);

		public override void Constructor()
		{
		}

		/// <summary>Scene Controller를 초기화합니다. OnCreated 보다 먼저 호출됩니다.</summary>
		public virtual void Initialize(GameSceneIdentity identity)
		{
			GameSceneIdentity = identity;
			// Setup Map Data
			MapData = GameSceneMapDataDB.GetGameSceneMapData(identity);
			_areaInfoBySection.Clear();
			foreach (var areaInfo in MapData.AreaInfos)
			{
				if (areaInfo.Index >= GlobalNetwork.SYSTEM_AREA_INDEX_LIMIT)
					continue;
				_areaInfoBySection.Add(areaInfo.Index, areaInfo);
			}
		}

		public override void OnCreated()
		{
			WorldManager.SetGameMapData(MapData);

			// Initialize
			_playerCharacterByPlayer.Clear();
			_playerCharacterTable.Clear();

			// Initialize spawn indices
			_spawnIndexTable[Faction.None] = 0;
			_spawnIndexTable[Faction.Red] = 0;
			_spawnIndexTable[Faction.Blue] = 0;
			_spawnIndexTable[Faction.Green] = 0;

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

		public void SpawnPlayersByTeam<T>(Action<T, Faction>? onCreated, params Faction[] factions)
			where T : PlayerCharacter, new()
		{
			GameplayController.GetPlayers(out var alivePlayers, out var eliminatedPlayers);
			alivePlayers.Shuffle();
			int factionCount = factions.Length;
			int divide = alivePlayers.Count / factionCount;
			if (divide == 0)
			{
				divide = 1;
			}
			int spawnIndex = 0;
			foreach (var player in alivePlayers)
			{
				int factionIndex = spawnIndex / divide;
				if (factionIndex > factionCount)
				{
					factionIndex = factionCount;
				}
				Faction curFaction = factions[factionIndex];
				player.Faction = curFaction;
				var character = SpawnPlayerBy<T>(player, curFaction);
				onCreated?.Invoke(character, curFaction);
				spawnIndex++;
			}
		}

		public T SpawnPlayerBy<T>(NetworkPlayer player, Faction faction = Faction.None)
			where T : PlayerCharacter, new()
		{
			Vector2 spawnPos = getNextSpawnPositionBy(faction);
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

			Vector2 getNextSpawnPositionBy(Faction faction)
			{
				var curSpawnList = MapData.SpawnPositionTable[faction];
				int spawnPosCount = curSpawnList.Count;
				int curSpawnIndex = _spawnIndexTable[faction];
				_spawnIndexTable[faction] = (curSpawnIndex + 1) % spawnPosCount;
				return curSpawnList[curSpawnIndex];
			}
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
				string typeName = existCharacter == null ? "Unknown" : existCharacter.GetType().Name;
				_log.Error($"{player} already has player character. {typeName}");
				playerCharacter = null;
				return false;
			}

			// Create object
			playerCharacter = WorldManager.CreateObject<T>(position);
			playerCharacter.Initialize(player);

			// Binding
			player.BindCharacter(playerCharacter);
			_playerCharacterByPlayer.Add(player, playerCharacter);
			_playerCharacterTable.AddPlayerByType(player, playerCharacter.Type);

			// Set Section
			foreach (var value in _areaInfoBySection.Values)
			{
				if (value.IsInnerPosition(position))
				{
					playerCharacter.Section = (byte)value.Index;
					player.CameraController?.ResetZoom();
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
				// There is no matched player character by NetworkPlayer
				return;
			}
			_playerCharacterByPlayer.TryRemove(player);

			// Destroy object
			playerCharacter.Destroy();
			
			// Releasing
			player.ReleaseCharacter(playerCharacter);
			_playerCharacterTable.DeletePlayer(player);

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

		public HashSet<NetworkPlayer> GetPlayerSetBy(NetworkObjectType type)
		{
			return _playerCharacterTable.GetPlayerSetBy(type);
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