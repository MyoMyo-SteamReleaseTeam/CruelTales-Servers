#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 120)]
	public class TestCube
	{
		[SyncVar]
		public float R;

		[SyncVar]
		public float G;

		[SyncVar]
		public float B;

		[SyncVar(SyncType.ColdData)]
		public float AnimationTime;

		[SyncRpc]
		public void TestRPC(long someMessage) { }
	}
}
#pragma warning restore IDE0051