using System.Collections.Generic;
using CT.Common.DataType;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.Gameplay
{
	public class PlayerVisibleTable
	{
		private NetworkPlayer? _networkPlayer { get; set; }
		public UserId UserId => _networkPlayer == null ? new UserId(0) : _networkPlayer.UserId;

		/// <summary>
		/// 가시성 내에서 생성된 오브젝트의 집합입니다.
		/// 최초 1회 생성 데이터를 송신한 뒤 추적 객체로 전환됩니다.
		/// </summary>
		public Dictionary<NetworkIdentity, MasterNetworkObject> SpawnObjects { get; private set; }

		/// <summary>
		/// 가시성 내에서 삭제된 오브젝트의 집합입니다.
		/// 최초 1회 생성 데이터를 송신한 뒤 추적 객체로 전환됩니다.
		/// </summary>
		public Dictionary<NetworkIdentity, MasterNetworkObject> DespawnObjects { get; private set; }

		/// <summary>
		/// 가시성에 진입한 오브젝트의 집합입니다.
		/// 최초 1회 생성 데이터를 송신한 뒤 추적 객체로 전환됩니다.
		/// </summary>
		public Dictionary<NetworkIdentity, MasterNetworkObject> EnterObjects { get; private set; }

		/// <summary>
		/// 추적중인 오브젝트의 집합입니다.
		/// 갱신 주기마다 동기화 데이터를 전송합니다.
		/// </summary>
		public Dictionary<NetworkIdentity, MasterNetworkObject> TraceObjects { get; private set; }

		/// <summary>
		/// 가시성에서 벗어난 오브젝트의 집합입니다.
		/// 삭제후 1회 삭제 데이터를 송신한 뒤 집합에서 제거됩니다.
		/// </summary>
		public Dictionary<NetworkIdentity, MasterNetworkObject> LeaveObjects { get; private set; }

		/// <summary>
		/// 거리에 따른 가시성 영향을 받지 않는 생성된 오브젝트의 집합입니다.
		/// 최초 1회 생성 데이터를 송신한 뒤 추적 객체로 전환됩니다.
		/// </summary>
		public Dictionary<NetworkIdentity, MasterNetworkObject> GlobalSpawnObjects { get; private set; }
		
		/// <summary>
		/// 거리에 따른 가시성 영향을 받지 않는 전역 객체의 집합입니다.
		/// 갱신 주기마다 동기화 데이터를 전송합니다.
		/// </summary>
		public Dictionary<NetworkIdentity, MasterNetworkObject> GlobalTraceObjects { get; private set; }

		/// <summary>
		/// 거리에 따른 가시성 영향을 받지 않는 삭제된 오브젝트의 집합입니다.
		/// 삭제후 1회 삭제 데이터를 송신한 뒤 집합에서 제거됩니다.
		/// </summary>
		public Dictionary<NetworkIdentity, MasterNetworkObject> GlobalDespawnObjects { get; private set; }

		public PlayerVisibleTable(InstanceInitializeOption option)
		{
			SpawnObjects = new(option.SpawnObjectCapacity);
			DespawnObjects = new(option.DespawnObjectCapacity);
			EnterObjects = new(option.EnterObjectCapacity);
			TraceObjects = new(option.TraceObjectCapacity);
			LeaveObjects = new(option.LeaveObjectCapacity);

			GlobalSpawnObjects = new(option.GlobalSpawnObjectCapacity);
			GlobalTraceObjects = new(option.GlobalTraceObjectCapacity);
			GlobalDespawnObjects = new(option.GlobalDespawnObjectCapacity);
		}

		public void Initialize(NetworkPlayer player)
		{
			_networkPlayer = player;
		}

		public void Clear()
		{
			_networkPlayer = null;

			SpawnObjects.Clear();
			EnterObjects.Clear();
			TraceObjects.Clear();
			LeaveObjects.Clear();

			GlobalSpawnObjects.Clear();
			GlobalTraceObjects.Clear();
			GlobalDespawnObjects.Clear();
		}

		public void TransitionVisibilityCycle()
		{
			// Spawn to Trace
			if (SpawnObjects.Count != 0)
			{
				foreach (var kv in SpawnObjects)
				{
					TraceObjects.Add(kv.Key, kv.Value);
				}
				SpawnObjects.Clear();
			}

			// Enter to Trace
			if (EnterObjects.Count != 0)
			{
				foreach (var kv in EnterObjects)
				{
					TraceObjects.Add(kv.Key, kv.Value);
				}
				EnterObjects.Clear();
			}

			// Clear Leave, Desapwn
			LeaveObjects.Clear();
			DespawnObjects.Clear();

			// Spawn to Trace
			if (GlobalSpawnObjects.Count != 0)
			{
				foreach (var kv in GlobalSpawnObjects)
				{
					GlobalTraceObjects.Add(kv.Key, kv.Value);
				}
				GlobalSpawnObjects.Clear();
			}
			GlobalDespawnObjects.Clear();
		}
	}
}
