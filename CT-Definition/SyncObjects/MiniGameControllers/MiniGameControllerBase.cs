#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 2)]
	public class MiniGameControllerBase : SceneControllerBase
	{
		[SyncVar]
		public GameplayState GameplayState;

		[SyncVar]
		public float GameTime;

		[SyncVar(SyncType.ColdData)]
		public float CurrentTime;

		[SyncObject]
		public SyncList<UserId> EliminatedPlayers = new();

		[SyncObject]
		public MapVoteController MapVoteController = new();

		[SyncRpc]
		public void Server_GameStartCountdown(float missionShowTime, float countdown) { }

		[SyncRpc]
		public void Server_GameStart(float timeLeft) { }

		[SyncRpc]
		public void Server_GameEnd(float freezeTime) { }

		[SyncRpc]
		public void Server_ShowResult(float resultTime) { }

		[SyncRpc]
		public void Server_ShowExecution(ExecutionCutSceneType cutSceneType, float playTime) { }

		[SyncRpc]
		public void Server_StartVoteMap(float mapVoteTime) { }

		[SyncRpc]
		public void Server_ShowVotedNextMap(GameSceneIdentity nextMap, float showTime) { }

		[SyncRpc]
		public void Server_SyncTimer(float timeLeft) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_ReadyGame(bool isReady) { }
	}
}
#pragma warning restore IDE0051