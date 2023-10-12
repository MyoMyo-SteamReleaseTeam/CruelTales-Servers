#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncObjectDefinition]
	public class PlayerState
	{
		[SyncVar]
		public UserId UserId;

		[SyncVar]
		public NetStringShort Username;

		[SyncVar]
		public Faction Faction;

		[SyncVar]
		public bool IsEliminated;

		[SyncVar]
		public bool IsHost;

		[SyncVar]
		public bool IsReady;

		[SyncVar]
		public bool IsMapLoaded;

		[SyncObject]
		public CostumeSet SelectedCostume = new();

		[SyncObject]
		public CostumeSet CurrentCostume = new();
	}
}
#pragma warning restore IDE0051