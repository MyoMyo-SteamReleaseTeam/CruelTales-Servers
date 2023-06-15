#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Synchronizations;

namespace CT.Common.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_FunctionDirection
	{
		[SyncRpc(dir: SyncDirection.FromMaster)]
		public void Server_FromServerVoid() { }
		
		[SyncRpc(dir: SyncDirection.FromMaster)]
		public void Server_FromServerArg(int a, int b) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_FromClientVoid() { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_FromServerArg(int a, int b) { }
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value8
	{
		[SyncVar] public NetString v0;
		[SyncVar] public NetStringShort v1;
		[SyncVar] public byte v2;
		[SyncRpc] public void f3(int a) { }
		[SyncVar] public TestEnumType v4;
		[SyncVar] private ushort v5;
		[SyncVar] private short v6;
		[SyncVar] private int v7;

		[SyncVar(SyncType.Unreliable)] public uint uv0;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf1(int a, byte b) { }
		[SyncVar(SyncType.Unreliable)] public ulong uv2;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf3(int a, double b) { }
		[SyncVar(SyncType.Unreliable)] public float uv4;
		[SyncRpc(SyncType.UnreliableTarget)] private void uf5() { }
		[SyncVar(SyncType.Unreliable)] public UserId uv6;
		[SyncVar(SyncType.Unreliable)] private int uv7;
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value16
	{
		[SyncVar] public int v0;
		[SyncVar] public int v1;
		[SyncVar] public int v2;
		[SyncRpc] public void f3(int a) { }
		[SyncVar] public NetString v4;
		[SyncVar] public NetStringShort v5;
		[SyncVar] public byte v6;
		[SyncVar] public int v7;
		[SyncVar] public ushort v8;
		[SyncRpc] private void f9() { }
		[SyncVar] private byte v10;
		[SyncVar] private int v12;
		[SyncVar] private short v13;
		[SyncRpc] private void f14(int a, sbyte b) { }
		[SyncVar] private int v15;

		[SyncVar(SyncType.Unreliable)] public int uv0;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf1(int a, sbyte b) { }
		[SyncVar(SyncType.Unreliable)] public ulong uv2;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf3(int a, float b, TestEnumType c) { }
		[SyncVar(SyncType.Unreliable)] public int uv4;
		[SyncVar(SyncType.Unreliable)] public int uv5;
		[SyncVar(SyncType.Unreliable)] public int uv6;
		[SyncVar(SyncType.Unreliable)] private int uv7;
		[SyncVar(SyncType.Unreliable)] private ushort uv8;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf9() { }
		[SyncVar(SyncType.Unreliable)] public float uv10;
		[SyncVar(SyncType.Unreliable)] public int uv12;
		[SyncVar(SyncType.Unreliable)] private int uv13;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf14(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv15;
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value32
	{
		[SyncVar] public int v0;
		[SyncVar] public int v1;
		[SyncVar] public int v2;
		[SyncRpc] public void f3(int a) { }
		[SyncVar] public int v4;
		[SyncVar] public int v5;
		[SyncVar] public int v6;
		[SyncVar] public int v7;
		[SyncVar] private int v8;
		[SyncVar] private int v9;
		[SyncVar] private int v10;
		[SyncVar] private int v12;
		[SyncVar] private int v13;
		[SyncRpc] public void f14() { }
		[SyncVar] public int v15;
		[SyncVar] public int v16;
		[SyncRpc] public void f17(int a) { }
		[SyncVar] public int v18;
		[SyncVar] public int v19;
		[SyncVar] public int v20;
		[SyncVar] public int v21;
		[SyncRpc] public void f22() { }
		[SyncVar] public int v23;
		[SyncRpc] public void f24(int a) { }
		[SyncVar] public int v25;
		[SyncVar] public int v26;
		[SyncVar] public int v27;
		[SyncRpc] private void f28(int a) { }
		[SyncVar] private int v29;
		[SyncVar] private int v30;
		[SyncVar] private int v31;
		[SyncVar] private int v32;

		[SyncVar(SyncType.Unreliable)] public int uv0;
		[SyncVar(SyncType.Unreliable)] public int uv1;
		[SyncVar(SyncType.Unreliable)] public int uv2;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf3(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv4;
		[SyncVar(SyncType.Unreliable)] public int uv5;
		[SyncVar(SyncType.Unreliable)] public int uv6;
		[SyncVar(SyncType.Unreliable)] public int uv7;
		[SyncVar(SyncType.Unreliable)] public int uv8;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf9() { }
		[SyncVar(SyncType.Unreliable)] public int uv10;
		[SyncRpc(SyncType.Unreliable)] public void uf12(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv13;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf14(int a, float b) { }
		[SyncVar(SyncType.Unreliable)] public int uv15;
		[SyncVar(SyncType.Unreliable)] public int uv16;
		[SyncRpc(SyncType.Unreliable)] private void uf17(int a) { }
		[SyncVar(SyncType.Unreliable)] private int uv18;
		[SyncVar(SyncType.Unreliable)] private int uv19;
		[SyncVar(SyncType.Unreliable)] private int uv20;
		[SyncVar(SyncType.Unreliable)] private int uv21;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf22(byte a, int b, uint c) { }
		[SyncVar(SyncType.Unreliable)] public int uv23;
		[SyncRpc(SyncType.Unreliable)] public void uf24() { }
		[SyncVar(SyncType.Unreliable)] public int uv25;
		[SyncVar(SyncType.Unreliable)] public int uv26;
		[SyncVar(SyncType.Unreliable)] public int uv27;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf28(int a) { }
		[SyncVar(SyncType.Unreliable)] private int uv29;
		[SyncVar(SyncType.Unreliable)] private int uv30;
		[SyncVar(SyncType.Unreliable)] private int uv31;
		[SyncVar(SyncType.Unreliable)] private int uv32;
	}

	[SyncObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_InnerObject
	{
		[SyncVar]
		private int _testInt;

		[SyncRpc]
		public void Server_Rename(NetStringShort newName) { }
	}

	[SyncObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_FuntionObject
	{
		[SyncRpc] public void Server_VoidArg() { }
		[SyncRpc] public void Server_PrimitiveArg_1(float v0) { }
		[SyncRpc] public void Server_PrimitiveArg_2(float v0, int v1) { }
		[SyncRpc] public void Server_PrimitiveArg_3(float v0, int v1, double v2) { }
		[SyncRpc] public void Server_ValueTypeArg_1(NetVec2 v0) { }
		[SyncRpc] public void Server_ValueTypeArg_2(NetVec2 v0, NetString v1) { }
		[SyncRpc] public void Server_ValueTypeArg_3(UserId v0, NetVec2 v1, NetString v2) { }
		[SyncRpc] public void Server_EnumTypeArg_1(TestEnumType v0, AckJoinMatch v1) { }
		[SyncRpc] public void Server_EnumTypeArg_2(AckJoinMatch v0, TestEnumType v1) { }
		[SyncRpc] public void Server_EnumTypeArg_3(AckJoinMatch v0, TestEnumType v1, AckJoinMatch v2) { }
		[SyncRpc] public void Server_CompositeArg_2_1(int v0, TestEnumType v1) { }
		[SyncRpc] public void Server_CompositeArg_2_2(NetVec2 v0, TestEnumType v1) { }
		[SyncRpc] public void Server_CompositeArg_3(NetVec2 v0, TestEnumType v1, float v2) { }
	}
}
#pragma warning restore IDE0051