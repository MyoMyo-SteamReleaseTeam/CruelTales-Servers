using System.Numerics;
using CT.Networks;

namespace CTS.Instance.Gameplay
{
	/// <summary>서버의 초기화 옵션입니다.</summary>
	public class InstanceInitializeOption
	{
		public int SystemMaxUser = GlobalNetwork.SYSTEM_MAX_USER;

		// Player visible table
		public int VisibleCellCapacity = 32;
		public int VisibleSpawnCapacity = 16;
		public int VisibleDespawnCapacity = 16;
		public int SpawnObjectCapacity = 16;
		public int DespawnObjectCapacity = 16;
		public int EnterObjectCapacity = 16;
		public int TraceObjectCapacity = 32;
		public int LeaveObjectCapacity = 16;
		public int GlobalSpawnObjectCapacity = 16;
		public int GlobalTraceObjectCapacity = 32;
		public int GlobalDespawnObjectCapacity = 16;

		// World Manager
		public int DestroyObjectStackCapacity = 16;

		// Half view boundary
		public Vector2 HalfViewInSize = new Vector2(32, 18) / 2;
		public Vector2 HalfViewOutSize = new Vector2(36, 22) / 2;

		public int SyncJobCapacity => SystemMaxUser * 20;
		public int SessionJobCapacity => (int)(SystemMaxUser * 1.5f);
		public int RemotePacketPoolCount => SystemMaxUser * 5;

		public InstanceInitializeOption() {}
	}
}
