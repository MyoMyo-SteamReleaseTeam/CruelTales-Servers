#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects.MiniGames
{
	[SyncNetworkObjectDefinition(capacity: 1)]
	public class BaseMiniGameController
	{
		[SyncVar]
		MiniGameMapData
	}
}

#pragma warning restore IDE0051
