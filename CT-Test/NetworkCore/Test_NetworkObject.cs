using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Serialization.Type;
using CT.Tools.Collections;
using log4net.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.NetworkCore
{
	[TestClass]
	public class Test_NetworkObject
	{
		[TestMethod]
		public void NetworkObjectSyncTest()
		{
			NetworkObjectServer serverObj = new NetworkObjectServer();
			NetworkObjectClient clientObj = new NetworkObjectClient();

			NetTransform testTransform = new NetTransform()
			{
				Position = new NetVec2() { X = 10, Y = 20 },
				Velocity = new NetVec2() { X = 30, Y = 40 }
			};

			serverObj.ABC = 10;
			serverObj.Transform = testTransform;
			serverObj.Server_Some(50, 10.0f);
			serverObj.Server_Some(20, 20.0f);

			byte[] syncData = new byte[1000];
			PacketWriter writer = new PacketWriter(syncData);
			serverObj.SerializeSyncReliable(writer);

			PacketReader reader = new PacketReader(syncData);
			clientObj.DeserializeSyncReliable(reader);

			Assert.AreEqual(10, clientObj.ABC);
			Assert.AreEqual(testTransform, clientObj.Transform);
			Assert.AreEqual(70, clientObj.Value1);
			Assert.AreEqual(30.0f, clientObj.Value2);
		}
	}

	public enum SyncType
	{
		None = 0,
		Reliable,
		Unreliable,
	}

	public class SyncVarAttribute : Attribute
	{
		public SyncType SyncType { get; private set; }

		public SyncVarAttribute(SyncType syncType = SyncType.Reliable)
		{
			SyncType = syncType;
		}
	}

	public class ServerRpcAttribute : Attribute
	{
		public SyncType SyncType { get; private set; }

		public ServerRpcAttribute(SyncType syncType = SyncType.Reliable)
		{
			SyncType = syncType;
		}
	}

	public abstract class S_NetworkObject
	{
		public abstract void SerializeSyncReliable(PacketWriter writer);
		public abstract void ClearDirtyBit();
	}

	public abstract class C_NetworkObject
	{
		public abstract void DeserializeSyncReliable(PacketReader reader);
	}

	// 서버 자동 구현부
	public partial class NetworkObjectServer : S_NetworkObject
	{
		[SyncVar]
		private NetTransform _transform = new();

		[SyncVar]
		private int _abc = 0;

		[ServerRpc]
		public partial void Server_Some(int value1, float value2);

		#region Synchronization

		public NetworkObjectServer()
		{
			_transform = new();
			_abc = 0;
		}

		private BitmaskByte _propertyDirty_0 = new();
		private BitmaskByte _rpcDirty_0 = new();
		public bool IsDirty
		{
			get
			{
				bool isDirty = true;
				isDirty &= _propertyDirty_0.AnyTrue();
				isDirty &= _rpcDirty_0.AnyTrue();
				return isDirty;
			}
		}

		public NetTransform Transform
		{
			get => _transform;
			set
			{
				if (_transform == value)
					return;
				_transform = value;
				_propertyDirty_0[0] = true;
			}
		}

		public int ABC
		{
			get => _abc;
			set
			{
				if (_abc == value)
					return;
				_abc = value;
				_propertyDirty_0[1] = true;
			}
		}

		public partial void Server_Some(int value1, float value2)
		{
			_someCallstack.Enqueue((value1, value2));
			_rpcDirty_0[0] = true;
		}
		private Queue<(int value1, float value2)> _someCallstack = new();

		public override void SerializeSyncReliable(PacketWriter writer)
		{
			BitmaskByte objectDirty = new BitmaskByte();

			objectDirty[0] = _propertyDirty_0.AnyTrue();
			objectDirty[4] = _rpcDirty_0.AnyTrue();

			objectDirty.Serialize(writer);

			// Property dirty bits
			if (objectDirty[0])
			{
				_propertyDirty_0.Serialize(writer);

				// Property
				if (_propertyDirty_0[0])
					_transform.Serialize(writer);
					writer.Put(_transform);
				if (_propertyDirty_0[1])
					writer.Put(_abc);
			}

			// Rpc
			if (objectDirty[4])
			{
				_rpcDirty_0.Serialize(writer);

				if (_rpcDirty_0[0])
				{
					byte count = (byte)_someCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var args = _someCallstack.Dequeue();
						writer.Put(args.value1);
						writer.Put(args.value2);
					}
				}
			}
		}

		public override void ClearDirtyBit()
		{
			_propertyDirty_0.Clear();
			_rpcDirty_0.Clear();
		}

		#endregion
	}

	// 서버 구현부
	public partial class NetworkObjectServer
	{

	}

	// 클라 자동 구현부
	public partial class NetworkObjectClient : C_NetworkObject
	{
		[SyncVar]
		private NetTransform _transform;
		public event Action<NetTransform>? OnTransformChanged;

		[SyncVar]
		private int _abc = 0;
		public event Action<int>? OnABCChanged;

		[ServerRpc]
		public partial void Server_Some(int value1, float value2);

		public override void DeserializeSyncReliable(PacketReader reader)
		{
			BitmaskByte objectDirty = reader.ReadBitmaskByte();

			// Property dirty bits
			if (objectDirty[0])
			{
				BitmaskByte propertyDirty_0 = reader.ReadBitmaskByte();

				// Property
				if (propertyDirty_0[0])
				{
					//NetTransform transform = new();
					_transform.Deserialize(reader);
					//Transform = transform;
					OnTransformChanged?.Invoke(_transform);
				}
				if (propertyDirty_0[1])
				{
					_abc = reader.ReadInt32();
					OnABCChanged?.Invoke(_abc);
				}
			}

			// Rpc
			if (objectDirty[4])
			{
				BitmaskByte rpcDirty_0 = reader.ReadBitmaskByte();
				if (rpcDirty_0[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						int value1 = reader.ReadInt32();
						//NetTransform value2 = new();
						//value2.Deserialize(reader);
						float value2 = reader.ReadSingle();
						Server_Some(value1, value2);
					}
				}
			}
		}
	}

	// 클라 구현부
	public partial class NetworkObjectClient
	{
		public NetTransform Transform => _transform;
		public int ABC => _abc;

		public int Value1;
		public float Value2;

		public partial void Server_Some(int value1, float value2)
		{
			Value1 += value1;
			Value2 += value2;
		}
	}
}
