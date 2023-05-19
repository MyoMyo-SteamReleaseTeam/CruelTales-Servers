using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.Gameplay
{
	public class PlayerVisibleTable
	{
		/// <summary>
		/// 곧바로 생성된 오브젝트의 집합입니다.
		/// 최초 1회 생성 데이터를 송신한 뒤 추적 객체로 전환됩니다.
		/// </summary>
		public HashSet<MasterNetworkObject> SpawnObjects { get; private set; }

		/// <summary>
		/// 재등장하는 오브젝트의 집합입니다.
		/// 최초 1회 생성 데이터를 송신한 뒤 추적 객체로 전환됩니다.
		/// </summary>
		public HashSet<MasterNetworkObject> RespawnObjects { get; private set; }

		/// <summary>
		/// 추적중인 오브젝트의 집합입니다.
		/// 갱신 주기마다 동기화 데이터를 전송합니다.
		/// </summary>
		public HashSet<MasterNetworkObject> TraceObjects { get; private set; }

		/// <summary>
		/// 삭제된 오브젝트의 집합입니다.
		/// 삭제후 1회 삭제 데이터를 송신한 뒤 집합에서 제거됩니다.
		/// </summary>
		public HashSet<MasterNetworkObject> DespawnObjects { get; private set; }

		/// <summary>
		/// 거리에 따른 가시성 영향을 받지 않는 생성된 오브젝트의 집합입니다.
		/// 최초 1회 생성 데이터를 송신한 뒤 추적 객체로 전환됩니다.
		/// </summary>
		public HashSet<MasterNetworkObject> GlobalSpawnObjects { get; private set; }
		
		/// <summary>
		/// 거리에 따른 가시성 영향을 받지 않는 전역 객체의 집합입니다.
		/// 갱신 주기마다 동기화 데이터를 전송합니다.
		/// </summary>
		public HashSet<MasterNetworkObject> GlobalTraceObjects { get; private set; }

		/// <summary>
		/// 거리에 따른 가시성 영향을 받지 않는 삭제된 오브젝트의 집합입니다.
		/// 삭제후 1회 삭제 데이터를 송신한 뒤 집합에서 제거됩니다.
		/// </summary>
		public HashSet<MasterNetworkObject> GlobalDespawnObjects { get; private set; }

		public PlayerVisibleTable(InstanceInitializeOption option)
		{
			SpawnObjects = new(option.SpawnObjectCapacity);
			RespawnObjects = new(option.SpawnObjectCapacity);
			TraceObjects = new(option.TraceObjectCapacity);
			DespawnObjects = new(option.DespawnObjectCapacity);

			GlobalSpawnObjects = new(option.GlobalSpawnObjectCapacity);
			GlobalTraceObjects = new(option.GlobalTraceObjectCapacity);
			GlobalDespawnObjects = new(option.GlobalDespawnObjectCapacity);
		}

		public void Clear()
		{
			SpawnObjects.Clear();
			RespawnObjects.Clear();
			TraceObjects.Clear();
			DespawnObjects.Clear();

			GlobalSpawnObjects.Clear();
			GlobalTraceObjects.Clear();
			GlobalDespawnObjects.Clear();
		}

		public void TransitionVisibilityCycle()
		{
			if (SpawnObjects.Count != 0)
			{
				TraceObjects.UnionWith(SpawnObjects);
				SpawnObjects.Clear();
			}
			if (RespawnObjects.Count != 0)
			{
				TraceObjects.UnionWith(RespawnObjects);
				RespawnObjects.Clear();
			}
			DespawnObjects.Clear();

			if (GlobalSpawnObjects.Count != 0)
			{
				GlobalTraceObjects.UnionWith(GlobalSpawnObjects);
				GlobalSpawnObjects.Clear();
			}
			GlobalDespawnObjects.Clear();
		}
	}
}
