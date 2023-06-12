﻿using CT.Common.DataType;
using CT.Common.Synchronizations;
using CTC.Networks.Synchronizations;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	public partial class ZTest_FuntionObject : IRemoteSynchronizable
	{
		public partial void Server_VoidArg() { }
		public partial void Server_PrimitiveArg_1(float v0) { }
		public partial void Server_PrimitiveArg_2(float v0, int v1) { }
		public partial void Server_PrimitiveArg_3(float v0, int v1, double v2) { }
		public partial void Server_ValueTypeArg_1(NetVec2 v0) { }
		public partial void Server_ValueTypeArg_2(NetVec2 v0, NetString v1) { }
		public partial void Server_ValueTypeArg_3(UserId v0, NetVec2 v1, NetString v2) { }
		public partial void Server_EnumTypeArg_1(DisconnectReasonType v0, AckJoinMatch v1) { }
		public partial void Server_EnumTypeArg_2(AckJoinMatch v0, DisconnectReasonType v1) { }
		public partial void Server_EnumTypeArg_3(AckJoinMatch v0, DisconnectReasonType v1, AckJoinMatch v2) { }
		public partial void Server_CompositeArg_2_1(int v0, DisconnectReasonType v1) { }
		public partial void Server_CompositeArg_2_2(NetVec2 v0, DisconnectReasonType v1) { }
		public partial void Server_CompositeArg_3(NetVec2 v0, DisconnectReasonType v1, float v2) {  }
	}

	public partial class ZTest_InnerObject : IRemoteSynchronizable
	{

	}

	public partial class ZTest_Value8 : RemoteNetworkObject
	{

	}

	public partial class ZTest_Value16 : RemoteNetworkObject
	{
		public partial void f3(int a) {}
		public partial void f14(int a) {}
		public partial void uf3(int a) {}
		public partial void uf14(int a) { }

	}

	public partial class ZTest_Value32 : RemoteNetworkObject
	{

	}
}