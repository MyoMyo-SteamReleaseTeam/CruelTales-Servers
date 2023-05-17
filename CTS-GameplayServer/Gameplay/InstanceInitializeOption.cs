using System.Numerics;

namespace CTS.Instance.Gameplay
{
	/// <summary>서버의 초기화 옵션입니다.</summary>
	public struct InstanceInitializeOption
	{
		public int SystemMaxUser { get; set; }
		public int PartitionCellCapacity { get; set; }

		// Player visible table
		public int SpawnObjectCapacity { get; set; }
		public int TraceObjectCapacity { get; set; }
		public int DespawnObjectCapacity { get; set; }
		public Vector2 ViewInSize { get; set; }
		public Vector2 ViewOutSize { get; set; }
		public int DestroyObjectStackCapacity { get; set; }

		public int SyncJobCapacity => SystemMaxUser * 20;
		public int SessionJobCapacity => (int)(SystemMaxUser * 1.5f);

		public InstanceInitializeOption()
		{
			SystemMaxUser = 7;
			PartitionCellCapacity = 12;

			SpawnObjectCapacity = 16;
			TraceObjectCapacity = 32;
			DespawnObjectCapacity = 16;

			DestroyObjectStackCapacity = 16;

			ViewInSize = new Vector2(20, 16);
			ViewOutSize = new Vector2(24, 20);
		}
	}
}
