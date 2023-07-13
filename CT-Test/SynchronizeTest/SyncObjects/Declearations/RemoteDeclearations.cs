﻿using System;
using CT.Common.DataType;
using CT.Common.Synchronizations;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	public enum NetworkObjectType
	{
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
		public partial void f1(NetStringShort a)
		{

		}
	}

	public partial class ZTest_InnerObjectTarget : IRemoteSynchronizable
	{
		public partial void f1(NetStringShort a)
		{

		}
	}

	public partial class ZTest_FunctionDirection : RemoteNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();

		public partial void Server_FromServerVoid()
		{

		}

		public partial void Server_FromServerArg(int a, int b)
		{

		}
	}

	public partial class ZTest_SyncCollection : RemoteNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
	}


	public partial class ZTest_Value8Target : RemoteNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();

		public NetString ft0v0;
		public NetStringShort ft0v1;
		public TestEnumType ft0v2;
		public int ft0v3;
		public int f1Count;
		public int uf0a;
		public byte uf0b;
		public int uf1a;
		public double uf1b;
		public int uft2Count;

		public partial void ft0(NetString v0, NetStringShort v1, TestEnumType v2, int v3)
		{
			ft0v0 = v0;
			ft0v1 = v1;
			ft0v2 = v2;
			ft0v3 += v3;
		}

		public partial void f1()
		{
			f1Count++;
		}

		public partial void uf0(int a, byte b)
		{
			uf0a += a;
			uf0b += b;
		}

		public partial void uf1(int a, double b)
		{
			uf1a += a;
			uf1b += b;
		}

		public partial void uft2()
		{
			uft2Count++;
		}
	}

	public partial class ZTest_Value8NonTarget : RemoteNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();

		public NetString f0v0;
		public NetStringShort f0v1;
		public TestEnumType f0v2;
		public int f0v3;
		public int f1Count;
		public int uf0a;
		public byte uf0b;
		public int uf1a;
		public double uf1b;
		public int uf2Count;

		public partial void f0(NetString v0, NetStringShort v1, TestEnumType v2, int v3)
		{
			f0v0 = v0;
			f0v1 = v1;
			f0v2 = v2;
			f0v3 += v3;
		}

		public partial void f1()
		{
			f1Count++;
		}

		public partial void uf0(int a, byte b)
		{
			uf0a += a;
			uf0b += b;
		}

		public partial void uf1(int a, double b)
		{
			uf1a += a;
			uf1b += b;
		}

		public partial void uf2()
		{
			uf2Count++;
		}
	}

	public partial class ZTest_Value16Target : RemoteNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();

		public int f0Count;
		public int uft1Count;
		public int uft2a;
		public NetString uft3a;
		public int uft3b;

		public partial void f0()
		{
			f0Count++;
		}

		public partial void uft1()
		{
			uft1Count++;
		}

		public partial void uft2(int a)
		{
			uft2a += a;
		}

		public partial void uft3(NetString a, int b)
		{
			uft3a = a;
			uft3b += b;
		}
	}

	public partial class ZTest_Value16NonTarget : RemoteNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();

		public int f0Count;

		public partial void f0()
		{
			f0Count++;
		}
	}

	public partial class ZTest_Value32Target : RemoteNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();

		public int ft15Count;
		public int f22Count;
		public int f24a;
		public int f28a;

		public partial void ft15()
		{
			ft15Count++;
		}

		public partial void f22()
		{
			f22Count++;
		}

		public partial void f24(int a)
		{
			f24a += a;
		}

		public partial void f28(int a)
		{
			f28a += a;
		}
	}

	public partial class ZTest_Value32NonTarget : RemoteNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();

		public int ft15Count;
		public int f22Count;

		public partial void ft15()
		{
			ft15Count++;
		}

		public partial void f22()
		{
			f22Count++;
		}
	}
}