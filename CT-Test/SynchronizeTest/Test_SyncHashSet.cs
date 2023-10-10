using System;
using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[TestClass]
	public class Test_SyncHashSetTest
	{
		[TestMethod]
		public void SyncHashSetTest()
		{
			ByteBuffer data = new(1000);
			IPacketWriter pw = data;
			IPacketReader pr = data;

			DirtyableMockup dsrc = new DirtyableMockup();
			DirtyableMockup ddes = new DirtyableMockup();

			SyncHashSet<NetInt32> src = new(dsrc) { 0, 1, 2, 3 };
			SyncHashSet<NetInt32> des = new(ddes);

			checkWithBuffercheck(src, des, pw, pr);
			dsrc.ClearDirtyReliable();

			src.Add(100);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			src.Add(123);
			dsrc.ClearDirtyReliable();
			src.Add(912);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			src.Add(55);
			Assert.IsFalse(src.Add(55));

			checkWithBuffercheck(src, des, pw, pr);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			dsrc.ClearDirtyReliable();

			src.Remove(123);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			Assert.IsFalse(src.Remove(123));
			dsrc.ClearDirtyReliable();
			src.Remove(912);
			Assert.IsTrue(dsrc.IsDirtyReliable);

			checkWithBuffercheck(src, des, pw, pr);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			dsrc.ClearDirtyReliable();

			src.Clear();

			checkWithBuffercheck(src, des, pw, pr);

			Assert.IsTrue(des.Count == 0);
		}

		private void checkWithBuffercheck<T>(SyncHashSet<T> lhs, SyncHashSet<T> rhs,
											 IPacketWriter pw, IPacketReader pr)
			where T : struct, IPacketSerializable, IEquatable<T>
		{
			lhs.SerializeSyncReliable(pw);
			Assert.IsTrue(rhs.TryDeserializeSyncReliable(pr));
			Assert.IsTrue(lhs.SetEquals(rhs));
			pw.ResetWriter();
			pr.ResetReader();

			lhs.ClearDirtyReliable();
		}
	}
}
