#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.Gameplay.RedHood;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 16)]
	public class RedHoodMissionInteractor : Interactor
	{
		[SyncVar]
		public RedHoodMission Mission;
	}
}
#pragma warning restore IDE0051