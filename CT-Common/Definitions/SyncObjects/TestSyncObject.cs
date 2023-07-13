﻿#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CT.Common.Synchronizations;

namespace CT.Common.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Parent
	{
		[SyncVar]
		public int P1;

		[SyncVar]
		public float P2;

		[SyncRpc(dir: SyncDirection.FromMaster)]
		public void SP1() { }

		[SyncRpc(dir: SyncDirection.FromMaster)]
		public void SP2(int a, int b) { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void CP1() { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void CP2(int a, int b) { }
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Child : ZTest_Parent
	{
		[SyncVar]
		public int C1;

		[SyncRpc(dir: SyncDirection.FromMaster)]
		public void SC1() { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void CC1() { }
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_SyncCollection
	{
		[SyncObject]
		public SyncList<UserId> UserIdList = new();

		[SyncObject(SyncType.ReliableOrUnreliable)]
		public ZTest_InnerObjectTarget SyncObj = new();
	}

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
	public partial class ZTest_Value8Target
	{
		[SyncVar] public NetString v0;
		[SyncVar] public NetStringShort v1;
		[SyncVar] public TestEnumType v2;
		[SyncVar] public int v3;
		[SyncObject]
		public SyncList<UserId> v4 = new();
		[SyncObject(SyncType.ReliableOrUnreliable)]
		public ZTest_InnerObjectTarget v5 = new();
		[SyncRpc(SyncType.ReliableTarget)]
		public void ft0(NetString v0, NetStringShort v1, TestEnumType v2, int v3) { }
		[SyncRpc(SyncType.Reliable)]
		public void f1() { }

		[SyncVar(SyncType.Unreliable)] public int uv0;
		[SyncRpc(SyncType.Unreliable)] public void uf0(int a, byte b) { }
		[SyncVar(SyncType.Unreliable)] public int uv1;
		[SyncRpc(SyncType.Unreliable)] public void uf1(int a, double b) { }
		[SyncVar(SyncType.Unreliable)] public int uv2;
		[SyncRpc(SyncType.UnreliableTarget)] public void uft2() { }
		[SyncVar(SyncType.Unreliable)] public int uv3;
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value8NonTarget
	{
		[SyncVar] public NetString v0;
		[SyncVar] public NetStringShort v1;
		[SyncVar] public TestEnumType v2;
		[SyncVar] public int v3;
		[SyncObject]
		public SyncList<UserId> v4 = new();
		[SyncObject(SyncType.ReliableOrUnreliable)]
		public ZTest_InnerObject v5 = new();
		[SyncRpc(SyncType.Reliable)]
		public void f0(NetString v0, NetStringShort v1, TestEnumType v2, int v3) { }
		[SyncRpc(SyncType.Reliable)]
		public void f1() { }

		[SyncVar(SyncType.Unreliable)] public int uv0;
		[SyncRpc(SyncType.Unreliable)] public void uf0(int a, byte b) { }
		[SyncVar(SyncType.Unreliable)] public int uv1;
		[SyncRpc(SyncType.Unreliable)] public void uf1(int a, double b) { }
		[SyncVar(SyncType.Unreliable)] public int uv2;
		[SyncRpc(SyncType.Unreliable)] public void uf2() { }
		[SyncVar(SyncType.Unreliable)] public int uv3;
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value16NonTarget
	{
		[SyncVar] public byte v0;
		[SyncVar] public sbyte v1;
		[SyncVar] public ushort v2;
		[SyncVar] public short v3;
		[SyncVar] public uint v4;
		[SyncVar] public int v5;
		[SyncVar] public ulong v6;
		[SyncRpc] public void f0() { }
		[SyncVar] public long v7;
		[SyncVar] public float v8;
		[SyncVar] public double v9;
		[SyncVar] public NetString v10;
		[SyncVar] public NetStringShort v11;
		[SyncObject] public SyncList<NetString> v12 = new();
		[SyncObject(SyncType.ReliableOrUnreliable)]
		public ZTest_InnerObject v14 = new();

		[SyncVar(SyncType.Unreliable)] public byte uv0;
		[SyncVar(SyncType.Unreliable)] public sbyte uv1;
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value16Target
	{
		[SyncVar] public byte v0;
		[SyncVar] public sbyte v1;
		[SyncVar] public ushort v2;
		[SyncVar] public short v3;
		[SyncVar] public uint v4;
		[SyncVar] public int v5;
		[SyncVar] public ulong v6;
		[SyncRpc] public void f0() { }
		[SyncVar] public long v7;
		[SyncVar] public float v8;
		[SyncVar] public double v9;
		[SyncVar] public NetString v10;
		[SyncVar] public NetStringShort v11;
		[SyncObject] public SyncList<NetString> v12 = new();
		[SyncObject(SyncType.ReliableOrUnreliable)]
		public ZTest_InnerObjectTarget v13 = new();
		[SyncObject(SyncType.ReliableOrUnreliable)]
		public ZTest_InnerObject v14 = new();

		[SyncVar(SyncType.Unreliable)] public byte uv0;
		[SyncVar(SyncType.Unreliable)] public sbyte uv1;
		[SyncRpc(SyncType.UnreliableTarget)] public void uft1() { }
		[SyncRpc(SyncType.UnreliableTarget)] public void uft2(int a) { }
		[SyncRpc(SyncType.UnreliableTarget)] public void uft3(NetString a, int b) { }
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value32Target
	{
		[SyncVar] public int v0;
		[SyncVar] public int v1;
		[SyncVar] public int v2;
		[SyncVar] public int v3;
		[SyncVar] public int v4;
		[SyncVar] public int v5;
		[SyncVar] public int v6;
		[SyncObject(SyncType.Reliable)]
		public SyncList<UserId> v7 = new();
		[SyncVar] private int v8;
		[SyncVar] private int v9;
		[SyncVar] private int v10;
		[SyncVar] private int v11;
		[SyncVar] private int v12;
		[SyncVar] private int v13;
		[SyncVar] public int v14;
		[SyncRpc(SyncType.ReliableTarget)]
		public void ft15() { }
		[SyncVar] public int v16;
		[SyncVar] public int v17;
		[SyncVar] public int v18;
		[SyncObject(SyncType.ReliableOrUnreliable)]
		public ZTest_InnerObjectTarget v19 = new();
		[SyncObject(SyncType.ReliableOrUnreliable)]
		public ZTest_InnerObject v20 = new();
		[SyncVar] public int v21;
		[SyncRpc] public void f22() { }
		[SyncObject(SyncType.Reliable)]
		public SyncList<UserId> v23 = new();
		[SyncRpc] public void f24(int a) { }
		[SyncVar] public int v25;
		[SyncVar] public int v26;
		[SyncVar] public int v27;
		[SyncRpc] public void f28(int a) { }
		[SyncVar] private int v29;
		[SyncVar] private int v30;
		[SyncVar] private int v31;
	}

	[SyncNetworkObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_Value32NonTarget
	{
		[SyncVar] public int v0;
		[SyncVar] public int v1;
		[SyncVar] public int v2;
		[SyncVar] public int v3;
		[SyncVar] public int v4;
		[SyncVar] public int v5;
		[SyncVar] public int v6;
		[SyncObject(SyncType.Reliable)]
		public SyncList<UserId> v7 = new();
		[SyncVar] private int v8;
		[SyncVar] private int v9;
		[SyncVar] private int v10;
		[SyncVar] private int v11;
		[SyncVar] private int v12;
		[SyncVar] private int v13;
		[SyncObject(SyncType.ReliableOrUnreliable)]
		public ZTest_InnerObject v15 = new();
		[SyncRpc] public void ft15() { }
		[SyncVar] public int v16;
		[SyncVar] public int v17;
		[SyncVar] public int v18;
		[SyncVar] public int v20;
		[SyncVar] public int v21;
		[SyncRpc] public void f22() { }
		[SyncObject(SyncType.Reliable)]
		public SyncList<UserId> v23 = new();
	}

	[SyncObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_InnerObjectTarget
	{
		[SyncVar] public int v0;

		[SyncRpc(SyncType.ReliableTarget)]
		public void f1(NetStringShort a) { }

		[SyncVar(SyncType.Unreliable)] public int uv1;
	}

	[SyncObjectDefinition(IsDebugOnly = true)]
	public partial class ZTest_InnerObject
	{
		[SyncVar] public int v0;

		[SyncRpc(SyncType.Reliable)]
		public void f1(NetStringShort a) { }

		[SyncVar(SyncType.Unreliable)] public int uv1;
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