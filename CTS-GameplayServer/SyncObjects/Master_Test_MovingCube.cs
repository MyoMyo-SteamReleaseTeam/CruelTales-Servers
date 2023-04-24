/*
 * Generated File : Master_Test_MovingCube.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#pragma warning disable CS0649

using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Serialization.Type;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	[Serializable]
	public partial class Test_MovingCube : MasterNetworkObject
	{
		public override NetworkObjectType Type => NetworkObjectType.Test_MovingCube;
		/// DECLARE MASTER SIDE SYNC ELEMETS ///
		[SyncVar]
		private NetworkIdentity _networkIdentity = new();
		[SyncVar]
		private byte _r;
		[SyncVar]
		private byte _g;
		[SyncVar]
		private byte _b;
		[SyncRpc]
		public partial void Server_MoveTo(NetVec2 _destination);
		/// DECLARE SYNCHRONIZATIONS ///
		private BitmaskByte _propertyDirty_0 = new();
		private BitmaskByte _rpcDirty_0 = new();
		public override bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _propertyDirty_0.AnyTrue();
				isDirty |= _rpcDirty_0.AnyTrue();
				return isDirty;
			}
		}
		public NetworkIdentity NetworkIdentity
		{
			get => _networkIdentity;
			set
			{
				if (_networkIdentity == value) return;
				_networkIdentity = value;
				_propertyDirty_0[0] = true;
			}
		}
		public byte R
		{
			get => _r;
			set
			{
				if (_r == value) return;
				_r = value;
				_propertyDirty_0[1] = true;
			}
		}
		public byte G
		{
			get => _g;
			set
			{
				if (_g == value) return;
				_g = value;
				_propertyDirty_0[2] = true;
			}
		}
		public byte B
		{
			get => _b;
			set
			{
				if (_b == value) return;
				_b = value;
				_propertyDirty_0[3] = true;
			}
		}
		public partial void Server_MoveTo(NetVec2 _destination)
		{
			Server_MoveToCallstack.Enqueue(_destination);
			_rpcDirty_0[0] = true;
		}
		private Queue<NetVec2> Server_MoveToCallstack = new();
		public override bool IsDirtyUnreliable => false;
#region FromMaster
		public override void SerializeSyncReliable(PacketWriter writer)
		{
			BitmaskByte objectDirty = new BitmaskByte();
			objectDirty[0] = _propertyDirty_0.AnyTrue();
			objectDirty[4] = _rpcDirty_0.AnyTrue();
			objectDirty.Serialize(writer);
			if (objectDirty[0])
			{
				_propertyDirty_0.Serialize(writer);
				if (_propertyDirty_0[0]) _networkIdentity.Serialize(writer);
				if (_propertyDirty_0[1]) writer.Put(_r);
				if (_propertyDirty_0[2]) writer.Put(_g);
				if (_propertyDirty_0[3]) writer.Put(_b);
			}
			if (objectDirty[4])
			{
				_rpcDirty_0.Serialize(writer);
				if (_rpcDirty_0[0])
				{
					byte count = (byte)Server_MoveToCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = Server_MoveToCallstack.Dequeue();
						arg.Serialize(writer);
					}
				}
			}
		}
		public override void SerializeSyncUnreliable(PacketWriter writer) { }
		public override void SerializeEveryProperty(PacketWriter writer)
		{
			_networkIdentity.Serialize(writer);
			writer.Put(_r);
			writer.Put(_g);
			writer.Put(_b);
		}
#endregion
		public override void DeserializeSyncReliable(PacketReader reader) { }
		public override void DeserializeSyncUnreliable(PacketReader reader) { }
		public override void DeserializeEveryProperty(PacketReader reader) { }
		public override void ClearDirtyReliable()
		{
			_propertyDirty_0.Clear();
			_rpcDirty_0.Clear();
		}
		public override void ClearDirtyUnreliable() {}
	}
}
#pragma warning restore CS0649
