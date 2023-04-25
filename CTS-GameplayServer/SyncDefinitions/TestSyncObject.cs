#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Synchronizations;

namespace CTS.Instance.SyncDefinitions
{
	[DEF_SyncNetworkObjectDefinition]
	public class Test_MovingCube
	{
		[DEF_SyncVar]
		public NetworkIdentity NetworkIdentity;

		[DEF_SyncVar]
		public byte R;

		[DEF_SyncVar]
		public byte G;

		[DEF_SyncVar]
		public byte B;

		[DEF_SyncVar]
		public float _speed;

		[DEF_SyncVar]
		public float _x;

		[DEF_SyncVar]
		public float _y;

		[DEF_SyncVar]
		public float _z;

		[DEF_SyncVar]
		public float _dest;

		[DEF_SyncRpc]
		public void Server_MoveTo(float y) { }
	}
}

//[SyncNetworkObjectDefinition]
//public partial class TestNetworkObject
//{
//	[SyncVar]
//	private UserToken _userToken;

//	[SyncVar(SyncType.Unreliable)]
//	private float _floatValue;

//	[SyncRpc]
//	public void Server_DoSomethiing() { }

//	[SyncRpc(SyncType.Unreliable)]
//	public void Server_SendMessage(NetString message) { }

//	[SyncVar(dir: SyncDirection.FromRemote)]
//	private NetTransform _remote_netTransform;

//	[SyncVar(SyncType.Unreliable, SyncDirection.FromRemote)]
//	public int _remote_Value;

//	[SyncRpc(dir: SyncDirection.FromRemote)]
//	public void Client_DoSomethiing() { }

//	[SyncRpc(dir: SyncDirection.FromRemote)]
//	public void Client_SendMessage(NetString message) { }
//}

//[SyncNetworkObjectDefinition]
//public partial class TestNetworkObject
//{
//	[SyncVar]
//	private UserToken _userToken;

//	[SyncVar]
//	private float _floatValue;

//	[SyncObject]
//	private TestSyncObject _testSyncObject = new();

//	[SyncObject(SyncType.Unreliable)]
//	private TestSyncObject _testUnreliableObject = new();

//	[SyncRpc]
//	public void Server_DoSomethiing() { }

//	[SyncRpc(SyncType.Unreliable)]
//	public void Server_SendMessage(NetString message) { }

//	[SyncRpc]
//	public void Server_Response(NetString message) { }
//}

//[SyncObjectDefinition]
//public partial class TestSyncObject
//{
//	[SyncVar]
//	private NetTransform _transform;

//	[SyncVar]
//	private int _abc = 0;

//	[SyncObject]
//	private TestInnerObject _innerObject = new();

//	[SyncRpc]
//	public void Server_Some(int value1, float value2) { }
//}

//[SyncObjectDefinition]
//public partial class TestInnerObject
//{
//	[SyncVar]
//	private UserId _userId;

//	[SyncVar]
//	private NetStringShort _name;

//	[SyncRpc]
//	public void Server_Rename(NetStringShort newName) { }
//}

#pragma warning restore IDE0051