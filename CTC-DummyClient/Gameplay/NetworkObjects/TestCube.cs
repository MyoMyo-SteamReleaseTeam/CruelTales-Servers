using System;
using CT.Common.DataType;
using CTC.Networks.Synchronizations;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	public partial class TestCube : RemoteNetworkObject
	{
		public partial void TestRPC(NetStringShort someMessage)
		{
			//Console.WriteLine(someMessage);
		}
	}
}
