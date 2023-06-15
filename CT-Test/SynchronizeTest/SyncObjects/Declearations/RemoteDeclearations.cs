using System;
using System.Security.Policy;
using CT.Common.DataType;
using CT.Common.Synchronizations;
using CTS.Instance.Gameplay;
//using CTC.Networks.Synchronizations;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	public enum NetworkObjectType
	{
		ZTest_Value8,
		ZTest_Value16,
		ZTest_Value32,
		ZTest_Value32NoTarget,
	}

	public partial class ZTest_FuntionObject : IRemoteSynchronizable
	{
		public partial void Server_VoidArg()
		{

		}

		public partial void Server_PrimitiveArg_1(float v0)
		{

		}

		public partial void Server_PrimitiveArg_2(float v0, int v1)
		{

		}

		public partial void Server_PrimitiveArg_3(float v0, int v1, double v2)
		{

		}

		public partial void Server_ValueTypeArg_1(NetVec2 v0)
		{

		}

		public partial void Server_ValueTypeArg_2(NetVec2 v0, NetString v1)
		{

		}

		public partial void Server_ValueTypeArg_3(UserId v0, NetVec2 v1, NetString v2)
		{

		}

		public partial void Server_EnumTypeArg_1(TestEnumType v0, AckJoinMatch v1)
		{

		}

		public partial void Server_EnumTypeArg_2(AckJoinMatch v0, TestEnumType v1)
		{

		}

		public partial void Server_EnumTypeArg_3(AckJoinMatch v0, TestEnumType v1, AckJoinMatch v2)
		{

		}

		public partial void Server_CompositeArg_2_1(int v0, TestEnumType v1)
		{

		}

		public partial void Server_CompositeArg_2_2(NetVec2 v0, TestEnumType v1)
		{

		}

		public partial void Server_CompositeArg_3(NetVec2 v0, TestEnumType v1, float v2)
		{

		}

	}

	public partial class ZTest_InnerObject : IRemoteSynchronizable
	{
		public partial void Server_Rename(NetStringShort newName)
		{

		}
	}

	public partial class ZTest_Value8 : RemoteNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();

		public int TestInt = 0;
		public byte TestByte = 0;
		public double TestDouble = 0;
		public int TestVoidCallCount = 0;

		public partial void f3(int a)
		{
			TestInt += a;
		}

		public partial void uf1(int a, byte b)
		{
			TestInt += a;
			TestByte += b;
		}

		public partial void uf3(int a, double b)
		{
			TestInt += a;
			TestDouble += b;
		}

		private partial void uf5()
		{
			TestVoidCallCount++;
		}
	}

	public partial class ZTest_Value16 : RemoteNetworkObject
	{
		public short V13 => _v13;

		public override NetworkObjectType Type => throw new NotImplementedException();

		public int v_f3 = 0;
		public int v_uf1int = 0;
		public int v_uf1sbyte = 0;
		public int v_uf3int = 0;
		public float v_uf3float = 0;
		public int v_uf3enum = 0;
		public int v_uf14int = 0;
		public int v_uf9Count = 0;

		public void ResetTestValue()
		{
			v_f3 = 0;
			v_uf1int = 0;
			v_uf1sbyte = 0;
			v_uf3int = 0;
			v_uf3float = 0;
			v_uf3enum = 0;
			v_uf14int = 0;
		}

		public partial void f3(int a)
		{
			v_f3 += a;
		}

		private partial void f9()
		{

		}

		private partial void f14(int a, sbyte b)
		{
		}

		public partial void uf1(int a, sbyte b)
		{
			v_uf1int += a;
			v_uf1sbyte += b;
		}

		public partial void uf3(int a, float b, TestEnumType c)
		{
			v_uf3int += a;
			v_uf3float += b;
			if (c == TestEnumType.B)
			{
				v_uf3enum++;
			}
		}

		public partial void uf9()
		{
			v_uf9Count++;
		}

		public partial void uf14(int a)
		{
			v_uf14int += a;
		}
	}

	public partial class ZTest_Value32 : RemoteNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();


		public partial void f3(int a)
		{ 

		}

		public partial void f14()
		{

		}

		public partial void f17(int a)
		{

		}

		public partial void f22()
		{

		}

		public partial void f24(int a)
		{

		}

		private partial void f28(int a)
		{

		}

		public partial void uf3(int a)
		{

		}

		public partial void uf9()
		{

		}

		public partial void uf12(int a)
		{

		}

		public partial void uf14(int a, float b)
		{

		}

		private partial void uf17(int a)
		{

		}

		public partial void uf22(byte a, int b, uint c)
		{

		}

		public partial void uf24()
		{

		}

		public partial void uf28(int a)
		{

		}

	}
}