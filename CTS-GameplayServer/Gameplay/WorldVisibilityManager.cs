#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Tools;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class WorldVisibilityManager
	{
		// Log
		private static readonly ILog _log = LogManager.GetLogger(typeof(WorldVisibilityManager));

		// Reference
		private GameplayInstance _gameplayInstance;
		private WorldManager _worldManager;

		// World cell sizes
		public const float INVERSE_CEll_SIZE = 1.0f / 8.0f;

		public const float ORIGIN_OFFSET_X = 128.0f;
		public const float ORIGIN_OFFSET_Z = 128.0f;

		public const int CELL_WIDTH = 32;
		public const int CELL_HEIGHT = 32;
		public const int CELL_HALF_WIDTH = CELL_WIDTH / 2;
		public const int CELL_HALF_HEIGHT = CELL_HEIGHT / 2;

		// Partitioned visibility set
		private List<NetworkIdentity> _spawnList;
		private Dictionary<NetworkIdentity, MasterNetworkObject>[,] _networkObjectByCell;
		private Dictionary<NetworkIdentity, MasterNetworkObject> _nullSet = new();

		// Player visible tables
		private ObjectPool<PlayerVisibleTable> _playerVisibleTablePool;
		private Dictionary<NetworkPlayer, PlayerVisibleTable> _playerVisibleBySession;

		public WorldVisibilityManager(GameplayInstance gameplayInstance,
									  WorldManager worldManager,
									  InstanceInitializeOption option)
		{
			_gameplayInstance = gameplayInstance;
			_worldManager = worldManager;

			_spawnList = new(option.VisibleSpawnCapacity);
			_networkObjectByCell = new Dictionary<NetworkIdentity, MasterNetworkObject>[CELL_HEIGHT, CELL_WIDTH];
			for (int z = 0; z < CELL_HEIGHT; z++)
			{
				for (int x = 0; x < CELL_WIDTH; x++)
				{
					_networkObjectByCell[z, x] = new(option.VisibleCellCapacity);
				}
			}

			// Visible Table
			_playerVisibleTablePool = new(() => new PlayerVisibleTable(option), option.SystemMaxUser);
			_playerVisibleBySession = new(option.SystemMaxUser);
		}

		public Dictionary<NetworkIdentity, MasterNetworkObject> GetCell(Vector3 pos)
		{
			return GetCell(GetWorldCell(pos));
		}

		public Dictionary<NetworkIdentity, MasterNetworkObject> GetCell(Vector2Int internalCell)
		{
			if (internalCell.Y < 0 || internalCell.Y > CELL_HEIGHT ||
				internalCell.X < 0 || internalCell.X > CELL_WIDTH)
			{
				return _nullSet;
			}

			return _networkObjectByCell[internalCell.Y, internalCell.X];
		}

		public Dictionary<NetworkIdentity, MasterNetworkObject> GetCell(int x, int z)
		{
			if (z < 0 || z > CELL_HEIGHT ||
				x < 0 || x > CELL_WIDTH)
			{
				return _nullSet;
			}

			return _networkObjectByCell[z, x];
		}

		public static Vector2Int GetWorldCell(Vector2 position)
		{
			float curPosX = position.X + ORIGIN_OFFSET_X;
			float curPosZ = position.Y + ORIGIN_OFFSET_Z;
			int cellX = (int)(curPosX * INVERSE_CEll_SIZE);
			int cellZ = (int)(curPosZ * INVERSE_CEll_SIZE);
			return new Vector2Int(cellX, cellZ);
		}

		public static Vector2Int GetWorldCell(Vector3 position)
		{
			float curPosX = position.X + ORIGIN_OFFSET_X;
			float curPosZ = position.Z + ORIGIN_OFFSET_Z;
			int cellX = (int)(curPosX * INVERSE_CEll_SIZE);
			int cellZ = (int)(curPosZ * INVERSE_CEll_SIZE);
			return new Vector2Int(cellX, cellZ);
		}

		private Dictionary<NetworkIdentity, MasterNetworkObject> _outObjectSet = new(16);
		public void UpdateVisibilityAndSendData()
		{
			foreach (var kv in _playerVisibleBySession)
			{
				NetworkPlayer player = kv.Key;
				PlayerVisibleTable viewTable = kv.Value;
				Vector3 viewPos3D = player.ViewTransform.Position;
				Vector2 viewPos = new Vector2(viewPos3D.X, viewPos3D.Z);

				Vector2 inBoundary = player.HalfViewInSize;
				Vector2 inboundLB = viewPos - inBoundary;
				Vector2 inboundRT = viewPos + inBoundary;

				Vector2 outBoundary = player.HalfViewOutSize;
				Vector2 outboundLB = viewPos - outBoundary;
				Vector2 outboundRT = viewPos + outBoundary;

				Vector2Int inboundCellLB = GetWorldCell(inboundLB);
				Vector2Int inboundCellRT = GetWorldCell(inboundRT);

				// Add spawn object
				int spawnCount = _spawnList.Count;
				for (int i = 0; i < spawnCount; i++)
				{
					var spawnObjectId = _spawnList[i];
					if (!_worldManager.TryGetNetworkObject(spawnObjectId, out var spawnObject))
						continue;

					Vector3 objPos = spawnObject.Transform.Position;
					if (objPos.X >= inboundLB.X && objPos.X <= inboundRT.X &&
						objPos.Z >= inboundLB.Y && objPos.Z <= inboundRT.Y)
					{
						if (spawnObject.IsValidVisibilityAuthority(player))
						{
							viewTable.SpawnObjects.TryAdd(spawnObject.Identity, spawnObject);
						}
					}
				}

				// Add respawn object
				for (int cz = inboundCellLB.Y; cz <= inboundCellRT.Y; cz++)
				{
					for (int cx = inboundCellLB.X; cx <= inboundCellRT.X; cx++)
					{
						var curCell = GetCell(cx, cz);
						foreach (var netObj in curCell.Values)
						{
							Vector3 objPos = netObj.Transform.Position;
							if (objPos.X >= inboundLB.X && objPos.X <= inboundRT.X &&
								objPos.Z >= inboundLB.Y && objPos.Z <= inboundRT.Y)
							{
								if (viewTable.TraceObjects.ContainsKey(netObj.Identity))
								{
									continue;
								} 

								// TODO : 초기 생성 객체는 셀 타일에 등록되지 않도록 관리되어야함
								// 임시 코드
								if (viewTable.SpawnObjects.ContainsKey(netObj.Identity))
								{
									//Debug.Assert(false);
									continue;
								}
								// 임시 코드

								if (netObj.IsValidVisibilityAuthority(player))
								{
									viewTable.RespawnObjects.TryAdd(netObj.Identity, netObj);
								}
							}
						}
					}
				}

				// Check if it's out of the view
				_outObjectSet.Clear();
				foreach (var netObj in viewTable.TraceObjects.Values)
				{
					Vector3 objPos = netObj.Transform.Position;
					if (objPos.X < outboundLB.X || objPos.X > outboundRT.X ||
						objPos.Z < outboundLB.Y || objPos.Z > outboundRT.Y)
					{
						_outObjectSet.TryAdd(netObj.Identity, netObj);
					}
				}

				foreach (var outObject in _outObjectSet.Values)
				{
					viewTable.TraceObjects.Remove(outObject.Identity);
					viewTable.DespawnObjects.TryAdd(outObject.Identity, outObject);
				}
			}

			// Add first spawn object to world partition
			int transitionCount = _spawnList.Count;
			for (int i = 0; i < transitionCount; i++)
			{
				var transitId = _spawnList[i];
				if (!_worldManager.TryGetNetworkObject(transitId, out var netObj))
					continue;
				GetCell(netObj.Transform.Position).TryAdd(netObj.Identity, netObj);
			}

			// Transition visibility cycle
			// Spawn -> Trace | Despawn -> delete
			foreach (var kv in _playerVisibleBySession)
			{
				UserSession? session = kv.Key.Session;
				PlayerVisibleTable visibleTable = kv.Value;
				if (session != null)
				{
					_worldManager.SendSynchronization(session, visibleTable);
				}
				visibleTable.TransitionVisibilityCycle();
			}

			// Reset spawn objects
			_spawnList.Clear();
		}

		public void OnCellChanged(MasterNetworkObject netObj, Vector2Int previous, Vector2Int current)
		{
			var previousCell = GetCell(previous);
			previousCell.Remove(netObj.Identity);

			var currentCell = GetCell(current);
			currentCell.Add(netObj.Identity, netObj);
		}

		public void OnCreated(MasterNetworkObject netObj)
		{
			var id = netObj.Identity;

			if (netObj.Visibility == VisibilityType.View)
			{
				_spawnList.Add(id);
			}
			else if (netObj.Visibility == VisibilityType.Global)
			{
				foreach (var kv in _playerVisibleBySession)
				{
					kv.Value.SpawnObjects.Add(id, netObj);
				}
			}
		}

		public void OnDestroy(MasterNetworkObject netObj)
		{
			var id = netObj.Identity;

			if (netObj.Visibility == VisibilityType.View)
			{
				if (!GetCell(netObj.Transform.Position).Remove(id))
				{
					_spawnList.Remove(id);
				}
			}
			else if (netObj.Visibility == VisibilityType.Global)
			{
				foreach (var kv in _playerVisibleBySession)
				{
					kv.Value.DespawnObjects.Add(id, netObj);
				}
			}
		}

		public void CreatePlayerVisibleTable(NetworkPlayer player)
		{
			var playerVisibleTable = _playerVisibleTablePool.Get();
			_playerVisibleBySession.Add(player, playerVisibleTable);
		}

		public void DestroyNetworkPlayer(NetworkPlayer player)
		{
			if (!_playerVisibleBySession.TryGetValue(player, out var visibleTable))
			{
				_log.Error($"[{_gameplayInstance}] There is no {player}'s visible table!");
				return;
			}
			visibleTable.Clear();
			_playerVisibleTablePool.Return(visibleTable);
			_playerVisibleBySession.Remove(player);
		}
	}
}

#nullable disable