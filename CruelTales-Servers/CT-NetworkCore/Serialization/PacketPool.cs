//namespace CT.Network.Serialization
//{
//	public static class PacketPool
//	{
//		public static int MtuSize => mMtuPacketPool.SliceSize;
//		public static int StreamSize => mStreamPacketPool.SliceSize;

//		private static NetPacketPool mMtuPacketPool = new();
//		private static NetPacketPool mStreamPacketPool = new();

//		public static bool TryAllocateForTest(int mtuSize, int count = 10)
//		{
//			bool result = true;
//			result &= mMtuPacketPool.TryAllocate(mtuSize, count);
//			result &= mStreamPacketPool.TryAllocate(mtuSize, count);
//			return result;
//		}

//		public static bool TryAllocate()
//		{
//			bool result = true;
//			result &= mMtuPacketPool.TryAllocate(1500, 500);
//			result &= mStreamPacketPool.TryAllocate(Numeric.KiB * 60, 40);
//			return result;
//		}

//		public static NetPacket GetMtuPacket() => mMtuPacketPool.GetPacket();
//		public static void ReturnMtuPacket(NetPacket netPacket) => mMtuPacketPool.Return(netPacket);

//		public static NetPacket GetStreamPacket() => mStreamPacketPool.GetPacket();
//		public static void ReturnStreamPacket(NetPacket netPacket) => mStreamPacketPool.Return(netPacket);
//	}

//	public class NetPacketPool
//	{
//		public int SliceSize => mSlice;
//		public int Count => mCount;

//		private int mSlice = 0;
//		private int mCount = 0;
//		private byte[] mMemory;
//		private ArraySegment<byte>[] mMemoryChunk;
//		private StaticObjectPool<NetPacket> mNetPacketPool;

//		private bool mIsInitialized = false;
//		private object mLock = new object();

//		public bool IsInitialized() => mIsInitialized;

//		public bool TryAllocate(int sliceSize, int count)
//		{
//			try
//			{
//				mCount = count;
//				mSlice = sliceSize;
//				mMemory = new byte[mSlice * mCount];
//				mMemoryChunk = new ArraySegment<byte>[mCount];
//				List<NetPacket> netPackets = new List<NetPacket>(mCount);

//				for (int i = 0; i < mCount; i++)
//				{
//					mMemoryChunk[i] = new ArraySegment<byte>(mMemory, mSlice * i, mSlice);
//					netPackets.Add(new NetPacket(mMemoryChunk[i]));
//				}

//				mNetPacketPool = new StaticObjectPool<NetPacket>(netPackets);

//				mIsInitialized = true;
//				return true;
//			}
//			catch (Exception e)
//			{
//				Ulog.LogError(UlogType.NetPacketPool, $"TryAllocate Error! : {e}");
//				mIsInitialized = false;
//				return false;
//			}
//		}

//		public NetPacket GetPacket()
//		{
//			lock (mLock)
//			{
//				return mNetPacketPool.Get();
//			}
//		}

//		public void Return(NetPacket netPacket)
//		{
//			lock (mLock)
//			{
//				if (netPacket == null)
//				{
//					return;
//				}

//				mNetPacketPool.Return(netPacket);
//			}
//		}
//	}
//}
