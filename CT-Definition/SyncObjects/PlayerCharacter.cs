#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using System.Numerics;
using CT.Common.DataType;
using CT.Common.DataType.Input;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Players;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(1, true)]
	public class PlayerCharacter
	{
		[SyncVar]
		public UserId UserId;

		[SyncVar]
		public NetStringShort Username;

		// Animations
		[SyncVar(SyncType.ColdData)]
		public DokzaAnimationState AnimationState;

		[SyncVar(SyncType.ColdData)]
		public ProxyDirection ProxyDirection;

		[SyncVar(SyncType.ColdData)]
		public float AnimationTime;

		[SyncRpc]
		public void Server_OnAnimationChanged(DokzaAnimationState state) { }

		[SyncRpc]
		public void Server_OnAnimationChanged(DokzaAnimationState state, ProxyDirection direction) { }

		[SyncRpc]
		public void Server_OnProxyDirectionChanged(ProxyDirection direction) { }

		[SyncRpc(SyncType.Unreliable, SyncDirection.FromRemote)]
		public void Client_RequestInput(InputData inputData) { }

		[SyncRpc(SyncType.ReliableTarget)]
		public void Server_OrderTest(int fromServer) { }
		
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_RequestTest(int fromClient) { }
		
		[SyncRpc]
		public void Server_BroadcastOrderTest(int userId, int fromSever) { }

		[SyncRpc]
		public void Server_TestPositionTickByTick(Vector2 curPosition) { }
		
		//[SyncRpc(SyncType.Unreliable, SyncDirection.FromRemote)]
		//public void Client_InputInteraction(NetworkIdentity target, Input_InteractType interactType) { }

		//[SyncRpc(SyncType.Unreliable, SyncDirection.FromRemote)]
		//public void Client_InputAction(Input_PlayerAction actionType, Vector2 direction) { }
	}
}
#pragma warning restore IDE0051