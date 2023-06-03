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

		[SyncVar(SyncType.ColdData)]
		public float AnimationTime;

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

		////For test
		//[SyncVar(SyncType.ColdData, SyncDirection.FromRemote)]
		//public float TestColdDataFromRemote;

		[SyncRpc(SyncType.Unreliable, SyncDirection.FromRemote)]
		public void Client_Input(float x, float z) { }

		[SyncRpc(SyncType.ReliableTarget)]
		public void Server_CommandTarget(NetString command, int number) { }

		[SyncRpc]
		public void Server_CommandBroadcast(NetString command, int number) { }
	}
}
#pragma warning restore IDE0051