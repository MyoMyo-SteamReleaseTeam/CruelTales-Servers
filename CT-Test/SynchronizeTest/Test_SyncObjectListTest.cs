using System;
using System.Runtime.ConstrainedExecution;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[TestClass]
	public class Test_SyncObjectListTest
	{
		private void createSyncObj(CTS.Instance.SyncObjects.ZTest_InnerTest syncObj)
		{
			syncObj.A = 10;
		}

		[TestMethod]
		public void SyncListObjectTest()
		{
			NetworkPlayer p1 = new NetworkPlayer(new UserId(1));
			NetworkPlayer p2 = new NetworkPlayer(new UserId(2));

			ByteBuffer data = new(1000);
			IPacketWriter pw = data;
			IPacketReader pr = data;

			CTS.Instance.Synchronizations.SyncObjectList
				<CTS.Instance.SyncObjects.ZTest_InnerTest> src = new();
			CT.Common.DataType.Synchronizations.SyncObjectList
				<CTC.Networks.SyncObjects.TestSyncObjects.ZTest_InnerTest> desp1 = new();
			CT.Common.DataType.Synchronizations.SyncObjectList
				<CTC.Networks.SyncObjects.TestSyncObjects.ZTest_InnerTest> desp2 = new();

			src.Add(createSyncObj);
			src[0].B = 20;

			// Sync
			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			Assert.AreEqual(src.Count, 1);
			Assert.AreEqual(src.Count, desp1.Count);
			Assert.AreEqual(20, desp1[0].B);
			src.RemoveAt(0);

			// Sync
			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			Assert.AreEqual(src.Count, 0);

			for (int i = 0; i < 8; i++)
			{
				src.Add(createSyncObj);
			}

			src[0].A = 30;
			src[7].A = 70;

			// Sync
			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			Assert.AreEqual(30, desp1[0].A);
			for (int i = 1; i < 7; i++)
			{
				Assert.AreEqual(10, desp1[i].A);
			}
			Assert.AreEqual(70, desp1[7].A);

			Assert.AreEqual(0, desp1[0].B);

			src[3].B = 30;
			src[4].B = 40;

			for (int i = 0; i < 3; i++)
			{
				src[3].Server_Test();
				src[4].Server_Test();
			}
			src[5].Server_TestTarget(p1);

			src[6].Server_TestTarget(p2);
			src[6].Server_TestTarget(p2);

			// Sync
			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			Assert.AreEqual(30, desp1[3].B);
			Assert.AreEqual(40, desp1[4].B);
			Assert.AreEqual(0, desp1[2].ServerTest);
			Assert.AreEqual(3, desp1[3].ServerTest);
			Assert.AreEqual(3, desp1[4].ServerTest);
			Assert.AreEqual(0, desp1[5].ServerTest);
			Assert.AreEqual(1, desp1[5].ServerTestTarget);
			Assert.AreEqual(0, desp1[6].ServerTestTarget);

			Assert.AreEqual(30, desp2[3].B);
			Assert.AreEqual(40, desp2[4].B);
			Assert.AreEqual(0, desp2[2].ServerTest);
			Assert.AreEqual(3, desp2[3].ServerTest);
			Assert.AreEqual(3, desp2[4].ServerTest);
			Assert.AreEqual(0, desp2[5].ServerTest);
			Assert.AreEqual(0, desp2[5].ServerTestTarget);
			Assert.AreEqual(2, desp2[6].ServerTestTarget);

			desp1[4].Client_Test();
			desp1[6].Client_Test();
			for (int i = 0; i < 2; i++)
			{
				desp1[3].Client_Test();
				desp1[5].Client_Test();

				desp2[4].Client_Test();
				desp2[6].Client_Test();
			}

			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			Assert.AreEqual(0, src[2].GetCallStackValue(p1));
			Assert.AreEqual(2, src[3].GetCallStackValue(p1));
			Assert.AreEqual(1, src[4].GetCallStackValue(p1));
			Assert.AreEqual(2, src[5].GetCallStackValue(p1));
			Assert.AreEqual(1, src[6].GetCallStackValue(p1));
			Assert.AreEqual(0, src[7].GetCallStackValue(p1));

			Assert.AreEqual(0, src[2].GetCallStackValue(p2));
			Assert.AreEqual(0, src[3].GetCallStackValue(p2));
			Assert.AreEqual(2, src[4].GetCallStackValue(p2));
			Assert.AreEqual(0, src[5].GetCallStackValue(p2));
			Assert.AreEqual(2, src[6].GetCallStackValue(p2));
			Assert.AreEqual(0, src[7].GetCallStackValue(p2));

			src.Clear();
			src.Add((obj) => { });

			Sync(data, p1, src, desp1);
			Sync(data, p2, src, desp2);
			Clear(src, desp1);
			Clear(src, desp2);

			src[0].Server_TestTarget(p1);

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