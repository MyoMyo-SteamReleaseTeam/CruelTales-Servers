#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Serialization.Type;
using CT.Common.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.SyncDefinitions
{
	[SyncNetworkObjectDefinition]
	public partial class TestValue64
	{
		[SyncVar] public int v0;
		[SyncVar] public int v1;
		[SyncVar] public int v2;
		[SyncRpc] public void f3(int a) { }
		[SyncVar] public int v4;
		[SyncVar] public int v5;
		[SyncVar] public int v6;
		[SyncVar] public int v7;
		[SyncVar] public int v8;
		[SyncVar] public int v9;
		[SyncVar] public int v10;
		[SyncVar] public int v12;
		[SyncVar] public int v13;
		[SyncRpc] public void f14(int a) { }
		[SyncVar] public int v15;
		[SyncVar] public int v16;
		[SyncRpc] public void f17(int a) { }
		[SyncVar] public int v18;
		[SyncVar] public int v19;
		[SyncVar] public int v20;
		[SyncVar] public int v21;
		[SyncRpc] public void f22(int a) { }
		[SyncVar] public int v23;
		[SyncRpc] public void f24(int a) { }
		[SyncVar] public int v25;
		[SyncVar] public int v26;
		[SyncVar] public int v27;
		[SyncRpc] public void f28(int a) { }
		[SyncVar] public int v29;
		[SyncVar] public int v30;
		[SyncVar] public int v31;

		[SyncVar] public int v32;
		[SyncVar] public int v33;
		[SyncRpc] public void f34(int a) { }
		[SyncVar] public int v35;
		[SyncVar] public int v36;
		[SyncVar] public int v37;
		[SyncVar] public int v38;
		[SyncVar] public int v39;
		[SyncVar] public int v40;
		[SyncRpc] public void f41(int a) { }
		[SyncRpc] public void f42(int a) { }
		[SyncVar] public int v43;
		[SyncVar] public int v44;
		[SyncVar] public int v45;
		[SyncVar] public int v46;
		[SyncVar] public int v47;
		[SyncVar] public int v48;
		[SyncRpc] public void f49(int a) { }
		[SyncVar] public int v50;
		[SyncVar] public int v51;
		[SyncVar] public int v52;
		[SyncVar] public int v53;
		[SyncRpc] public void f54(int a) { }
		[SyncVar] public int v55;
		[SyncVar] public int v56;
		[SyncVar] public int v57;
		[SyncVar] public int v58;
		[SyncVar] public int v59;
		[SyncVar] public int v60;
		[SyncRpc] public void f61(int a) { }
		[SyncVar] public int v62;
		[SyncVar] public int v63;

		[SyncVar(SyncType.Unreliable)] public int uv0;
		[SyncVar(SyncType.Unreliable)] public int uv1;
		[SyncVar(SyncType.Unreliable)] public int uv2;
		[SyncRpc(SyncType.Unreliable)] public void uf3(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv4;
		[SyncVar(SyncType.Unreliable)] public int uv5;
		[SyncVar(SyncType.Unreliable)] public int uv6;
		[SyncVar(SyncType.Unreliable)] public int uv7;
		[SyncVar(SyncType.Unreliable)] public int uv8;
		[SyncVar(SyncType.Unreliable)] public int uv9;
		[SyncVar(SyncType.Unreliable)] public int uv10;
		[SyncVar(SyncType.Unreliable)] public int uv12;
		[SyncVar(SyncType.Unreliable)] public int uv13;
		[SyncRpc(SyncType.Unreliable)] public void uf14(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv15;
		[SyncVar(SyncType.Unreliable)] public int uv16;
		[SyncRpc(SyncType.Unreliable)] public void uf17(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv18;
		[SyncVar(SyncType.Unreliable)] public int uv19;
		[SyncVar(SyncType.Unreliable)] public int uv20;
		[SyncVar(SyncType.Unreliable)] public int uv21;
		[SyncRpc(SyncType.Unreliable)] public void uf22(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv23;
		[SyncRpc(SyncType.Unreliable)] public void uf24(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv25;
		[SyncVar(SyncType.Unreliable)] public int uv26;
		[SyncVar(SyncType.Unreliable)] public int uv27;
		[SyncRpc(SyncType.Unreliable)] public void uf28(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv29;
		[SyncVar(SyncType.Unreliable)] public int uv30;
		[SyncVar(SyncType.Unreliable)] public int uv31;

		[SyncVar(SyncType.Unreliable)] public int uv32;
		[SyncVar(SyncType.Unreliable)] public int uv33;
		[SyncRpc(SyncType.Unreliable)] public void uf34(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv35;
		[SyncVar(SyncType.Unreliable)] public int uv36;
		[SyncVar(SyncType.Unreliable)] public int uv37;
		[SyncVar(SyncType.Unreliable)] public int uv38;
		[SyncVar(SyncType.Unreliable)] public int uv39;
		[SyncVar(SyncType.Unreliable)] public int uv40;
		[SyncRpc(SyncType.Unreliable)] public void uf41(int a) { }
		[SyncRpc(SyncType.Unreliable)] public void uf42(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv43;
		[SyncVar(SyncType.Unreliable)] public int uv44;
		[SyncVar(SyncType.Unreliable)] public int uv45;
		[SyncVar(SyncType.Unreliable)] public int uv46;
		[SyncVar(SyncType.Unreliable)] public int uv47;
		[SyncVar(SyncType.Unreliable)] public int uv48;
		[SyncRpc(SyncType.Unreliable)] public void uf49(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv50;
		[SyncVar(SyncType.Unreliable)] public int uv51;
		[SyncVar(SyncType.Unreliable)] public int uv52;
		[SyncVar(SyncType.Unreliable)] public int uv53;
		[SyncRpc(SyncType.Unreliable)] public void uf54(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv55;
		[SyncVar(SyncType.Unreliable)] public int uv56;
		[SyncVar(SyncType.Unreliable)] public int uv57;
		[SyncVar(SyncType.Unreliable)] public int uv58;
		[SyncVar(SyncType.Unreliable)] public int uv59;
		[SyncVar(SyncType.Unreliable)] public int uv60;
		[SyncRpc(SyncType.Unreliable)] public void uf61(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv62;
		[SyncVar(SyncType.Unreliable)] public int uv63;
	}

	[SyncNetworkObjectDefinition]
	public partial class TestValue16
	{
		[SyncVar] public int v0;
		[SyncVar] public int v1;
		[SyncVar] public int v2;
		[SyncRpc] public void f3(int a) { }
		[SyncVar] public int v4;
		[SyncVar] public int v5;
		[SyncVar] public int v6;
		[SyncVar] public int v7;
		[SyncVar] public int v8;
		[SyncVar] public int v9;
		[SyncVar] public int v10;
		[SyncVar] public int v12;
		[SyncVar] public int v13;
		[SyncRpc] public void f14(int a) { }
		[SyncVar] public int v15;

		[SyncVar(SyncType.Unreliable)] public int uv0;
		[SyncVar(SyncType.Unreliable)] public int uv1;
		[SyncVar(SyncType.Unreliable)] public int uv2;
		[SyncRpc(SyncType.Unreliable)] public void uf3(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv4;
		[SyncVar(SyncType.Unreliable)] public int uv5;
		[SyncVar(SyncType.Unreliable)] public int uv6;
		[SyncVar(SyncType.Unreliable)] public int uv7;
		[SyncVar(SyncType.Unreliable)] public int uv8;
		[SyncVar(SyncType.Unreliable)] public int uv9;
		[SyncVar(SyncType.Unreliable)] public int uv10;
		[SyncVar(SyncType.Unreliable)] public int uv12;
		[SyncVar(SyncType.Unreliable)] public int uv13;
		[SyncRpc(SyncType.Unreliable)] public void uf14(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv15;
	}

	[SyncNetworkObjectDefinition]
	public partial class TestValue8
	{
		[SyncVar] public int v0;
		[SyncVar] public int v1;
		[SyncVar] public int v2;
		[SyncRpc] public void f3(int a) { }
		[SyncVar] public int v4;
		[SyncVar] public int v5;
		[SyncVar] public int v6;
		[SyncVar] public int v7;

		[SyncVar(SyncType.Unreliable)] public int uv0;
		[SyncVar(SyncType.Unreliable)] public int uv1;
		[SyncVar(SyncType.Unreliable)] public int uv2;
		[SyncRpc(SyncType.Unreliable)] public void uf3(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv4;
		[SyncVar(SyncType.Unreliable)] public int uv5;
		[SyncVar(SyncType.Unreliable)] public int uv6;
		[SyncVar(SyncType.Unreliable)] public int uv7;
	}

	[SyncNetworkObjectDefinition]
	public partial class TestNetworkObject
	{
		[SyncVar]
		private UserToken _userToken;

		[SyncVar(SyncType.Unreliable)]
		private float _floatValue;

		[SyncRpc]
		public void Server_DoSomethiing() { }

		[SyncRpc(SyncType.Unreliable)]
		public void Server_SendMessage(NetString message) { }

		[SyncVar(dir: SyncDirection.FromRemote)]
		private NetTransform _remote_netTransform;

		[SyncVar(SyncType.Unreliable, SyncDirection.FromRemote)]
		public int _remote_Value;

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_DoSomethiing() { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_SendMessage(NetString message) { }
	}

	[SyncNetworkObjectDefinition]
	public partial class TestTypeSync
	{
		[SyncVar]
		private UserToken _valueTypeUserToken;

		[SyncVar]
		private float _primitiveType;

		[SyncVar]
		private NetEntityType _enumType;

		[SyncVar]
		private NetTransform _valueTypeTransform;

		[SyncVar(SyncType.RelibaleOrUnreliable)]
		public NetString _stringValue;

		[SyncObject(SyncType.RelibaleOrUnreliable)]
		private TestInnerObject _syncObjectBothSide = new();

		[SyncObject]
		private TestInnerObject _syncObjectReliable = new();

		[SyncRpc]
		public void Server_Reliable() { }

		[SyncRpc(SyncType.Unreliable)]
		public void Server_Unreliable(NetString message) { }
	}

	[SyncObjectDefinition]
	public partial class TestInnerObject
	{
		[SyncVar]
		private int _testInt;

		[SyncRpc]
		public void Server_Rename(NetStringShort newName) { }
	}

	[SyncObjectDefinition]
	public partial class TestFuntionObject
	{
		[SyncRpc] public void Server_VoidArg() { }
		[SyncRpc] public void Server_PrimitiveArg_1(float v0) { }
		[SyncRpc] public void Server_PrimitiveArg_2(float v0, int v1) { }
		[SyncRpc] public void Server_PrimitiveArg_3(float v0, int v1, double v2) { }
		[SyncRpc] public void Server_ValueTypeArg_1(NetVec2 v0) { }
		[SyncRpc] public void Server_ValueTypeArg_2(NetVec2 v0, NetString v1) { }
		[SyncRpc] public void Server_ValueTypeArg_3(UserId v0, NetVec2 v1, NetString v2) { }
		[SyncRpc] public void Server_EnumTypeArg_1(DisconnectReasonType v0, AckJoinMatch v1) { }
		[SyncRpc] public void Server_EnumTypeArg_2(AckJoinMatch v0, DisconnectReasonType v1) { }
		[SyncRpc] public void Server_EnumTypeArg_3(NetworkObjectType v0, DisconnectReasonType v1, AckJoinMatch v2) { }
		[SyncRpc] public void Server_CompositeArg_2_1(int v0, DisconnectReasonType v1) { }
		[SyncRpc] public void Server_CompositeArg_2_2(NetVec2 v0, DisconnectReasonType v1) { }
		[SyncRpc] public void Server_CompositeArg_3(NetVec2 v0, DisconnectReasonType v1, float v2) { }
	}
}
#pragma warning restore IDE0051