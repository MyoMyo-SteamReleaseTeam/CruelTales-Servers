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
		public NetworkIdentity Target;

		[SyncRpc]
		public void Server_MoveTo(Vector2 position) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_CannotFindBindTarget() { }
	}
}

#pragma warning restore IDE0051