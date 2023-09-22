#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncObjectDefinition]
	public class MapVoteController
	{
		[SyncObject]
		public SyncList<GameSceneIdentity> NextMapVoteList = new();

		[SyncVar]
		public GameSceneIdentity NextMap;

		[SyncObject(cc: Constant.CAPACITY_BY_PLAYER)]
		public SyncDictionary<UserId, NetInt32> MapIndexByUserId = new();

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_VoteMap(int mapIndex) { }
	}
}
#pragma warning restore IDE0051