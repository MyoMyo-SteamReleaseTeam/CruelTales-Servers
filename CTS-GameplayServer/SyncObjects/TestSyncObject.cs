using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Serialization.Type;
using CT.Common.Synchronizations;
using CT.Tools.Collections;
using CTS.Instance.Synchronizations;


namespace CTS.Instance.SyncObjects
{

	[Serializable]
	public partial class TestSyncObject : IMasterSynchronizable
	{
		[SyncVar]
		private NetTransform _transform = new();

		[SyncVar]
		private int _abc;

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


			objectDirty[0] = _propertyDirty_0.AnyTrue();
			objectDirty[4] = _rpcDirty_0.AnyTrue();


			objectDirty.Serialize(writer);


			if (objectDirty[0])
			{
				_propertyDirty_0.Serialize(writer);

				if (_propertyDirty_0[0]) _transform.Serialize(writer);
				if (_propertyDirty_0[1]) writer.Put(_abc);

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

		}


		#endregion
	}

}