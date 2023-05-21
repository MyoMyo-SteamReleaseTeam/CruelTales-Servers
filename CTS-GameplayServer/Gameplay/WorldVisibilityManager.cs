#nullable enable

using System;
using System.Collections.Generic;
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
		private List<MasterNetworkObject> _spawnList;
		private HashSet<MasterNetworkObject>[,] _networkObjectByCell;
		private HashSet<MasterNetworkObject> _nullSet = new();

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
			_networkObjectByCell = new HashSet<MasterNetworkObject>[CELL_HEIGHT, CELL_WIDTH];
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

		public HashSet<MasterNetworkObject> GetCell(Vector3 pos) => GetCell(GetWorldCell(pos));

		public HashSet<MasterNetworkObject> GetCell(Vector2Int internalCell)
		{
			if (internalCell.Y < 0 || internalCell.Y > CELL_HEIGHT ||
				internalCell.X < 0 || internalCell.X > CELL_WIDTH)
			{
				return _nullSet;
			}

			return _networkObjectByCell[internalCell.Y, internalCell.X];
		}

		public HashSet<MasterNetworkObject> GetCell(int x, int z)
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

		private HashSet<MasterNetworkObject> _outObjectSet = new(16);
		public void UpdateVisibilityAndSendData()
		{
			foreach (var kv in _playerVisibleBySession)
			{
				NetworkPlayer player = kv.Key;
				PlayerVisibleTable viewTable = kv.Value;
				Vector3 viewPos3D = player.ViewTransform.Position;
				Vector2 viewPos = new Vector2(viewPos3D.X, viewPos3D.Z);
				Vector2 outBoundary = player.HalfViewOutSize;

				Vector2Int inboundLB = GetWorldCell(viewPos - player.HalfViewInSize);
				Vector2Int inboundRT = GetWorldCell(viewPos + player.HalfViewOutSize);

				// Add spawn object
				int spawnCount = _spawnList.Count;
				for (int i = 0; i < spawnCount; i++)
				{
					var spawnObject = _spawnList[i];

					Vector3 objPos = spawnObject.Transform.Position;
					if (objPos.X < inboundLB.X || objPos.X > inboundRT.X ||
						objPos.Y < inboundLB.Y || objPos.Y > inboundRT.Y)
					{
						if (spawnObject.IsValidVisibilityAuthority(player))
						{
							viewTable.SpawnObjects.Add(spawnObject);
						}
					}
				}

				// Add respawn object
				for (int cz = inboundLB.Y; cz <= inboundRT.Y; cz++)
				{
					for (int cx = inboundLB.X; cx <= inboundRT.X; cx++)
					{
						var curCell = GetCell(cx, cz);
						foreach (var netObj in curCell)
						{
							Vector3 objPos = netObj.Transform.Position;
							if (objPos.X < inboundLB.X || objPos.X > inboundRT.X ||
								objPos.Y < inboundLB.Y || objPos.Y > inboundRT.Y)
							{
								if (viewTable.TraceObjects.Contains(netObj))
								{
									continue;
								} 

								// TODO : 초기 생성 객체는 셀 타일에 등록되지 않도록 관리되어야함
								// 임시 코드
								if (viewTable.SpawnObjects.Contains(netObj))
								{
									continue;
								}
								// 임시 코드

								if (netObj.IsValidVisibilityAuthority(player))
								{
									viewTable.RespawnObjects.Add(netObj);
								}
							}
						}
					}
				}

				// Check if it's out of the view
				_outObjectSet.Clear();
				foreach (var netObj in viewTable.TraceObjects)
				{
					Vector3 objPos = netObj.Transform.Position;
					if (objPos.X < viewPos.X - outBoundary.X || objPos.X > viewPos.X + outBoundary.X ||
						objPos.Y < viewPos.Y - outBoundary.Y || objPos.Y > viewPos.Y + outBoundary.Y)
					{
						_outObjectSet.Add(netObj);
					}
				}

				foreach (var outObject in _outObjectSet)
				{
					viewTable.TraceObjects.Remove(outObject);
					viewTable.DespawnObjects.Add(outObject);
				}
			}

			// Add first spawn object to world partition
			int transitionCount = _spawnList.Count;
			for (int i = 0; i < transitionCount; i++)
			{
				var netObj = _spawnList[i];
				GetCell(netObj.Transform.Position).Add(netObj);
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

		public void OnCellChanged(MasterNetworkObject netObject, Vector2Int previous, Vector2Int current)
		{
			GetCell(previous).Remove(netObject);
			GetCell(current).Add(netObject);
		}

		public void OnCreated(MasterNetworkObject netObj)
		{
			if (netObj.Visibility == VisibilityType.View)
			{
				_spawnList.Add(netObj);
			}
			else if (netObj.Visibility == VisibilityType.Global)
			{
				foreach (var kv in _playerVisibleBySession)
				{
					kv.Value.SpawnObjects.Add(netObj);
				}
			}
		}

		public void OnDestroy(MasterNetworkObject netObj)
		{
			if (netObj.Visibility == VisibilityType.View)
			{
				if (!GetCell(netObj.Transform.Position).Remove(netObj))
				{
					_spawnList.Remove(netObj);
				}
			}
			else if (netObj.Visibility == VisibilityType.Global)
			{
				foreach (var kv in _playerVisibleBySession)
				{
					kv.Value.DespawnObjects.Add(netObj);
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