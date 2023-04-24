#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Serialization.Type;
using CT.Common.Synchronizations;

namespace CTS.Instance.SyncDefinitions
{
	[SyncNetworkObjectDefinition]
	public partial class TestNetworkObject
	{
		[SyncVar]
		private UserToken _userToken;

		[SyncVar]
		private float _floatValue;

		[SyncObject]
		private TestSyncObject _testSyncObject = new();

		[SyncObject(SyncType.Unreliable)]
		private TestSyncObject _testUnreliableObject = new();

		[SyncRpc]
		public void Server_DoSomethiing() { }

		[SyncRpc(SyncType.Unreliable)]
		public void Server_SendMessage(NetString message) { }

		[SyncRpc]
		public void Server_Response(NetString message) { }
	}

	[SyncObjectDefinition]
	public partial class TestSyncObject
	{
		[SyncVar]
		private NetTransform _transform;

		[SyncVar]
		private int _abc = 0;

		[SyncObject]
		private TestInnerObject _innerObject = new();

		[SyncRpc]
		public void Server_Some(int value1, float value2) { }
	}

	[SyncObjectDefinition]
	public partial class TestInnerObject
	{
		[SyncVar]
		private UserId _userId;

		[SyncVar]
		private NetStringShort _name;

		[SyncRpc]
		public void Server_Rename(NetStringShort newName) { }
	}
}

#pragma warning restore IDE0051