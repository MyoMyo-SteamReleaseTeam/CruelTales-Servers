using CT.Common.DataType;
using CT.Common.Serialization;
using CTC.Networks.SyncObjects.TestSyncObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.SynchronizeTest
{
	[TestClass]
	public class Test_SyncObjects
	{
		[TestMethod]
		public void SyncObjectTest()
		{
			TestMasterNetworkObject serverObj = new();
			TestRemoteNetworkObject clientObj = new();

			NetTransform testTransform = new NetTransform()
			{
				Position = new NetVec2() { X = 10, Y = 20 },
				Velocity = new NetVec2() { X = 30, Y = 40 }
			};

			serverObj.UserToken = new UserToken(1122);
			serverObj.IntValue = 200;
			serverObj.Server_SendValue(10.50f);
			serverObj.Server_SendMessage("User token");

			byte[] syncData = new byte[1000];
			PacketWriter writer = new PacketWriter(syncData);
			serverObj.SerializeSyncReliable(writer);

			PacketReader reader = new PacketReader(syncData);
			clientObj.DeserializeSyncReliable(reader);

			Assert.AreEqual(new UserToken(1122), clientObj.UserToken);
			Assert.AreEqual(200, clientObj.IntValue);
			Assert.AreEqual(10.50f, clientObj.FloatParam);
			Assert.AreEqual("User token", clientObj.TextParam);
		}
	}
}