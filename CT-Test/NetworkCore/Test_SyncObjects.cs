using System;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Tools.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.NetworkCore
{
	[TestClass]
	public class Test_SyncObjects
	{
		[TestMethod]
		public void SyncObjectTest()
		{
			CTS.Instance.SyncObjects.TestSyncObject serverObj = new();
			TestSyncObject clientObj = new();

			NetTransform testTransform = new NetTransform()
			{
				Position = new NetVec2() { X = 10, Y = 20 },
				Velocity = new NetVec2() { X = 30, Y = 40 }
			};

			serverObj.Abc = 10;
			serverObj.Transform = testTransform;
			serverObj.Server_Some(50, 10.0f);
			serverObj.Server_Some(20, 20.0f);
			serverObj.Server_Some(55, 66.66f);

			byte[] syncData = new byte[1000];
			PacketWriter writer = new PacketWriter(syncData);
			serverObj.SerializeSyncReliable(writer);

			PacketReader reader = new PacketReader(syncData);
			clientObj.DeserializeSyncReliable(reader);

			Assert.AreEqual(10, clientObj.Abc);
			Assert.AreEqual(testTransform, clientObj.Transform);
			Assert.AreEqual(55, clientObj.value1);
			Assert.AreEqual(66.66f, clientObj.value2);
		}
	}

	public partial class TestSyncObject
	{
		public int Abc => _abc;
		public NetTransform Transform => _transform;
		public int value1 = 0;
		public float value2 = 0;
	}

	[Serializable]
	public partial class TestSyncObject : IRemoteSynchronizable
	{
		[SyncVar]
		private NetTransform _transform = new();
		public event Action<NetTransform>? OnTransformChanged;

		[SyncVar]
		private int _abc;
		public event Action<int>? OnAbcChanged;

		[SyncRpc]
		public partial void Server_Some(int value1, float value2);

		public partial void Server_Some(int value1, float value2)
		{
			this.value1 = value1;
			this.value2 = value2;
		}


		#region Synchronization
		public void DeserializeSyncReliable(PacketReader reader)
		{
			BitmaskByte objectDirty = reader.ReadBitmaskByte();


			if (objectDirty[0])
			{
				BitmaskByte _propertyDirty_0 = reader.ReadBitmaskByte();

				if (_propertyDirty_0[0])
				{
					_transform.Deserialize(reader);
					OnTransformChanged?.Invoke(_transform);
				}
				if (_propertyDirty_0[1])
				{
					_abc = reader.ReadInt32();
					OnAbcChanged?.Invoke(_abc);
				}

			}

			if (objectDirty[4])
			{
				BitmaskByte _rpcDirty_0 = reader.ReadBitmaskByte();

				if (_rpcDirty_0[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						int value1 = reader.ReadInt32();
						float value2 = reader.ReadSingle();

						Server_Some(value1, value2);
					}
				}

			}

		}


		public void DeserializeEveryProperty(PacketReader reader)
		{
			_transform.Deserialize(reader);
			_abc = reader.ReadInt32();

		}

		public void DeserializeSyncUnreliable(PacketReader reader)
		{
			throw new NotImplementedException();
		}



		#endregion
	}

}