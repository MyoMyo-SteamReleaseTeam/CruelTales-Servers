#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Synchronizations;

namespace CT.Common.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition]
	public class TestCube
	{
		[SyncVar]
		public float R;

		[SyncVar]
		public float G;

		[SyncVar]
		public float B;

		[SyncRpc]
		public void TestRPC(long someMessage) { }
	}

	[SyncNetworkObjectDefinition]
	public class PlayerCharacter
	{
		[SyncVar]
		public UserId UserId;

		[SyncVar]
		public NetStringShort Username;

		[SyncVar]
		public int Costume;

		[SyncRpc(SyncType.Unreliable, SyncDirection.FromRemote)]
		public void Client_Input(float x, float z) { }
	}
}
#pragma warning restore IDE0051