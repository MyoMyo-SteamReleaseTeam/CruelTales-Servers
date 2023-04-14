using CT.Tools.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tool
{
	[TestClass]
	public class Test_Bitmask
	{
		[TestMethod]
		public void Bitmask32()
		{
			Bitmask32 curMask = new Bitmask32();

			Assert.IsTrue(curMask.IsAllFalse());
			Assert.IsFalse(curMask[1]);
			Assert.IsFalse(curMask[2]);

			curMask[10] = true;
			Assert.IsTrue(curMask[10]);
			Assert.IsTrue(!curMask.IsAllFalse());
			curMask[10] = false;
			Assert.IsFalse(curMask[10]);
			Assert.IsTrue(curMask.IsAllFalse());

			Assert.IsTrue(curMask.IsValidIndex(32 - 1));
			Assert.IsFalse(curMask.IsValidIndex(32));
			Assert.IsFalse(curMask.IsValidIndex(-1));

			curMask[23] = true;
			Assert.IsTrue(curMask[23]);
			Assert.IsFalse(curMask.IsAllFalse());

			curMask[30] = true;
			Assert.IsTrue(curMask[30]);
			Assert.IsFalse(curMask.IsAllFalse());

			// Test entire operation
			curMask.Clear(true);

			Assert.IsTrue(curMask.IsAllTrue());
			Assert.IsFalse(curMask.IsAllFalse());

			for (int i = 0; i < 32; i++)
			{
				Assert.IsTrue(curMask[i]);
			}

			curMask.Clear(false);

			Assert.IsTrue(curMask.IsAllFalse());
			Assert.IsFalse(curMask.IsAllTrue());

			for (int i = 0; i < 32; i++)
			{
				Assert.IsFalse(curMask[i]);
			}
		}

		[TestMethod]
		public void Test_SingleBitmask()
		{
			BitmaskByte mask = new BitmaskByte(true);

			Assert.IsTrue(mask.IsAllTrue());
			Assert.IsFalse(mask.IsAllFalse());

			for (int i = 0; i < 8; i++)
			{
				mask[i] = false;
			}

			Assert.IsFalse(mask.IsAllTrue());
			Assert.IsTrue(mask.IsAllFalse());

			mask.SetTrue(3);
			Assert.IsTrue(mask[3]);

			mask[5] = true;
			Assert.IsTrue(mask[5]);

			Assert.IsFalse(mask.IsAllFalse());
			Assert.IsFalse(mask.IsAllTrue());

			mask[5] = false;
			mask[3] = false;
			Assert.IsTrue(mask.IsAllFalse());
		}
	}
}
