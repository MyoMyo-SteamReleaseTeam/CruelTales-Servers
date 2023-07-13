﻿using CT.Common.DataType;
using CT.Common.Serialization;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.SynchronizeTest.SyncObjects
{
	[TestClass]
	public class Test_SyncObjects
	{
		[TestMethod]
		public void TestValue8NonTarget()
		{
			ByteBuffer buffer = new ByteBuffer(1024 * 16);

			NetworkPlayer p1 = new(new UserId(1));
			NetworkPlayer p2 = new(new UserId(2));

			CTS.Instance.SyncObjects.ZTest_Value8NonTarget master = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value8NonTarget remote = new();

			Assert.IsFalse(master.IsDirtyReliable);
			master.f0("NetString0", "NetStringShort0", TestEnumType.A, 10);
			Assert.IsTrue(master.IsDirtyReliable);
			master.f0("NetString1", "NetStringShort1", TestEnumType.B, 10);
			master.f1();
			master.f1();
			master.f1();
			master.V0 =	"V0";
			master.V1 =	"V1";
			master.V2 = TestEnumType.C;
			master.V3 = 99;

			Sync(buffer, p1, master, remote);

			Assert.AreEqual(master.V0.Value, remote.V0.Value);
			Assert.AreEqual(master.V1.Value, remote.V1.Value);
			Assert.AreEqual(master.V2, remote.V2);
			Assert.AreEqual(master.V3, remote.V3);
			Assert.IsTrue("NetString1" == remote.f0v0);
			Assert.IsTrue("NetStringShort1" == remote.f0v1);
			Assert.AreEqual(TestEnumType.B, remote.f0v2);
			Assert.AreEqual(20, remote.f0v3);
			Assert.AreEqual(3, remote.f1Count);
		}

		[TestMethod]
		public void TestValue8Target()
		{
			ByteBuffer buffer = new ByteBuffer(1024 * 16);

			NetworkPlayer p1 = new(new UserId(1));
			NetworkPlayer p2 = new(new UserId(2));

			CTS.Instance.SyncObjects.ZTest_Value8Target master = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value8Target remote1 = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value8Target remote2 = new();

			Assert.IsFalse(master.IsDirtyReliable);
			master.ft0(p1, "NetString0", "NetStringShort0", TestEnumType.A, 10);
			Assert.IsTrue(master.IsDirtyReliable);
			master.ft0(p1, "NetString1", "NetStringShort1", TestEnumType.B, 10);
			master.ft0(p2, "NetString0", "NetStringShort0", TestEnumType.C, 10);
			master.f1();
			master.f1();
			master.f1();
			master.V0 = "V0";
			master.V1 = "V1";
			master.V2 = TestEnumType.C;
			master.V3 = 99;
			Assert.IsFalse(master.IsDirtyUnreliable);
			master.uft2(p1);
			Assert.IsTrue(master.IsDirtyUnreliable);
			master.uft2(p2);
			master.uft2(p2);

			Sync(buffer, p1, master, remote1);
			Assert.AreEqual(master.V0.Value, remote1.V0.Value);
			Assert.AreEqual(master.V1.Value, remote1.V1.Value);
			Assert.AreEqual(master.V2, remote1.V2);
			Assert.AreEqual(master.V3, remote1.V3);
			Assert.IsTrue("NetString1" == remote1.ft0v0);
			Assert.IsTrue("NetStringShort1" == remote1.ft0v1);
			Assert.AreEqual(TestEnumType.B, remote1.ft0v2);
			Assert.AreEqual(20, remote1.ft0v3);
			Assert.AreEqual(3, remote1.f1Count);
			Assert.AreEqual(1, remote1.uft2Count);

			Sync(buffer, p2, master, remote2);
			Assert.AreEqual(master.V0.Value, remote2.V0.Value);
			Assert.AreEqual(master.V1.Value, remote2.V1.Value);
			Assert.AreEqual(master.V2, remote2.V2);
			Assert.AreEqual(master.V3, remote2.V3);
			Assert.IsTrue("NetString0" == remote2.ft0v0);
			Assert.IsTrue("NetStringShort0" == remote2.ft0v1);
			Assert.AreEqual(TestEnumType.C, remote2.ft0v2);
			Assert.AreEqual(10, remote2.ft0v3);
			Assert.AreEqual(3, remote2.f1Count);
			Assert.AreEqual(2, remote2.uft2Count);
		}

		[TestMethod]
		public void TestValue16NonTarget()
		{
			ByteBuffer buffer = new ByteBuffer(1024 * 16);

			NetworkPlayer p1 = new(new UserId(1));
			NetworkPlayer p2 = new(new UserId(2));

			CTS.Instance.SyncObjects.ZTest_Value16NonTarget master = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value16NonTarget remote1 = new();

			Assert.IsFalse(master.IsDirtyReliable);
			master.V0 = 123;
			master.V1 = -123;
			master.V2 = 12345;
			master.V3 = -12345;
			master.V4 = 1234567890;
			master.V5 = -1234567890;
			master.V6 = 1234567890123456789;
			master.V7 = -1234567890123456789;
			master.V8 = -12345.6789f;
			master.V9 = -123456789.123456789;
			master.V10 = "NetString";
			master.V11 = "NetStringShort";
			//master.V12


			Sync(buffer, p1, master, remote1);
		}
		//[TestMethod]
		//public void TestValue8()
		//{
		//	ByteBuffer buffer = new ByteBuffer(1024 * 16);

		//	NetworkPlayer p1 = new(new UserId(1));
		//	NetworkPlayer p2 = new(new UserId(2));

		//	CTS.Instance.SyncObjects.ZTest_Value8 master = new();
		//	CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value8 remote = new();

		//	master.Call_uf5(p1);
		//	Assert.IsTrue(master.IsDirtyUnreliable);

		//	master.SerializeSyncUnreliable(p2, buffer);
		//	Assert.AreEqual(0, buffer.Size);
		//	master.ClearDirtyUnreliable();

		//	master.f3(10);
		//	master.uf1(p1, 20, 30);
		//	master.uf1(p2, 90, 100);
		//	for (int i = 0; i < 13; i++)
		//	{
		//		master.Call_uf5(p1);
		//	}

		//	for (int i = 0; i < 5; i++)
		//	{
		//		master.Call_uf5(p2);
		//	}
		//	master.V0 = "가나다";
		//	master.V2 = 99;

		//	Sync(buffer, p1, master, remote);

		//	Assert.AreEqual(30, remote.TestInt);
		//	Assert.AreEqual(30, remote.TestByte);
		//	Assert.AreEqual(13, remote.TestVoidCallCount);
		//	Assert.AreEqual("가나다", remote.V0.Value);
		//	Assert.AreEqual(99, remote.V2);

		//	remote.TestInt = 0;
		//	remote.TestByte = 0;
		//	remote.TestVoidCallCount = 0;

		//	remote.InitializeRemoteProperties();
		//	Sync(buffer, p2, master, remote);

		//	Assert.AreEqual(100, remote.TestInt);
		//	Assert.AreEqual(100, remote.TestByte);
		//	Assert.AreEqual(5, remote.TestVoidCallCount);
		//	Assert.AreEqual("가나다", remote.V0.Value);
		//	Assert.AreEqual(99, remote.V2);
		//}

		//[TestMethod]
		//public void TestValue16()
		//{
		//	ByteBuffer buffer = new ByteBuffer(1024 * 16);

		//	NetworkPlayer p1 = new(new UserId(1));
		//	NetworkPlayer p2 = new(new UserId(2));

		//	CTS.Instance.SyncObjects.ZTest_Value16 master = new();
		//	CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value16 remote = new();

		//	master.uf1(p1, 10, 10);
		//	Assert.IsTrue(master.IsDirtyUnreliable);
		//	master.SerializeSyncUnreliable(p2, buffer);
		//	Assert.AreEqual(0, buffer.Size);
		//	master.ClearDirtyUnreliable();

		//	for (int i = 0; i < 10; i++)
		//	{
		//		master.f3(10);
		//		master.uf1(p1, 10, 10);
		//		master.uf3(p1, 10, 20, TestEnumType.B);
		//		master.uf9(p1);
		//	}
		//	for (int i = 0; i < 3; i++)
		//	{
		//		master.uf1(p2, 10, 10);
		//		master.uf14(p2, 10);
		//	}

		//	master.V0 = 99;

		//	Sync(buffer, p1, master, remote);

		//	Assert.AreEqual(100, remote.v_f3);
		//	Assert.AreEqual(100, remote.v_uf3int);
		//	Assert.AreEqual(100, remote.v_uf1int);
		//	Assert.AreEqual(100, remote.v_uf1sbyte);
		//	Assert.AreEqual(10, remote.v_uf9Count);

		//	Assert.AreEqual(99, remote.V0);

		//	remote.ResetTestValue();
		//	remote.InitializeRemoteProperties();

		//	Sync(buffer, p2, master, remote);

		//	Assert.AreEqual(100, remote.v_f3);
		//	Assert.AreEqual(30, remote.v_uf1int);
		//	Assert.AreEqual(30, remote.v_uf1sbyte);
		//	Assert.AreEqual(30, remote.v_uf14int);

		//	Assert.AreEqual(99, remote.V0);
		//}

		//[TestMethod]
		//public void TestValue32()
		//{
		//	ByteBuffer buffer = new ByteBuffer(1024 * 16);

		//	NetworkPlayer p1 = new(new UserId(1));
		//	NetworkPlayer p2 = new(new UserId(2));

		//	CTS.Instance.SyncObjects.ZTest_Value32 master = new();
		//	CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value32 remote = new();

		//	master.uf3(p1, 10);
		//	Assert.IsTrue(master.IsDirtyUnreliable);
		//	master.SerializeSyncUnreliable(p2, buffer);
		//	Assert.AreEqual(0, buffer.Size);
		//	master.ClearDirtyUnreliable();

		//	for (int i = 0; i < 4; i++)
		//	{
		//		master.f3(10);
		//		master.f14();
		//		master.f22();
		//		master.CallF28(10);
		//	}

		//	for (int i = 0; i < 3; i++)
		//	{
		//		master.uf3(p1, 10);
		//		master.uf9(p1);
		//	}

		//	for (int i = 0; i < 2; i++)
		//	{
		//		master.uf9(p2);
		//		master.uf22(p2, 10, 10, 10);
		//		master.uf28(p2, 10);
		//	}

		//	master.V0 = 99;
		//	master.V5 = 99;
		//	master.Uv10 = 99;

		//	Sync(buffer, p1, master, remote);

		//	Assert.AreEqual(40, remote.v_f3);
		//	Assert.AreEqual(4, remote.v_f14);
		//	Assert.AreEqual(4, remote.v_f22);
		//	Assert.AreEqual(40, remote.v_f28);

		//	Assert.AreEqual(30, remote.v_uf3);
		//	Assert.AreEqual(3, remote.v_uf9);

		//	Assert.AreEqual(99, remote.V0);
		//	Assert.AreEqual(99, remote.V5);
		//	Assert.AreEqual(99, remote.Uv10);

		//	remote.ResetTestValue();
		//	remote.InitializeRemoteProperties();

		//	Sync(buffer, p2, master, remote);

		//	Assert.AreEqual(40, remote.v_f3);
		//	Assert.AreEqual(4, remote.v_f14);
		//	Assert.AreEqual(4, remote.v_f22);
		//	Assert.AreEqual(40, remote.v_f28);

		//	Assert.AreEqual(20, remote.v_uf22byte);
		//	Assert.AreEqual(20, remote.v_uf22int);
		//	Assert.AreEqual(20U, remote.v_uf22uint);
		//	Assert.AreEqual(20, remote.v_uf28);
		//	Assert.AreEqual(2, remote.v_uf9);

		//	Assert.AreEqual(99, remote.V0);
		//	Assert.AreEqual(99, remote.V5);
		//	Assert.AreEqual(99, remote.Uv10);
		//}

		//[TestMethod]
		//public void TestSyncCollection()
		//{
		//	int testCount = 10;

		//	ByteBuffer buffer = new ByteBuffer(1024 * 16);
		//	NetworkPlayer p1 = new(new UserId(1));

		//	CTS.Instance.SyncObjects.ZTest_SyncCollection master = new();
		//	CTC.Networks.SyncObjects.TestSyncObjects.ZTest_SyncCollection remote = new();

		//	Assert.AreEqual(0, master.UserIdList.Count);
		//	Assert.AreEqual(0, remote.UserIdList.Count);

		//	for (int i = 0; i < testCount; i++)
		//	{
		//		Assert.AreEqual(i, master.UserIdList.Count);
		//		master.UserIdList.Add(new UserId((ulong)(i * 10)));
		//	}

		//	Assert.AreEqual(testCount, master.UserIdList.Count);
		//	Assert.AreEqual(0, remote.UserIdList.Count);

		//	Assert.IsTrue(master.IsDirtyReliable);
		//	master.SerializeSyncReliable(p1, buffer);

		//	Sync(buffer, p1, master, remote);

		//	Assert.AreEqual(testCount, master.UserIdList.Count);
		//	Assert.AreEqual(testCount, remote.UserIdList.Count);

		//	for (int i = 0; i < testCount; i++)
		//	{
		//		ulong value = (ulong)(i * 10);
		//		Assert.AreEqual(value, master.UserIdList[i].Id);
		//		Assert.AreEqual(value, remote.UserIdList[i].Id);
		//	}
		//}

		public void Sync(ByteBuffer buffer,
						 NetworkPlayer player,
						 MasterNetworkObject master,
						 CTC.Networks.SyncObjects.TestSyncObjects.RemoteNetworkObject remote)
		{
			buffer.Reset();
			master.SerializeSyncReliable(player, buffer);
			remote.TryDeserializeSyncReliable(buffer);

			buffer.Reset();
			master.SerializeSyncUnreliable(player, buffer);
			remote.TryDeserializeSyncUnreliable(buffer);
		}

		public void Clear(MasterNetworkObject master)
		{
			master.ClearDirtyReliable();
			master.ClearDirtyUnreliable();
		}
	}
}
