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

			CTS.Instance.Synchronizations.SyncObjectList<CTS.Instance.SyncObjects.ZTest_InnerTest> src = new();
			CT.Common.DataType.Synchronizations.SyncObjectList<CTC.Networks.SyncObjects.TestSyncObjects.ZTest_InnerTest> des = new();

			src.Add(createSyncObj);
			src[0].B = 20;

			Sync(data, p1, src, des);
			Clear(src, des);

			Assert.AreEqual(src.Count, 1);
			Assert.AreEqual(src.Count, des.Count);
			Assert.AreEqual(src[0].B, des[0].B);
			src.RemoveAt(0);

			Sync(data, p1, src, des);
			Clear(src, des);

			Assert.AreEqual(src.Count, 0);

			src.Add(createSyncObj);
			src.Add(createSyncObj);
			src.Add(createSyncObj);
			src.Add(createSyncObj);
			src.Add(createSyncObj);
			src.Add(createSyncObj);
			src.Add(createSyncObj);
			src.Add(createSyncObj);

			Sync(data, p1, src, des);
			Clear(src, des);

		}

		public void Sync(ByteBuffer buffer,
						 NetworkPlayer player,
						 IMasterSynchronizable master,
						 IRemoteSynchronizable remote)
		{
			// Master -> Remote
			buffer.Reset();
			master.SerializeSyncReliable(player, buffer);
			remote.TryDeserializeSyncReliable(buffer);

			// Remote -> Master
			buffer.Reset();
			remote.SerializeSyncReliable(buffer);
			master.TryDeserializeSyncReliable(player, buffer);
		}

		public void Clear(IMasterSynchronizable master,
						  IRemoteSynchronizable remote)
		{
			master.ClearDirtyReliable();
			remote.ClearDirtyReliable();
		}
	}
}