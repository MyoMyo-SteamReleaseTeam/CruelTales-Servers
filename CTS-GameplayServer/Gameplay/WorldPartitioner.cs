﻿#nullable enable

using System.Collections.Generic;
using System.Numerics;
using CT.Common.DataType;

namespace CTS.Instance.Gameplay
{
	public class WorldPartitioner
	{
		public const float INVERSE_CEll_SIZE = 1.0f / 8.0f;

		public const float ORIGIN_OFFSET_X = 128.0f;
		public const float ORIGIN_OFFSET_Z = 128.0f;

		public const int CELL_WIDTH = 32;
		public const int CELL_HEIGHT = 32;
		public const int CELL_HALF_WIDTH = CELL_WIDTH / 2;
		public const int CELL_HALF_HEIGHT = CELL_HEIGHT / 2;

		private HashSet<NetworkIdentity>[,] _networkObjectByCell;
		private HashSet<NetworkIdentity> _nullSet = new();

		public WorldPartitioner(int capacityByCell)
		{
			_networkObjectByCell = new HashSet<NetworkIdentity>[CELL_HEIGHT, CELL_WIDTH];
			for (int z = 0; z < CELL_HEIGHT; z++)
			{
				for (int x = 0; x < CELL_WIDTH; x++)
				{
					_networkObjectByCell[z, x] = new HashSet<NetworkIdentity>(capacityByCell);
				}
			}
		}

		//private HashSet<NetworkIdentity> getCell(Vector3 pos) => getCell(GetWorldCell(pos));
		private HashSet<NetworkIdentity> getCell(Vector2Int internalCell)
		{
			if (internalCell.Y < 0 || internalCell.Y > CELL_HEIGHT ||
				internalCell.X < 0 || internalCell.X > CELL_WIDTH)
			{
				return _nullSet;
			}

			return _networkObjectByCell[internalCell.Y, internalCell.X];
		}

		public static Vector2Int GetWorldCell(Vector3 position)
		{
			float curPosX = position.X + ORIGIN_OFFSET_X;
			float curPosZ = position.Z + ORIGIN_OFFSET_Z;
			int cellX = (int)(curPosX * INVERSE_CEll_SIZE);
			int cellZ = (int)(curPosZ * INVERSE_CEll_SIZE);
			return new Vector2Int(cellX, cellZ);
		}

		public void OnCellChanged(NetworkIdentity id, Vector2Int previous, Vector2Int current)
		{
			getCell(previous).Remove(id);
			getCell(current).Add(id);
		}

		public void OnCreated(NetworkIdentity id, Vector2Int cellPos)
		{
			getCell(cellPos).Add(id);
		}

		public void OnDestroy(NetworkIdentity id, Vector2Int cellPos)
		{
			getCell(cellPos).Remove(id);
		}
	}
}

#nullable disable