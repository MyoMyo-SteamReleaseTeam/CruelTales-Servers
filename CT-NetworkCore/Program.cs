#if NET7_0_OR_GREATER
#else
using System;
using System.Threading;
#endif
using LiteNetLib;

namespace CT.Network
{
	internal class Program
	{
		static void Main(string[] args)
		{
			SerializationTest();
		}

		public static void SerializationTest()
		{
			byte[] buffer = new byte[16];

			ArraySegment<byte> segment = new ArraySegment<byte>(buffer, 8, 4);

			Console.WriteLine(segment.Count);
			Console.WriteLine(segment.Offset);
		}

		public static void ClientTest()
		{
			Console.WriteLine("Hello, World!");

			EventBasedNetListener listener = new EventBasedNetListener();
			NetManager client = new NetManager(listener);
			client.Start();
			client.Connect("localhost" /* host ip or name */, 9050 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);
			listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
			{
				Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
				dataReader.Recycle();
			};

			while (!Console.KeyAvailable)
			{
				client.PollEvents();
				Thread.Sleep(15);
			}

			client.Stop();
		}
	}
}