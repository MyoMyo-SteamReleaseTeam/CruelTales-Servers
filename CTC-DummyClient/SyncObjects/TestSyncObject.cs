using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Serialization.Type;
using CT.Common.Synchronizations;
using CT.Tools.Collections;


namespace CTC.Networks.SyncObjects.TestSyncObjects
{

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