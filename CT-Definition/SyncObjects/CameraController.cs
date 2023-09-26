#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using System.Numerics;
using CT.Common.DataType;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(1, true)]
	public class CameraController
	{
		[SyncVar]
		public NetworkIdentity TargetId;

		[SyncVar]
		public float FollowSpeed;

		/// <summary>카메라가 특정 지점으로 강제 이동함</summary>
		[SyncRpc]
		public void Server_MoveTo(Vector2 position) { }

		/// <summary>카메라가 특정 지점을 바라봄</summary>
		[SyncRpc]
		public void Server_LookAt(Vector2 position) { }

		/// <summary>카메라가 특정 지점을 특정 시간동안 바라봄</summary>
		[SyncRpc]
		public void Server_LookAt(Vector2 position, float time) { }

		[SyncRpc]
		public void Server_Shake() { }

		[SyncRpc]
		public void Server_Zoom(float zoom) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_CannotFindBindTarget() { }
	}
}

#pragma warning restore IDE0051