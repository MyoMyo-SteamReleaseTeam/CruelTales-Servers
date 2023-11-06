using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Gameplay.Events
{
	public interface IMissionTableHandler
	{
		public bool TryGetMissionTable(NetworkPlayer player,
									   [MaybeNullWhen(false)]
									   out SyncDictionary<NetByte, NetBool> missionTable);

		public void OnInteracted(NetworkPlayer player, PlayerCharacter Character, Interactor interactor);
	}
}
