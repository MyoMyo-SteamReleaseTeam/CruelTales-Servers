#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using CT.Common.DataType;
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
		//public const float WORLD_MAX_WIDTH = 256.0f;
		//public const float WORLD_MAX_HEIGHT = 256.0f;
		public const float WORLD_MAX_WIDTH = 128.0f;
		public const float WORLD_MAX_HEIGHT = 128.0f;

		public const float CELL_SIZE = 8.0f;
		public const float INVERSE_CEll_SIZE = 1.0f / CELL_SIZE;

		public const float ORIGIN_OFFSET_X = WORLD_MAX_WIDTH / 2; //128.0f;
		public const float ORIGIN_OFFSET_Y = WORLD_MAX_HEIGHT / 2; //128.0f;

		public const int CELL_WIDTH = (int)(WORLD_MAX_WIDTH / CELL_SIZE); // 32;
		public const int CELL_HEIGHT = (int)(WORLD_MAX_HEIGHT / CELL_SIZE); // 32;

		// Partitioned visibility set
		private List<NetworkIdentity> _tempSpawnList;
		private List<MasterNetworkObject> _tempDespawnList;
		private Dictionary<NetworkIdentity, MasterNetworkObject>[,] _networkObjectByCell;
		private Dictionary<NetworkIdentity, MasterNetworkObject> _nullSet = new();

		// Global visibility set
		private HashSet<MasterNetworkObject> _globalObjectSet;

		// Player visible tables
		private ObjectPool<PlayerVisibleTable> _playerVisibleTablePool;
		private Dictionary<NetworkPlayer, PlayerVisibleTable> _playerVisibleBySession;

		public WorldVisibilityManager(GameplayInstance gameplayInstance,
									  WorldManager worldManager,
									  InstanceInitializeOption option)
		{
			_gameplayInstance = gameplayInstance;
			_worldManager = worldManager;

			// Initialize partitioned visibility set
			_tempSpawnList = new(option.VisibleSpawnCapacity);
			_tempDespawnList = new(option.VisibleDespawnCapacity);
			_networkObjectByCell = new Dictionary<NetworkIdentity, MasterNetworkObject>[CELL_HEIGHT, CELL_WIDTH];
			for (int z = 0; z < CELL_HEIGHT; z++)
			{
				for (int x = 0; x < CELL_WIDTH; x++)
				{
					_networkObjectByCell[z, x] = new(option.VisibleCellCapacity);
				}
			}

			// Initialize global visibility set
			_globalObjectSet = new(option.GlobalTraceObjectCapacity);

			// Visible Table
			_playerVisibleTablePool = new(() => new PlayerVisibleTable(option), option.SystemMaxUser);
			_playerVisibleBySession = new(option.SystemMaxUser);
		}

		public void Reset()
		{
			_tempSpawnList.Clear();
			_tempDespawnList.Clear();

			for (int z = 0; z < CELL_HEIGHT; z++)
			{
				for (int x = 0; x < CELL_WIDTH; x++)
				{
					_networkObjectByCell[z, x].Clear();
				}
			}

			_globalObjectSet.Clear();

			foreach (PlayerVisibleTable ptable in _playerVisibleBySession.Values)
			{
				_playerVisibleTablePool.Return(ptable);
			}

			_playerVisibleBySession.Clear();
		}

		private Dictionary<NetworkIdentity, MasterNetworkObject> getCell(Vector2Int internalCell)
		{
			if (internalCell.Y < 0 || internalCell.Y >= CELL_HEIGHT ||
				internalCell.X < 0 || internalCell.X >= CELL_WIDTH)
			{
				//Debug.Assert(false);
				return _nullSet;
			}

			return _networkObjectByCell[internalCell.Y, internalCell.X];
		}

		private Dictionary<NetworkIdentity, MasterNetworkObject> getCell(int x, int z)
		{
			if (z < 0 || z >= CELL_HEIGHT ||
				x < 0 || x >= CELL_WIDTH)
			{
				//Debug.Assert(false);
				return _nullSet;
			}

			return _networkObjectByCell[z, x];
		}

		public static Vector2Int GetWorldCell(Vector2 position)
		{
			float curPosX = position.X + ORIGIN_OFFSET_X;
			float curPosZ = position.Y + ORIGIN_OFFSET_Y;
			int cellX = (int)(curPosX * INVERSE_CEll_SIZE);
			int cellZ = (int)(curPosZ * INVERSE_CEll_SIZE);
			return new Vector2Int(cellX, cellZ);
		}

		public static Vector2Int GetWorldCell(Vector3 position)
		{
			float curPosX = position.X + ORIGIN_OFFSET_X;
			float curPosZ = position.Z + ORIGIN_OFFSET_Y;
			int cellX = (int)(curPosX * INVERSE_CEll_SIZE);
			int cellZ = (int)(curPosZ * INVERSE_CEll_SIZE);
			return new Vector2Int(cellX, cellZ);
		}

		private Dictionary<NetworkIdentity, MasterNetworkObject> _outObjectSet = new(16);

