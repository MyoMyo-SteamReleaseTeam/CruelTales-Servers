using System;
using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[TestClass]
	public class Test_SyncListTest
	{
		[TestMethod]
		public void SyncListTest()
		{
			ByteBuffer data = new(1000);
			IPacketWriter pw = data;
			IPacketReader pr = data;

			DirtyableMockup dsrc = new DirtyableMockup();
			DirtyableMockup ddes = new DirtyableMockup();

			SyncList<NetInt32> src = new(dsrc)
			{
				0,
				1,
				2,
				3,
			};
			SyncList<NetInt32> des = new(ddes);

			checkWithBuffercheck(src, des, pw, pr);
			dsrc.ClearDirtyReliable();

			src.Add(100);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			src.Add(123);
			dsrc.ClearDirtyReliable();
			src.Insert(1, 99);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			src.Add(55);

			checkWithBuffercheck(src, des, pw, pr);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			dsrc.ClearDirtyReliable();

			src.Remove(123);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			dsrc.ClearDirtyReliable();
			Assert.IsFalse(src.Remove(123));
			src.RemoveAt(0);
			Assert.IsTrue(dsrc.IsDirtyReliable);

			checkWithBuffercheck(src, des, pw, pr);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			dsrc.ClearDirtyReliable();

			src.Clear();

			checkWithBuffercheck(src, des, pw, pr);

			Assert.IsTrue(des.Count == 0);
		}

		private void checkWithBuffercheck<T>(SyncList<T> lhs, SyncList<T> rhs,
											 IPacketWriter pw, IPacketReader pr)
			where T : struct, IPacketSerializable, IEquatable<T>
		{
			lhs.SerializeSyncReliable(pw);
			Assert.IsTrue(rhs.TryDeserializeSyncReliable(pr));
			for (int i = 0; i < lhs.Count; i++)
			{
				Assert.IsTrue(lhs[i].Equals(rhs[i]));
			}
			pw.ResetWriter();
			pr.ResetReader();

			lhs.ClearDirtyReliable();
		}
	}
}
