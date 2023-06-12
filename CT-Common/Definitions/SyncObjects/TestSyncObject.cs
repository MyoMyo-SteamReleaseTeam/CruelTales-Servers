#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.Synchronizations;

namespace CT.Common.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value8
	{
		[SyncVar] public NetString v0;
		[SyncVar] public NetStringShort v1;
		[SyncVar] public byte v2;
		[SyncRpc(SyncType.ReliableTarget)] public void f3(int a) { }
		[SyncVar] public sbyte v4;
		[SyncVar] public ushort v5;
		[SyncVar] public short v6;
		[SyncVar] public int v7;

		[SyncVar(SyncType.Unreliable)] public uint uv0;
		[SyncVar(SyncType.Unreliable)] public long uv1;
		[SyncVar(SyncType.Unreliable)] public ulong uv2;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf3(int a) { }
		[SyncVar(SyncType.Unreliable)] public float uv4;
		[SyncVar(SyncType.Unreliable)] public double uv5;
		[SyncVar(SyncType.Unreliable)] public UserId uv6;
		[SyncVar(SyncType.Unreliable)] public int uv7;
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value16
	{
		[SyncVar] public int v0;
		[SyncVar] public int v1;
		[SyncVar] public int v2;
		[SyncRpc(SyncType.ReliableTarget)] public void f3(int a) { }
		[SyncVar] public NetString v4;
		[SyncVar] public NetStringShort v5;
		[SyncVar] public byte v6;
		[SyncVar] public int v7;
		[SyncVar] public ushort v8;
		[SyncVar] public int v9;
		[SyncVar] public byte v10;
		[SyncVar] public int v12;
		[SyncVar] public short v13;
		[SyncRpc] public void f14(int a) { }
		[SyncVar] public int v15;

		[SyncVar(SyncType.Unreliable)] public int uv0;
		[SyncVar(SyncType.Unreliable)] public int uv1;
		[SyncVar(SyncType.Unreliable)] public ulong uv2;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf3(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv4;
		[SyncVar(SyncType.Unreliable)] public int uv5;
		[SyncVar(SyncType.Unreliable)] public int uv6;
		[SyncVar(SyncType.Unreliable)] public int uv7;
		[SyncVar(SyncType.Unreliable)] public ushort uv8;
		[SyncVar(SyncType.Unreliable)] public int uv9;
		[SyncVar(SyncType.Unreliable)] public float uv10;
		[SyncVar(SyncType.Unreliable)] public int uv12;
		[SyncVar(SyncType.Unreliable)] public int uv13;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf14(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv15;
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value32
	{
		[SyncVar] public int v0;
		[SyncVar] public int v1;
		[SyncVar] public int v2;
		[SyncRpc(SyncType.ReliableTarget)] public void f3(int a) { }
		[SyncVar] public int v4;
		[SyncVar] public int v5;
		[SyncVar] public int v6;
		[SyncVar] public int v7;
		[SyncVar] public int v8;
		[SyncVar] public int v9;
		[SyncVar] public int v10;
		[SyncVar] public int v12;
		[SyncVar] public int v13;
		[SyncRpc(SyncType.ReliableTarget)] public void f14(int a) { }
		[SyncVar] public int v15;
		[SyncVar] public int v16;
		[SyncRpc] public void f17(int a) { }
		[SyncVar] public int v18;
		[SyncVar] public int v19;
		[SyncVar] public int v20;
		[SyncVar] public int v21;
		[SyncRpc(SyncType.ReliableTarget)] public void f22(int a) { }
		[SyncVar] public int v23;
		[SyncRpc] public void f24(int a) { }
		[SyncVar] public int v25;
		[SyncVar] public int v26;
		[SyncVar] public int v27;
		[SyncRpc(SyncType.ReliableTarget)] public void f28(int a) { }
		[SyncVar] public int v29;
		[SyncVar] public int v30;
		[SyncVar] public int v31;
		[SyncVar] public int v32;

		[SyncVar(SyncType.Unreliable)] public int uv0;
		[SyncVar(SyncType.Unreliable)] public int uv1;
		[SyncVar(SyncType.Unreliable)] public int uv2;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf3(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv4;
		[SyncVar(SyncType.Unreliable)] public int uv5;
		[SyncVar(SyncType.Unreliable)] public int uv6;
		[SyncVar(SyncType.Unreliable)] public int uv7;
		[SyncVar(SyncType.Unreliable)] public int uv8;
		[SyncVar(SyncType.Unreliable)] public int uv9;
		[SyncVar(SyncType.Unreliable)] public int uv10;
		[SyncVar(SyncType.Unreliable)] public int uv12;
		[SyncVar(SyncType.Unreliable)] public int uv13;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf14(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv15;
		[SyncVar(SyncType.Unreliable)] public int uv16;
		[SyncRpc(SyncType.Unreliable)] public void uf17(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv18;
		[SyncVar(SyncType.Unreliable)] public int uv19;
		[SyncVar(SyncType.Unreliable)] public int uv20;
		[SyncVar(SyncType.Unreliable)] public int uv21;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf22(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv23;
		[SyncRpc(SyncType.Unreliable)] public void uf24(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv25;
		[SyncVar(SyncType.Unreliable)] public int uv26;
		[SyncVar(SyncType.Unreliable)] public int uv27;
		[SyncRpc(SyncType.UnreliableTarget)] public void uf28(int a) { }
		[SyncVar(SyncType.Unreliable)] public int uv29;
		[SyncVar(SyncType.Unreliable)] public int uv30;
		[SyncVar(SyncType.Unreliable)] public int uv31;
		[SyncVar(SyncType.Unreliable)] public int uv32;
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value32NoTarget
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
		[SyncRpc] public void Server_EnumTypeArg_1(DisconnectReasonType v0, AckJoinMatch v1) { }
		[SyncRpc] public void Server_EnumTypeArg_2(AckJoinMatch v0, DisconnectReasonType v1) { }
		[SyncRpc] public void Server_EnumTypeArg_3(AckJoinMatch v0, DisconnectReasonType v1, AckJoinMatch v2) { }
		[SyncRpc] public void Server_CompositeArg_2_1(int v0, DisconnectReasonType v1) { }
		[SyncRpc] public void Server_CompositeArg_2_2(NetVec2 v0, DisconnectReasonType v1) { }
		[SyncRpc] public void Server_CompositeArg_3(NetVec2 v0, DisconnectReasonType v1, float v2) { }
	}
}
#pragma warning restore IDE0051