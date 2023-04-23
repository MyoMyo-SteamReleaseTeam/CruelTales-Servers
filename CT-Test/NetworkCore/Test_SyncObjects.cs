using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Serialization.Type;
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
			TestSyncObjectServer serverObj = new();
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
			serverObj.InnerObject.Name = "Test";
			serverObj.InnerObject.UserId = new UserId(99);
			serverObj.InnerObject.Server_Rename("Vector");

			byte[] syncData = new byte[1000];
			PacketWriter writer = new PacketWriter(syncData);
			serverObj.SerializeSyncReliable(writer);

			PacketReader reader = new PacketReader(syncData);
			clientObj.DeserializeSyncReliable(reader);

			Assert.AreEqual(10, clientObj.Abc);
			Assert.AreEqual(testTransform, clientObj.Transform);
			Assert.AreEqual(55, clientObj.value1);
			Assert.AreEqual(66.66f, clientObj.value2);
			Assert.AreEqual("Test", clientObj.InnerObject.Name.Value);
			Assert.AreEqual(99U, clientObj.InnerObject.UserId.Id);
			Assert.AreEqual("Vector", clientObj.InnerObject.Rename.Value);
		}
	}

	[Serializable]
	public partial class TestInnerObjectServer : IMasterSynchronizable
	{
		[SyncVar]
		private UserId _userId = new();

		[SyncVar]
		private NetStringShort _name = string.Empty;

		[SyncRpc]
		public partial void Server_Rename(NetStringShort newName);



		#region Synchronization
		private BitmaskByte _propertyDirty_0 = new();
		private BitmaskByte _rpcDirty_0 = new();

		public bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _propertyDirty_0.AnyTrue();
				isDirty |= _rpcDirty_0.AnyTrue();

				return isDirty;
			}
		}

		public UserId UserId
		{
			get => _userId;
			set
			{
				if (_userId == value) return;
				_userId = value;
				_propertyDirty_0[0] = true;
			}
		}

		public NetStringShort Name
		{
			get => _name;
			set
			{
				if (_name == value) return;
				_name = value;
				_propertyDirty_0[1] = true;
			}
		}

		public partial void Server_Rename(NetStringShort newName)
		{
			Server_RenameCallstack.Enqueue(newName);
			_rpcDirty_0[0] = true;
		}
		private Queue<NetStringShort> Server_RenameCallstack = new();


		public bool IsDirtyUnreliable => false;

		public void SerializeSyncReliable(PacketWriter writer)
		{
			BitmaskByte objectDirty = new BitmaskByte();


			objectDirty[0] = _propertyDirty_0.AnyTrue();
			objectDirty[4] = _rpcDirty_0.AnyTrue();


			objectDirty.Serialize(writer);


			if (objectDirty[0])
			{
				_propertyDirty_0.Serialize(writer);

				if (_propertyDirty_0[0]) _userId.Serialize(writer);
				if (_propertyDirty_0[1]) _name.Serialize(writer);

			}

			if (objectDirty[4])
			{
				_rpcDirty_0.Serialize(writer);

				if (_rpcDirty_0[0])
				{
					byte count = (byte)Server_RenameCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_RenameCallstack.Dequeue();
						arg.Serialize(writer);

					}
				}

			}

		}

		public void SerializeSyncUnreliable(PacketWriter writer) { }

		public void ClearDirtyReliable()
		{
			_propertyDirty_0.Clear();
			_rpcDirty_0.Clear();

		}

		public void ClearDirtyUnreliable() { }

		public void SerializeEveryProperty(PacketWriter writer)
		{
			_userId.Serialize(writer);
			_name.Serialize(writer);

		}


		#endregion
	}

	public partial class TestSyncObjectServer
	{
		public TestInnerObjectServer InnerObject => _innerObject;
	}

	[Serializable]
	public partial class TestSyncObjectServer : IMasterSynchronizable
	{
		[SyncVar]
		private NetTransform _transform = new();

		[SyncVar]
		private int _abc;

		[SyncObject]
		private TestInnerObjectServer _innerObject = new();

		[SyncRpc]
		public partial void Server_Some(int value1, float value2);


		#region Synchronization
		private BitmaskByte _propertyDirty_0 = new();
		private BitmaskByte _rpcDirty_0 = new();

		public bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _propertyDirty_0.AnyTrue();
				isDirty |= _rpcDirty_0.AnyTrue();

				return isDirty;
			}
		}

		public NetTransform Transform
		{
			get => _transform;
			set
			{
				if (_transform == value) return;
				_transform = value;
				_propertyDirty_0[0] = true;
			}
		}

		public int Abc
		{
			get => _abc;
			set
			{
				if (_abc == value) return;
				_abc = value;
				_propertyDirty_0[1] = true;
			}
		}

		public partial void Server_Some(int value1, float value2)
		{
			Server_SomeCallstack.Enqueue((value1, value2));
			_rpcDirty_0[0] = true;
		}
		private Queue<(int value1, float value2)> Server_SomeCallstack = new();


		public bool IsDirtyUnreliable => false;

		public void SerializeSyncReliable(PacketWriter writer)
		{
			BitmaskByte objectDirty = new BitmaskByte();

			_propertyDirty_0[2] = _innerObject.IsDirtyReliable;

			objectDirty[0] = _propertyDirty_0.AnyTrue();
			objectDirty[4] = _rpcDirty_0.AnyTrue();


			objectDirty.Serialize(writer);


			if (objectDirty[0])
			{
				_propertyDirty_0.Serialize(writer);

				if (_propertyDirty_0[0]) _transform.Serialize(writer);
				if (_propertyDirty_0[1]) writer.Put(_abc);
				if (_propertyDirty_0[2]) _innerObject.SerializeSyncReliable(writer);

			}

			if (objectDirty[4])
			{
				_rpcDirty_0.Serialize(writer);

				if (_rpcDirty_0[0])
				{
					byte count = (byte)Server_SomeCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var args = Server_SomeCallstack.Dequeue();
						writer.Put(args.value1);
						writer.Put(args.value2);

					}
				}

			}

		}

		public void SerializeSyncUnreliable(PacketWriter writer) { }

		public void ClearDirtyReliable()
		{
			_propertyDirty_0.Clear();
			_rpcDirty_0.Clear();

		}

		public void ClearDirtyUnreliable() { }

		public void SerializeEveryProperty(PacketWriter writer)
		{
			_transform.Serialize(writer);
			writer.Put(_abc);
			_innerObject.SerializeEveryProperty(writer);

		}


		#endregion
	}

	public partial class TestSyncObject
	{
		public int Abc => _abc;
		public NetTransform Transform => _transform;
		public int value1 = 0;
		public float value2 = 0;
		public TestInnerObject InnerObject => _innerObject;

		public partial void Server_Some(int value1, float value2)
		{
			this.value1 = value1;
			this.value2 = value2;
		}
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

		[SyncObject]
		private TestInnerObject _innerObject = new();
		public event Action<TestInnerObject>? OnInnerObjectChanged;

		[SyncRpc]
		public partial void Server_Some(int value1, float value2);




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
				if (_propertyDirty_0[2])
				{
					_innerObject.DeserializeSyncReliable(reader);
					OnInnerObjectChanged?.Invoke(_innerObject);
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

		public void DeserializeSyncUnreliable(PacketReader reader) { }


		public void DeserializeEveryProperty(PacketReader reader)
		{
			_transform.Deserialize(reader);
			_abc = reader.ReadInt32();
			_innerObject.DeserializeEveryProperty(reader);

		}



		#endregion
	}

	public partial class TestInnerObject
	{
		public UserId UserId => _userId;
		public NetStringShort Name => _name;
		public NetStringShort Rename;

		public partial void Server_Rename(NetStringShort newName)
		{
			Rename = newName;
		}
	}

	[Serializable]
	public partial class TestInnerObject : IRemoteSynchronizable
	{
		[SyncVar]
		private UserId _userId = new();
		public event Action<UserId>? OnUserIdChanged;

		[SyncVar]
		private NetStringShort _name = string.Empty;
		public event Action<NetStringShort>? OnNameChanged;

		[SyncRpc]
		public partial void Server_Rename(NetStringShort newName);

		#region Synchronization
		public void DeserializeSyncReliable(PacketReader reader)
		{
			BitmaskByte objectDirty = reader.ReadBitmaskByte();


			if (objectDirty[0])
			{
				BitmaskByte _propertyDirty_0 = reader.ReadBitmaskByte();

				if (_propertyDirty_0[0])
				{
					_userId.Deserialize(reader);
					OnUserIdChanged?.Invoke(_userId);
				}
				if (_propertyDirty_0[1])
				{
					_name.Deserialize(reader);
					OnNameChanged?.Invoke(_name);
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
						NetStringShort newName = new();
						newName.Deserialize(reader);

						Server_Rename(newName);
					}
				}

			}

		}

		public void DeserializeSyncUnreliable(PacketReader reader) { }


		public void DeserializeEveryProperty(PacketReader reader)
		{
			_userId.Deserialize(reader);
			_name.Deserialize(reader);

		}



		#endregion
	}
}