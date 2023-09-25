#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 2)]
	public class DueoksiniTable : Interactor
	{
		[SyncVar]
		public Faction Team;

		[SyncObject]
		public SyncDictionary<NetInt32, NetByte> ItemCountByType = new();
	}
}
#pragma warning restore IDE0051