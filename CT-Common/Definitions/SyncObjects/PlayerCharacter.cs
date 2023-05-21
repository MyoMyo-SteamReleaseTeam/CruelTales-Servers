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
		public void TestRPC(NetStringShort someMessage) { }
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
	}
}
#pragma warning restore IDE0051