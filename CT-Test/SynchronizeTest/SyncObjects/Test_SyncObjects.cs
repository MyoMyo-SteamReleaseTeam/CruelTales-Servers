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
		public void TestValue8()
		{
			ByteBuffer buffer = new ByteBuffer(1024 * 16);

			NetworkPlayer p1 = new(new UserId(1));
			NetworkPlayer p2 = new(new UserId(2));

			CTS.Instance.SyncObjects.ZTest_Value8 master = new ();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value8 remote = new();

			master.SerializeSyncReliable(p1, buffer);
			Assert.AreEqual(0, buffer.Size);

			master.SerializeSyncUnreliable(p1, buffer);
			Assert.AreEqual(0, buffer.Size);

			master.f3(10);
			master.uf1(p1, 20, 30);
			master.uf1(p2, 90, 100);
			for (int i = 0; i < 13; i++)
			{
				master.Call_uf5(p1);
			}

			for (int i = 0; i < 5; i++)
			{
				master.Call_uf5(p2);
			}
			master.V0 = "가나다";
			master.V2 = 99;

			Sync(buffer, p1, master, remote);

			Assert.AreEqual(30, remote.TestInt);
			Assert.AreEqual(30, remote.TestByte);
			Assert.AreEqual(13, remote.TestVoidCallCount);
			Assert.AreEqual("가나다", remote.V0.Value);
			Assert.AreEqual(99, remote.V2);

			remote.TestInt = 0;
			remote.TestByte = 0;
			remote.TestVoidCallCount = 0;

			remote.InitializeRemoteProperties();
			Sync(buffer, p2, master, remote);

			Assert.AreEqual(100, remote.TestInt);
			Assert.AreEqual(100, remote.TestByte);
			Assert.AreEqual(5, remote.TestVoidCallCount);
			Assert.AreEqual("가나다", remote.V0.Value);
			Assert.AreEqual(99, remote.V2);
		}

		[TestMethod]
		public void TestValue16()
		{
			ByteBuffer buffer = new ByteBuffer(1024 * 16);

			NetworkPlayer p1 = new(new UserId(1));
			NetworkPlayer p2 = new(new UserId(2));

			CTS.Instance.SyncObjects.ZTest_Value16 master = new();
			CTC.Networks.SyncObjects.TestSyncObjects.ZTest_Value16 remote = new();

			master.SerializeSyncReliable(p1, buffer);
			Assert.AreEqual(0, buffer.Size);

			master.SerializeSyncUnreliable(p1, buffer);
			Assert.AreEqual(0, buffer.Size);

			for (int i = 0; i < 10; i++)
			{
				master.f3(10);
				master.uf1(p1, 10, 10);
				master.uf3(p1, 10, 20, TestEnumType.B);
				master.uf9(p1);
			}
			for (int i = 0; i < 3; i++)
			{
				master.uf1(p2, 10, 10);
				master.uf14(p2, 10);
			}

			master.V0 = 99;
			master.SetV13(99);

			Sync(buffer, p1, master, remote);

			Assert.AreEqual(100, remote.v_uf3int);
			Assert.AreEqual(100, remote.v_uf1int);
			Assert.AreEqual(100, remote.v_uf1sbyte);
			Assert.AreEqual(10, remote.v_uf9Count);

			Assert.AreEqual(99, remote.V0);
			Assert.AreEqual(99, remote.V13);

			remote.ResetTestValue();
			remote.InitializeRemoteProperties();

			Sync(buffer, p2, master, remote);

			Assert.AreEqual(100, remote.v_uf3int);
			Assert.AreEqual(30, remote.v_uf1int);
			Assert.AreEqual(30, remote.v_uf1sbyte);
			Assert.AreEqual(30, remote.v_uf14int);

			Assert.AreEqual(99, remote.V0);
			Assert.AreEqual(99, remote.V13);
		}

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
