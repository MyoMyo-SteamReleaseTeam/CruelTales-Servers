using System;
using CT.Common.DataType;
using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[TestClass]
	public class Test_SyncDictionaryTest
	{
		[TestMethod]
		public void SyncDictionaryTest()
		{
			ByteBuffer data = new(1000);
			IPacketWriter pw = data;
			IPacketReader pr = data;

			DirtyableMockup dsrc = new DirtyableMockup();
			DirtyableMockup ddes = new DirtyableMockup();

			SyncDictionary<NetworkIdentity, NetInt32> src = new(dsrc)
			{
				{ new(0), 25 },
				{ new(1), 50 },
				{ new(2), 75 },
				{ new(3), 100 },
				{ new(4), 125 },
				{ new(5), 150 },
			};
			SyncDictionary<NetworkIdentity, NetInt32> des = new(ddes);

			checkWithBuffercheck(src, des, pw, pr);
			dsrc.ClearDirtyReliable();

			src.Add(new(6), 175);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			src.Add(new(7), 200);

			checkWithBuffercheck(src, des, pw, pr);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			dsrc.ClearDirtyReliable();

			src.Remove(new NetworkIdentity(2));
			Assert.IsTrue(dsrc.IsDirtyReliable);
			src.Add(new(8), 200);

			checkWithBuffercheck(src, des, pw, pr);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			dsrc.ClearDirtyReliable();

			src[new(1)] = 100;
			Assert.IsTrue(dsrc.IsDirtyReliable);

			checkWithBuffercheck(src, des, pw, pr);
			Assert.IsTrue(dsrc.IsDirtyReliable);
			dsrc.ClearDirtyReliable();

			src.Clear();
			Assert.IsTrue(dsrc.IsDirtyReliable);

			checkWithBuffercheck(src, des, pw, pr);

			Assert.IsTrue(des.Count == 0);
		}

		private void checkWithBuffercheck<Key, Value>(SyncDictionary<Key, Value> lhs,
													  SyncDictionary<Key, Value> rhs,
													  IPacketWriter pw, IPacketReader pr)
			where Key : struct, IPacketSerializable, IEquatable<Key>
			where Value : struct, IPacketSerializable, IEquatable<Value>
		{
			lhs.SerializeSyncReliable(pw);
			Assert.IsTrue(rhs.TryDeserializeSyncReliable(pr));
			Assert.AreEqual(lhs.Count, rhs.Count);

			foreach (var lhsKey in lhs.Keys)
			{
				Assert.IsTrue(lhs[lhsKey].Equals(rhs[lhsKey]));
			}

			pw.ResetWriter();
			pr.ResetReader();

			lhs.ClearDirtyReliable();
		}
	}
}
