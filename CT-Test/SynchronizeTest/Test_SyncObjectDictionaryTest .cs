using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[TestClass]
	public class Test_SyncObjectDictionaryTest
	{
		private void createSyncObj(CTS.Instance.SyncObjects.ZTest_InnerTest syncObj)
		{
			syncObj.A = 10;
		}

		[TestMethod]
		public void SyncObjectDictionaryTest()
		{
			NetworkPlayer p1 = new NetworkPlayer(new UserId(1));
			NetworkPlayer p2 = new NetworkPlayer(new UserId(2));

			ByteBuffer data = new(1000);
			IPacketWriter pw = data;
			IPacketReader pr = data;

			int count = 10;
			NetworkIdentity[] id = new NetworkIdentity[count];
			for (int i = 0; i < count; i++)
			{
				id[i] = new NetworkIdentity(i);
			}

			DirtyableMockup dsrc = new DirtyableMockup();
			DirtyableMockup ddes1 = new DirtyableMockup();
			DirtyableMockup ddes2 = new DirtyableMockup();

			CTS.Instance.Synchronizations.SyncObjectDictionary
				<NetworkIdentity, CTS.Instance.SyncObjects.ZTest_InnerTest> src = new(dsrc);
			CT.Common.DataType.Synchronizations.SyncObjectDictionary
				<NetworkIdentity, CTC.Networks.SyncObjects.TestSyncObjects.ZTest_InnerTest> desp1 = new(ddes1);
			CT.Common.DataType.Synchronizations.SyncObjectDictionary
				<NetworkIdentity, CTC.Networks.SyncObjects.TestSyncObjects.ZTest_InnerTest> desp2 = new(ddes2);

			Assert.IsFalse(dsrc.IsDirtyReliable);
			src.Add(id[1], createSyncObj);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			dsrc.ClearDirtyReliable();
			src[id[1]].B = 20;
			Assert.IsTrue(dsrc.IsDirtyReliable);

			// Sync
			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			Assert.AreEqual(src.Count, 1);
			Assert.AreEqual(src.Count, desp1.Count);
			Assert.AreEqual(20, desp1[id[1]].B);
			dsrc.ClearDirtyReliable();
			src.Remove(id[1]);
			Assert.IsTrue(dsrc.IsDirtyReliable);

			// Sync
			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			Assert.AreEqual(src.Count, 0);

			for (int i = 0; i < 8; i++)
			{
				src.Add(id[i], createSyncObj);
			}

			dsrc.ClearDirtyReliable();
			src[id[0]].A = 30;
			Assert.IsTrue(dsrc.IsDirtyReliable);
			src[id[7]].A = 70;

			// Sync
			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			Assert.AreEqual(30, desp1[id[0]].A);
			for (int i = 1; i < 7; i++)
			{
				Assert.AreEqual(10, desp1[id[i]].A);
			}
			Assert.AreEqual(70, desp1[id[7]].A);

			Assert.AreEqual(0, desp1[id[0]].B);

			dsrc.ClearDirtyReliable();
			src[id[3]].B = 30;
			src[id[4]].B = 40;
			Assert.IsTrue(dsrc.IsDirtyReliable);

			for (int i = 0; i < 3; i++)
			{
				src[id[3]].Server_Test();
				src[id[4]].Server_Test();
			}
			dsrc.ClearDirtyReliable();
			src[id[5]].Server_TestTarget(p1);
			Assert.IsTrue(dsrc.IsDirtyReliable);

			src[id[6]].Server_TestTarget(p2);
			src[id[6]].Server_TestTarget(p2);

			// Sync
			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			Assert.AreEqual(30, desp1[id[3]].B);
			Assert.AreEqual(40, desp1[id[4]].B);
			Assert.AreEqual(0, desp1[id[2]].ServerTest);
			Assert.AreEqual(3, desp1[id[3]].ServerTest);
			Assert.AreEqual(3, desp1[id[4]].ServerTest);
			Assert.AreEqual(0, desp1[id[5]].ServerTest);
			Assert.AreEqual(1, desp1[id[5]].ServerTestTarget);
			Assert.AreEqual(0, desp1[id[6]].ServerTestTarget);

			Assert.AreEqual(30, desp2[id[3]].B);
			Assert.AreEqual(40, desp2[id[4]].B);
			Assert.AreEqual(0, desp2[id[2]].ServerTest);
			Assert.AreEqual(3, desp2[id[3]].ServerTest);
			Assert.AreEqual(3, desp2[id[4]].ServerTest);
			Assert.AreEqual(0, desp2[id[5]].ServerTest);
			Assert.AreEqual(0, desp2[id[5]].ServerTestTarget);
			Assert.AreEqual(2, desp2[id[6]].ServerTestTarget);

			desp1[id[4]].Client_Test();
			desp1[id[6]].Client_Test();
			for (int i = 0; i < 2; i++)
			{
				desp1[id[3]].Client_Test();
				desp1[id[5]].Client_Test();

				desp2[id[4]].Client_Test();
				desp2[id[6]].Client_Test();
			}

			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			Assert.AreEqual(0, src[id[2]].GetCallStackValue(p1));
			Assert.AreEqual(2, src[id[3]].GetCallStackValue(p1));
			Assert.AreEqual(1, src[id[4]].GetCallStackValue(p1));
			Assert.AreEqual(2, src[id[5]].GetCallStackValue(p1));
			Assert.AreEqual(1, src[id[6]].GetCallStackValue(p1));
			Assert.AreEqual(0, src[id[7]].GetCallStackValue(p1));

			Assert.AreEqual(0, src[id[2]].GetCallStackValue(p2));
			Assert.AreEqual(0, src[id[3]].GetCallStackValue(p2));
			Assert.AreEqual(2, src[id[4]].GetCallStackValue(p2));
			Assert.AreEqual(0, src[id[5]].GetCallStackValue(p2));
			Assert.AreEqual(2, src[id[6]].GetCallStackValue(p2));
			Assert.AreEqual(0, src[id[7]].GetCallStackValue(p2));

			dsrc.ClearDirtyReliable();
			src.Clear();
			Assert.IsTrue(dsrc.IsDirtyReliable);

			dsrc.ClearDirtyReliable();
			src.Add(id[0], (obj) => { });
			Assert.IsTrue(dsrc.IsDirtyReliable);

			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			dsrc.ClearDirtyReliable();
			src[id[0]].Server_TestTarget(p1);
			Assert.IsTrue(dsrc.IsDirtyReliable);

			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);
		}

		public void Sync(ByteBuffer buffer,
						 NetworkPlayer player,
						 IMasterSynchronizable master,
						 IRemoteSynchronizable remote)
		{
			// Master -> Remote
			buffer.Reset();
			master.SerializeSyncReliable(player, buffer);
			if (buffer.Size > 0)
			{
				Assert.IsTrue(remote.TryDeserializeSyncReliable(buffer));
			}

			// Remote -> Master
			buffer.Reset();
			remote.SerializeSyncReliable(buffer);
			if (buffer.Size > 0)
			{
				Assert.IsTrue(master.TryDeserializeSyncReliable(player, buffer));
			}
		}

		public void Clear(IMasterSynchronizable master,
						  IRemoteSynchronizable remote)
		{
			master.ClearDirtyReliable();
			remote.ClearDirtyReliable();
		}
	}
}