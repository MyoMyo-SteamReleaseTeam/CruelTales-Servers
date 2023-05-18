using System.Collections.Generic;
using CT.Common.DataType;

namespace CTS.Instance.Gameplay
{
	public class PlayerVisibleTable
	{
		/// <summary>
		/// 생성된 오브젝트의 집합입니다.
		/// 최초 1회 생성 데이터를 송신한 뒤 추적 객체로 전환됩니다.
		/// </summary>
		public HashSet<NetworkIdentity> SpawnObjects { get; private set; }

		/// <summary>
		/// 추적중인 오브젝트의 집합입니다.
		/// 갱신 주기마다 동기화 데이터를 전송합니다.
		/// </summary>
		public HashSet<NetworkIdentity> TraceObjects { get; private set; }

		/// <summary>
		/// 거리에 따른 가시성 영향을 받지 않는 전역 객체의 집합입니다.
		/// 갱신 주기마다 동기화 데이터를 전송합니다.
		/// </summary>
		public HashSet<NetworkIdentity> GlobalObjects { get; private set; }

		/// <summary>
		/// 삭제된 오브젝트의 집합입니다.
		/// 삭제후 1회 삭제 데이터를 송신한 뒤 집합에서 제거됩니다.
		/// </summary>
		public HashSet<NetworkIdentity> DespawnObjects { get; private set; }

		public PlayerVisibleTable(InstanceInitializeOption option)
		{
			SpawnObjects = new HashSet<NetworkIdentity>(option.SpawnObjectCapacity);
			TraceObjects = new HashSet<NetworkIdentity>(option.TraceObjectCapacity);
			DespawnObjects = new HashSet<NetworkIdentity>(option.DespawnObjectCapacity);
			GlobalObjects = new HashSet<NetworkIdentity>(option.GlobalObjectCapacity);
		}

		public void Clear()
		{
			SpawnObjects.Clear();
			TraceObjects.Clear();
			DespawnObjects.Clear();
			GlobalObjects.Clear();
		}
	}
}
