using System.Numerics;

namespace CTS.Instance.Gameplay
{
	/// <summary>서버의 초기화 옵션입니다.</summary>
	public struct InstanceInitializeOption
	{
		public int SystemMaxUser { get; set; }

		// Player visible table
		public int VisibleCellCapacity { get; set; }
		public int VisibleSpawnCapacity { get; set; }
		public int SpawnObjectCapacity { get; set; }
		public int RespawnObjectCapacity { get; set; }
		public int TraceObjectCapacity { get; set; }
		public int DespawnObjectCapacity { get; set; }
		public int GlobalSpawnObjectCapacity { get; set; }
		public int GlobalTraceObjectCapacity { get; set; }
		public int GlobalDespawnObjectCapacity { get; set; }

		// World Manager
		public int DestroyObjectStackCapacity { get; set; }

		// Half view boundary
		public Vector2 HalfViewInSize { get; set; }
		public Vector2 HalfViewOutSize { get; set; }

		public int SyncJobCapacity => SystemMaxUser * 20;
		public int SessionJobCapacity => (int)(SystemMaxUser * 1.5f);

		public InstanceInitializeOption()
		{
			SystemMaxUser = 7;

			VisibleCellCapacity = 16;
			VisibleSpawnCapacity = 8;
			SpawnObjectCapacity = 8;
			RespawnObjectCapacity = 16;
			TraceObjectCapacity = 32;
			DespawnObjectCapacity = 16;
			GlobalSpawnObjectCapacity = 16;
			GlobalTraceObjectCapacity = 16;
			GlobalDespawnObjectCapacity = 16;

			// World Manager
			DestroyObjectStackCapacity = 16;

			// Half view boundary
			HalfViewInSize = new Vector2(20, 16) / 2;
			HalfViewOutSize = new Vector2(24, 20) / 2;
		}
	}
}
