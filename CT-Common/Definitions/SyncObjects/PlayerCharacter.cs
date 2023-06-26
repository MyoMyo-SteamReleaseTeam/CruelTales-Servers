#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Common.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(1, true)]
	public class PlayerCharacter
	{
		[SyncVar]
		public UserId UserId;

		[SyncVar]
		public NetStringShort Username;

		[SyncVar]
		public int Costume;

		[SyncVar]
		public bool IsFreezed;

		// Animations
		[SyncVar(SyncType.ColdData)]
		public DokzaAnimation CurrentAnimation;

		[SyncVar(SyncType.ColdData)]
		public float AnimationTime;

		[SyncVar(SyncType.ColdData)]
		public bool IsRight;

		[SyncVar(SyncType.ColdData)]
		public bool IsUp;

		[SyncRpc]
		public void Server_OnAnimationChanged(DokzaAnimation animation, bool isRight, bool isUp) {}

		[SyncRpc(SyncType.Unreliable, SyncDirection.FromRemote)]
		public void Client_InputMovement(Vector2 direction, bool isWalk) { }

		[SyncRpc(SyncType.Unreliable, SyncDirection.FromRemote)]
		public void Client_InputInteraction(NetworkIdentity target, Input_InteractType interactType) { }

		[SyncRpc(SyncType.Unreliable, SyncDirection.FromRemote)]
		public void Client_InputAction(Input_PlayerAction actionType, Vector2 direction) { }
	}
}
#pragma warning restore IDE0051