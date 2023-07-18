using CT.Common.DataType;
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
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value16NonTarget remote2 = new();

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
			master.V12.Add("string_0");
			master.V12.Add("string_1");
			master.V12.Add("string_2");
			master.V12.Add("string_3");
			master.V12.Add("string_4");
			master.V12.Remove("string_2");
			master.V12.RemoveAt(0);
			master.V13.f1(p1, "YouAreP1");
			master.V13.f1(p2, "YouAreP2");
			master.V13.V0 = 13;
			master.V13.Uv1 = 1313;
			master.V14.f1("Both");
			master.V14.V0 = 14;
			master.V14.Uv1 = 1414;
			master.Uv0 = 123;
			master.Uv1 = -123;

			Sync(buffer, p1, master, remote1);
			Assert.AreEqual(master.V0, remote1.V0);
			Assert.AreEqual(master.V1, remote1.V1);
			Assert.AreEqual(master.V2, remote1.V2);
			Assert.AreEqual(master.V3, remote1.V3);
			Assert.AreEqual(master.V4, remote1.V4);
			Assert.AreEqual(master.V5, remote1.V5);
			Assert.AreEqual(master.V6, remote1.V6);
			Assert.AreEqual(master.V7, remote1.V7);
			Assert.AreEqual(master.V8, remote1.V8);
			Assert.AreEqual(master.V9, remote1.V9);
			Assert.IsTrue(master.V10 == remote1.V10);
			Assert.IsTrue(master.V11 == remote1.V11);
			Assert.AreEqual(3, remote1.V12.Count);
			Assert.IsTrue("string_1" == remote1.V12[0]);
			Assert.IsTrue("string_3" == remote1.V12[1]);
			Assert.IsTrue("string_4" == remote1.V12[2]);
			Assert.IsTrue("YouAreP1" == remote1.V13.f1a);
			Assert.AreEqual(master.V13.V0, remote1.V13.V0);
			Assert.AreEqual(master.V13.Uv1, remote1.V13.Uv1);
			Assert.IsTrue("Both" == remote1.V14.f1a);
			Assert.AreEqual(master.V14.V0, remote1.V14.V0);
			Assert.AreEqual(master.V14.Uv1, remote1.V14.Uv1);
			Assert.AreEqual(master.Uv0, remote1.Uv0);
			Assert.AreEqual(master.Uv1, remote1.Uv1);

			Sync(buffer, p2, master, remote2);
			Assert.AreEqual(master.V0, remote2.V0);
			Assert.AreEqual(master.V1, remote2.V1);
			Assert.AreEqual(master.V2, remote2.V2);
			Assert.AreEqual(master.V3, remote2.V3);
			Assert.AreEqual(master.V4, remote2.V4);
			Assert.AreEqual(master.V5, remote2.V5);
			Assert.AreEqual(master.V6, remote2.V6);
			Assert.AreEqual(master.V7, remote2.V7);
			Assert.AreEqual(master.V8, remote2.V8);
			Assert.AreEqual(master.V9, remote2.V9);
			Assert.IsTrue(master.V10 == remote2.V10);
			Assert.IsTrue(master.V11 == remote2.V11);
			Assert.AreEqual(3, remote2.V12.Count);
			Assert.IsTrue("string_1" == remote2.V12[0]);
			Assert.IsTrue("string_3" == remote2.V12[1]);
			Assert.IsTrue("string_4" == remote2.V12[2]);
			Assert.IsTrue("YouAreP2" == remote2.V13.f1a);
			Assert.AreEqual(master.V13.V0, remote2.V13.V0);
			Assert.AreEqual(master.V13.Uv1, remote2.V13.Uv1);
			Assert.IsTrue("Both" == remote2.V14.f1a);
			Assert.AreEqual(master.V14.V0, remote2.V14.V0);
			Assert.AreEqual(master.V14.Uv1, remote2.V14.Uv1);
			Assert.AreEqual(master.Uv0, remote2.Uv0);
			Assert.AreEqual(master.Uv1, remote2.Uv1);
		}

		[TestMethod]
		public void TestValue16Target()
		{
			ByteBuffer buffer = new ByteBuffer(1024 * 16);

			NetworkPlayer p1 = new(new UserId(1));
			NetworkPlayer p2 = new(new UserId(2));

			CTS.Instance.SyncObjects.ZTest_Value16Target master = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value16Target remote1 = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value16Target remote2 = new();

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
			master.V12.Add("string_0");
			master.V12.Add("string_1");
			master.V12.Add("string_2");
			master.V12.Add("string_3");
			master.V12.Add("string_4");
			master.V12.Remove("string_2");
			master.V12.RemoveAt(0);
			master.V13.f1(p1, "YouAreP1");
			master.V13.f1(p2, "YouAreP2");
			master.V13.V0 = 13;
			master.V13.Uv1 = 1313;
			master.V14.f1("Both");
			master.V14.V0 = 14;
			master.V14.Uv1 = 1414;
			master.Uv0 = 123;
			master.Uv1 = -123;
			master.uft1(p1);
			master.uft1(p2);
			master.uft1(p2);
			master.uft2(p1, 10);
			master.uft2(p1, 10);
			master.uft2(p2, 10);
			master.uft3(p1, "YouAreP1", 11);
			master.uft3(p2, "YouAreP2", 22);

			Sync(buffer, p1, master, remote1);
			Assert.AreEqual(master.V0, remote1.V0);
			Assert.AreEqual(master.V1, remote1.V1);
			Assert.AreEqual(master.V2, remote1.V2);
			Assert.AreEqual(master.V3, remote1.V3);
			Assert.AreEqual(master.V4, remote1.V4);
			Assert.AreEqual(master.V5, remote1.V5);
			Assert.AreEqual(master.V6, remote1.V6);
			Assert.AreEqual(master.V7, remote1.V7);
			Assert.AreEqual(master.V8, remote1.V8);
			Assert.AreEqual(master.V9, remote1.V9);
			Assert.IsTrue(master.V10 == remote1.V10);
			Assert.IsTrue(master.V11 == remote1.V11);
			Assert.AreEqual(3, remote1.V12.Count);
			Assert.IsTrue("string_1" == remote1.V12[0]);
			Assert.IsTrue("string_3" == remote1.V12[1]);
			Assert.IsTrue("string_4" == remote1.V12[2]);
			Assert.IsTrue("YouAreP1" == remote1.V13.f1a);
			Assert.AreEqual(master.V13.V0, remote1.V13.V0);
			Assert.AreEqual(master.V13.Uv1, remote1.V13.Uv1);
			Assert.IsTrue("Both" == remote1.V14.f1a);
			Assert.AreEqual(master.V14.V0, remote1.V14.V0);
			Assert.AreEqual(master.V14.Uv1, remote1.V14.Uv1);
			Assert.AreEqual(master.Uv0, remote1.Uv0);
			Assert.AreEqual(master.Uv1, remote1.Uv1);
			Assert.AreEqual(1, remote1.uft1Count);
			Assert.AreEqual(20, remote1.uft2a);
			Assert.IsTrue("YouAreP1" == remote1.uft3a);
			Assert.AreEqual(11, remote1.uft3b);

			Sync(buffer, p2, master, remote2);
			Assert.AreEqual(master.V0, remote2.V0);
			Assert.AreEqual(master.V1, remote2.V1);
			Assert.AreEqual(master.V2, remote2.V2);
			Assert.AreEqual(master.V3, remote2.V3);
			Assert.AreEqual(master.V4, remote2.V4);
			Assert.AreEqual(master.V5, remote2.V5);
			Assert.AreEqual(master.V6, remote2.V6);
			Assert.AreEqual(master.V7, remote2.V7);
			Assert.AreEqual(master.V8, remote2.V8);
			Assert.AreEqual(master.V9, remote2.V9);
			Assert.IsTrue(master.V10 == remote2.V10);
			Assert.IsTrue(master.V11 == remote2.V11);
			Assert.AreEqual(3, remote2.V12.Count);
			Assert.IsTrue("string_1" == remote2.V12[0]);
			Assert.IsTrue("string_3" == remote2.V12[1]);
			Assert.IsTrue("string_4" == remote2.V12[2]);
			Assert.IsTrue("YouAreP2" == remote2.V13.f1a);
			Assert.AreEqual(master.V13.V0, remote2.V13.V0);
			Assert.AreEqual(master.V13.Uv1, remote2.V13.Uv1);
			Assert.IsTrue("Both" == remote2.V14.f1a);
			Assert.AreEqual(master.V14.V0, remote2.V14.V0);
			Assert.AreEqual(master.V14.Uv1, remote2.V14.Uv1);
			Assert.AreEqual(master.Uv0, remote2.Uv0);
			Assert.AreEqual(master.Uv1, remote2.Uv1);
			Assert.AreEqual(2, remote2.uft1Count);
			Assert.AreEqual(10, remote2.uft2a);
			Assert.IsTrue("YouAreP2" == remote2.uft3a);
			Assert.AreEqual(22, remote2.uft3b);
		}

		[TestMethod]
		public void TestValue32NonTarget()
		{
			ByteBuffer buffer = new ByteBuffer(1024 * 16);

			NetworkPlayer p1 = new(new UserId(1));

			CTS.Instance.SyncObjects.ZTest_Value32NonTarget master = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value32NonTarget remote = new();

			Assert.IsFalse(master.IsDirtyReliable);
			master.V0 = 123;
			master.V1 = -123;
			master.V2 = 12345;
			master.V3 = -12345;
			master.V4 = 1234567890;
			master.V5 = -1234567890;
			master.V7.Add(new UserId(0));
			master.V7.Add(new UserId(1));
			master.V7.Add(new UserId(2));
			master.V7.Add(new UserId(3));
			master.V7.Add(new UserId(4));
			master.V7.Remove(new UserId(2));
			master.V7.RemoveAt(0);
			master.V15.f1("Both");
			master.V15.V0 = 14;
			master.V15.Uv1 = 1414;
			master.f22();
			master.f22();
			master.f22();

			Sync(buffer, p1, master, remote);
			Assert.AreEqual(master.V0, remote.V0);
			Assert.AreEqual(master.V1, remote.V1);
			Assert.AreEqual(master.V2, remote.V2);
			Assert.AreEqual(master.V3, remote.V3);
			Assert.AreEqual(master.V4, remote.V4);
			Assert.AreEqual(master.V5, remote.V5);
			Assert.AreEqual(3, remote.V7.Count);
			Assert.IsTrue(new UserId(1) == remote.V7[0]);
			Assert.IsTrue(new UserId(3) == remote.V7[1]);
			Assert.IsTrue(new UserId(4) == remote.V7[2]);
			Assert.IsTrue("Both" == remote.V15.f1a);
			Assert.AreEqual(master.V15.V0, remote.V15.V0);
			Assert.AreEqual(master.V15.Uv1, remote.V15.Uv1);
			Assert.AreEqual(3, remote.f22Count);
		}

		[TestMethod]
		public void TestValue32Target()
		{
			ByteBuffer buffer = new ByteBuffer(1024 * 16);

			NetworkPlayer p1 = new(new UserId(1));
			NetworkPlayer p2 = new(new UserId(2));

			CTS.Instance.SyncObjects.ZTest_Value32Target master = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value32Target remote1 = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value32Target remote2 = new();

			Assert.IsFalse(master.IsDirtyReliable);
			master.V0 = 123;
			master.V1 = -123;
			master.V2 = 12345;
			master.V3 = -12345;
			master.V4 = 1234567890;
			master.V5 = -1234567890;
			master.V7.Add(new UserId(0));
			master.V7.Add(new UserId(1));
			master.V7.Add(new UserId(2));
			master.V7.Add(new UserId(3));
			master.V7.Add(new UserId(4));
			master.V7.Remove(new UserId(2));
			master.V7.RemoveAt(0);
			master.ft15(p1);
			master.ft15(p1);
			master.ft15(p1);
			master.ft15(p2);
			master.V19.f1(p1, "YouAreP1");
			master.V19.f1(p2, "YouAreP2");
			master.V19.V0 = 13;
			master.V19.Uv1 = 1313;
			master.V20.f1("Both");
			master.V20.V0 = 14;
			master.V20.Uv1 = 1414;
			master.V23.Add(new UserId(0));
			master.V23.Add(new UserId(1));
			master.V23.Add(new UserId(2));
			master.V23.Add(new UserId(3));
			master.V23.Add(new UserId(4));
			master.V23.Remove(new UserId(2));
			master.V23.RemoveAt(0);
			master.f24(10);
			master.f24(10);
			master.f24(10);
			master.f28(10);
			master.f28(10);

			Sync(buffer, p1, master, remote1);
			Assert.AreEqual(master.V0, remote1.V0);
			Assert.AreEqual(master.V1, remote1.V1);
			Assert.AreEqual(master.V2, remote1.V2);
			Assert.AreEqual(master.V3, remote1.V3);
			Assert.AreEqual(master.V4, remote1.V4);
			Assert.AreEqual(master.V5, remote1.V5);
			Assert.AreEqual(3, remote1.V7.Count);
			Assert.IsTrue(new UserId(1) == remote1.V7[0]);
			Assert.IsTrue(new UserId(3) == remote1.V7[1]);
			Assert.IsTrue(new UserId(4) == remote1.V7[2]);
			Assert.AreEqual(3, remote1.ft15Count);
			Assert.IsTrue("YouAreP1" == remote1.V19.f1a);
			Assert.AreEqual(master.V19.V0, remote1.V19.V0);
			Assert.AreEqual(master.V19.Uv1, remote1.V19.Uv1);
			Assert.IsTrue("Both" == remote1.V20.f1a);
			Assert.AreEqual(master.V20.V0, remote1.V20.V0);
			Assert.AreEqual(master.V20.Uv1, remote1.V20.Uv1);
			Assert.AreEqual(3, remote1.V23.Count);
			Assert.IsTrue(new UserId(1) == remote1.V23[0]);
			Assert.IsTrue(new UserId(3) == remote1.V23[1]);
			Assert.IsTrue(new UserId(4) == remote1.V23[2]);
			Assert.AreEqual(30, remote1.f24a);
			Assert.AreEqual(20, remote1.f28a);

			Sync(buffer, p2, master, remote2);
			Assert.AreEqual(master.V0, remote2.V0);
			Assert.AreEqual(master.V1, remote2.V1);
			Assert.AreEqual(master.V2, remote2.V2);
			Assert.AreEqual(master.V3, remote2.V3);
			Assert.AreEqual(master.V4, remote2.V4);
			Assert.AreEqual(master.V5, remote2.V5);
			Assert.AreEqual(3, remote2.V7.Count);
			Assert.IsTrue(new UserId(1) == remote2.V7[0]);
			Assert.IsTrue(new UserId(3) == remote2.V7[1]);
			Assert.IsTrue(new UserId(4) == remote2.V7[2]);
			Assert.AreEqual(1, remote2.ft15Count);
			Assert.IsTrue("YouAreP2" == remote2.V19.f1a);
			Assert.AreEqual(master.V19.V0, remote2.V19.V0);
			Assert.AreEqual(master.V19.Uv1, remote2.V19.Uv1);
			Assert.IsTrue("Both" == remote2.V20.f1a);
			Assert.AreEqual(master.V20.V0, remote2.V20.V0);
			Assert.AreEqual(master.V20.Uv1, remote2.V20.Uv1);
			Assert.AreEqual(3, remote2.V23.Count);
			Assert.IsTrue(new UserId(1) == remote2.V23[0]);
			Assert.IsTrue(new UserId(3) == remote2.V23[1]);
			Assert.IsTrue(new UserId(4) == remote2.V23[2]);
			Assert.AreEqual(30, remote2.f24a);
			Assert.AreEqual(20, remote2.f28a);
		}

		[TestMethod]
		public void TestInheritParent()
		{
			// Child in Parent
			ByteBuffer buffer = new ByteBuffer(1024 * 16);

			NetworkPlayer p1 = new(new UserId(1));
			NetworkPlayer p2 = new(new UserId(2));

			CTS.Instance.SyncObjects.ZTest_Parent master = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Parent remote1 = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Parent remote2 = new();

			master.Server_P1();
			master.Server_P1();
			master.Server_P2_Public(p1, 10, 20);
			master.Server_P2_Public(p1, 10, 20);
			master.Server_P2_Public(p2, 20, 40);
			master.Field_Server_P1 = 12345;
			master.Field_Server_P2_Public = 12345.12345f;

			remote1.Field_Client_P1 = 98765;
			remote1.Field_Client_P2_Public = 98765;
			remote1.Client_P1();
			remote1.Client_P1();
			remote1.Client_P2_Public(10, 20);
			remote1.Client_P2_Public(10, 20);

			remote2.Field_Client_P1 = 87654;
			remote2.Field_Client_P2_Public = 87654;
			remote2.Client_P1();
			remote2.Client_P1();
			remote2.Client_P1();
			remote2.Client_P2_Public(20, 40);
			remote2.Client_P2_Public(20, 40);

			Sync(buffer, p1, master, remote1);

			Assert.AreEqual(master.Field_Server_P1, remote1.Field_Server_P1);
			Assert.AreEqual(master.Field_Server_P2_Public, remote1.Field_Server_P2_Public);
			Assert.AreEqual(2, remote1.Server_P1_Count);
			Assert.AreEqual(20, remote1.Server_P2_a);
			Assert.AreEqual(40, remote1.Server_P2_b);

			Assert.AreEqual(remote1.Field_Client_P1, master.Field_Client_P1);
			Assert.AreEqual(remote1.Field_Client_P2_Public, master.Field_Client_P2_Public);
			Assert.AreEqual(2, master.Client_P1_CountTable[p1]);
			Assert.AreEqual(20, master.Client_P2_Table[p1].a);
			Assert.AreEqual(40, master.Client_P2_Table[p1].b);

			Sync(buffer, p2, master, remote2);

			Assert.AreEqual(master.Field_Server_P1, remote2.Field_Server_P1);
			Assert.AreEqual(master.Field_Server_P2_Public, remote2.Field_Server_P2_Public);
			Assert.AreEqual(2, remote2.Server_P1_Count);
			Assert.AreEqual(20, remote2.Server_P2_a);
			Assert.AreEqual(40, remote2.Server_P2_b);

			Assert.AreEqual(remote2.Field_Client_P1, master.Field_Client_P1);
			Assert.AreEqual(remote2.Field_Client_P2_Public, master.Field_Client_P2_Public);
			Assert.AreEqual(3, master.Client_P1_CountTable[p2]);
			Assert.AreEqual(40, master.Client_P2_Table[p2].a);
			Assert.AreEqual(80, master.Client_P2_Table[p2].b);
		}

		[TestMethod]
		public void TestInheritChild()
		{
			// Child in Parent
			ByteBuffer buffer = new ByteBuffer(1024 * 16);

			NetworkPlayer p1 = new(new UserId(1));
			NetworkPlayer p2 = new(new UserId(2));

			CTS.Instance.SyncObjects.ZTest_Child masterChild = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Child remoteChild1 = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Child remoteChild2 = new();

			CTS.Instance.SyncObjects.ZTest_Parent master = masterChild;
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Parent remote1 = remoteChild1;
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Parent remote2 = remoteChild2;

			master.Server_P1();
			master.Server_P1();
			master.Server_P2_Public(p1, 10, 20);
			master.Server_P2_Public(p1, 10, 20);
			master.Server_P2_Public(p2, 20, 40);
			master.Field_Server_P1 = 111;
			master.Field_Server_P2_Public = 222.222f;
			masterChild.Server_C3();
			masterChild.Server_C3();
			masterChild.Server_C3();
			masterChild.Server_C4_Public(p1);
			masterChild.Server_C4_Public(p2);
			masterChild.Server_C4_Public(p2);
			masterChild.Field_Server_C3 = 333;
			masterChild.Field_Server_C4_Public = 444;

			remote1.Field_Client_P1 = 11111;
			remote1.Field_Client_P2_Public = 22222;
			remote1.Client_P1();
			remote1.Client_P1();
			remote1.Client_P2_Public(10, 20);
			remote1.Client_P2_Public(10, 20);
			for (int i = 0; i < 7 ; i++)
				remoteChild1.Client_C3();
			for (int i = 0; i < 8 ; i++)
				remoteChild1.Client_C4_Public();
			remoteChild1.Field_Client_C3 = 33333;
			remoteChild1.Field_Client_C4_Public = 44444;

			remote2.Field_Client_P1 = 10111;
			remote2.Field_Client_P2_Public = 20222;
			remote2.Client_P1();
			remote2.Client_P1();
			remote2.Client_P1();
			remote2.Client_P2_Public(20, 40);
			remote2.Client_P2_Public(20, 40);
			for (int i = 0; i < 14; i++)
				remoteChild2.Client_C3();
			for (int i = 0; i < 16; i++)
				remoteChild2.Client_C4_Public();
			remoteChild2.Field_Client_C3 = 30333;
			remoteChild2.Field_Client_C4_Public = 40444;

			// Master <--> Remote 1
			Sync(buffer, p1, masterChild, remoteChild1);

			// Check parent
			Assert.AreEqual(master.Field_Server_P1, remote1.Field_Server_P1);
			Assert.AreEqual(master.Field_Server_P2_Public, remote1.Field_Server_P2_Public);
			Assert.AreEqual(2, remote1.Server_P1_Count);
			Assert.AreEqual(40, remote1.Server_P2_a);
			Assert.AreEqual(80, remote1.Server_P2_b);

			Assert.AreEqual(remote1.Field_Client_P1, master.Field_Client_P1);
			Assert.AreEqual(remote1.Field_Client_P2_Public, master.Field_Client_P2_Public);
			Assert.AreEqual(2, master.Client_P1_CountTable[p1]);
			Assert.AreEqual(20, master.Client_P2_Table[p1].a);
			Assert.AreEqual(40, master.Client_P2_Table[p1].b);

			// Check child
			Assert.AreEqual(masterChild.Field_Server_C3, remoteChild1.Field_Server_C3);
			Assert.AreEqual(masterChild.Field_Server_C4_Public, remoteChild1.Field_Server_C4_Public);
			Assert.AreEqual(3, remoteChild1.Server_C3_Count);
			Assert.AreEqual(1, remoteChild1.Server_C4_Count);

			Assert.AreEqual(remoteChild1.Field_Client_C3, masterChild.Field_Client_C3);
			Assert.AreEqual(remoteChild1.Field_Client_C4_Public, masterChild.Field_Client_C4_Public);
			Assert.AreEqual(7, masterChild.Client_C3_CountTable[p1]);
			Assert.AreEqual(8, masterChild.Client_C4_CountTable[p1]);

			// Master <--> Remote 2
			Sync(buffer, p2, masterChild, remoteChild2);

			// Check parent
			Assert.AreEqual(master.Field_Server_P1, remote2.Field_Server_P1);
			Assert.AreEqual(master.Field_Server_P2_Public, remote2.Field_Server_P2_Public);
			Assert.AreEqual(2, remote2.Server_P1_Count);
			Assert.AreEqual(40, remote2.Server_P2_a);
			Assert.AreEqual(80, remote2.Server_P2_b);

			Assert.AreEqual(remote2.Field_Client_P1, master.Field_Client_P1);
			Assert.AreEqual(remote2.Field_Client_P2_Public, master.Field_Client_P2_Public);
			Assert.AreEqual(3, master.Client_P1_CountTable[p2]);
			Assert.AreEqual(40, master.Client_P2_Table[p2].a);
			Assert.AreEqual(80, master.Client_P2_Table[p2].b);

			// Check child
			Assert.AreEqual(masterChild.Field_Server_C3, remoteChild2.Field_Server_C3);
			Assert.AreEqual(masterChild.Field_Server_C4_Public, remoteChild2.Field_Server_C4_Public);
			Assert.AreEqual(3, remoteChild2.Server_C3_Count);
			Assert.AreEqual(2, remoteChild2.Server_C4_Count);

			Assert.AreEqual(remoteChild2.Field_Client_C3, masterChild.Field_Client_C3);
			Assert.AreEqual(remoteChild2.Field_Client_C4_Public, masterChild.Field_Client_C4_Public);
			Assert.AreEqual(14, masterChild.Client_C3_CountTable[p2]);
			Assert.AreEqual(16, masterChild.Client_C4_CountTable[p2]);

		}

		public void Sync(ByteBuffer buffer,
						 NetworkPlayer player,
						 MasterNetworkObject master,
						 CTC.Networks.SyncObjects.TestSyncObjects.RemoteNetworkObject remote)
		{
			// Master -> Remote
			buffer.Reset();
			master.SerializeSyncReliable(player, buffer);
			remote.TryDeserializeSyncReliable(buffer);

			buffer.Reset();
			master.SerializeSyncUnreliable(player, buffer);
			remote.TryDeserializeSyncUnreliable(buffer);

			// Remote -> Master
			buffer.Reset();
			remote.SerializeSyncReliable(buffer);
			master.TryDeserializeSyncReliable(player, buffer);

			buffer.Reset();
			remote.SerializeSyncUnreliable(buffer);
			master.TryDeserializeSyncUnreliable(player, buffer);
		}

		public void Clear(MasterNetworkObject master)
		{
			master.ClearDirtyReliable();
			master.ClearDirtyUnreliable();
		}
	}
}
