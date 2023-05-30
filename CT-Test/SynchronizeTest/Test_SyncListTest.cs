﻿using System;
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

			SyncList<NetInt32> src = new()
			{
				0,
				1,
				2,
				3,
			};
			SyncList<NetInt32> des = new();

			checkWithBuffercheck(src, des, pw, pr);

			src.Add(100);
			src.Add(123);
			src.Insert(1, 99);
			src.Add(55);

			checkWithBuffercheck(src, des, pw, pr);

			src.Remove(123);
			src.RemoveAt(0);

			checkWithBuffercheck(src, des, pw, pr);

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