#if DEBUG
		string DEBUG_output = "";
#endif

		public void UpdateVisibilityAndSendData()
		{
#if DEBUG
			//{
			//	StringBuilder sb = new StringBuilder(1024);
			//	sb.Append($"Cur Objects : ");

			//	for (int y = 0; y < CELL_HEIGHT; y++)
			//	{
			//		for (int x = 0; x < CELL_WIDTH; x++)
			//		{
			//			foreach (var kv in _networkObjectByCell[y, x])
			//			{
			//				sb.Append(kv.Value.Identity);
			//				sb.Append(",");
			//			}
			//		}
			//	}

			//	string output = sb.ToString();
			//	if (DEBUG_output != output)
			//	{
			//		DEBUG_output = output;
			//		Console.WriteLine(output);
			//	}
			//}
#endif

			// TODO : 구문 최적화 필요
			// Check visibility by player
			foreach (var kv in _playerVisibleBySession)
			{
				NetworkPlayer player = kv.Key;
				PlayerVisibleTable viewTable = kv.Value;
				Vector2 viewPos = player.ViewPosition;

				Vector2 inBoundary = player.HalfViewInSize;
				Vector2 inboundLB = viewPos - inBoundary;
				Vector2 inboundRT = viewPos + inBoundary;

				Vector2 outBoundary = player.HalfViewOutSize;
				Vector2 outboundLB = viewPos - outBoundary;
				Vector2 outboundRT = viewPos + outBoundary;

				Vector2Int inboundCellLB = GetWorldCell(inboundLB);
				Vector2Int inboundCellRT = GetWorldCell(inboundRT);

				// Add spawn object
				int sCount = _tempSpawnList.Count;
				for (int i = 0; i < sCount; i++)
				{
					var spawnObjectId = _tempSpawnList[i];
					if (!_worldManager.TryGetNetworkObjectInternal(spawnObjectId, out var spawnObject))
					{
						Debug.Assert(false);
						continue;
					}

					Vector2 objPos = spawnObject.RigidBody.Position;
					if ((objPos.X >= inboundLB.X && objPos.X <= inboundRT.X &&
						objPos.Y >= inboundLB.Y && objPos.Y <= inboundRT.Y) ||
						shouldForceShow(player, spawnObject))
					{
						if (spawnObject.IsValidVisibilityAuthority(player))
						{
							bool isAdded = viewTable.SpawnObjects.TryAdd(spawnObject.Identity, spawnObject);
							Debug.Assert(isAdded);
						}
					}
				}

				// Add enter object
				for (int cz = inboundCellLB.Y; cz <= inboundCellRT.Y; cz++)
				{
					for (int cx = inboundCellLB.X; cx <= inboundCellRT.X; cx++)
					{
						var curCell = getCell(cx, cz);
						foreach (var netObj in curCell.Values)
						{
							Vector2 objPos = netObj.RigidBody.Position;
							if ((objPos.X >= inboundLB.X && objPos.X <= inboundRT.X &&
								objPos.Y >= inboundLB.Y && objPos.Y <= inboundRT.Y) ||
								shouldForceShow(player, netObj))
							{
								if (viewTable.TraceObjects.ContainsKey(netObj.Identity))
								{
									continue;
								}

								Debug.Assert(!viewTable.SpawnObjects.ContainsKey(netObj.Identity));

								if (netObj.IsValidVisibilityAuthority(player))
								{
									bool isAdded = viewTable.EnterObjects.TryAdd(netObj.Identity, netObj);
									//Debug.Assert(isAdded);
								}
							}
						}
					}
				}

				// Despawn : Check if it's despawn object
				int dCount = _tempDespawnList.Count;
				for (int i = 0; i < dCount; i++)
				{
					var despawnObj = _tempDespawnList[i];
					var despawnId = despawnObj.Identity;
					if (viewTable.TraceObjects.Remove(despawnId))
					{
						viewTable.DespawnObjects.Add(despawnId, despawnObj);
					}
					else if (viewTable.EnterObjects.Remove(despawnId))
					{
						// It was destroyed and entered at the same time!
					}
					else if (viewTable.LeaveObjects.Remove(despawnId))
					{
						// It was destroyed and leave at the same time!
						Debug.Assert(false);
					}
					else if (viewTable.SpawnObjects.Remove(despawnId))
					{
						// or, it was spawned and immediately despawned
						Debug.Assert(false);
					}
				}

				// Leave : Check if it's out of the view
				_outObjectSet.Clear();
				foreach (var netObj in viewTable.TraceObjects.Values)
				{
					Vector2 objPos = netObj.RigidBody.Position;
					if ((objPos.X < outboundLB.X || objPos.X > outboundRT.X ||
						objPos.Y < outboundLB.Y || objPos.Y > outboundRT.Y ||
						!netObj.IsValidVisibilityAuthority(player)) && 
						!shouldForceShow(player, netObj))
					{
						bool isAdded = _outObjectSet.TryAdd(netObj.Identity, netObj);
						Debug.Assert(isAdded);
					}
				}

				foreach (var outObject in _outObjectSet.Values)
				{
					Debug.Assert(!viewTable.DespawnObjects.ContainsKey(outObject.Identity));
					Debug.Assert(!viewTable.LeaveObjects.ContainsKey(outObject.Identity));

					bool isTraceRemoved = viewTable.TraceObjects.Remove(outObject.Identity);
					if (isTraceRemoved)
					{
						bool isAdded = viewTable.LeaveObjects.TryAdd(outObject.Identity, outObject);
						Debug.Assert(isAdded);
					}

					// Object was destoryed before leave
					Debug.Assert(isTraceRemoved);
				}
			}

			// Add spawn object to world partition
			int spawnCount = _tempSpawnList.Count;
			for (int i = 0; i < spawnCount; i++)
			{
				var transitId = _tempSpawnList[i];
				if (!_worldManager.TryGetNetworkObjectInternal(transitId, out var netObj))
				{
					Debug.Assert(false);
					continue;
				}
				var curCell = getCell(netObj.CurrentCellPos);
				bool isAdded = curCell.TryAdd(netObj.Identity, netObj);
				Debug.Assert(isAdded);
			}

			// Remove despawn object from world partition
			int despawnCount = _tempDespawnList.Count;
			for (int i = 0; i < despawnCount; i++)
			{
				var transitId = _tempDespawnList[i];
				if (!_worldManager.TryGetNetworkObjectInternal(transitId.Identity, out var netObj))
				{
					Debug.Assert(false);
					continue;
				}
				var curCell = getCell(netObj.CurrentCellPos);
				bool isRemoved = curCell.Remove(netObj.Identity);
				Debug.Assert(isRemoved);
			}

			_tempSpawnList.Clear();
			_tempDespawnList.Clear();

			// Transition visibility cycle
			// Spawn, Enter -> Trace
			// Trace -> Trace
			// Despawn, Leave -> delete
			foreach (var kv in _playerVisibleBySession)
			{
				PlayerVisibleTable visibleTable = kv.Value;
				_worldManager.SendSynchronization(kv.Key, visibleTable);
				visibleTable.TransitionVisibilityCycle();
			}

			bool shouldForceShow(NetworkPlayer player, MasterNetworkObject netObj)
			{
				if (player.IsShowAll)
					return true;

				if (netObj.Visibility == VisibilityType.ViewAndOwner)
					return player.UserId == netObj.Owner;

				return false;
			}
		}

		public void OnCellChanged(MasterNetworkObject netObj, Vector2Int previous, Vector2Int current)
		{
			var previousCell = getCell(previous);
			bool isRemoved = previousCell.Remove(netObj.Identity);
			Debug.Assert(isRemoved);

			var currentCell = getCell(current);
			Debug.Assert(!currentCell.ContainsKey(netObj.Identity));
			currentCell.Add(netObj.Identity, netObj);
		}

		public void OnCreated(MasterNetworkObject netObj)
		{
			var id = netObj.Identity;

			VisibilityType visibility = netObj.Visibility;

			if (visibility.IsViewType())
			{
				_tempSpawnList.Add(id);
			}
			else if (visibility.IsGlobalType())
			{
				_globalObjectSet.Add(netObj);

				foreach (var kv in _playerVisibleBySession)
				{
					if (netObj.IsValidVisibilityAuthority(kv.Key))
					{
						kv.Value.GlobalSpawnObjects.Add(id, netObj);
					}
				}
			}
		}

		public void OnDestroy(MasterNetworkObject netObj)
		{
			var id = netObj.Identity;

			VisibilityType visibility = netObj.Visibility;

			if (visibility.IsViewType())
			{
				if (_tempSpawnList.Contains(id))
				{
					_tempSpawnList.Remove(id);
				}
				else
				{
					_tempDespawnList.Add(netObj);
				}
			}
			else if (visibility.IsGlobalType())
			{
				_globalObjectSet.Remove(netObj);

				foreach (var kv in _playerVisibleBySession)
				{
					var visibleTable = kv.Value;

					if (visibleTable.GlobalTraceObjects.Remove(id))
					{
						visibleTable.GlobalDespawnObjects.Add(id, netObj);
					}
					else
					{
						// It's was spawning and immediately despawning
						visibleTable.GlobalSpawnObjects.Remove(id);
					}
				}
			}
		}

		public void CreatePlayerVisibleTable(NetworkPlayer player)
		{
			var playerVisibleTable = _playerVisibleTablePool.Get();
			playerVisibleTable.Initialize(player);
			foreach (var globalObj in _globalObjectSet)
			{
				if (globalObj.IsValidVisibilityAuthority(player))
				{
					playerVisibleTable.GlobalSpawnObjects.Add(globalObj.Identity, globalObj);
				}
			}
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